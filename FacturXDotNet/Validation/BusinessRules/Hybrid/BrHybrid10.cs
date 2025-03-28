using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

/// <summary>
///     BR-HYBRID-10: The fx:Version SHOULD be 1.0.
/// </summary>
public record BrHybrid10() : HybridBusinessRule("BR-HYBRID-10", "The fx:Version SHOULD be 1.0.", BusinessRuleSeverity.Warning)
{
    /// <inheritdoc />
    public override bool Check(XmpMetadata? xmp, string? ciiAttachmentName, CrossIndustryInvoice? cii, IBusinessRuleDetailsLogger? logger = null) =>
        xmp?.FacturX is { Version: "1.0" };
}
