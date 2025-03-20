namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

record BrHybrid06() : HybridBusinessRule("BR-HYBRID-06", "The fx:DocumentType in the XMP instance SHALL be a value from the HybridDocumentType code list.")
{
    public override bool Check(FacturX invoice) => invoice.XmpMetadata.FacturX is { DocumentType: not null } && Enum.IsDefined(invoice.XmpMetadata.FacturX.DocumentType.Value);
}
