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
    ///     Sets the PDF generator to be used for creating the Factur-X document.
    ///     Using this method will replace the currently set PDF generator and cancel any previous calls to
    ///     <see cref="UsePdfGenerator" />, <see cref="WithStandardPdf" /> or <see cref="WithBasePdf" />.
    /// </summary>
    /// <param name="generator">The PDF generator to use.</param>
    /// <returns>The builder itself for chaining.</returns>
    public FacturXDocumentBuilder UsePdfGenerator(IPdfGenerator generator)
    {
        _args.PdfGenerator = generator;
        return this;
    }

    /// <summary>
    ///     Sets the PDF generator of the builder to the standard generator.
    ///     The standard generator is the default generator used when no custom generator is provided.
    /// </summary>
    /// <returns>The builder itself for chaining.</returns>
    public FacturXDocumentBuilder WithStandardPdf()
    {
        UsePdfGenerator(new StandardPdfGenerator());
        return this;
    }

    /// <summary>
    ///     Set the base PDF image for the Factur-X document.
    ///     The builder will edit this PDF image to add the XMP metadata, the Cross Industry Invoice and other attachments, then save it to the output stream.
    /// </summary>
    /// <param name="pdfImageStream">The stream containing the base PDF image.</param>
    /// <param name="configure">
    ///     An optional action to configure the base PDF stream options, such as setting a password or indicating whether the stream should remain open after
    ///     processing.
    /// </param>
    /// <returns>The builder itself for chaining.</returns>
    public FacturXDocumentBuilder WithBasePdf(Stream pdfImageStream, Action<BasePdfStreamOptions>? configure = null)
    {
        BasePdfStreamOptions options = new();
        configure?.Invoke(options);

        UsePdfGenerator(new PdfFromFileGenerator(pdfImageStream, options.Password, options.LeaveOpen));
        return this;
    }

    /// <summary>
    ///     Set the Cross Industry Invoice data for the Factur-X document.
    ///     The data will be added as an attachment to the PDF document.
    /// </summary>
    /// <remarks>
    ///     This method takes the structured CII data.
    /// </remarks>
    /// <param name="cii">The Cross Industry Invoice (CII) data that contains the structured invoice details to be attached to the Factur-X document.</param>
    /// <param name="configure">An optional action to configure additional options for the invoice attachment.</param>
    /// <returns>The builder itself for chaining.</returns>
    public FacturXDocumentBuilder WithCrossIndustryInvoice(CrossIndustryInvoice cii, Action<CrossIndustryInvoiceOptions>? configure = null)
    {
        CrossIndustryInvoiceOptions options = new();
        configure?.Invoke(options);

        _args.CiiProvider = new CrossIndustryInvoiceFromStructuredDataProvider(cii);
        _args.CiiAttachmentName = options.CiiAttachmentName ?? _args.CiiAttachmentName;
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
    /// <param name="configure">An optional action to configure additional options for the invoice attachment.</param>
    /// <returns>The builder itself for chaining.</returns>
    public FacturXDocumentBuilder WithCrossIndustryInvoice(Stream cii, Action<CrossIndustryInvoiceStreamOptions>? configure = null)
    {
        CrossIndustryInvoiceStreamOptions options = new();
        configure?.Invoke(options);

        _args.CiiProvider = new CrossIndustryInvoiceFromStreamDataProvider(cii, options.LeaveOpen);
        _args.CiiAttachmentName = options.CiiAttachmentName ?? _args.CiiAttachmentName;
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
    /// <param name="configure">An optional action to configure additional options for the invoice attachment.</param>
    /// <returns>The builder itself for chaining.</returns>
    public FacturXDocumentBuilder WithXmpMetadata(Stream xmpStream, Action<XmpMetadataStreamOptions>? configure = null)
    {
        XmpMetadataStreamOptions options = new();
        configure?.Invoke(options);

        _args.XmpProvider = new XmpFromStreamDataProvider(xmpStream, options.LeaveOpen);
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
    /// <param name="configure">An optional action to configure additional options for the invoice attachment.</param>
    /// <returns>The builder itself for chaining.</returns>
    public FacturXDocumentBuilder WithAttachment(PdfAttachmentData attachment, Action<AttachmentOptions>? configure = null)
    {
        AttachmentOptions options = new();
        configure?.Invoke(options);

        _args.Attachments.Add((attachment, options.ConflictResolution));
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
