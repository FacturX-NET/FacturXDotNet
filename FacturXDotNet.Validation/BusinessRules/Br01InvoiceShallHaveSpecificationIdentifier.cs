using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.BusinessRules;

class Br01InvoiceShallHaveSpecificationIdentifier() : FacturXBusinessRule(
    "BR-01",
    "An Invoice shall have a Specification identifier (BT-24).",
    FacturXProfileFlags.Minimum.AndHigher()
)
{
    public override bool Check(FacturXCrossIndustryInvoice invoice) => Enum.IsDefined(invoice.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameterId);
}
