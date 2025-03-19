using FacturXDotNet.Models;
using FacturXDotNet.Validation.Common;

namespace FacturXDotNet.Validation.CII.BusinessRules;

class Br01InvoiceShallHaveSpecificationIdentifier() : FacturXBusinessRule(
    "BR-01",
    "An Invoice shall have a Specification identifier (BT-24).",
    FacturXProfileFlags.Minimum.AndHigher()
)
{
    public override bool Check(CrossIndustryInvoice invoice) => Enum.IsDefined(invoice.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameterId);
}
