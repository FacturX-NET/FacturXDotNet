using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

/// <summary>
///     BR-HYBRID-7: The fx:ConformanceLevel in the XMP instance SHALL be a value from the HybridConformanceType code list.
/// </summary>
public record BrHybrid07() : HybridBusinessRule("BR-HYBRID-7", "The fx:ConformanceLevel in the XMP instance SHALL be a value from the HybridConformanceType code list.")
{
    /// <inheritdoc />
    public override bool Check(XmpMetadata? xmp, string? ciiAttachmentName, CrossIndustryInvoice? cii, IBusinessRuleDetailsLogger? logger = null) =>
        xmp?.FacturX?.ConformanceLevel is not null && Enum.IsDefined(xmp.FacturX.ConformanceLevel.Value);
}
