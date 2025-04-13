using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;

namespace FacturXDotNet.Validation.BusinessRules.CII.Br;

/// <summary>
///     BR-1: An Invoice shall have a Specification identifier (BT-24).
/// </summary>
public record Br01InvoiceShallHaveSpecificationIdentifier() : CrossIndustryInvoiceBusinessRule(
    "BR-1",
    "An Invoice shall have a Specification identifier (BT-24).",
    FacturXProfile.Minimum.AndHigher(),
    ["BT-24"]
)
{
    /// <inheritdoc />
    public override bool Check(CrossIndustryInvoice? cii, IBusinessRuleDetailsLogger? logger = null) =>
        cii?.ExchangedDocumentContext?.GuidelineSpecifiedDocumentContextParameterId is not null
        && Enum.IsDefined(cii.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameterId.Value);
}
