using System.Diagnostics.CodeAnalysis;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.Filters;

namespace FacturXDotNet.Parsing.FacturX;

/// <summary>
///     Extract the Cross-Industry Invoice XML attachment from a Factur-X PDF document.
/// </summary>
class ExtractCiiFromFacturX(string ciiAttachmentName)
{
    /// <summary>
    ///     Extract the Cross-Industry Invoice XML attachment from a Factur-X PDF document.
    /// </summary>
    /// <param name="document"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public Stream ExtractFacturXAttachment(PdfDocument document) =>
        TryExtractFacturXAttachment(document, out Stream? result)
            ? result
            : throw new InvalidOperationException($"The Cross-Industry Invoice XML attachment with name '{ciiAttachmentName}' could not be found.");

    /// <summary>
    ///     Extract the Cross-Industry Invoice XML attachment from a Factur-X PDF document.
    /// </summary>
    /// <param name="document">The Factur-X PDF document to parse.</param>
    /// <param name="facturXAttachment">The Cross-Industry Invoice document.</param>
    public bool TryExtractFacturXAttachment(PdfDocument document, [NotNullWhen(true)] out Stream? facturXAttachment)
    {
        PdfCatalog catalog = document.Internals.Catalog;
        PdfArray? attachedFiles = catalog.Elements.GetArray("/AF");

        if (attachedFiles == null)
        {
            facturXAttachment = null;
            return false;
        }

        foreach (PdfItem? attachedFile in attachedFiles.Elements)
        {
            if (attachedFile is not PdfReference { Value: PdfDictionary fileSpec })
            {
                continue;
            }

            string attachedFileName = fileSpec.Elements.GetString("/F");
            if (attachedFileName != ciiAttachmentName)
            {
                continue;
            }

            if (fileSpec.Elements.GetDictionary("/EF") is not { } embeddedFile)
            {
                facturXAttachment = null;
                return false;
            }

            if (embeddedFile.Elements.GetReference("/F") is not { } pdfStreamReference)
            {
                facturXAttachment = null;
                return false;
            }

            if (pdfStreamReference.Value is not PdfDictionary pdfStreamDictionary)
            {
                facturXAttachment = null;
                return false;
            }

            PdfDictionary.PdfStream pdfStream = pdfStreamDictionary.Stream;
            if (pdfStream.Length == 0)
            {
                facturXAttachment = null;
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

            facturXAttachment = new MemoryStream(bytes);
            return true;
        }

        facturXAttachment = null;
        return false;
    }
}
