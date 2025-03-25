using FacturXDotNet.Models.CII;
using FacturXDotNet.Validation.BusinessRules.CII.Br;
using Shouldly;
using Tests.FacturXDotNet.TestTools;

namespace Tests.FacturXDotNet.Validation.CII.Br;

[TestClass]
public class Br13InvoiceShallHaveTotalAmountWithoutVatTest
{
    [TestMethod]
    public void ShouldValidate_WhenValueIsValid()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.SupplyChainTradeTransaction!.ApplicableHeaderTradeSettlement!.SpecifiedTradeSettlementHeaderMonetarySummation!.TaxBasisTotalAmount = 1;

        Br13InvoiceShallHaveTotalAmountWithoutVat rule = new();
        bool result = rule.Check(cii);

        result.ShouldBeTrue();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenCiiIsNull()
    {
        Br13InvoiceShallHaveTotalAmountWithoutVat rule = new();
        bool result = rule.Check(null);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenApplicableHeaderTradeSettlementIsNull()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.SupplyChainTradeTransaction!.ApplicableHeaderTradeSettlement = null;

        Br13InvoiceShallHaveTotalAmountWithoutVat rule = new();
        bool result = rule.Check(null);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenValueIsInvalid()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.SupplyChainTradeTransaction!.ApplicableHeaderTradeSettlement!.SpecifiedTradeSettlementHeaderMonetarySummation!.TaxBasisTotalAmount = 0;

        Br13InvoiceShallHaveTotalAmountWithoutVat rule = new();
        bool result = rule.Check(cii);

        result.ShouldBeFalse();
    }
}
