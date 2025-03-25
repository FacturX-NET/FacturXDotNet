using FacturXDotNet;
using FacturXDotNet.Models;
using FacturXDotNet.Validation.BusinessRules;

namespace Tests.FacturXDotNet.Validation.CII.Br;

record Br09InvoiceShallHaveSellerPostalAddressWithCountryCode() : CrossIndustryInvoiceBusinessRule(
    "BR-09",
    "The Seller postal address (BG-5) shall contain a Seller country code (BT-40).",
    FacturXProfile.Minimum.AndHigher()
)
{
    public override bool Check(CrossIndustryInvoice? cii) =>
        !string.IsNullOrWhiteSpace(cii?.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryId);
}
