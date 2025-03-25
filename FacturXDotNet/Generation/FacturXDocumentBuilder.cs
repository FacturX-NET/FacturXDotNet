using FacturXDotNet.Generation.PDF;
using FacturXDotNet.Utils;
using Microsoft.Extensions.Logging;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace FacturXDotNet.Generation;

/// <summary>
/// </summary>
public class FacturXDocumentBuilder
{
    Stream? _basePdf;
    string? _basePdfPassword;
    bool _basePdfLeaveOpen;
    Stream? _cii;
    string? _ciiAttachmentName;
    bool _ciiLeaveOpen;
    Stream? _xmp;
    bool _xmpLeaveOpen;
    readonly List<(PdfAttachmentData Name, PdfAttachmentConflictResolution ConflictResolution)> _attachments = [];
    ILogger? _logger;

    /// <summary>
    ///     Call <see cref="FacturXDocument.Create" /> to instantiate a new builder.
    /// </summary>
    internal FacturXDocumentBuilder() { }

    /// <summary>
    ///     Set the logger that will be used during the call to <see cref="BuildAsync" />.
    /// </summary>
    /// <param name="logger">The logger to use.</param>
    /// <returns>The builder itself for chaining.</returns>
    public FacturXDocumentBuilder SetLogger(ILogger? logger)
    {
        _logger = logger;
        return this;
    }

    /// <summary>
    ///     Set the base PDF image for the Factur-X document.
    ///     The builder will edit this PDF image to add the XMP metadata, the Cross Industry Invoice and other attachments, then save it to the output stream.
    /// </summary>
    /// <param name="pdfImageStream">The stream containing the base PDF image.</param>
    /// <param name="password">The password to open the PDF image.</param>
    /// <param name="leaveOpen">Whether to leave the stream open after the Factur-X document is built.</param>
    /// <returns>The builder itself for chaining.</returns>
    public FacturXDocumentBuilder WithBasePdf(Stream pdfImageStream, string? password = null, bool leaveOpen = true)
    {
        _basePdf = pdfImageStream;
        _basePdfPassword = password;
        _basePdfLeaveOpen = leaveOpen;
        return this;
    }

    /// <summary>
    ///     Set the Cross Industry Invoice data for the Factur-X document.
    ///     The data will be added as an attachment to the PDF document.
    /// </summary>
    /// <remarks>
    ///     This method takes the raw CII data as a stream. It can be used to add data without having to parse it into a <see cref="CrossIndustryInvoice" /> object.
    /// </remarks>
    /// <param name="ciiStream">The stream containing the Cross Industry Invoice data.</param>
    /// <param name="ciiAttachmentName">The name of the attachment.</param>
    /// <param name="leaveOpen">Whether to leave the stream open after the Factur-X document is built.</param>
    /// <returns>The builder itself for chaining.</returns>
    public FacturXDocumentBuilder WithCrossIndustryInvoice(Stream ciiStream, string? ciiAttachmentName = null, bool leaveOpen = true)
    {
        _cii = ciiStream;
        _ciiAttachmentName = ciiAttachmentName;
        _ciiLeaveOpen = leaveOpen;
        return this;
    }

    /// <summary>
    ///     Set the XMP metadata for the Factur-X document.
    /// </summary>
    /// <remarks>
    ///     This method takes the raw XMP metadata as a stream. It can be used to add data without having to parse it into a <see cref="XmpMetadata" /> object.
    /// </remarks>
    /// <param name="xmpStream">The stream containing the XMP metadata.</param>
    /// <param name="leaveOpen">Whether to leave the stream open after the Factur-X document is built.</param>
    /// <returns>The builder itself for chaining.</returns>
    public FacturXDocumentBuilder WithXmpMetadata(Stream xmpStream, bool leaveOpen = true)
    {
        _xmp = xmpStream;
        _xmpLeaveOpen = leaveOpen;
        return this;
    }

    /// <summary>
    ///     Add an attachment to the Factur-X document.
    /// </summary>
    /// <param name="attachment">The attachment to add.</param>
    /// <param name="conflictResolution">The action to take when an attachment with the same name already exists in the base document.</param>
    /// <returns>The builder itself for chaining.</returns>
    public FacturXDocumentBuilder WithAttachment(PdfAttachmentData attachment, PdfAttachmentConflictResolution conflictResolution = PdfAttachmentConflictResolution.Overwrite)
    {
        _attachments.Add((attachment, conflictResolution));
        return this;
    }

    /// <summary>
    ///     Create a new Factur-X document.
    /// </summary>
    /// <returns>The Factur-X document.</returns>
    public async Task<FacturXDocument> BuildAsync()
    {
        if (_basePdf == null)
        {
            throw new InvalidOperationException("A base PDF image must be provided.");
        }

        string ciiAttachmentName = _ciiAttachmentName ?? "factur-x.xml";

        await using MemoryStream resultStream = new();

        using PdfDocument pdfDocument = OpenPdfDocumentAsync(_basePdf, _basePdfPassword);

        if (!_basePdfLeaveOpen)
        {
            await _basePdf.DisposeAsync();
        }

        if (_xmp != null)
        {
            byte[] newMetadataBytes = new byte[(int)_xmp.Length];
            await _xmp.ReadExactlyAsync(newMetadataBytes);

            if (!_xmpLeaveOpen)
            {
                await _xmp.DisposeAsync();
            }

            ReplaceXmpMetadataOfPdfDocument.ReplaceXmpMetadata(pdfDocument, newMetadataBytes);

            _logger?.LogInformation("Added XMP metadata to the PDF document.");
        }

        if (_cii != null)
        {
            PdfAttachmentData ciiAttachment = PdfAttachmentData.LoadFromStream(ciiAttachmentName, _cii);
            ciiAttachment.Description = "CII XML - FacturX";
            ciiAttachment.Relationship = AfRelationship.Alternative;
            ciiAttachment.MimeType = "application/xml";

            if (!_ciiLeaveOpen)
            {
                await _cii.DisposeAsync();
            }

            AddAttachment(pdfDocument, ciiAttachment, PdfAttachmentConflictResolution.Overwrite);

            _logger?.LogInformation("Added CII attachment to the PDF document.");
        }

        foreach ((PdfAttachmentData attachment, PdfAttachmentConflictResolution conflictResolution) in _attachments)
        {
            AddAttachment(pdfDocument, attachment, conflictResolution);
            _logger?.LogInformation("Added attachment {AttachmentName} to the PDF document.", attachment.Name);
        }

        pdfDocument.Options.FlateEncodeMode = PdfFlateEncodeMode.BestCompression;
        pdfDocument.Options.CompressContentStreams = true;
        pdfDocument.Options.NoCompression = false;
        pdfDocument.Options.EnableCcittCompressionForBilevelImages = true;
        pdfDocument.Options.UseFlateDecoderForJpegImages = PdfUseFlateDecoderForJpegImages.Automatic;
        pdfDocument.Options.AutomaticXmpGeneration = false;

        await pdfDocument.SaveAsync(resultStream);

        return FacturXDocument.LoadFromBuffer(resultStream.GetBuffer().AsMemory(0, (int)resultStream.Length));
    }

    void AddAttachment(PdfDocument document, PdfAttachmentData attachment, PdfAttachmentConflictResolution conflictResolution)
    {
        if (document.ListAttachments().Contains(attachment.Name))
        {
            switch (conflictResolution)
            {
                case PdfAttachmentConflictResolution.KeepOld:
                    // nothing to do, we keep the old attachment
                    _logger?.LogWarning("An attachment with the name {AttachmentName} already exists in the PDF document, will keep the old attachment.", attachment.Name);
                    return;
                case PdfAttachmentConflictResolution.Overwrite:
                    // we remove the old attachment and add the new one
                    document.RemoveAttachment(attachment.Name);
                    _logger?.LogWarning("An attachment with the name {AttachmentName} already exists in the PDF document, will overwrite the old attachment.", attachment.Name);
                    break;
                case PdfAttachmentConflictResolution.KeepBoth:
                    // we add the new attachment in addition to the old one
                    _logger?.LogWarning("An attachment with the name {AttachmentName} already exists in the PDF document, will keep both attachments.", attachment.Name);
                    break;
                case PdfAttachmentConflictResolution.Throw:
                    throw new InvalidOperationException($"An attachment with the name {attachment.Name} already exists in the PDF document.");
                default:
                    throw new ArgumentOutOfRangeException(nameof(conflictResolution), conflictResolution, null);
            }
        }

        document.AddAttachment(attachment);
    }

    static PdfDocument OpenPdfDocumentAsync(Stream stream, string? password)
    {
        PdfDocument document;

        if (password != null)
        {
            document = PdfReader.Open(stream, PdfDocumentOpenMode.Modify, args => args.Password = password);
        }
        else
        {
            document = PdfReader.Open(stream, PdfDocumentOpenMode.Modify);
        }

        return document;
    }
}
