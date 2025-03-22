using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

record BrHybrid04() : HybridBusinessRule("BR-HYBRID-04", "The URI in the extension schema SHALL be urn:factur-x:pdfa:CrossIndustryDocument:1p0#.")
{
    public override bool Check(XmpMetadata xmp, string ciiAttachmentName, CrossIndustryInvoice cii) =>
        // a bit of a tautology
        xmp.PdfAExtensions?.Schemas.SingleOrDefault(s => s.NamespaceUri == XmpFacturXMetadata.NamespaceUri) is { NamespaceUri: XmpFacturXMetadata.NamespaceUri };
}
