using System.Security.Cryptography;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Filters;

namespace FacturXDotNet.Generation.PDF;

/// <summary>
///     Add an attachment to a PDF document.
/// </summary>
static class AddAttachmentToPdfDocument
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
        byte[] encoded = flateDecode.Encode(file.Content, PdfFlateEncodeMode.BestCompression);

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

        if (file.Description != null)
        {
            fileSpecificationDictionary.Elements.Add("/Desc", new PdfString(file.Description));
        }

        PdfArray? attachedFiles = document.Internals.Catalog.Elements.GetArray("/AF");
        if (attachedFiles == null)
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
            document.Internals.Catalog.Names.Elements.SetReference("/EmbeddedFiles", embeddedFiles.Reference);
        }

        if (embeddedFiles.Elements.GetObject("/Names") is not PdfArray embeddedFileNames)
        {
            embeddedFileNames = new PdfArray(document);
            embeddedFiles.Elements.Add("/Names", embeddedFileNames);
        }

        embeddedFileNames.Elements.Add(new PdfString(file.Name));
        embeddedFileNames.Elements.Add(fileSpecificationDictionary.ReferenceNotNull);
    }

    static string ComputeChecksum(byte[] data)
    {
        byte[] contentHash = MD5.HashData(data);
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

/// <summary>
///     The relationship of a file to a PDF document.
/// </summary>
/// <remarks>
///     See Section 7.11.3 of ISO 32000-2:2020 (PDF 2.0). <br />
///     https://pdfa.org/resource/iso-32000-2/
/// </remarks>
public enum AfRelationship
{
    /// <summary>
    ///     Shall be used when the relationship is not known or cannot be described using one of the other values.
    /// </summary>
    Unspecified = 0,

    /// <summary>
    ///     Shall be used if this file specification is the original source material for the associated content.
    /// </summary>
    Source,

    /// <summary>
    ///     Shall be used if this file specification represents information used to derive a visual presentation – such as for a table or a graph.
    /// </summary>
    Data,

    /// <summary>
    ///     Shall be used if this file specification is an alternative representation of content, for example audio.
    /// </summary>
    Alternative,

    /// <summary>
    ///     Shall be used if this file specification represents a supplemental representation of the original source or data that may be more easily consumable (e.g., A MathML version of
    ///     an equation).
    /// </summary>
    Supplement,

    /// <summary>
    ///     Shall be used if this file specification is an encrypted payload document that should be displayed to the user if the PDF processor has the cryptographic filter needed to
    ///     decrypt the document.
    /// </summary>
    EncryptedPayload,

    /// <summary>
    ///     Shall be used if this file specification is the data associated with the AcroForm (see 12.7.3, "Interactive form dictionary") of this PDF.
    /// </summary>
    FormData,

    /// <summary>
    ///     Shall be used if this file specification is a schema definition for the associated object (e.g. an XML schema associated with a metadata stream).
    /// </summary>
    Schema
}

/// <summary>
///     A file to attach to a PDF document.
/// </summary>
/// <param name="name">The name of the file.</param>
/// <param name="content">The content of the file.</param>
public class PdfAttachmentData(string name, byte[] content)
{
    /// <summary>
    ///     The name of the file.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    ///     The content of the file.
    /// </summary>
    public byte[] Content { get; } = content;

    /// <summary>
    ///     The description of the file.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     The MIME type of the file.
    /// </summary>
    public string? MimeType { get; set; }

    /// <summary>
    ///     The relationship of the file to the PDF document.
    /// </summary>
    public AfRelationship? Relationship { get; set; }

    /// <summary>
    ///     The date of the creation of the file. If not set, the current date is used.
    /// </summary>
    public DateTime? CreationDate { get; set; }

    /// <summary>
    ///     The date of the last modification of the file. It not set, the current date is used.
    /// </summary>
    public DateTime? ModificationDate { get; set; }

    /// <summary>
    ///     Load a file from a stream.
    /// </summary>
    /// <param name="name">The name of the file.</param>
    /// <param name="stream">The stream to load the file from.</param>
    /// <returns>The file.</returns>
    public static PdfAttachmentData LoadFromStream(string name, Stream stream)
    {
        byte[] content = new byte[stream.Length];
        stream.ReadExactly(content);
        return new PdfAttachmentData(name, content);
    }
}
