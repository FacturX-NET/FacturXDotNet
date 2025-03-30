using FacturXDotNet.Models.CII;
using FacturXDotNet.Validation.BusinessRules.CII.BrCo;
using FluentAssertions;
using Tests.FacturXDotNet.TestTools;

namespace Tests.FacturXDotNet.Validation.CII.BrCo;

[TestClass]
public class BrCo09Test
{
    [TestMethod]
    public void ShouldValidate_WhenValueIsValid()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.SupplyChainTradeTransaction!.ApplicableHeaderTradeAgreement!.SellerTradeParty!.SpecifiedTaxRegistration =
            new SellerTradePartySpecifiedTaxRegistration { Id = "FR123456789" };

        BrCo09 rule = new();
        bool result = rule.Check(cii);

        result.Should().BeTrue();
    }

    [TestMethod]
    public void ShouldValidate_WhenValueIsEl()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.SupplyChainTradeTransaction!.ApplicableHeaderTradeAgreement!.SellerTradeParty!.SpecifiedTaxRegistration =
            new SellerTradePartySpecifiedTaxRegistration { Id = "EL123456789" };

        BrCo09 rule = new();
        bool result = rule.Check(cii);

        result.Should().BeTrue();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenCiiIsNull()
    {
        BrCo09 rule = new();
        bool result = rule.Check(null);

        result.Should().BeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenSpecifiedTaxRegistrationIsNull()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.SupplyChainTradeTransaction!.ApplicableHeaderTradeAgreement!.SellerTradeParty!.SpecifiedTaxRegistration = null;

        BrCo09 rule = new();
        bool result = rule.Check(null);

        result.Should().BeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenValueIsInvalid()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.SupplyChainTradeTransaction!.ApplicableHeaderTradeAgreement!.SellerTradeParty!.SpecifiedTaxRegistration =
            new SellerTradePartySpecifiedTaxRegistration { Id = "123456789" };

        BrCo09 rule = new();
        bool result = rule.Check(cii);

        result.Should().BeFalse();
    }
}
