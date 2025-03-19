using FacturXDotNet.Validation.CII.Utils;
using FacturXDotNet.Validation.Common;

namespace FacturXDotNet.Validation.CII.BusinessRules;

class BrDec18InvoiceDueAmountHasTwoDecimals() : FacturXBusinessRule(
    "BR-DEC-18",
    "The allowed maximum number of decimals for the Amount due for payment (BT-115) is 2.",
    FacturXProfileFlags.Minimum.AndHigher()
)
{
    public override bool Check(CrossIndustryInvoice invoice) =>
        invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount == null
        || invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount.CountDecimals() <= 2;
}
