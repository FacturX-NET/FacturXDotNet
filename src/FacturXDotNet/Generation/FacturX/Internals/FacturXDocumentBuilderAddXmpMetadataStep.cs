using FacturXDotNet.Generation.PDF.Internals;
using FacturXDotNet.Generation.XMP;
using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;
using FacturXDotNet.Parsing.XMP;
using Microsoft.Extensions.Logging;
using PdfSharp.Pdf;

namespace FacturXDotNet.Generation.FacturX.Internals;

static class FacturXDocumentBuilderAddXmpMetadataStep
{
    static readonly string? Version = typeof(FacturXDocumentBuilderAddXmpMetadataStep).Assembly.GetName().Version?.ToString();

    public static async Task<XmpMetadata> RunAsync(PdfDocument pdfDocument, CrossIndustryInvoice cii, FacturXDocumentBuildArgs args)
    {
        XmpMetadata xmpMetadata = await GetOrCreateBaseXmpMetadata(pdfDocument, args);

        DateTimeOffset now = DateTimeOffset.Now;

        if (!args.DisableXmpMetadataAutoGeneration)
        {
            // Note: some values are overwritten on purpose, they correspond to values that are the responsibility of the writer.
            // For example
            // - the PDF/A version must be 3 or higher
            // - the FacturX DocumentFileName must be set to the name of the CII attachment
            // - the FacturX ConformanceLevel must be set to the value of the GuidelineSpecifiedDocumentContextParameterId
            // - ... 
            //
            // They can still be post processed by the user: it is a choice to give them more control. Changing these values is dangerous: it might make the document invalid
            // with respect to the PDF/A standard.
            //
            // Finally, some values cannot be post-processed: for example the name of the creator tool (this lib)

            xmpMetadata.PdfAIdentification ??= new XmpPdfAIdentificationMetadata();
            xmpMetadata.PdfAIdentification.Conformance ??= XmpPdfAConformanceLevel.B;
            xmpMetadata.PdfAIdentification.Part = 3;
            xmpMetadata.Basic ??= new XmpBasicMetadata();
            xmpMetadata.Basic.CreateDate = now;
            xmpMetadata.Basic.ModifyDate = now;
            xmpMetadata.Basic.MetadataDate = now;
            xmpMetadata.PdfAExtensions ??= new XmpPdfAExtensionsMetadata();
            AddFacturXPdfAExtensionIfNecessary(xmpMetadata.PdfAExtensions);
            xmpMetadata.FacturX ??= new XmpFacturXMetadata();
            xmpMetadata.FacturX.DocumentFileName = args.CiiAttachmentName;
            xmpMetadata.FacturX.DocumentType = XmpFacturXDocumentType.Invoice;
            xmpMetadata.FacturX.Version = "1.0";
            xmpMetadata.FacturX.ConformanceLevel =
                cii.ExchangedDocumentContext?.GuidelineSpecifiedDocumentContextParameterId?.ToFacturXProfileOrNull()?.ToXmpFacturXConformanceLevel()
                ?? throw new InvalidOperationException("The CII document does not contain a valid GuidelineSpecifiedDocumentContextParameterId.");
        }

        args.PostProcess.ConfigureXmpMetadata(xmpMetadata);

        string toolName = string.IsNullOrWhiteSpace(Version) ? "FacturX.NET ~dev" : $"FacturX.NET v{Version}";
        xmpMetadata.Basic ??= new XmpBasicMetadata();
        xmpMetadata.Basic.CreatorTool = toolName;
        xmpMetadata.Pdf ??= new XmpPdfMetadata();
        xmpMetadata.Pdf.Producer = toolName;

        await using MemoryStream finalXmpStream = new();
        XmpMetadataWriter xmpWriter = new();
        await xmpWriter.WriteAsync(finalXmpStream, xmpMetadata);

        pdfDocument.ReplaceXmpMetadata(finalXmpStream.GetBuffer().AsSpan(0, (int)finalXmpStream.Length));
        args.Logger?.LogInformation("Added XMP metadata to the PDF document.");

        return xmpMetadata;
    }

    static async Task<XmpMetadata> GetOrCreateBaseXmpMetadata(PdfDocument pdfDocument, FacturXDocumentBuildArgs args)
    {
        if (args.XmpProvider is not null)
        {
            return args.XmpProvider.GetXmpMetadata();
        }

        ExtractXmpFromPdf extractor = new();
        if (extractor.TryExtractXmpMetadata(pdfDocument, out Stream? xmpStream))
        {
            return await ReadXmpMetadataFromStream(xmpStream, true);
        }

        return new XmpMetadata();
    }

    static void AddFacturXPdfAExtensionIfNecessary(XmpPdfAExtensionsMetadata extensions)
    {
        const string namespaceUri = "urn:factur-x:pdfa:CrossIndustryDocument:invoice:1p0#";
        const string prefix = "fx";
        const string name = "Factur-X PDFA Extension Schema";

        if (extensions.Schemas.Any(s => s.NamespaceUri == namespaceUri))
        {
            return;
        }

        extensions.Schemas.Add(
            new XmpPdfASchemaMetadata
            {
                NamespaceUri = namespaceUri,
                Prefix = prefix,
                Property =
                [
                    new XmpPdfAPropertyMetadata
                    {
                        Category = XmpPdfAPropertyCategory.External,
                        Description = "The name of the embedded XML document",
                        Name = "DocumentFileName",
                        ValueType = "Text"
                    },
                    new XmpPdfAPropertyMetadata
                    {
                        Category = XmpPdfAPropertyCategory.External,
                        Description = "The type of the hybrid document in capital letters, e.g. INVOICE or ORDER",
                        Name = "DocumentType",
                        ValueType = "Text"
                    },
                    new XmpPdfAPropertyMetadata
                    {
                        Category = XmpPdfAPropertyCategory.External,
                        Description = "The actual version of the standard applying to the embedded XML document",
                        Name = "Version",
                        ValueType = "Text"
                    },
                    new XmpPdfAPropertyMetadata
                    {
                        Category = XmpPdfAPropertyCategory.External,
                        Description = "The conformance level of the embedded XML document",
                        Name = "ConformanceLevel",
                        ValueType = "Text"
                    }
                ],
                Schema = name,
                ValueType = []
            }
        );
    }

    static async Task<XmpMetadata> ReadXmpMetadataFromStream(Stream stream, bool closeStream)
    {
        try
        {
            XmpMetadataReader xmpReader = new();
            return xmpReader.Read(stream);
        }
        finally
        {
            if (closeStream)
            {
                await stream.DisposeAsync();
            }
        }
    }
}
