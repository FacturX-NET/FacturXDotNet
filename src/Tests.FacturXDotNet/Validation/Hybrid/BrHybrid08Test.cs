using FacturXDotNet.Models.XMP;
using FacturXDotNet.Validation.BusinessRules.Hybrid;
using FluentAssertions;
using Tests.FacturXDotNet.TestTools;

namespace Tests.FacturXDotNet.Validation.Hybrid;

[TestClass]
public class BrHybrid08Test
{
    [TestMethod]
    public void ShouldValidate_WhenFileNameIsFacturX()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.FacturX!.DocumentFileName = "factur-x.xml";

        BrHybrid08 rule = new();
        bool result = rule.Check(xmp, null, null);

        result.Should().BeTrue();
    }

    [TestMethod]
    public void ShouldValidate_WhenFileNameIsXrechnung()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.FacturX!.DocumentFileName = "xrechnung.xml";

        BrHybrid08 rule = new();
        bool result = rule.Check(xmp, null, null);

        result.Should().BeTrue();
    }

    [TestMethod]
    public void ShouldValidate_WhenFileNameIsOrderX()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.FacturX!.DocumentFileName = "order-x.xml";

        BrHybrid08 rule = new();
        bool result = rule.Check(xmp, null, null);

        result.Should().BeTrue();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenValueIsInvalid()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.FacturX!.DocumentFileName = "invalid";

        BrHybrid08 rule = new();
        bool result = rule.Check(xmp, null, null);

        result.Should().BeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenXmpIsNull()
    {
        BrHybrid08 rule = new();
        bool result = rule.Check(null, null, null);

        result.Should().BeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenFacturXIsNull()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.FacturX = null;

        BrHybrid08 rule = new();
        bool result = rule.Check(null, null, null);

        result.Should().BeFalse();
    }
}
