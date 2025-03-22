namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

record BrHybrid01() : HybridBusinessRule(
    "BR-HYBRID-01",
    "A hybrid document consists of a machine readable interchange format on XML syntax and a human readable PDF envelope.",
    FacturXBusinessRuleSeverity.Information
)
{
    public override bool Check(FacturXDocument invoice) =>
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        invoice.XmpMetadata != null;
}
