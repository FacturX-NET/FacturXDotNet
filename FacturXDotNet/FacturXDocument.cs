using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using CommunityToolkit.HighPerformance;
using FacturXDotNet.Generation;
using FacturXDotNet.Parsing.XMP;
using FacturXDotNet.Utils;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace FacturXDotNet;

/// <summary>
///     A Factur-X document.
/// </summary>
public partial class FacturXDocument
{
    /// <summary>
    ///     Create a new Factur-X document.
    /// </summary>
    public FacturXDocument(ReadOnlyMemory<byte> data)
    {
        Data = data;
    }

    /// <summary>
    ///     The raw document.
    /// </summary>
    public ReadOnlyMemory<byte> Data { get; }

    /// <summary>
    ///     Get the XMP metadata of the Factur-X document as a stream.
    /// </summary>
    /// <param name="password">The password to open the PDF document.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The XMP metadata of the Factur-X document as a stream.</returns>
    /// <seealso cref="GetXmpMetadataAsync" />
    public async Task<Stream> GetXmpMetadataStreamAsync(string? password = null, CancellationToken cancellationToken = default)
    {
        using PdfDocument pdfDocument = await OpenPdfDocumentReadOnlyAsync(password, cancellationToken);

        ExtractXmpFromPdf extractor = new();
        return extractor.ExtractXmpMetadata(pdfDocument);
    }

    /// <summary>
    ///     Get the XMP metadata of the Factur-X document as a structured object.
    /// </summary>
    /// <param name="password">The password to open the PDF document.</param>
    /// <param name="xmpParserOptions">The options to parse the XMP metadata.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The XMP metadata of the Factur-X document.</returns>
    /// <seealso cref="GetXmpMetadataStreamAsync" />
    public async Task<XmpMetadata?> GetXmpMetadataAsync(string? password = null, XmpMetadataReaderOptions? xmpParserOptions = null, CancellationToken cancellationToken = default)
    {
        using PdfDocument pdfDocument = await OpenPdfDocumentReadOnlyAsync(password, cancellationToken);

        ExtractXmpFromPdf extractor = new();
        if (!extractor.TryExtractXmpMetadata(pdfDocument, out Stream? xmpStream))
        {
            return null;
        }

        await using Stream _ = xmpStream;
        XmpMetadataReader xmpReader = new(xmpParserOptions);
        return xmpReader.Read(xmpStream);
    }

    /// <summary>
    ///     Get the Cross-Industry Invoice of the Factur-X document.
    /// </summary>
    /// <param name="attachmentFileName">The name of the attachment containing the Cross-Industry Invoice XML file. If not specified, the default name 'factur-x.xml' will be used.</param>
    /// <param name="password">The password to open the PDF document.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The Cross-Industry Invoice of the Factur-X document.</returns>
    public async Task<CrossIndustryInvoiceAttachment?> GetCrossIndustryInvoiceAttachmentAsync(
        string? attachmentFileName = null,
        string? password = null,
        CancellationToken cancellationToken = default
    )
    {
        attachmentFileName ??= "factur-x.xml";
        using PdfDocument pdfDocument = await OpenPdfDocumentReadOnlyAsync(password, cancellationToken);

        if (!pdfDocument.ListAttachments().Contains(attachmentFileName))
        {
            return null;
        }

        return new CrossIndustryInvoiceAttachment(this, attachmentFileName);
    }

    /// <summary>
    ///     Get the attachments of the Factur-X document.
    /// </summary>
    /// <param name="password">The password to open the PDF document.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The attachments of the Factur-X document.</returns>
    public async IAsyncEnumerable<FacturXDocumentAttachment> GetAttachmentsAsync(string? password = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using PdfDocument pdfDocument = await OpenPdfDocumentReadOnlyAsync(password, cancellationToken);

        foreach ((string Name, Stream Content) attachment in pdfDocument.ListAttachments().Select(n => (n, pdfDocument.ExtractAttachment(n))))
        {
            byte[] attachmentBytes = new byte[attachment.Content.Length];
            attachment.Content.ReadExactly(attachmentBytes);
            yield return new FacturXDocumentAttachment(this, attachment.Name);
        }
    }

    /// <summary>
    ///     Export the Factur-X document to a stream.
    /// </summary>
    /// <param name="outputStream">The stream to write the Factur-X document to.</param>
    public async Task ExportAsync(Stream outputStream) => await outputStream.WriteAsync(Data);

    internal async Task<PdfDocument> OpenPdfDocumentReadOnlyAsync(string? password, CancellationToken _ = default)
    {
        await using Stream stream = Data.AsStream();
        PdfDocument document;

        if (password != null)
        {
            document = PdfReader.Open(stream, PdfDocumentOpenMode.Import, args => args.Password = password);
        }
        else
        {
            document = PdfReader.Open(stream, PdfDocumentOpenMode.Import);
        }

        return document;
    }

    /// <summary>
    ///     Create a new Factur-X document builder.
    ///     The builder must be configured with the desired PDF image, Cross-Industry Invoice, XMP metadata.
    /// </summary>
    public static FacturXDocumentBuilder Create() => new();

    /// <summary>
    ///     Create a new Factur-X document from a file.
    /// </summary>
    public static async Task<FacturXDocument> LoadFromFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        byte[] buffer = await File.ReadAllBytesAsync(filePath, cancellationToken);
        return new FacturXDocument(buffer);
    }

    /// <summary>
    ///     Create a new Factur-X document from a stream.
    /// </summary>
    /// <remarks>This method will copy the entire stream. Consider using the <see cref="FacturXDocument(ReadOnlyMemory{byte})" /> constructor if the data is already in memory.</remarks>
    public static async Task<FacturXDocument> LoadFromStream(Stream stream, CancellationToken cancellationToken = default)
    {
        byte[] buffer = new byte[stream.Length];
        await stream.ReadExactlyAsync(buffer, cancellationToken);
        return new FacturXDocument(buffer);
    }

    /// <summary>
    ///     Create a new Factur-X document from a buffer.
    /// </summary>
    public static FacturXDocument LoadFromBuffer(ReadOnlyMemory<byte> buffer) => new(buffer);

    [GeneratedRegex("<\\?xpacket.*?\\?>")]
    private static partial Regex PacketInstructions();
}
