using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.BusinessRules;

class Br15InvoiceShallHaveAmountDueForPayment() : FacturXBusinessRule(
    "BR-15",
    "An Invoice shall have the Amount due for payment (BT- 115).",
    FacturXProfileFlags.Minimum.AndHigher()
)
{
    public override bool Check(FacturXCrossIndustryInvoice invoice) =>
        invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement != null
        && invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount != 0;
}
