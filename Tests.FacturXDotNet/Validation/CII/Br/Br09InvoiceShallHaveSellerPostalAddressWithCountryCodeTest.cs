using FacturXDotNet.Models.CII;
using FacturXDotNet.Validation.BusinessRules.CII.Br;
using Shouldly;
using Tests.FacturXDotNet.TestTools;

namespace Tests.FacturXDotNet.Validation.CII.Br;

[TestClass]
public class Br09InvoiceShallHaveSellerPostalAddressWithCountryCodeTest
{
    [TestMethod]
    public void ShouldValidate_WhenValueIsValid()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.SupplyChainTradeTransaction!.ApplicableHeaderTradeAgreement!.SellerTradeParty!.PostalTradeAddress!.CountryId = "DE";

        Br09InvoiceShallHaveSellerPostalAddressWithCountryCode rule = new();
        bool result = rule.Check(cii);

        result.ShouldBeTrue();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenCiiIsNull()
    {
        Br09InvoiceShallHaveSellerPostalAddressWithCountryCode rule = new();
        bool result = rule.Check(null);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenValueIsInvalid()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.SupplyChainTradeTransaction!.ApplicableHeaderTradeAgreement!.SellerTradeParty!.PostalTradeAddress!.CountryId = string.Empty;

        Br09InvoiceShallHaveSellerPostalAddressWithCountryCode rule = new();
        bool result = rule.Check(cii);

        result.ShouldBeFalse();
    }
}
