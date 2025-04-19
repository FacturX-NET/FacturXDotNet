using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;

namespace FacturXDotNet.Validation.BusinessRules.CII.Br;

/// <summary>
///     BR-2: An Invoice shall have an Invoice number (BT-1).
/// </summary>
public record Br02InvoiceShallHaveInvoiceNumber() : CrossIndustryInvoiceBusinessRule(
    "BR-2",
    "An Invoice shall have an Invoice number (BT-1).",
    FacturXProfile.Minimum.AndHigher(),
    ["BT-1"]
)
{
    /// <inheritdoc />
    public override bool Check(CrossIndustryInvoice? cii, IBusinessRuleDetailsLogger? logger = null) => !string.IsNullOrWhiteSpace(cii?.ExchangedDocument?.Id);
}
