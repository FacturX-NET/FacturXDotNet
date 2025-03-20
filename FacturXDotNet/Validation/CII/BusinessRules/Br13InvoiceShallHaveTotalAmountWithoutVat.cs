using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.CII.BusinessRules;

record Br13InvoiceShallHaveTotalAmountWithoutVat() : FacturXBusinessRule(
    "BR-13",
    "An Invoice shall have the Invoice total amount without VAT (BT-109).",
    FacturXProfile.Minimum.AndHigher()
)
{
    public override bool Check(CrossIndustryInvoice invoice) =>
        invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement != null
        && invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount != 0;
}
