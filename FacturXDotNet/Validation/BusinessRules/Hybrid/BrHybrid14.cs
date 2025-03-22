namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

record BrHybrid14() : HybridBusinessRule("BR-HYBRID-14", "The embedded file name SHOULD match the fx:DocumentFileName.", FacturXBusinessRuleSeverity.Warning)
{
    public override bool Check(XmpMetadata xmp, string ciiAttachmentName, CrossIndustryInvoice cii) => xmp.FacturX != null && ciiAttachmentName == xmp.FacturX.DocumentFileName;
}
