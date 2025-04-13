using FacturXDotNet.Generation.PDF;
using Microsoft.Extensions.Logging;

namespace FacturXDotNet.Generation.FacturX.Internals;

/// <summary>
///     Represents the arguments required to build a FacturX document.
/// </summary>
class FacturXDocumentBuildArgs
{
    /// <summary>
    ///     The base PDF stream used to build the FacturX document. This represents the primary PDF
    ///     document onto which additional metadata and attachments will be added during the document generation process.
    /// </summary>
    public Stream? BasePdf { get; set; }

    /// <summary>
    ///     The password used to decrypt the base PDF stream. This is required when the base PDF is password-protected
    ///     and needs to be accessed during the FacturX document building process.
    /// </summary>
    public string? BasePdfPassword { get; set; }

    /// <summary>
    ///     The property indicating whether the base PDF stream should be left open after processing.
    ///     This controls whether the provided stream remains accessible for further operations or is
    ///     automatically closed after it is read and processed during the FacturX document generation.
    /// </summary>
    public bool BasePdfLeaveOpen { get; set; }

    /// <summary>
    ///     The stream containing the Cross-Industry Invoice (CII) data to be added to the FacturX document.
    ///     This data is used to embed the structured invoice information as a compliant XML attachment.
    ///     This property can be omitted if the base PDF already includes CII data as an attached file.
    /// </summary>
    public Stream? Cii { get; set; }

    /// <summary>
    ///     The name of the attachment for the Cross-Industry Invoice (CII) file in the FacturX document.
    ///     This specifies the filename of the embedded CII XML attachment within the PDF document.
    /// </summary>
    public string CiiAttachmentName { get; set; } = "factur-x.xml";

    /// <summary>
    ///     The property indicating whether the Cross-Industry Invoice data stream should be left open after processing.
    ///     This controls whether the provided stream remains accessible for further operations or is
    ///     automatically closed after it is read and processed during the FacturX document generation.
    /// </summary>
    public bool CiiLeaveOpen { get; set; }

    /// <summary>
    ///     The optional XMP metadata stream to be included in the FacturX document. This stream provides
    ///     additional descriptive information for the PDF, following the XMP (Extensible Metadata Platform) standard.
    ///     The provided metadata will be used in addition to the ones that are generated automatically during the document creation process.
    /// </summary>
    public Stream? Xmp { get; set; }

    /// <summary>
    ///     The property indicating whether the XMP metadata stream should be left open after processing.
    ///     This controls whether the provided stream remains accessible for further operations or is
    ///     automatically closed after it is read and processed during the FacturX document generation.
    /// </summary>
    public bool XmpLeaveOpen { get; set; }

    /// <summary>
    ///     The property that determines whether XMP metadata auto-generation should be disabled.
    ///     If set to true, the builder will not automatically generate or modify the XMP metadata
    ///     during the document creation process, relying instead on manually provided metadata, if any.
    /// </summary>
    public bool DisableXmpMetadataAutoGeneration { get; set; }

    /// <summary>
    ///     The property that determines whether existing output intents in the PDF document
    ///     should be overwritten during the document generation process. If set to true,
    ///     any pre-existing output intents will be removed before adding new ones.
    /// </summary>
    public bool OverwriteOutputIntents { get; set; }

    /// <summary>
    ///     The post-process configuration options for the FacturX document generation process.
    ///     This property allows customization of the final output PDF document by applying
    ///     additional settings or transformations after the main content has been built.
    /// </summary>
    public FacturXDocumentPostProcessOptions PostProcess { get; set; } = new();

    /// <summary>
    ///     The collection of attachments to be added to the generated FacturX document. Each attachment
    ///     is paired with a conflict resolution strategy that specifies how to handle existing attachments
    ///     with the same name during the document generation process.
    /// </summary>
    public List<(PdfAttachmentData Name, FacturXDocumentBuilderAttachmentConflictResolution ConflictResolution)> Attachments { get; set; } = [];

    /// <summary>
    ///     The logger instance used to log informational, warning, or error messages during the execution of
    ///     the FacturX document generation process. This provides a centralized mechanism for capturing runtime events
    ///     and assists in tracing or debugging the workflow.
    /// </summary>
    public ILogger? Logger { get; set; }
}
