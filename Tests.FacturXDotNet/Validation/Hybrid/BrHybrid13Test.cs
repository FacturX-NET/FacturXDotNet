using FacturXDotNet.Validation.BusinessRules.Hybrid;
using Shouldly;

namespace Tests.FacturXDotNet.Validation.Hybrid;

[TestClass]
public class BrHybrid13Test
{
    [TestMethod]
    public void ShouldValidate_WhenValueIsFacturX()
    {
        BrHybrid13 rule = new();
        bool result = rule.Check(null, "factur-x.xml", null);

        result.ShouldBeTrue();
    }

    [TestMethod]
    public void ShouldValidate_WhenValueIsXrechnung()
    {
        BrHybrid13 rule = new();
        bool result = rule.Check(null, "xrechnung.xml", null);

        result.ShouldBeTrue();
    }

    [TestMethod]
    public void ShouldValidate_WhenValueIsOrderX()
    {
        BrHybrid13 rule = new();
        bool result = rule.Check(null, "order-x.xml", null);

        result.ShouldBeTrue();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenValueIsInvalid()
    {
        BrHybrid13 rule = new();
        bool result = rule.Check(null, "invalid.xml", null);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenValueIsNull()
    {
        BrHybrid13 rule = new();
        bool result = rule.Check(null, null, null);

        result.ShouldBeFalse();
    }
}
