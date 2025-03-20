using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.BusinessRules.CII;

record Br07InvoiceShallHaveBuyerName() : CrossIndustryInvoiceBusinessRule("BR-07", "An Invoice shall contain the Buyer name (BT-44).", FacturXProfile.Minimum.AndHigher())
{
    public override bool Check(CrossIndustryInvoice invoice) => !string.IsNullOrWhiteSpace(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name);
}
