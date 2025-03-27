using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Generation.XMP;

/// <summary>
///     A class that contains all the XMP metadata for a Factur-X document.
/// </summary>
public class FullyDefinedXmpMetadata
{
    readonly XmpMetadata _metadata;

    internal FullyDefinedXmpMetadata(XmpMetadata metadata)
    {
        _metadata = metadata;
    }

    /// <inheritdoc cref="XmpMetadata.PdfAIdentification" />
    public XmpPdfAIdentificationMetadata PdfAIdentification => _metadata.PdfAIdentification!;

    /// <inheritdoc cref="XmpMetadata.Basic" />
    public XmpBasicMetadata Basic => _metadata.Basic!;

    /// <inheritdoc cref="XmpMetadata.Pdf" />
    public XmpPdfMetadata Pdf => _metadata.Pdf!;

    /// <inheritdoc cref="XmpMetadata.DublinCore" />
    public XmpDublinCoreMetadata DublinCore => _metadata.DublinCore!;

    /// <inheritdoc cref="XmpMetadata.PdfAExtensions" />
    public XmpPdfAExtensionsMetadata PdfAExtensions => _metadata.PdfAExtensions!;

    /// <inheritdoc cref="XmpMetadata.FacturX" />
    public XmpFacturXMetadata FacturX => _metadata.FacturX!;
}
