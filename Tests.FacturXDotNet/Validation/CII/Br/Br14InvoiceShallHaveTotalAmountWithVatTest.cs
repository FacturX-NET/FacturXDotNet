using FacturXDotNet.Models.CII;
using FacturXDotNet.Validation.BusinessRules.CII.Br;
using FluentAssertions;
using Tests.FacturXDotNet.TestTools;

namespace Tests.FacturXDotNet.Validation.CII.Br;

[TestClass]
public class Br14InvoiceShallHaveTotalAmountWithVatTest
{
    [TestMethod]
    public void ShouldValidate_WhenValueIsValid()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.SupplyChainTradeTransaction!.ApplicableHeaderTradeSettlement!.SpecifiedTradeSettlementHeaderMonetarySummation!.GrandTotalAmount = 1;

        Br14InvoiceShallHaveTotalAmountWithVat rule = new();
        bool result = rule.Check(cii);

        result.Should().BeTrue();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenCiiIsNull()
    {
        Br14InvoiceShallHaveTotalAmountWithVat rule = new();
        bool result = rule.Check(null);

        result.Should().BeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenApplicableHeaderTradeSettlementIsNull()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.SupplyChainTradeTransaction!.ApplicableHeaderTradeSettlement = null;

        Br14InvoiceShallHaveTotalAmountWithVat rule = new();
        bool result = rule.Check(null);

        result.Should().BeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenValueIsInvalid()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.SupplyChainTradeTransaction!.ApplicableHeaderTradeSettlement!.SpecifiedTradeSettlementHeaderMonetarySummation!.GrandTotalAmount = 0;

        Br14InvoiceShallHaveTotalAmountWithVat rule = new();
        bool result = rule.Check(cii);

        result.Should().BeFalse();
    }
}
