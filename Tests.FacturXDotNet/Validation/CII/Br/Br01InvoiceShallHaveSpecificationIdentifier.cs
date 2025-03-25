using FacturXDotNet;
using FacturXDotNet.Models.CII;
using FacturXDotNet.Validation;
using Shouldly;
using Tests.FacturXDotNet.TestTools;

namespace Tests.FacturXDotNet.Validation.CII.Br;

[TestClass]
public class Br01InvoiceShallHaveSpecificationIdentifierTest
{
    [TestMethod]
    public async Task ShouldValidate()
    {
        CrossIndustryInvoice cii = CiiGenerators.CrossIndustryInvoice.Generate();
        cii.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameterId = GuidelineSpecifiedDocumentContextParameterId.En16931;

        FacturXValidator validator = new();
        await result = validator.GetValidationResultAsync(cii);

        result.ShouldBeTrue();
    }

    [TestMethod]
    public async Task ShouldNotValidate_WhenNull()
    {
        FacturXValidator validator = new();
        await result = validator.GetValidationResultAsync(null);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenInvalidProfile()
    {
        CrossIndustryInvoice cii = CiiGenerators.CrossIndustryInvoice.Generate();
        cii.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameterId = (GuidelineSpecifiedDocumentContextParameterId)(-1);

        Br01InvoiceShallHaveSpecificationIdentifier validator = new();
        bool result = validator.Check(cii);

        result.ShouldBeFalse();
    }
}
