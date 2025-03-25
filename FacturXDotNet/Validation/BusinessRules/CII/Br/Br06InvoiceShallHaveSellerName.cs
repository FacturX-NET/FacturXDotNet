using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.BusinessRules.CII.Br;

record Br06InvoiceShallHaveSellerName() : CrossIndustryInvoiceBusinessRule("BR-06", "An Invoice shall contain the Seller name (BT-27).", FacturXProfile.Minimum.AndHigher())
{
    public override bool Check(CrossIndustryInvoice? cii) => !string.IsNullOrWhiteSpace(cii?.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.Name);
}
