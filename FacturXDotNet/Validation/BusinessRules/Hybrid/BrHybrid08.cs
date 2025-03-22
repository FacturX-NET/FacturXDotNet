namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

record BrHybrid08() : HybridBusinessRule("BR-HYBRID-08", "The fx:DocumentFileName in the XMP instance SHALL be a value defined in the HybridDocumentFilename code list.")
{
    public override bool Check(XmpMetadata? xmp, string? ciiAttachmentName, CrossIndustryInvoice? cii) =>
        xmp?.FacturX is { DocumentFileName: "factur-x.xml" or "xrechnung.xml" or "order-x.xml" };
}
