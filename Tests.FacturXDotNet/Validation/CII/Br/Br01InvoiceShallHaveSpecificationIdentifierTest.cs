using FacturXDotNet;
using FacturXDotNet.Models.CII;
using FacturXDotNet.Validation.BusinessRules.CII.Br;
using Shouldly;
using Tests.FacturXDotNet.TestTools;

namespace Tests.FacturXDotNet.Validation.CII.Br;

[TestClass]
public class Br01InvoiceShallHaveSpecificationIdentifierTest
{
    [TestMethod]
    public void ShouldValidate_WhenValueIsValid()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameterId = GuidelineSpecifiedDocumentContextParameterId.En16931;

        Br01InvoiceShallHaveSpecificationIdentifier rule = new();
        bool result = rule.Check(cii);

        result.ShouldBeTrue();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenCiiIsNull()
    {
        Br01InvoiceShallHaveSpecificationIdentifier rule = new();
        bool result = rule.Check(null);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenValueIsInvalid()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameterId = (GuidelineSpecifiedDocumentContextParameterId)(-1);

        Br01InvoiceShallHaveSpecificationIdentifier rule = new();
        bool result = rule.Check(cii);

        result.ShouldBeFalse();
    }
}
