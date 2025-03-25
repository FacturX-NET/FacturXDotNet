using FacturXDotNet.Models.XMP;
using FacturXDotNet.Validation.BusinessRules.Hybrid;
using Shouldly;
using Tests.FacturXDotNet.TestTools;

namespace Tests.FacturXDotNet.Validation.Hybrid;

[TestClass]
public class BrHybrid09Test
{
    [TestMethod]
    public void ShouldValidate_WhenValueIsValid()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.FacturX!.Version = "1.0";

        BrHybrid09 rule = new();
        bool result = rule.Check(xmp, null, null);

        result.ShouldBeTrue();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenXmpIsNull()
    {
        BrHybrid09 rule = new();
        bool result = rule.Check(null, null, null);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenFacturXIsNull()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.FacturX = null;

        BrHybrid09 rule = new();
        bool result = rule.Check(null, null, null);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenValueIsInvalid()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.FacturX!.Version = "0.0";

        BrHybrid09 rule = new();
        bool result = rule.Check(xmp, null, null);

        result.ShouldBeFalse();
    }
}
