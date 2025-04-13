using FacturXDotNet.Generation.CII.Internals.Providers;
using FacturXDotNet.Generation.PDF;
using FacturXDotNet.Generation.XMP.Internals.Providers;
using Microsoft.Extensions.Logging;

namespace FacturXDotNet.Generation.FacturX.Internals;

/// <summary>
///     Represents the arguments required to build a FacturX document.
/// </summary>
class FacturXDocumentBuildArgs
{
    /// <summary>
    ///     The PDF generator responsible for creating PDF documents from provided data and settings.
    ///     This is an implementation of the IPdfGenerator interface, which defines methods for building
    ///     PDF documents based on structured invoice information.
    /// </summary>
    public IPdfGenerator? PdfGenerator { get; set; }

    /// <summary>
    ///     The provider responsible for generating Cross-Industry Invoice (CII) data streams used in the construction
    ///     of FacturX documents. This property is an implementation of the ICrossIndustryInvoiceDataProvider interface,
    ///     which provides methods to retrieve structured invoice data in CII format.
    /// </summary>
    public ICrossIndustryInvoiceDataProvider? CiiProvider { get; set; }

    /// <summary>
    ///     The name of the attachment for the Cross-Industry Invoice (CII) file in the FacturX document.
    ///     This specifies the filename of the embedded CII XML attachment within the PDF document.
    /// </summary>
    public string CiiAttachmentName { get; set; } = "factur-x.xml";

    /// <summary>
    ///     The XMP provider responsible for generating and supplying XMP metadata
    ///     for a FacturX document. This is an implementation of the IXmpDataProvider
    ///     interface, which defines methods for retrieving XMP metadata and its stream representation.
    /// </summary>
    public IXmpDataProvider? XmpProvider { get; set; }

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
