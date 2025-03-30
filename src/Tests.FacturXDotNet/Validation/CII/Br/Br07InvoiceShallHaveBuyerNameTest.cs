using FacturXDotNet.Models.CII;
using FacturXDotNet.Validation.BusinessRules.CII.Br;
using FluentAssertions;
using Tests.FacturXDotNet.TestTools;

namespace Tests.FacturXDotNet.Validation.CII.Br;

[TestClass]
public class Br07InvoiceShallHaveBuyerNameTest
{
    [TestMethod]
    public void ShouldValidate_WhenValueIsValid()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.SupplyChainTradeTransaction!.ApplicableHeaderTradeAgreement!.BuyerTradeParty!.Name = "Buyer Name";

        Br07InvoiceShallHaveBuyerName rule = new();
        bool result = rule.Check(cii);

        result.Should().BeTrue();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenCiiIsNull()
    {
        Br07InvoiceShallHaveBuyerName rule = new();
        bool result = rule.Check(null);

        result.Should().BeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenValueIsInvalid()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.SupplyChainTradeTransaction!.ApplicableHeaderTradeAgreement!.BuyerTradeParty!.Name = string.Empty;

        Br07InvoiceShallHaveBuyerName rule = new();
        bool result = rule.Check(cii);

        result.Should().BeFalse();
    }
}
