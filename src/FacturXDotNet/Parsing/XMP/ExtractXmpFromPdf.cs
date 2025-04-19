using System.Diagnostics.CodeAnalysis;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;

namespace FacturXDotNet.Parsing.XMP;

/// <summary>
///     Extracts the XMP metadata from a Factur-X invoice.
/// </summary>
class ExtractXmpFromPdf
{
    /// <summary>
    ///     Extracts the XMP metadata from a Factur-X PDF document.
    /// </summary>
    public Stream ExtractXmpMetadata(PdfDocument document) =>
        TryExtractXmpMetadata(document, out Stream? result) ? result : throw new InvalidOperationException("The XMP metadata could not be found.");

    /// <summary>
    ///     Extracts the XMP metadata from a Factur-X PDF document.
    /// </summary>
    public bool TryExtractXmpMetadata(PdfDocument document, [NotNullWhen(true)] out Stream? xmpMetadataStream)
    {
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

        pdfStream.TryUncompress();
        byte[] bytes = pdfStream.Value;

        xmpMetadataStream = new MemoryStream(bytes);
        return true;
    }
}
