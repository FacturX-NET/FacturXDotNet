using FacturXDotNet.Models.CII;
using FacturXDotNet.Validation.BusinessRules.CII.BrCo;
using Shouldly;
using Tests.FacturXDotNet.TestTools;

namespace Tests.FacturXDotNet.Validation.CII.BrCo;

[TestClass]
public class BrCo26Test
{
    [TestMethod]
    public void ShouldValidate_WhenValueIsValid()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization!.Id = "SELLER_ID";
        cii.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration!.Id = "SELLER_VAT";

        BrCo26 rule = new();
        bool result = rule.Check(cii);

        result.ShouldBeTrue();
    }

    [TestMethod]
    public void ShouldValidate_WhenOnlySpecifiedLegalOrganization()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization!.Id = "SELLER_ID";
        cii.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration = null;

        BrCo26 rule = new();
        bool result = rule.Check(cii);

        result.ShouldBeTrue();
    }

    [TestMethod]
    public void ShouldValidate_WhenOnlySpecifiedTaxRegistration()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization = null;
        cii.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration!.Id = "SELLER_VAT";

        BrCo26 rule = new();
        bool result = rule.Check(cii);

        result.ShouldBeTrue();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenCiiIsNull()
    {
        BrCo26 rule = new();
        bool result = rule.Check(null);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenValueIsInvalid()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization!.Id = "";
        cii.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration!.Id = "";

        BrCo26 rule = new();
        bool result = rule.Check(cii);

        result.ShouldBeFalse();
    }
}
