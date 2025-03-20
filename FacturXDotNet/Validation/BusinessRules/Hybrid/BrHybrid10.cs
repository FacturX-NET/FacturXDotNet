namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

record BrHybrid10() : HybridBusinessRule("BR-HYBRID-10", "The fx:Version SHOULD be 1.0.", FacturXBusinessRuleSeverity.Warning)
{
    public override bool Check(FacturX invoice) => invoice.XmpMetadata.FacturX is { Version: "1.0" };
}
