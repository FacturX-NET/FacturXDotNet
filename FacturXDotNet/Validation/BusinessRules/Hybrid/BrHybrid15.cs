using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

record BrHybrid15() : HybridBusinessRule("BR-HYBRID-15", "The fx:ConformanceLevel SHOULD match the profile of the embedded XML document.", FacturXBusinessRuleSeverity.Warning)
{
    public override bool Check(FacturX invoice) =>
        invoice.XmpMetadata.FacturX != null
        && invoice.CrossIndustryInvoice.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameterId.ToFacturXProfile()
        == invoice.XmpMetadata.FacturX.ConformanceLevel?.ToFacturXProfile();
}
