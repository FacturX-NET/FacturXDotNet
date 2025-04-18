using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;

namespace FacturXDotNet.Validation.BusinessRules.CII.Br;

/// <summary>
///     BR-7: An Invoice shall contain the Buyer name (BT-44).
/// </summary>
public record Br07InvoiceShallHaveBuyerName() : CrossIndustryInvoiceBusinessRule(
    "BR-7",
    "An Invoice shall contain the Buyer name (BT-44).",
    FacturXProfile.Minimum.AndHigher(),
    ["BT-44"]
)
{
    /// <inheritdoc />
    public override bool Check(CrossIndustryInvoice? cii, IBusinessRuleDetailsLogger? logger = null) =>
        !string.IsNullOrWhiteSpace(cii?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.Name);
}
