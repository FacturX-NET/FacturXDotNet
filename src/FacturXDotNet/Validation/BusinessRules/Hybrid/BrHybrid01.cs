using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

/// <summary>
///     BR-HYBRID-1: A hybrid document consists of a machine readable interchange format on XML syntax and a human readable PDF envelope.
/// </summary>
public record BrHybrid01() : HybridBusinessRule(
    "BR-HYBRID-1",
    "A hybrid document consists of a machine readable interchange format on XML syntax and a human readable PDF envelope.",
    BusinessRuleSeverity.Information
)
{
    /// <inheritdoc />
    public override bool Check(XmpMetadata? xmp, string? ciiAttachmentName, CrossIndustryInvoice? cii, IBusinessRuleDetailsLogger? logger = null) => xmp is not null;
}
