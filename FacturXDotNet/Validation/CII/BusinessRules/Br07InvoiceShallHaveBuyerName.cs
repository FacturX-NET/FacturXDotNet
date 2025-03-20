using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.CII.BusinessRules;

record Br07InvoiceShallHaveBuyerName() : FacturXBusinessRule("BR-07", "An Invoice shall contain the Buyer name (BT-44).", FacturXProfile.Minimum.AndHigher())
{
    public override bool Check(CrossIndustryInvoice invoice) => !string.IsNullOrWhiteSpace(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name);
}
