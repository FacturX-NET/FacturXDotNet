using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Generation.XMP.Internals.Providers;

/// <summary>
///     Encapsulate the generation of a Cross-Industry Invoice data stream.
/// </summary>
interface IXmpDataProvider
{
    XmpMetadata GetXmpMetadata();
}
