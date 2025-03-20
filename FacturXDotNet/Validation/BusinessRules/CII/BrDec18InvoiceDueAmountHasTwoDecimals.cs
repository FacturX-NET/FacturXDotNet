using FacturXDotNet.Models;
using FacturXDotNet.Validation.Utils;

namespace FacturXDotNet.Validation.BusinessRules.CII;

record BrDec18InvoiceDueAmountHasTwoDecimals() : CrossIndustryInvoiceBusinessRule(
    "BR-DEC-18",
    "The allowed maximum number of decimals for the Amount due for payment (BT-115) is 2.",
    FacturXProfile.Minimum.AndHigher()
)
{
    public override bool Check(CrossIndustryInvoice invoice) =>
        invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount == null
        || invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount.CountDecimals() <= 2;
}
