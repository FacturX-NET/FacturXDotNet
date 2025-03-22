using FacturXDotNet.Models;
using FacturXDotNet.Validation.Utils;

namespace FacturXDotNet.Validation.BusinessRules.CII;

record BrDec14InvoiceTotalAmountHasTwoDecimals() : CrossIndustryInvoiceBusinessRule(
    "BR-DEC-14",
    "The allowed maximum number of decimals for the Invoice total amount with VAT (BT-112) is 2.",
    FacturXProfile.Minimum.AndHigher()
)
{
    public override bool Check(CrossIndustryInvoice? cii) =>
        cii?.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount == null
        || cii.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount.CountDecimals() <= 2;
}
