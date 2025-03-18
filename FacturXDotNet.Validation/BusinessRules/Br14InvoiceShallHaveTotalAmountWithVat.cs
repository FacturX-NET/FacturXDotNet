using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.BusinessRules;

class Br14InvoiceShallHaveTotalAmountWithVat() : FacturXBusinessRule(
    "BR-14",
    "An Invoice shall have the Invoice total amount with VAT (BT-112).",
    FacturXProfileFlags.Minimum.AndHigher()
)
{
    public override bool Check(FacturXCrossIndustryInvoice invoice) =>
        invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement != null
        && invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount != 0;
}
