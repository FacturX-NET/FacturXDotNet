using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Generation.XMP.Internals.Providers;

/// <summary>
///     Provides functionality to generate a Cross-Industry Invoice data stream from structured data.
/// </summary>
class XmpFromStructuredDataProvider(XmpMetadata xmp) : IXmpDataProvider
{
    public XmpMetadata GetXmpMetadata() => xmp;
}
