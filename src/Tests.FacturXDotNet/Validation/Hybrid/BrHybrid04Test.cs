using FacturXDotNet.Models.XMP;
using FacturXDotNet.Validation.BusinessRules.Hybrid;
using FluentAssertions;
using Tests.FacturXDotNet.TestTools;

namespace Tests.FacturXDotNet.Validation.Hybrid;

[TestClass]
public class BrHybrid04Test
{
    [TestMethod]
    public void ShouldValidate_WhenSchemaIsFound()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.PdfAExtensions!.Schemas.Add(new XmpPdfASchemaMetadata { NamespaceUri = XmpFacturXMetadata.NamespaceUri });

        BrHybrid04 rule = new();
        bool result = rule.Check(xmp, null, null);

        result.Should().BeTrue();
    }

    [TestMethod]
    public void ShouldValidate_WhenSchemaIsFound_WithOthers()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.PdfAExtensions!.Schemas.Add(new XmpPdfASchemaMetadata { NamespaceUri = "Some other schema" });
        xmp.PdfAExtensions!.Schemas.Add(new XmpPdfASchemaMetadata { NamespaceUri = XmpFacturXMetadata.NamespaceUri });

        BrHybrid04 rule = new();
        bool result = rule.Check(xmp, null, null);

        result.Should().BeTrue();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenXmpIsNull()
    {
        BrHybrid04 rule = new();
        bool result = rule.Check(null, null, null);

        result.Should().BeFalse();
    }

    [TestMethod]
    public void ShouldNotValidate_WhenSchemaNotFound()
    {
        XmpMetadata xmp = FakeData.XmpMetadata;
        xmp.PdfAExtensions!.Schemas.Add(new XmpPdfASchemaMetadata { NamespaceUri = "INVALID" });

        BrHybrid04 rule = new();
        bool result = rule.Check(xmp, null, null);

        result.Should().BeFalse();
    }
}
