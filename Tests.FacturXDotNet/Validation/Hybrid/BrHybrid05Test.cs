using FacturXDotNet;
using FacturXDotNet.Models.XMP;
using FacturXDotNet.Validation.BusinessRules.Hybrid;
using Shouldly;
using Tests.FacturXDotNet.TestTools;

namespace Tests.FacturXDotNet.Validation.Hybrid;

[TestClass]
public class BrHybrid05Test
{
    [TestMethod]
    public void ShouldValidate_WhenSchemaIsFound()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.PdfAExtensions!.Schemas.Add(new XmpPdfASchemaMetadata { NamespaceUri = XmpFacturXMetadata.NamespaceUri, Prefix = "fx" });

        BrHybrid05 rule = new();
        bool result = rule.Check(xmp, null, null);

        result.ShouldBeTrue();
    }

    [TestMethod]
    public void ShouldValidate_WhenSchemaIsFound_WithOthers()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.PdfAExtensions!.Schemas.Add(new XmpPdfASchemaMetadata { NamespaceUri = "Some other schema", Prefix = "other prefix" });
        xmp.PdfAExtensions!.Schemas.Add(new XmpPdfASchemaMetadata { NamespaceUri = XmpFacturXMetadata.NamespaceUri, Prefix = "fx" });

        BrHybrid05 rule = new();
        bool result = rule.Check(xmp, null, null);

        result.ShouldBeTrue();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenXmpIsNull()
    {
        BrHybrid05 rule = new();
        bool result = rule.Check(null, null, null);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenSchemaNotFound()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.PdfAExtensions!.Schemas.Add(new XmpPdfASchemaMetadata { NamespaceUri = XmpFacturXMetadata.NamespaceUri, Prefix = "other prefix" });

        BrHybrid05 rule = new();
        bool result = rule.Check(xmp, null, null);

        result.ShouldBeFalse();
    }
}
