using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;

namespace FacturXDotNet.Validation.BusinessRules.CII.Br;

/// <summary>
///     BR-01: An Invoice shall have a Specification identifier (BT-24).
/// </summary>
public record Br01InvoiceShallHaveSpecificationIdentifier() : CrossIndustryInvoiceBusinessRule(
    "BR-01",
    "An Invoice shall have a Specification identifier (BT-24).",
    FacturXProfile.Minimum.AndHigher()
)
{
    /// <inheritdoc />
    public override bool Check(CrossIndustryInvoice? cii) => cii != null && Enum.IsDefined(cii.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameterId);
}
