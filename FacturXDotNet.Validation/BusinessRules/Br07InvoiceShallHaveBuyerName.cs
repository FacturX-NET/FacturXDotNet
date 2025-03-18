using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.BusinessRules;

class Br07InvoiceShallHaveBuyerName() : FacturXBusinessRule("BR-07", "An Invoice shall contain the Buyer name (BT-44).", FacturXProfileFlags.Minimum.AndHigher())
{
    public override bool Check(FacturXCrossIndustryInvoice invoice) =>
        !string.IsNullOrWhiteSpace(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name);
}
