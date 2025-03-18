namespace FacturXDotNet.Models.Validation.BusinessRules;

class Br06InvoiceShallHaveSellerName() : FacturXBusinessRule("BR-06", "An Invoice shall contain the Seller name (BT-27).", FacturXProfileFlags.Minimum.AndHigher())
{
    public override bool Check(FacturXCrossIndustryInvoice invoice) =>
        !string.IsNullOrWhiteSpace(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.Name);
}
