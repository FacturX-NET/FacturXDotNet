using CommunityToolkit.HighPerformance;
using FacturXDotNet.Extensions;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace FacturXDotNet;

/// <summary>
///     A file attached to the Factur-X PDF.
/// </summary>
public class FacturXDocumentAttachment
{
    readonly FacturXDocument _facturX;

    internal FacturXDocumentAttachment(FacturXDocument facturX, string name)
    {
        _facturX = facturX;
        Name = name;
    }

    /// <summary>
    ///     The name of the attachment.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Read the attachment from the Factur-X document to memory.
    /// </summary>
    /// <param name="password">The password to open the PDF document.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The attachment content.</returns>
    /// <exception cref="InvalidOperationException">The attachment with the specified name could not be found.</exception>
    public async Task<ReadOnlyMemory<byte>> ReadAsync(string? password = null, CancellationToken cancellationToken = default)
    {
        await using Stream dataStream = _facturX.Data.AsStream();
        using PdfDocument pdfDocument = dataStream.OpenPdfDocumentAsync(PdfDocumentOpenMode.Import, password);
        await using Stream attachmentStream = await FindAttachmentStreamAsync(password, cancellationToken);

        byte[] attachmentBytes = new byte[attachmentStream.Length];
        attachmentStream.ReadExactly(attachmentBytes);

        return attachmentBytes;
    }

    /// <summary>
    ///     Write the attachment from the Factur-X document to a stream.
    /// </summary>
    /// <param name="outputStream">The stream to write the attachment to.</param>
    /// <param name="password">The password to open the PDF document.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task CopyToAsync(Stream outputStream, string? password = null, CancellationToken cancellationToken = default)
    {
        await using Stream dataStream = _facturX.Data.AsStream();
        using PdfDocument pdfDocument = dataStream.OpenPdfDocumentAsync(PdfDocumentOpenMode.Import, password);
        await using Stream attachmentStream = await FindAttachmentStreamAsync(password, cancellationToken);
        await attachmentStream.CopyToAsync(outputStream, cancellationToken);
    }

    /// <summary>
    ///     Get the stream of the attachment from the Factur-X document.
    /// </summary>
    /// <param name="password">The password to open the PDF document.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The attachment stream.</returns>
    /// <exception cref="InvalidOperationException">The attachment with the specified name could not be found.</exception>
    protected async Task<Stream> FindAttachmentStreamAsync(string? password = null, CancellationToken cancellationToken = default)
    {
        await using Stream dataStream = _facturX.Data.AsStream();
        using PdfDocument pdfDocument = dataStream.OpenPdfDocumentAsync(PdfDocumentOpenMode.Import, password);
        return pdfDocument.ExtractAttachment(Name);
    }
}
