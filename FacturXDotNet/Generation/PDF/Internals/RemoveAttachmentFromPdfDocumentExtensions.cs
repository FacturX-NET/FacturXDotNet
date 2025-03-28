using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;

namespace FacturXDotNet.Generation.PDF.Internals;

/// <summary>
///     Remove an attachment from a PDF document.
/// </summary>
static class RemoveAttachmentFromPdfDocumentExtensions
{
    /// <summary>
    ///     Remove an attachment from a PDF document.
    /// </summary>
    /// <remarks>
    ///     See Section 14.13 of ISO 32000-2:2020 (PDF 2.0). <br />
    ///     https://pdfa.org/resource/iso-32000-2/
    /// </remarks>
    /// <param name="document">The PDF document.</param>
    /// <param name="name">The name of the attachment to remove.</param>
    public static void RemoveAttachment(this PdfDocument document, string name)
    {
        RemoveFileSpecificationsInEmbeddedFiles(document, name);
        RemoveFileSpecificationsInAssociatedFiles(document, name);
    }

    /// <summary>
    ///     Remove an attachment from the /EmbeddedFiles dictionary of a PDF document.
    /// </summary>
    /// <remarks>
    ///     See Section 14.13 of ISO 32000-2:2020 (PDF 2.0). <br />
    ///     https://pdfa.org/resource/iso-32000-2/
    /// </remarks>
    /// <param name="document">The PDF document.</param>
    /// <param name="name">The name of the attachment to remove.</param>
    static void RemoveFileSpecificationsInEmbeddedFiles(PdfDocument document, string name)
    {
        PdfDictionary? names = document.Internals.Catalog.Elements.GetDictionary("/Names");
        PdfDictionary? embeddedFiles = names?.Elements.GetDictionary("/EmbeddedFiles");
        PdfArray? embeddedFilesNames = embeddedFiles?.Elements.GetArray("/Names");

        if (embeddedFilesNames is null)
        {
            return;
        }

        List<int> indexes = [];
        List<PdfDictionary> fileSpecificationsToRemove = [];

        for (int i = 0; i < embeddedFilesNames.Elements.Count - 1; i++)
        {
            if (embeddedFilesNames.Elements[i] is PdfString nameString && nameString.Value == name && embeddedFilesNames.Elements[i + 1] is PdfReference reference)
            {
                indexes.Add(i);
                indexes.Add(i + 1);
                i++;

                if (reference.Value is PdfDictionary fileSpecificationObject)
                {
                    fileSpecificationsToRemove.Add(fileSpecificationObject);
                }
            }
        }

        for (int i = indexes.Count - 1; i >= 0; i--)
        {
            int index = indexes[i];
            embeddedFilesNames.Elements.RemoveAt(index);
        }

        foreach (PdfDictionary fileSpecificationObject in fileSpecificationsToRemove)
        {
            RemoveFileSpecification(document, fileSpecificationObject);
        }
    }

    static void RemoveFileSpecificationsInAssociatedFiles(PdfDocument document, string name)
    {
        PdfArray? attachedFiles = document.Internals.Catalog.Elements.GetArray("/AF");
        if (attachedFiles is null)
        {
            return;
        }

        List<PdfDictionary> fileSpecificationsToRemove = [];

        foreach (PdfItem attachedFile in attachedFiles.Elements)
        {
            if (attachedFile is not PdfReference { Value: PdfDictionary fileSpecification })
            {
                continue;
            }

            if (fileSpecification.Elements.GetString("/F") == name)
            {
                fileSpecificationsToRemove.Add(fileSpecification);
                break;
            }
        }

        foreach (PdfDictionary fileSpecification in fileSpecificationsToRemove)
        {
            attachedFiles.Elements.Remove(fileSpecification.ReferenceNotNull);
            RemoveFileSpecification(document, fileSpecification);
        }
    }

    static void RemoveFileSpecification(PdfDocument document, PdfDictionary fileSpecification)
    {
        if (fileSpecification.Elements.GetDictionary("/EF") is { } embeddedFile && embeddedFile.Elements.GetReference("/F") is { Value: { } embeddedFileObject })
        {
            document.Internals.RemoveObject(embeddedFileObject);
        }

        document.Internals.RemoveObject(fileSpecification);
    }
}
