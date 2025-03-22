using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

record BrHybrid05() : HybridBusinessRule("BR-HYBRID-05", "The schema namespace prefix in the XMP extension schema SHALL be fx.")
{
    public override bool Check(XmpMetadata? xmp, string? ciiAttachmentName, CrossIndustryInvoice? cii) =>
        xmp?.PdfAExtensions?.Schemas.SingleOrDefault(s => s.NamespaceUri == XmpFacturXMetadata.NamespaceUri) is { Prefix: "fx" };
}
