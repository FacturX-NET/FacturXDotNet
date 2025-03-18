using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.BusinessRules;

class Br04InvoiceShallHaveTypeCode() : FacturXBusinessRule("BR-04", "An Invoice shall have an Invoice type code (BT-3).", FacturXProfileFlags.Minimum.AndHigher())
{
    public override bool Check(FacturXCrossIndustryInvoice invoice) => Enum.IsDefined(invoice.ExchangedDocument.TypeCode);
}
