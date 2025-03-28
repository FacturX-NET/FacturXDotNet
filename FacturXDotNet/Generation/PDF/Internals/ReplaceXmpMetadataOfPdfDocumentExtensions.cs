using System.Security.Cryptography;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.Filters;

namespace FacturXDotNet.Generation.PDF.Internals;

/// <summary>
///     Replace the XMP metadata of a PDF document.
/// </summary>
static class ReplaceXmpMetadataOfPdfDocumentExtensions
{
    /// <summary>
    ///     Replace the XMP metadata of a PDF document.
    /// </summary>
    /// <remarks>
    ///     See Section 14.3.2 of ISO 32000-2:2020 (PDF 2.0). <br />
    ///     https://pdfa.org/resource/iso-32000-2/
    /// </remarks>
    /// <param name="document">The PDF document.</param>
    /// <param name="newXmpMetadata">The new XMP metadata.</param>
    public static void ReplaceXmpMetadata(this PdfDocument document, ReadOnlySpan<byte> newXmpMetadata)
    {
        PdfCatalog catalog = document.Internals.Catalog;
        PdfReference? metadataReference = catalog.Elements.GetReference("/Metadata");
        if (metadataReference?.Value is PdfDictionary metadataDictionary)
        {
            catalog.Elements.Remove("/Metadata");
            document.Internals.RemoveObject(metadataDictionary);
        }

        CreateMetadata(document, newXmpMetadata);
    }

    static void CreateMetadata(PdfDocument document, ReadOnlySpan<byte> metadata)
    {
        FlateDecode flateDecode = new();
        byte[] encoded = flateDecode.Encode(metadata.ToArray(), PdfFlateEncodeMode.BestCompression);

        PdfDictionary metadataDictionary = new();
        metadataDictionary.CreateStream(encoded.ToArray());
        metadataDictionary.Elements.Add("/Filter", new PdfName("/FlateDecode"));
        metadataDictionary.Elements.Add("/Type", new PdfName("/Metadata"));
        metadataDictionary.Elements.Add("/SubType", new PdfString("XML"));

        PdfDictionary pdfStreamParams = new();
        pdfStreamParams.Elements.Add("/CheckSum", new PdfString(ComputeChecksum(metadata)));
        pdfStreamParams.Elements.Add("/Size", new PdfInteger(metadata.Length));
        metadataDictionary.Elements.Add("/Params", pdfStreamParams);

        document.Internals.AddObject(metadataDictionary);
        document.Internals.Catalog.Elements.Add("/Metadata", metadataDictionary);
    }

    static string ComputeChecksum(ReadOnlySpan<byte> data)
    {
        byte[] contentHash = MD5.HashData(data);
        return BitConverter.ToString(contentHash).Replace("-", "").ToLowerInvariant();
    }
}
