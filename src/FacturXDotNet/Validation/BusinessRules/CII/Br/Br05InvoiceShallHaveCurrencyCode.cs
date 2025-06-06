using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;

namespace FacturXDotNet.Validation.BusinessRules.CII.Br;

/// <summary>
///     BR-5: An Invoice shall have an Invoice currency code (BT-5).
/// </summary>
public record Br05InvoiceShallHaveCurrencyCode() : CrossIndustryInvoiceBusinessRule(
    "BR-5",
    "An Invoice shall have an Invoice currency code (BT-5).",
    FacturXProfile.Minimum.AndHigher(),
    ["BT-5"]
)
{
    /// <inheritdoc />
    public override bool Check(CrossIndustryInvoice? cii, IBusinessRuleDetailsLogger? logger = null) =>
        !string.IsNullOrWhiteSpace(cii?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode);
}
