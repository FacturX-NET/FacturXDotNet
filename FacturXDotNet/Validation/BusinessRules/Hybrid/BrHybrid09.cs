namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

record BrHybrid09() : HybridBusinessRule("BR-HYBRID-09", "The fx:Version in the XMP instance SHALL be a value defined in the HybridDocumentVersion codelist.")
{
    public override bool Check(FacturX invoice) => invoice.XmpMetadata.FacturX is { Version: "1.0" or "1p0" or "2p0" or "2p1" or "2p2" };
}
