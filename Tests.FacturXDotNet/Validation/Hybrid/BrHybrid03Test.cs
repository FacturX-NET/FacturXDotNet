using FacturXDotNet;
using FacturXDotNet.Models.XMP;
using FacturXDotNet.Validation.BusinessRules.Hybrid;
using Shouldly;
using Tests.FacturXDotNet.TestTools;

namespace Tests.FacturXDotNet.Validation.Hybrid;

[TestClass]
public class BrHybrid03Test
{
    [TestMethod]
    public void ShouldValidate_WhenSchemaIsFound()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.PdfAExtensions!.Schemas.Add(new XmpPdfASchemaMetadata { NamespaceUri = XmpFacturXMetadata.NamespaceUri });

        BrHybrid03 rule = new();
        bool result = rule.Check(xmp, null, null);

        result.ShouldBeTrue();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenXmpIsNull()
    {
        BrHybrid03 rule = new();
        bool result = rule.Check(null, null, null);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenNoSchema()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;

        BrHybrid03 rule = new();
        bool result = rule.Check(xmp, null, null);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenSchemaIsNotFound()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.PdfAExtensions!.Schemas.Add(new XmpPdfASchemaMetadata { NamespaceUri = "Some other URI" });

        BrHybrid03 rule = new();
        bool result = rule.Check(xmp, null, null);

        result.ShouldBeFalse();
    }
}
