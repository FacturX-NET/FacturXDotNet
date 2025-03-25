using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;

namespace FacturXDotNet.Validation.BusinessRules.CII.Br;

/// <summary>
///     BR-15: An Invoice shall have the Amount due for payment (BT- 115).
/// </summary>
public record Br15InvoiceShallHaveAmountDueForPayment() : CrossIndustryInvoiceBusinessRule(
    "BR-15",
    "An Invoice shall have the Amount due for payment (BT- 115).",
    FacturXProfile.Minimum.AndHigher()
)
{
    /// <inheritdoc />
    public override bool Check(CrossIndustryInvoice? cii) =>
        cii?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount is not null
        && cii.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount != 0;
}
