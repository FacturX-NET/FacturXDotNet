using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.BusinessRules.CII;

record Br14InvoiceShallHaveTotalAmountWithVat() : CrossIndustryInvoiceBusinessRule(
    "BR-14",
    "An Invoice shall have the Invoice total amount with VAT (BT-112).",
    FacturXProfile.Minimum.AndHigher()
)
{
    public override bool Check(CrossIndustryInvoice invoice) =>
        invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement != null
        && invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount != 0;
}
