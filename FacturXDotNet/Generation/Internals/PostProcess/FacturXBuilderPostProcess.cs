using FacturXDotNet.Generation.XMP;
using FacturXDotNet.Models.XMP;
using PdfSharp.Pdf;

namespace FacturXDotNet.Generation.Internals.PostProcess;

/// <summary>
///     Builder for post-processing a Factur-X document.
/// </summary>
public class FacturXBuilderPostProcess
{
    Action<PdfDocument>? _configurePdfDocument;
    Action<FullyDefinedXmpMetadata>? _configureXmp;

    /// <summary>
    ///     Configures the PDF document.
    /// </summary>
    /// <param name="configure">The action to configure the PDF document.</param>
    /// <returns>The builder itself for chaining.</returns>
    public FacturXBuilderPostProcess PdfDocument(Action<PdfDocument> configure)
    {
        _configurePdfDocument = configure;
        return this;
    }

    /// <summary>
    ///     Configures the XMP metadata for the document.
    /// </summary>
    /// <param name="configure">The action to configure the XMP metadata.</param>
    /// <returns>The builder itself for chaining.</returns>
    public FacturXBuilderPostProcess XmpMetadata(Action<FullyDefinedXmpMetadata> configure)
    {
        _configureXmp = configure;
        return this;
    }

    internal void ConfigurePdfDocument(PdfDocument doc) => _configurePdfDocument?.Invoke(doc);
    internal void ConfigureXmpMetadata(XmpMetadata xmp) => _configureXmp?.Invoke(new FullyDefinedXmpMetadata(xmp));
}
