using FacturXDotNet;
using FacturXDotNet.Models;
using FacturXDotNet.Validation.BusinessRules;

namespace Tests.FacturXDotNet.Validation.CII.Br;

record Br14InvoiceShallHaveTotalAmountWithVat() : CrossIndustryInvoiceBusinessRule(
    "BR-14",
    "An Invoice shall have the Invoice total amount with VAT (BT-112).",
    FacturXProfile.Minimum.AndHigher()
)
{
    public override bool Check(CrossIndustryInvoice? cii) =>
        cii?.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement != null
        && cii.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount != 0;
}
