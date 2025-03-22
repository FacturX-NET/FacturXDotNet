using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using CommunityToolkit.HighPerformance;
using FacturXDotNet.Parsing;
using FacturXDotNet.Parsing.XMP;
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
    ///     Get the XMP metadata of the Factur-X document.
    /// </summary>
    /// <param name="password">The password to open the PDF document.</param>
    /// <param name="xmpParserOptions">The options to parse the XMP metadata.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The XMP metadata of the Factur-X document.</returns>
    public async Task<XmpMetadata?> GetXmpMetadataAsync(string? password = null, XmpMetadataParserOptions? xmpParserOptions = null, CancellationToken cancellationToken = default)
    {
        using PdfDocument pdfDocument = await OpenPdfDocumentAsync(password, cancellationToken);

        ExtractXmpFromFacturX extractor = new();
        if (!extractor.TryExtractXmpMetadata(pdfDocument, out Stream? xmpStream))
        {
            return null;
        }

        await using Stream _ = xmpStream;

        // TODO: avoid these two extra copies, it is only required because TurboXML doesn't support the <?xpacket...?> processing instructions
        // an issue has been opened to address this: https://github.com/xoofx/TurboXml/issues/6
        // I need to fix this in the library, but it will take some time

        using StreamReader reader = new(xmpStream);
        string content = await reader.ReadToEndAsync(cancellationToken);
        string transformedContent = PacketInstructions().Replace(content, string.Empty);

        await using MemoryStream transformedStream = new(transformedContent.Length + 54);
        await using StreamWriter writer = new(transformedStream);
        await writer.WriteAsync("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>");
        await writer.WriteAsync(transformedContent);
        await writer.FlushAsync(cancellationToken);
        transformedStream.Seek(0, SeekOrigin.Begin);

        XmpMetadataParser xmpParser = new(xmpParserOptions);
        return xmpParser.ParseXmpMetadata(transformedStream);
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
        using PdfDocument pdfDocument = await OpenPdfDocumentAsync(password, cancellationToken);

        ExtractAttachmentsFromFacturX extractor = new();
        foreach ((string Name, Stream Content) attachment in extractor.ExtractFacturXAttachments(pdfDocument))
        {
            if (attachment.Name != attachmentFileName)
            {
                continue;
            }

            return new CrossIndustryInvoiceAttachment(this, attachmentFileName);
        }

        return null;
    }

    /// <summary>
    ///     Get the attachments of the Factur-X document.
    /// </summary>
    /// <param name="password">The password to open the PDF document.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The attachments of the Factur-X document.</returns>
    public async IAsyncEnumerable<FacturXDocumentAttachment> GetAttachmentsAsync(string? password = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using PdfDocument pdfDocument = await OpenPdfDocumentAsync(password, cancellationToken);

        ExtractAttachmentsFromFacturX extractor = new();
        foreach ((string Name, Stream Content) attachment in extractor.ExtractFacturXAttachments(pdfDocument))
        {
            byte[] attachmentBytes = new byte[attachment.Content.Length];
            attachment.Content.ReadExactly(attachmentBytes);
            yield return new FacturXDocumentAttachment(this, attachment.Name);
        }
    }

    internal async Task<PdfDocument> OpenPdfDocumentAsync(string? password, CancellationToken _ = default)
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
    ///     Create a new Factur-X document from a file.
    /// </summary>
    public static async Task<FacturXDocument> FromFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        byte[] buffer = await File.ReadAllBytesAsync(filePath, cancellationToken);
        return new FacturXDocument(buffer);
    }

    /// <summary>
    ///     Create a new Factur-X document from a stream.
    /// </summary>
    /// <remarks>This method will copy the entire stream. Consider using the <see cref="FacturXDocument(ReadOnlyMemory{byte})" /> constructor if the data is already in memory.</remarks>
    public static async Task<FacturXDocument> FromStream(Stream stream, CancellationToken cancellationToken = default)
    {
        byte[] buffer = new byte[stream.Length];
        await stream.ReadExactlyAsync(buffer, cancellationToken);
        return new FacturXDocument(buffer);
    }

    [GeneratedRegex("<\\?xpacket.*?\\?>")]
    private static partial Regex PacketInstructions();
}
