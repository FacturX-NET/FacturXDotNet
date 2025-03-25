using FacturXDotNet;
using FacturXDotNet.Validation.BusinessRules.CII.Br;
using Shouldly;
using Tests.FacturXDotNet.TestTools;

namespace Tests.FacturXDotNet.Validation.CII.Br;

[TestClass]
public class Br14InvoiceShallHaveTotalAmountWithVatTest
{
    [TestMethod]
    public void ShouldValidate_WhenValueIsValid()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement!.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount = 1;

        Br14InvoiceShallHaveTotalAmountWithVat rule = new();
        bool result = rule.Check(cii);

        result.ShouldBeTrue();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenCiiIsNull()
    {
        Br14InvoiceShallHaveTotalAmountWithVat rule = new();
        bool result = rule.Check(null);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenApplicableHeaderTradeSettlementIsNull()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement = null;

        Br14InvoiceShallHaveTotalAmountWithVat rule = new();
        bool result = rule.Check(null);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenValueIsInvalid()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement!.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount = 0;

        Br14InvoiceShallHaveTotalAmountWithVat rule = new();
        bool result = rule.Check(cii);

        result.ShouldBeFalse();
    }
}
