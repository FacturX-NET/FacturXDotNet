namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

record BrHybrid04() : HybridBusinessRule("BR-HYBRID-04", "The URI in the extension schema SHALL be urn:factur-x:pdfa:CrossIndustryDocument:1p0#.")
{
    public override bool Check(FacturX invoice) =>
        // a bit of a tautology
        invoice.XmpMetadata.PdfAExtensions?.Schemas.SingleOrDefault(s => s.NamespaceUri == "urn:factur-x:pdfa:CrossIndustryDocument:invoice:1p0#") is
            { NamespaceUri: "urn:factur-x:pdfa:CrossIndustryDocument:invoice:1p0#" };
}
