using FacturXDotNet.Extensions;
using FacturXDotNet.Generation.PDF;
using FacturXDotNet.Generation.XMP;
using FacturXDotNet.Models.XMP;
using FacturXDotNet.Parsing.XMP;
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
    Action<XmpMetadata>? _postProcessXmpMetadata;
    readonly List<(PdfAttachmentData Name, FacturXDocumentBuilderAttachmentConflictResolution ConflictResolution)> _attachments = [];
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
    ///     This method takes the raw CII data as a stream.
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
    ///     The metadata will be added as-is to the PDF document. It will replace any existing metadata found in the document.
    /// </summary>
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
    ///     Post-process the XMP metadata after it has been added to the PDF document.
    ///     The metadata is either provided as-is using <see cref="WithXmpMetadata" />, or a combination of the existing metadata of the base PDF and the CII data.
    /// </summary>
    /// <param name="postProcess">The action to perform on the XMP metadata.</param>
    /// <returns>The builder itself for chaining.</returns>
    public FacturXDocumentBuilder PostProcessXmpMetadata(Action<XmpMetadata>? postProcess)
    {
        _postProcessXmpMetadata = postProcess;
        return this;
    }

    /// <summary>
    ///     Add an attachment to the Factur-X document.
    /// </summary>
    /// <param name="attachment">The attachment to add.</param>
    /// <param name="conflictResolution">The action to take when an attachment with the same name already exists in the base document.</param>
    /// <returns>The builder itself for chaining.</returns>
    public FacturXDocumentBuilder WithAttachment(
        PdfAttachmentData attachment,
        FacturXDocumentBuilderAttachmentConflictResolution conflictResolution = FacturXDocumentBuilderAttachmentConflictResolution.Overwrite
    )
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
        if (_basePdf is null)
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

        await AddXmpMetadataAsync(pdfDocument);
        _logger?.LogInformation("Added XMP metadata to the PDF document.");


        if (_cii is not null)
        {
            PdfAttachmentData ciiAttachment = PdfAttachmentData.LoadFromStream(ciiAttachmentName, _cii);
            ciiAttachment.Description = "CII XML - FacturX";
            ciiAttachment.Relationship = AfRelationship.Alternative;
            ciiAttachment.MimeType = "application/xml";

            if (!_ciiLeaveOpen)
            {
                await _cii.DisposeAsync();
            }

            AddAttachment(pdfDocument, ciiAttachment, FacturXDocumentBuilderAttachmentConflictResolution.Overwrite);

            _logger?.LogInformation("Added CII attachment to the PDF document.");
        }

        foreach ((PdfAttachmentData attachment, FacturXDocumentBuilderAttachmentConflictResolution conflictResolution) in _attachments)
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

    async Task AddXmpMetadataAsync(PdfDocument pdfDocument)
    {
        Stream xmpStream;
        if (_xmp is null)
        {
            ExtractXmpFromPdf extractor = new();
            xmpStream = extractor.ExtractXmpMetadata(pdfDocument);

            // ensure xmp stream is disposed
            _xmpLeaveOpen = false;
        }
        else
        {
            xmpStream = _xmp;
        }

        XmpMetadataReader xmpReader = new();
        XmpMetadata xmpMetadata = xmpReader.Read(xmpStream);

        _postProcessXmpMetadata?.Invoke(xmpMetadata);

        if (!_xmpLeaveOpen)
        {
            await xmpStream.DisposeAsync();
        }

        await using MemoryStream finalXmpStream = new();
        XmpMetadataWriter xmpWriter = new();
        await xmpWriter.WriteAsync(finalXmpStream, xmpMetadata);

        ReplaceXmpMetadataOfPdfDocument.ReplaceXmpMetadata(pdfDocument, finalXmpStream.GetBuffer().AsSpan());
    }

    void AddAttachment(PdfDocument document, PdfAttachmentData attachment, FacturXDocumentBuilderAttachmentConflictResolution conflictResolution)
    {
        if (document.ListAttachments().Contains(attachment.Name))
        {
            switch (conflictResolution)
            {
                case FacturXDocumentBuilderAttachmentConflictResolution.KeepOld:
                    // nothing to do, we keep the old attachment
                    _logger?.LogWarning("An attachment with the name {AttachmentName} already exists in the PDF document, will keep the old attachment.", attachment.Name);
                    return;
                case FacturXDocumentBuilderAttachmentConflictResolution.Overwrite:
                    // we remove the old attachment and add the new one
                    document.RemoveAttachment(attachment.Name);
                    _logger?.LogWarning("An attachment with the name {AttachmentName} already exists in the PDF document, will overwrite the old attachment.", attachment.Name);
                    break;
                case FacturXDocumentBuilderAttachmentConflictResolution.KeepBoth:
                    // we add the new attachment in addition to the old one
                    _logger?.LogWarning("An attachment with the name {AttachmentName} already exists in the PDF document, will keep both attachments.", attachment.Name);
                    break;
                case FacturXDocumentBuilderAttachmentConflictResolution.Throw:
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

        if (password is not null)
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
