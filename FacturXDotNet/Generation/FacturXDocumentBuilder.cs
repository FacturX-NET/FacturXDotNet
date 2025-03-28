using FacturXDotNet.Generation.Internals;
using FacturXDotNet.Generation.Internals.PostProcess;
using FacturXDotNet.Generation.PDF;
using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;
using Microsoft.Extensions.Logging;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace FacturXDotNet.Generation;

/// <summary>
/// </summary>
public class FacturXDocumentBuilder
{
    readonly FacturXDocumentBuildArgs _args = new();

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
        _args.Logger = logger;
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
        _args.BasePdf = pdfImageStream;
        _args.BasePdfPassword = password;
        _args.BasePdfLeaveOpen = leaveOpen;
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
        _args.Cii = ciiStream;
        _args.CiiAttachmentName = ciiAttachmentName ?? _args.CiiAttachmentName;
        _args.CiiLeaveOpen = leaveOpen;
        return this;
    }

    /// <summary>
    ///     Set the XMP metadata for the Factur-X document.
    ///     The metadata will be added as-is to the PDF document, it will replace any existing metadata found in the document.
    /// </summary>
    /// <param name="xmpStream">The stream containing the XMP metadata.</param>
    /// <param name="leaveOpen">Whether to leave the stream open after the Factur-X document is built.</param>
    /// <returns>The builder itself for chaining.</returns>
    public FacturXDocumentBuilder WithXmpMetadata(Stream xmpStream, bool leaveOpen = true)
    {
        _args.Xmp = xmpStream;
        _args.XmpLeaveOpen = leaveOpen;
        _args.DisableXmpMetadataAutoGeneration = true;
        return this;
    }

    /// <summary>
    ///     Post-process the XMP metadata after it has been added to the PDF document.
    ///     The metadata is either provided as-is using <see cref="WithXmpMetadata" />, or a combination of the existing metadata of the base PDF and the CII data.
    /// </summary>
    /// <param name="postProcess">The action to perform on the XMP metadata.</param>
    /// <returns>The builder itself for chaining.</returns>
    public FacturXDocumentBuilder PostProcess(Action<FacturXBuilderPostProcess> postProcess)
    {
        postProcess(_args.PostProcess);
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
        _args.Attachments.Add((attachment, conflictResolution));
        return this;
    }

    /// <summary>
    ///     Create a new Factur-X document.
    /// </summary>
    /// <returns>The Factur-X document.</returns>
    public async Task<FacturXDocument> BuildAsync()
    {
        if (_args.BasePdf is null)
        {
            throw new InvalidOperationException("A base PDF image must be provided.");
        }

        await using MemoryStream resultStream = new();

        using PdfDocument pdfDocument = OpenPdfDocumentAsync(_args.BasePdf, _args.BasePdfPassword);

        if (!_args.BasePdfLeaveOpen)
        {
            await _args.BasePdf.DisposeAsync();
        }

        CrossIndustryInvoice cii = await FacturXBuilderCrossIndustryInvoice.AddCrossIndustryInvoiceAttachmentAsync(pdfDocument, _args);
        XmpMetadata xmp = await FacturXBuilderXmpMetadata.AddXmpMetadataAsync(pdfDocument, cii, _args);
        FacturXBuilderAttachments.AddAttachments(pdfDocument, _args);

        pdfDocument.Info.Title = FirstString(xmp.DublinCore?.Title) ?? pdfDocument.Info.Title;
        pdfDocument.Info.Subject = FirstString(xmp.DublinCore?.Description) ?? pdfDocument.Info.Subject;
        pdfDocument.Info.CreationDate = xmp.Basic?.CreateDate?.LocalDateTime ?? pdfDocument.Info.CreationDate;
        pdfDocument.Info.ModificationDate = xmp.Basic?.ModifyDate?.LocalDateTime ?? pdfDocument.Info.ModificationDate;
        pdfDocument.Info.Keywords = xmp.Pdf?.Keywords ?? pdfDocument.Info.Keywords;
        pdfDocument.Info.Author = JoinStrings(xmp.DublinCore?.Creator) ?? pdfDocument.Info.Author;

        _args.PostProcess.ConfigurePdfDocument(pdfDocument);

        pdfDocument.Info.Creator = xmp.Pdf?.Producer ?? pdfDocument.Info.Creator;
        pdfDocument.Options.FlateEncodeMode = PdfFlateEncodeMode.BestCompression;
        pdfDocument.Options.CompressContentStreams = true;
        pdfDocument.Options.NoCompression = false;
        pdfDocument.Options.EnableCcittCompressionForBilevelImages = true;
        pdfDocument.Options.UseFlateDecoderForJpegImages = PdfUseFlateDecoderForJpegImages.Automatic;
        pdfDocument.Options.AutomaticXmpGeneration = false;

        await pdfDocument.SaveAsync(resultStream);

        return FacturXDocument.LoadFromBuffer(resultStream.GetBuffer().AsMemory(0, (int)resultStream.Length));
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

    static string? FirstString(IEnumerable<string>? parts) => parts?.FirstOrDefault(s => !string.IsNullOrWhiteSpace(s));

    static string? JoinStrings(IEnumerable<string>? parts, string separator = ", ") =>
        parts == null ? null : string.Join(separator, parts.Where(s => !string.IsNullOrWhiteSpace(s)));
}

class FacturXDocumentBuildArgs
{
    public Stream? BasePdf { get; set; }
    public string? BasePdfPassword { get; set; }
    public bool BasePdfLeaveOpen { get; set; }
    public Stream? Cii { get; set; }
    public string CiiAttachmentName { get; set; } = "factur-x.xml";
    public bool CiiLeaveOpen { get; set; }
    public Stream? Xmp { get; set; }
    public bool XmpLeaveOpen { get; set; }
    public bool DisableXmpMetadataAutoGeneration { get; set; }
    public FacturXBuilderPostProcess PostProcess { get; set; } = new();
    public List<(PdfAttachmentData Name, FacturXDocumentBuilderAttachmentConflictResolution ConflictResolution)> Attachments { get; set; } = [];
    public ILogger? Logger { get; set; }
}
