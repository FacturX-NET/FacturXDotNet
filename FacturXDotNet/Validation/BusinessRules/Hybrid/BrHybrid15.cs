using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

/// <summary>
///     BR-HYBRID-15: The fx:ConformanceLevel SHOULD match the profile of the embedded XML document.
/// </summary>
public record BrHybrid15() : HybridBusinessRule(
    "BR-HYBRID-15",
    "The fx:ConformanceLevel SHOULD match the profile of the embedded XML document.",
    FacturXBusinessRuleSeverity.Warning
)
{
    /// <inheritdoc />
    public override bool Check(XmpMetadata? xmp, string? ciiAttachmentName, CrossIndustryInvoice? cii) =>
        cii != null
        && xmp?.FacturX != null
        && cii?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameterId.ToFacturXProfile() == xmp.FacturX.ConformanceLevel?.ToFacturXProfile();
}
