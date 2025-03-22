namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

record BrHybrid07() : HybridBusinessRule("BR-HYBRID-07", "The fx:ConformanceLevel in the XMP instance SHALL be a value from the HybridConformanceType code list.")
{
    public override bool Check(FacturXDocument invoice) =>
        invoice.XmpMetadata.FacturX is { ConformanceLevel: not null } && Enum.IsDefined(invoice.XmpMetadata.FacturX.ConformanceLevel.Value);
}
