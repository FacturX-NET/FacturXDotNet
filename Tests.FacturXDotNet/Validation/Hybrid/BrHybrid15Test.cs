using FacturXDotNet;
using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;
using FacturXDotNet.Validation.BusinessRules.Hybrid;
using Shouldly;
using Tests.FacturXDotNet.TestTools;

namespace Tests.FacturXDotNet.Validation.Hybrid;

[TestClass]
public class BrHybrid15Test
{
    [TestMethod]
    public void ShouldValidate_WhenValueIsValid()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameterId = GuidelineSpecifiedDocumentContextParameterId.Extended;
        xmp.FacturX!.ConformanceLevel = XmpFacturXConformanceLevel.Extended;

        BrHybrid15 rule = new();
        bool result = rule.Check(xmp, null, cii);

        result.ShouldBeTrue();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenXmpIsNull()
    {
        BrHybrid15 rule = new();
        bool result = rule.Check(null, null, null);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenCiiIsNull()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;

        BrHybrid15 rule = new();
        bool result = rule.Check(xmp, null, null);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenConformanceLevelIsNull()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameterId = GuidelineSpecifiedDocumentContextParameterId.Extended;
        xmp.FacturX!.ConformanceLevel = null;

        BrHybrid15 rule = new();
        bool result = rule.Check(xmp, null, cii);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenBothAreNull()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.FacturX!.ConformanceLevel = null;

        BrHybrid15 rule = new();
        bool result = rule.Check(xmp, null, null);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenValueIsInvalid()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        CrossIndustryInvoice cii = FakeData.CrossIndustryInvoice;
        cii.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameterId = GuidelineSpecifiedDocumentContextParameterId.Extended;
        xmp.FacturX!.ConformanceLevel = XmpFacturXConformanceLevel.Basic;

        BrHybrid15 rule = new();
        bool result = rule.Check(xmp, null, cii);

        result.ShouldBeFalse();
    }
}
