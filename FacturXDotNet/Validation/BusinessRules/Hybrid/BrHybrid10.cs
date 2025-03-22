namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

record BrHybrid10() : HybridBusinessRule("BR-HYBRID-10", "The fx:Version SHOULD be 1.0.", FacturXBusinessRuleSeverity.Warning)
{
    public override bool Check(XmpMetadata xmp, string ciiAttachmentName, CrossIndustryInvoice cii) => xmp.FacturX is { Version: "1.0" };
}
