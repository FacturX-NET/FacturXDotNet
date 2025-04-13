using FacturXDotNet.Generation.CII.Internals.Providers;
using FacturXDotNet.Generation.FacturX.Internals;
using FacturXDotNet.Generation.PDF;
using FacturXDotNet.Generation.PDF.Internals.Generators;
using FacturXDotNet.Generation.XMP.Internals.Providers;
using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;
using Microsoft.Extensions.Logging;
using PdfSharp.Pdf;

namespace FacturXDotNet.Generation.FacturX;

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
    ///     Sets the PDF generator of the builder to the standard generator.
    ///     The standard generator is the default generator used when no custom generator is provided.
    /// </summary>
    /// <returns>The builder itself for chaining.</returns>
    public FacturXDocumentBuilder WithStandardPdf()
    {
        _args.PdfGenerator = new StandardPdfGenerator();
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
        _args.PdfGenerator = new PdfFromFileGenerator(pdfImageStream, password, leaveOpen);
        return this;
    }

    /// <summary>
    ///     Set the Cross Industry Invoice data for the Factur-X document.
    ///     The data will be added as an attachment to the PDF document.
    /// </summary>
    /// <remarks>
    ///     This method takes the structured CII data.
    /// </remarks>
    /// <param name="cii">The Cross Industry Invoice data.</param>
    /// <param name="ciiAttachmentName">The name of the attachment.</param>
    /// <returns>The builder itself for chaining.</returns>
    public FacturXDocumentBuilder WithCrossIndustryInvoice(CrossIndustryInvoice cii, string? ciiAttachmentName = null)
    {
        _args.CiiProvider = new CrossIndustryInvoiceFromStructuredDataProvider(cii);
        _args.CiiAttachmentName = ciiAttachmentName ?? _args.CiiAttachmentName;
        return this;
    }

    /// <summary>
    ///     Set the Cross Industry Invoice data for the Factur-X document.
    ///     The data will be added as an attachment to the PDF document.
    /// </summary>
    /// <remarks>
    ///     This method takes the raw CII data as a stream.
    /// </remarks>
    /// <param name="cii">The Cross Industry Invoice data.</param>
    /// <param name="ciiAttachmentName">The name of the attachment.</param>
    /// <param name="leaveOpen">Wheter to leave the stream open after the Factur-X document is built.</param>
    /// <returns>The builder itself for chaining.</returns>
    public FacturXDocumentBuilder WithCrossIndustryInvoice(Stream cii, string? ciiAttachmentName = null, bool leaveOpen = true)
    {
        _args.CiiProvider = new CrossIndustryInvoiceFromStreamDataProvider(cii, leaveOpen);
        _args.CiiAttachmentName = ciiAttachmentName ?? _args.CiiAttachmentName;
        return this;
    }

    /// <summary>
    ///     Set the XMP metadata for the Factur-X document.
    ///     The metadata will be added as-is to the PDF document, it will replace any existing metadata found in the document.
    /// </summary>
    /// <param name="xmp">The XMP metadata.</param>
    /// <returns>The builder itself for chaining.</returns>
    public FacturXDocumentBuilder WithXmpMetadata(XmpMetadata xmp)
    {
        _args.XmpProvider = new XmpFromStructuredDataProvider(xmp);
        _args.DisableXmpMetadataAutoGeneration = true;
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
        _args.XmpProvider = new XmpFromStreamDataProvider(xmpStream, leaveOpen);
        _args.DisableXmpMetadataAutoGeneration = true;
        return this;
    }

    /// <summary>
    ///     Post-process the XMP metadata after it has been added to the PDF document.
    ///     The metadata is either provided as-is using one of the <see cref="WithXmpMetadata(XmpMetadata)" /> overloads,
    ///     or a combination of the existing metadata of the base PDF and the CII data.
    /// </summary>
    /// <param name="postProcess">The action to perform on the XMP metadata.</param>
    /// <returns>The builder itself for chaining.</returns>
    public FacturXDocumentBuilder PostProcess(Action<FacturXDocumentPostProcessOptions> postProcess)
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
        if (_args.CiiProvider == null)
        {
            throw new InvalidOperationException("The CII provider has not been provided.");
        }

        _args.PdfGenerator ??= new StandardPdfGenerator();

        await using MemoryStream resultStream = new();

        CrossIndustryInvoice cii = await _args.CiiProvider.GetCrossIndustryInvoiceAsync();
        using PdfDocument pdfDocument = _args.PdfGenerator.Build(cii);

        await FacturXDocumentBuilderAddCrossIndustryInvoiceAttachmentStep.AttachCiiStreamToPdf(pdfDocument, _args.CiiProvider, _args.CiiAttachmentName, _args.Logger);
        XmpMetadata xmp = await FacturXDocumentBuilderAddXmpMetadataStep.RunAsync(pdfDocument, cii, _args);
        FacturXDocumentBuilderAddAttachmentsStep.Run(pdfDocument, _args);
        await FacturXDocumentBuilderSetOutputIntentsStep.RunAsync(pdfDocument, _args);

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
        pdfDocument.PageMode = PdfPageMode.UseAttachments;
        pdfDocument.ViewerPreferences.DisplayDocTitle = true;

        await pdfDocument.SaveAsync(resultStream);

        return FacturXDocument.LoadFromBuffer(resultStream.GetBuffer().AsMemory(0, (int)resultStream.Length));
    }

    static string? FirstString(IEnumerable<string>? parts) => parts?.FirstOrDefault(s => !string.IsNullOrWhiteSpace(s));

    static string? JoinStrings(IEnumerable<string>? parts, string separator = ", ") =>
        parts == null ? null : string.Join(separator, parts.Where(s => !string.IsNullOrWhiteSpace(s)));
}
