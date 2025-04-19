using FacturXDotNet.Models.XMP;
using FacturXDotNet.Validation.BusinessRules.Hybrid;
using FluentAssertions;
using Tests.FacturXDotNet.TestTools;

namespace Tests.FacturXDotNet.Validation.Hybrid;

[TestClass]
public class BrHybrid10Test
{
    [TestMethod]
    public void ShouldValidate_WhenValueIsValid()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.FacturX!.Version = "1.0";

        BrHybrid10 rule = new();
        bool result = rule.Check(xmp, null, null);

        result.Should().BeTrue();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenXmpIsNull()
    {
        BrHybrid10 rule = new();
        bool result = rule.Check(null, null, null);

        result.Should().BeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenFacturXIsNull()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.FacturX = null;

        BrHybrid10 rule = new();
        bool result = rule.Check(null, null, null);

        result.Should().BeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenValueIsInvalid()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.FacturX!.Version = "2.0";

        BrHybrid10 rule = new();
        bool result = rule.Check(xmp, null, null);

        result.Should().BeFalse();
    }
}
