using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;

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
        cii?.ExchangedDocumentContext?.GuidelineSpecifiedDocumentContextParameterId is not null
        && xmp?.FacturX?.ConformanceLevel is not null
        && cii.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameterId.Value.ToFacturXProfile() == xmp.FacturX.ConformanceLevel.Value.ToFacturXProfile();
}
