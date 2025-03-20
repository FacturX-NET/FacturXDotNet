using FacturXDotNet.Models;
using FacturXDotNet.Validation.Utils;

namespace FacturXDotNet.Validation.BusinessRules.CII;

record BrDec13InvoiceTotalVatAmountHasTwoDecimals() : CrossIndustryInvoiceBusinessRule(
    "BR-DEC-13",
    "The allowed maximum number of decimals for the Invoice total VAT amount (BT-110) is 2.",
    FacturXProfile.Minimum.AndHigher()
)
{
    public override bool Check(CrossIndustryInvoice invoice) =>
        invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount == null
        || invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.Value.CountDecimals() <= 2;
}
