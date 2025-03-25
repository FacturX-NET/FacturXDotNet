using FacturXDotNet;
using FacturXDotNet.Models;
using FacturXDotNet.Validation.BusinessRules;

namespace Tests.FacturXDotNet.Validation.CII.Br;

record Br13InvoiceShallHaveTotalAmountWithoutVat() : CrossIndustryInvoiceBusinessRule(
    "BR-13",
    "An Invoice shall have the Invoice total amount without VAT (BT-109).",
    FacturXProfile.Minimum.AndHigher()
)
{
    public override bool Check(CrossIndustryInvoice? cii) =>
        cii?.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement != null
        && cii.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount != 0;
}
