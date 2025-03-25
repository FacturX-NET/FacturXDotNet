using FacturXDotNet;
using FacturXDotNet.Models.XMP;
using FacturXDotNet.Validation.BusinessRules.Hybrid;
using Shouldly;
using Tests.FacturXDotNet.TestTools;

namespace Tests.FacturXDotNet.Validation.Hybrid;

[TestClass]
public class BrHybrid06Test
{
    [TestMethod]
    public void ShouldValidate_WhenValueIsValid()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.FacturX!.DocumentType = XmpFacturXDocumentType.Invoice;

        BrHybrid06 rule = new();
        bool result = rule.Check(xmp, null, null);

        result.ShouldBeTrue();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenXmpIsNull()
    {
        BrHybrid06 rule = new();
        bool result = rule.Check(null, null, null);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenFacturXIsNull()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.FacturX = null;

        BrHybrid06 rule = new();
        bool result = rule.Check(null, null, null);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenValueIsInvalid()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.FacturX!.DocumentType = (XmpFacturXDocumentType)(-1);

        BrHybrid06 rule = new();
        bool result = rule.Check(xmp, null, null);

        result.ShouldBeFalse();
    }
}
