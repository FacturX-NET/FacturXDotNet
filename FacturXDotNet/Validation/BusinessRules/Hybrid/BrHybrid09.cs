using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

/// <summary>
///     BR-HYBRID-09: The fx:Version in the XMP instance SHALL be a value defined in the HybridDocumentVersion codelist.
/// </summary>
public record BrHybrid09() : HybridBusinessRule("BR-HYBRID-09", "The fx:Version in the XMP instance SHALL be a value defined in the HybridDocumentVersion codelist.")
{
    /// <inheritdoc />
    public override bool Check(XmpMetadata? xmp, string? ciiAttachmentName, CrossIndustryInvoice? cii) => xmp?.FacturX is { Version: "1.0" or "1p0" or "2p0" or "2p1" or "2p2" };
}
