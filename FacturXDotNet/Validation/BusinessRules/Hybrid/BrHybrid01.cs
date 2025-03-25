using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

/// <summary>
///     BR-HYBRID-01: A hybrid document consists of a machine readable interchange format on XML syntax and a human readable PDF envelope.
/// </summary>
public record BrHybrid01() : HybridBusinessRule(
    "BR-HYBRID-01",
    "A hybrid document consists of a machine readable interchange format on XML syntax and a human readable PDF envelope.",
    FacturXBusinessRuleSeverity.Information
)
{
    /// <inheritdoc />
    public override bool Check(XmpMetadata? xmp, string? ciiAttachmentName, CrossIndustryInvoice? cii) => xmp is not null;
}
