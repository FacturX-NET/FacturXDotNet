using FacturXDotNet.Models;
using FacturXDotNet.Validation.CII.Utils;

namespace FacturXDotNet.Validation.CII.BusinessRules;

record BrDec14InvoiceTotalAmountHasTwoDecimals() : FacturXBusinessRule(
    "BR-DEC-14",
    "The allowed maximum number of decimals for the Invoice total amount with VAT (BT-112) is 2.",
    FacturXProfile.Minimum.AndHigher()
)
{
    public override bool Check(CrossIndustryInvoice invoice) =>
        invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount == null
        || invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount.CountDecimals() <= 2;
}
