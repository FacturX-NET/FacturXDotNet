namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

record BrHybrid05() : HybridBusinessRule("BR-HYBRID-05", "The schema namespace prefix in the XMP extension schema SHALL be fx.")
{
    public override bool Check(FacturX invoice) =>
        invoice.XmpMetadata.PdfAExtensions?.Schemas.SingleOrDefault(s => s.NamespaceUri == "urn:factur-x:pdfa:CrossIndustryDocument:invoice:1p0#") is { Prefix: "fx" };
}
