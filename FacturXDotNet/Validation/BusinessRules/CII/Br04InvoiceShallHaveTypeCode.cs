using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.BusinessRules.CII;

record Br04InvoiceShallHaveTypeCode() : CrossIndustryInvoiceBusinessRule("BR-04", "An Invoice shall have an Invoice type code (BT-3).", FacturXProfile.Minimum.AndHigher())
{
    public override bool Check(CrossIndustryInvoice invoice) => Enum.IsDefined(invoice.ExchangedDocument.TypeCode);
}
