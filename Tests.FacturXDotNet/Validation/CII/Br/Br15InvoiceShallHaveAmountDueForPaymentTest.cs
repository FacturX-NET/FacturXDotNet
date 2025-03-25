using FacturXDotNet;
using FacturXDotNet.Validation.BusinessRules.CII.Br;
using Shouldly;
using Tests.FacturXDotNet.TestTools;

namespace Tests.FacturXDotNet.Validation.CII.Br;

[TestClass]
public class Br15InvoiceShallHaveAmountDueForPaymentTest
{
    [TestMethod]
    public void ShouldValidate_WhenValueIsValid()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement!.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount = 100;

        Br15InvoiceShallHaveAmountDueForPayment rule = new();
        bool result = rule.Check(cii);

        result.ShouldBeTrue();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenCiiIsNull()
    {
        Br15InvoiceShallHaveAmountDueForPayment rule = new();
        bool result = rule.Check(null);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenApplicableHeaderTradeSettlementIsNull()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement = null;

        Br15InvoiceShallHaveAmountDueForPayment rule = new();
        bool result = rule.Check(null);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenValueIsInvalid()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement!.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount = 0;

        Br15InvoiceShallHaveAmountDueForPayment rule = new();
        bool result = rule.Check(cii);

        result.ShouldBeFalse();
    }
}
