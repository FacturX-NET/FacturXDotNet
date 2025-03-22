using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.Filters;

namespace FacturXDotNet.Parsing;

/// <summary>
///     Extract attachments from a Factur-X PDF document.
/// </summary>
class ExtractAttachmentsFromFacturX
{
    /// <summary>
    ///     Extract attachments from a Factur-X PDF document.
    /// </summary>
    public IEnumerable<(string Name, Stream Content)> ExtractFacturXAttachments(PdfDocument document)
    {
        PdfCatalog catalog = document.Internals.Catalog;
        PdfArray? attachedFiles = catalog.Elements.GetArray("/AF");

        if (attachedFiles == null)
        {
            yield break;
        }

        foreach (PdfItem? attachedFile in attachedFiles.Elements)
        {
            if (attachedFile is not PdfReference { Value: PdfDictionary fileSpec })
            {
                continue;
            }

            string attachmentName = fileSpec.Elements.GetString("/F");

            if (fileSpec.Elements.GetDictionary("/EF") is not { } embeddedFile)
            {
                continue;
            }

            if (embeddedFile.Elements.GetReference("/F") is not { } pdfStreamReference)
            {
                continue;
            }

            if (pdfStreamReference.Value is not PdfDictionary pdfStreamDictionary)
            {
                continue;
            }

            PdfDictionary.PdfStream pdfStream = pdfStreamDictionary.Stream;
            if (pdfStream.Length == 0)
            {
                continue;
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

            yield return new ValueTuple<string, Stream>(attachmentName, new MemoryStream(bytes));
        }
    }
}
