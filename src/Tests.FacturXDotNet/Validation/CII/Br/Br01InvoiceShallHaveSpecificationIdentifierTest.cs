using FacturXDotNet.Models.CII;
using FacturXDotNet.Validation.BusinessRules.CII.Br;
using FluentAssertions;
using Tests.FacturXDotNet.TestTools;

namespace Tests.FacturXDotNet.Validation.CII.Br;

[TestClass]
public class Br01InvoiceShallHaveSpecificationIdentifierTest
{
    [TestMethod]
    public void ShouldValidate_WhenValueIsValid()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.ExchangedDocumentContext!.GuidelineSpecifiedDocumentContextParameterId = GuidelineSpecifiedDocumentContextParameterId.En16931;

        Br01InvoiceShallHaveSpecificationIdentifier rule = new();
        bool result = rule.Check(cii);

        result.Should().BeTrue();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenCiiIsNull()
    {
        Br01InvoiceShallHaveSpecificationIdentifier rule = new();
        bool result = rule.Check(null);

        result.Should().BeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenValueIsInvalid()
    {
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.ExchangedDocumentContext!.GuidelineSpecifiedDocumentContextParameterId = (GuidelineSpecifiedDocumentContextParameterId)(-1);

        Br01InvoiceShallHaveSpecificationIdentifier rule = new();
        bool result = rule.Check(cii);

        result.Should().BeFalse();
    }
}
