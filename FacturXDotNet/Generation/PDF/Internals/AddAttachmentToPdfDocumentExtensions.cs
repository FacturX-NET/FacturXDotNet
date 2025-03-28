using System.Security.Cryptography;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Filters;

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
        FlateDecode flateDecode = new();
        byte[] encoded = flateDecode.Encode(file.Content.ToArray(), PdfFlateEncodeMode.BestCompression);

        PdfDictionary embeddedFileStreamDictionary = new();
        embeddedFileStreamDictionary.CreateStream(encoded);
        embeddedFileStreamDictionary.Elements.Add("/Filter", new PdfName("/FlateDecode"));
        embeddedFileStreamDictionary.Elements.Add("/Type", new PdfName("/EmbeddedFile"));
        embeddedFileStreamDictionary.Elements.Add("/SubType", new PdfString(file.MimeType ?? "application/octet-stream"));

        PdfDictionary pdfStreamParams = new();
        pdfStreamParams.Elements.Add("/CheckSum", new PdfString(ComputeChecksum(file.Content)));
        pdfStreamParams.Elements.Add("/Size", new PdfInteger(file.Content.Length));
        pdfStreamParams.Elements.Add("/CreationDate", new PdfString(FormatDate(file.CreationDate ?? DateTime.Now)));
        pdfStreamParams.Elements.Add("/ModDate", new PdfString(FormatDate(file.ModificationDate ?? DateTime.Now)));
        embeddedFileStreamDictionary.Elements.Add("/Params", pdfStreamParams);

        document.Internals.AddObject(embeddedFileStreamDictionary);

        PdfDictionary embeddedFileDictionary = new();
        embeddedFileDictionary.Elements.Add("/F", embeddedFileStreamDictionary.Reference);
        embeddedFileDictionary.Elements.Add("/UF", embeddedFileStreamDictionary.Reference);

        PdfDictionary fileSpecificationDictionary = new();
        fileSpecificationDictionary.Elements.Add("/AFRelationship", new PdfName(FormatRelationship(file.Relationship ?? AfRelationship.Unspecified)));
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
            document.Internals.Catalog.Elements.Add("/AF", attachedFiles.Reference);
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

    static string ComputeChecksum(ReadOnlyMemory<byte> data)
    {
        byte[] contentHash = MD5.HashData(data.Span);
        return BitConverter.ToString(contentHash).Replace("-", "").ToLowerInvariant();
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
