using FacturXDotNet.Models.XMP;
using FacturXDotNet.Validation.BusinessRules.Hybrid;
using FluentAssertions;
using Tests.FacturXDotNet.TestTools;

namespace Tests.FacturXDotNet.Validation.Hybrid;

[TestClass]
public class BrHybrid01Test
{
    [TestMethod]
    public void ShouldValidate_WhenValueIsValid()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;

        BrHybrid01 rule = new();
        bool result = rule.Check(xmp, null, null);

        result.Should().BeTrue();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenValueIsInvalid()
    {
        BrHybrid01 rule = new();
        bool result = rule.Check(null, null, null);

        result.Should().BeFalse();
    }
}
