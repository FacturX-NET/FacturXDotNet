using FacturXDotNet.Models;
using FacturXDotNet.Validation.Utils;

namespace FacturXDotNet.Validation.BusinessRules;

class BrDec14InvoiceTotalAmountHasTwoDecimals() : FacturXBusinessRule(
    "BR-DEC-14",
    "The allowed maximum number of decimals for the Invoice total amount with VAT (BT-112) is 2.",
    FacturXProfileFlags.Minimum.AndHigher()
)
{
    public override bool Check(FacturXCrossIndustryInvoice invoice) =>
        invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount == null
        || invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount.CountDecimals() <= 2;
}
