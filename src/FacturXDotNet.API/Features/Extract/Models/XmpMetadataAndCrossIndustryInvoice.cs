using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.API.Features.Extract.Models;

/// <summary>
///     XMP Metadata and Cross-Industry Invoice.
/// </summary>
public class XmpMetadataAndCrossIndustryInvoice
{
    /// <summary>
    ///     The XMP Metadata.
    /// </summary>
    public required XmpMetadata XmpMetadata { get; init; }

    /// <summary>
    ///     The Cross-Industry Invoice.
    /// </summary>
    public required CrossIndustryInvoice CrossIndustryInvoice { get; init; }
}
