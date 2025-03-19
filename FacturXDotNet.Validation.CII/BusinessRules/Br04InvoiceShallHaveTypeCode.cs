using FacturXDotNet.Validation.Common;

namespace FacturXDotNet.Validation.CII.BusinessRules;

class Br04InvoiceShallHaveTypeCode() : FacturXBusinessRule("BR-04", "An Invoice shall have an Invoice type code (BT-3).", FacturXProfileFlags.Minimum.AndHigher())
{
    public override bool Check(CrossIndustryInvoice invoice) => Enum.IsDefined(invoice.ExchangedDocument.TypeCode);
}
