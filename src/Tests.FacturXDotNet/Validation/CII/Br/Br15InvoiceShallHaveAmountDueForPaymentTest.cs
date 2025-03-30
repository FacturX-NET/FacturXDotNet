using FacturXDotNet.Models.CII;
using FacturXDotNet.Validation.BusinessRules.CII.Br;
using FluentAssertions;
using Tests.FacturXDotNet.TestTools;

namespace Tests.FacturXDotNet.Validation.CII.Br;

[TestClass]
public class Br15InvoiceShallHaveAmountDueForPaymentTest
{
    [TestMethod]
    public void ShouldValidate_WhenValueIsValid()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.SupplyChainTradeTransaction!.ApplicableHeaderTradeSettlement!.SpecifiedTradeSettlementHeaderMonetarySummation!.DuePayableAmount = 100;

        Br15InvoiceShallHaveAmountDueForPayment rule = new();
        bool result = rule.Check(cii);

        result.Should().BeTrue();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenCiiIsNull()
    {
        Br15InvoiceShallHaveAmountDueForPayment rule = new();
        bool result = rule.Check(null);

        result.Should().BeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenApplicableHeaderTradeSettlementIsNull()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.SupplyChainTradeTransaction!.ApplicableHeaderTradeSettlement = null;

        Br15InvoiceShallHaveAmountDueForPayment rule = new();
        bool result = rule.Check(null);

        result.Should().BeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenValueIsInvalid()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.SupplyChainTradeTransaction!.ApplicableHeaderTradeSettlement!.SpecifiedTradeSettlementHeaderMonetarySummation!.DuePayableAmount = 0;

        Br15InvoiceShallHaveAmountDueForPayment rule = new();
        bool result = rule.Check(cii);

        result.Should().BeFalse();
    }
}
