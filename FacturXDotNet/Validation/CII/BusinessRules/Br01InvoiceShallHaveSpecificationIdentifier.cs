using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.CII.BusinessRules;

record Br01InvoiceShallHaveSpecificationIdentifier() : FacturXBusinessRule("BR-01", "An Invoice shall have a Specification identifier (BT-24).", FacturXProfile.Minimum.AndHigher())
{
    public override bool Check(CrossIndustryInvoice invoice) => Enum.IsDefined(invoice.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameterId);
}
