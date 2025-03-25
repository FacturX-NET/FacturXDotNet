using FacturXDotNet.Models.XMP;
using FacturXDotNet.Validation.BusinessRules.Hybrid;
using Shouldly;
using Tests.FacturXDotNet.TestTools;

namespace Tests.FacturXDotNet.Validation.Hybrid;

[TestClass]
public class BrHybrid14Test
{
    [TestMethod]
    public void ShouldValidate_WhenValueIsValid()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.FacturX!.DocumentFileName = "test.pdf";

        BrHybrid14 rule = new();
        bool result = rule.Check(xmp, "test.pdf", null);

        result.ShouldBeTrue();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenXmpIsNull()
    {
        BrHybrid14 rule = new();
        bool result = rule.Check(null, null, null);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenFacturXIsNull()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.FacturX = null;

        BrHybrid14 rule = new();
        bool result = rule.Check(xmp, null, null);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenDocumentFileNameIsNull()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.FacturX!.DocumentFileName = null;

        BrHybrid14 rule = new();
        bool result = rule.Check(xmp, "test.pdf", null);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenCiiAttachmentNameIsNull()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.FacturX!.DocumentFileName = "test.pdf";

        BrHybrid14 rule = new();
        bool result = rule.Check(xmp, null, null);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenBothNamesAreNull()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.FacturX!.DocumentFileName = null;

        BrHybrid14 rule = new();
        bool result = rule.Check(xmp, null, null);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenValueIsInvalid()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.FacturX!.DocumentFileName = "test.pdf";

        BrHybrid14 rule = new();
        bool result = rule.Check(xmp, "invalid.pdf", null);

        result.ShouldBeFalse();
    }
}
