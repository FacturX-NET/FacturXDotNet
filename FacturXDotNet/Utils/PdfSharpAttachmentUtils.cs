using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;

namespace FacturXDotNet.Utils;

static class PdfSharpAttachmentUtils
{
    /// <summary>
    ///     Extract attachments from a Factur-X PDF document.
    /// </summary>
    /// <param name="document">The PDF document.</param>
    /// <returns>The names of the attachments of the Factur-X document.</returns>
    public static IEnumerable<string> ListAttachments(this PdfDocument document)
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

            yield return fileSpec.Elements.GetString("/F");
        }
    }

    public static Stream ExtractAttachment(this PdfDocument document, string name)
    {
        PdfCatalog catalog = document.Internals.Catalog;
        PdfArray? attachedFiles = catalog.Elements.GetArray("/AF");

        if (attachedFiles != null)
        {
            foreach (PdfItem? attachedFile in attachedFiles.Elements)
            {
                if (attachedFile is not PdfReference { Value: PdfDictionary fileSpec })
                {
                    continue;
                }

                string attachmentName = fileSpec.Elements.GetString("/F");
                if (attachmentName != name)
                {
                    continue;
                }

                if (fileSpec.Elements.GetDictionary("/EF") is not { } embeddedFile)
                {
                    break;
                }

                if (embeddedFile.Elements.GetReference("/F") is not { } pdfStreamReference)
                {
                    break;
                }

                if (pdfStreamReference.Value is not PdfDictionary pdfStreamDictionary)
                {
                    break;
                }

                PdfDictionary.PdfStream pdfStream = pdfStreamDictionary.Stream;
                if (pdfStream.Length == 0)
                {
                    break;
                }

                pdfStream.TryUncompress();
                byte[] bytes = pdfStream.Value;
                return new MemoryStream(bytes);
            }
        }

        throw new InvalidOperationException($"Could not find the attachment {name} in the PDF document.");
    }
}
