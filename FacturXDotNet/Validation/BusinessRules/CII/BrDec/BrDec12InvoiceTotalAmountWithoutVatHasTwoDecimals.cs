using FacturXDotNet.Models;
using FacturXDotNet.Validation.Utils;

namespace FacturXDotNet.Validation.BusinessRules.CII.BrDec;

record BrDec12InvoiceTotalAmountWithoutVatHasTwoDecimals() : CrossIndustryInvoiceBusinessRule(
    "BR-DEC-12",
    "The allowed maximum number of decimals for the Invoice total amount without VAT (BT-109) is 2.",
    FacturXProfile.Minimum.AndHigher()
)
{
    public override bool Check(CrossIndustryInvoice? cii) =>
        cii?.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount == null
        || cii.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount.CountDecimals() <= 2;
}
