using FacturXDotNet.Models;
using FacturXDotNet.Validation.Utils;

namespace FacturXDotNet.Validation.BusinessRules;

class BrDec13InvoiceTotalVatAmountHasTwoDecimals() : FacturXBusinessRule(
    "BR-DEC-13",
    "The allowed maximum number of decimals for the Invoice total VAT amount (BT-110) is 2.",
    FacturXProfileFlags.Minimum.AndHigher()
)
{
    public override bool Check(FacturXCrossIndustryInvoice invoice) =>
        invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount == null
        || invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.Value.CountDecimals() <= 2;
}
