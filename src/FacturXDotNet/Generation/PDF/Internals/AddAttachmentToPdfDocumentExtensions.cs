using PdfSharp.Pdf;

namespace FacturXDotNet.Generation.PDF.Internals;

/// <summary>
///     Add an attachment to a PDF document.
/// </summary>
static class AddAttachmentToPdfDocumentExtensions
{
    /// <summary>
    ///     Add an attachment to a PDF document.
    /// </summary>
    /// <remarks>
    ///     See Section 14.13 of ISO 32000-2:2020 (PDF 2.0). <br />
    ///     https://pdfa.org/resource/iso-32000-2/
    /// </remarks>
    /// <param name="document">The PDF document.</param>
    /// <param name="file">The file to attach.</param>
    public static void AddAttachment(this PdfDocument document, PdfAttachmentData file)
    {
        DateTime now = DateTime.Now;
        string mimeType = file.MimeType ?? "application/octet-stream";
        AfRelationship relationship = file.Relationship ?? AfRelationship.Unspecified;
        DateTime creationDate = file.CreationDate ?? now;
        DateTime modificationDate = file.ModificationDate ?? now;


        PdfDictionary embeddedFileStreamDictionary = new();
        embeddedFileStreamDictionary.Elements.Add("/Type", new PdfName("/EmbeddedFile"));
        embeddedFileStreamDictionary.Elements.Add("/Subtype", new PdfName($"/{mimeType}"));
        embeddedFileStreamDictionary.WriteFlateEncodedData(
            file.Content.Span,
            p =>
            {
                p.Elements.Add("/CreationDate", new PdfString(FormatDate(creationDate)));
                p.Elements.Add("/ModDate", new PdfString(FormatDate(modificationDate)));
            }
        );

        document.Internals.AddObject(embeddedFileStreamDictionary);

        PdfDictionary embeddedFileDictionary = new();
        embeddedFileDictionary.Elements.Add("/F", embeddedFileStreamDictionary.ReferenceNotNull);
        embeddedFileDictionary.Elements.Add("/UF", embeddedFileStreamDictionary.ReferenceNotNull);

        PdfDictionary fileSpecificationDictionary = new();
        fileSpecificationDictionary.Elements.Add("/AFRelationship", new PdfName(FormatRelationship(relationship)));
        fileSpecificationDictionary.Elements.Add("/Type", new PdfName("/Filespec"));
        fileSpecificationDictionary.Elements.Add("/F", new PdfString(file.Name));
        fileSpecificationDictionary.Elements.Add("/UF", new PdfString(file.Name));
        fileSpecificationDictionary.Elements.Add("/EF", embeddedFileDictionary);
        document.Internals.AddObject(fileSpecificationDictionary);

        if (file.Description is not null)
        {
            fileSpecificationDictionary.Elements.Add("/Desc", new PdfString(file.Description));
        }

        PdfArray? attachedFiles = document.Internals.Catalog.Elements.GetArray("/AF");
        if (attachedFiles is null)
        {
            attachedFiles = new PdfArray(document);
            document.Internals.AddObject(attachedFiles);
            document.Internals.Catalog.Elements.Add("/AF", attachedFiles.ReferenceNotNull);
        }

        attachedFiles.Elements.Add(fileSpecificationDictionary.ReferenceNotNull);

        PdfDictionary? names = document.Internals.Catalog.Elements.GetDictionary("/Names");
        if (names?.Elements.GetObject("/EmbeddedFiles") is not PdfDictionary embeddedFiles)
        {
            embeddedFiles = new PdfDictionary();
            document.Internals.AddObject(embeddedFiles);
            document.Internals.Catalog.Names.Elements.SetReference("/EmbeddedFiles", embeddedFiles.ReferenceNotNull);
        }

        if (embeddedFiles.Elements.GetObject("/Names") is not PdfArray embeddedFileNames)
        {
            embeddedFileNames = new PdfArray(document);
            embeddedFiles.Elements.Add("/Names", embeddedFileNames);
        }

        embeddedFileNames.Elements.Add(new PdfString(file.Name));
        embeddedFileNames.Elements.Add(fileSpecificationDictionary.ReferenceNotNull);
    }

    static string FormatRelationship(AfRelationship relationship) =>
        relationship switch
        {
            AfRelationship.Source => "/Source",
            AfRelationship.Data => "/Data",
            AfRelationship.Alternative => "/Alternative",
            AfRelationship.Supplement => "/Supplement",
            AfRelationship.EncryptedPayload => "/EncryptedPayload",
            AfRelationship.FormData => "/FormData",
            AfRelationship.Schema => "/Schema",
            _ => "/Unspecified"
        };

    static string FormatDate(DateTime date)
    {
        TimeSpan offset = TimeZoneInfo.Local.GetUtcOffset(date);
        string offsetString = offset >= TimeSpan.Zero ? $"+{offset.Hours:00}'{offset.Minutes:00}'" : $"-{offset.Hours:00}'{offset.Minutes:00}'";
        return $"D:{date:yyyyMMddHHmmss}{offsetString}";
    }
}
