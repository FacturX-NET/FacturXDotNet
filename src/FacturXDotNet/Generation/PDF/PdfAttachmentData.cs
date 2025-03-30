namespace FacturXDotNet.Generation.PDF;

/// <summary>
///     A file to attach to a PDF document.
/// </summary>
/// <param name="name">The name of the file.</param>
/// <param name="content">The content of the file.</param>
public class PdfAttachmentData(string name, ReadOnlyMemory<byte> content)
{
    /// <summary>
    ///     The name of the file.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    ///     The content of the file.
    /// </summary>
    public ReadOnlyMemory<byte> Content { get; } = content;

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
