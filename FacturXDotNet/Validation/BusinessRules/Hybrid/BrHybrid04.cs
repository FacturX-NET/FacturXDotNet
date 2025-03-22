using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

record BrHybrid04() : HybridBusinessRule("BR-HYBRID-04", "The URI in the extension schema SHALL be urn:factur-x:pdfa:CrossIndustryDocument:1p0#.")
{
    public override bool Check(FacturXDocument invoice) =>
        // a bit of a tautology
        invoice.XmpMetadata.PdfAExtensions?.Schemas.SingleOrDefault(s => s.NamespaceUri == XmpFacturXMetadata.NamespaceUri) is { NamespaceUri: XmpFacturXMetadata.NamespaceUri };
}
