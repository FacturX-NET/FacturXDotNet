using System.Diagnostics.CodeAnalysis;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.Filters;
using PdfSharp.Pdf.IO;

namespace FacturXDotNet.Parsers.FacturX;

/// <summary>
///     Extracts the XMP metadata from a Factur-X invoice.
/// </summary>
class ExtractXmpFromFacturX(FacturXParserOptions? options = null)
{
    readonly FacturXParserOptions _options = options ?? new FacturXParserOptions();

    public Stream ExtractXmpAttachment(Stream facturXStream) =>
        TryExtractXmpMetadata(facturXStream, out Stream? result) ? result : throw new InvalidOperationException("The XMP metadata could not be found.");

    /// <summary>
    ///     Extracts the XMP metadata from a Factur-X invoice.
    /// </summary>
    public bool TryExtractXmpMetadata(Stream facturXStream, [NotNullWhen(true)] out Stream? xmpMetadataStream)
    {
        PdfDocument document;

        if (_options.Password != null)
        {
            document = PdfReader.Open(facturXStream, PdfDocumentOpenMode.Import, args => args.Password = _options.Password);
        }
        else
        {
            document = PdfReader.Open(facturXStream, PdfDocumentOpenMode.Import);
        }

        using PdfDocument _ = document;

        PdfCatalog catalog = document.Internals.Catalog;
        PdfReference? metadataReference = catalog.Elements.GetReference("/Metadata");
        if (metadataReference?.Value is not PdfDictionary metadataDictionary)
        {
            xmpMetadataStream = null;
            return false;
        }

        PdfDictionary.PdfStream pdfStream = metadataDictionary.Stream;
        if (pdfStream.Length == 0)
        {
            xmpMetadataStream = null;
            return false;
        }

        byte[] bytes;
        if (pdfStream.TryUnfilter())
        {
            bytes = pdfStream.Value;
        }
        else
        {
            FlateDecode flate = new();
            bytes = flate.Decode(pdfStream.Value, new PdfDictionary());
        }

        xmpMetadataStream = new MemoryStream(bytes);
        return true;
    }
}
