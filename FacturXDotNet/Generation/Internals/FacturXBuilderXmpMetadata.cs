using FacturXDotNet.Generation.PDF;
using FacturXDotNet.Generation.XMP;
using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;
using FacturXDotNet.Parsing.XMP;
using Microsoft.Extensions.Logging;
using PdfSharp.Pdf;

namespace FacturXDotNet.Generation.Internals;

static class FacturXBuilderXmpMetadata
{
    static readonly string? Version = typeof(FacturXBuilderXmpMetadata).Assembly.GetName().Version?.ToString();

    public static async Task AddXmpMetadataAsync(PdfDocument pdfDocument, CrossIndustryInvoice cii, FacturXDocumentBuildArgs args)
    {
        Stream xmpStream;
        if (args.Xmp is null)
        {
            ExtractXmpFromPdf extractor = new();
            xmpStream = extractor.ExtractXmpMetadata(pdfDocument);

            // ensure xmp stream is disposed
            args.XmpLeaveOpen = false;
        }
        else
        {
            xmpStream = args.Xmp;
        }

        XmpMetadataReader xmpReader = new();
        XmpMetadata xmpMetadata = xmpReader.Read(xmpStream);

        DateTimeOffset now = DateTimeOffset.Now;

        xmpMetadata.PdfAIdentification ??= new XmpPdfAIdentificationMetadata();
        xmpMetadata.PdfAIdentification.Conformance ??= XmpPdfAConformanceLevel.B;
        xmpMetadata.PdfAIdentification.Part ??= 3;
        xmpMetadata.Basic ??= new XmpBasicMetadata();
        xmpMetadata.Basic.CreateDate ??= now;
        xmpMetadata.Basic.ModifyDate = now;
        xmpMetadata.Basic.MetadataDate = now;
        xmpMetadata.Pdf ??= new XmpPdfMetadata();
        xmpMetadata.DublinCore ??= new XmpDublinCoreMetadata();
        xmpMetadata.DublinCore.Date.Add(now);
        xmpMetadata.PdfAExtensions ??= new XmpPdfAExtensionsMetadata();
        AddFacturXPdfAExtensionIfNecessary(xmpMetadata.PdfAExtensions);
        xmpMetadata.FacturX ??= new XmpFacturXMetadata();
        xmpMetadata.FacturX.DocumentFileName = args.CiiAttachmentName;
        xmpMetadata.FacturX.DocumentType = XmpFacturXDocumentType.Invoice;
        xmpMetadata.FacturX.Version = "1.0";
        xmpMetadata.FacturX.ConformanceLevel = cii.ExchangedDocumentContext?.GuidelineSpecifiedDocumentContextParameterId?.ToFacturXProfileOrNull()?.ToXmpFacturXConformanceLevel()
                                               ?? throw new InvalidOperationException("The CII document does not contain a valid GuidelineSpecifiedDocumentContextParameterId.");

        args.PostProcessXmpMetadata?.Invoke(xmpMetadata);

        string toolName = string.IsNullOrWhiteSpace(Version) ? "FacturX.NET ~dev" : $"FacturX.NET v{Version}";
        xmpMetadata.Basic.CreatorTool = toolName;
        xmpMetadata.Pdf.Producer = toolName;

        if (!args.XmpLeaveOpen)
        {
            await xmpStream.DisposeAsync();
        }

        await using MemoryStream finalXmpStream = new();
        XmpMetadataWriter xmpWriter = new();
        await xmpWriter.WriteAsync(finalXmpStream, xmpMetadata);

        ReplaceXmpMetadataOfPdfDocument.ReplaceXmpMetadata(pdfDocument, finalXmpStream.GetBuffer().AsSpan(0, (int)finalXmpStream.Length));
        args.Logger?.LogInformation("Added XMP metadata to the PDF document.");
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
}
