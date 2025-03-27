using FacturXDotNet.Generation.PDF;
using FacturXDotNet.Generation.XMP;
using FacturXDotNet.Models.XMP;
using FacturXDotNet.Parsing.XMP;
using Microsoft.Extensions.Logging;
using PdfSharp.Pdf;

namespace FacturXDotNet.Generation.Internals;

static class FacturXBuilderXmpMetadata
{
    public static async Task AddXmpMetadataAsync(PdfDocument pdfDocument, FacturXDocumentBuildArgs args)
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

        args.PostProcessXmpMetadata?.Invoke(xmpMetadata);

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
}
