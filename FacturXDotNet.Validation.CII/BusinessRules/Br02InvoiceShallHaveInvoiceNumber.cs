using FacturXDotNet.Models;
using FacturXDotNet.Validation.Common;

namespace FacturXDotNet.Validation.CII.BusinessRules;

class Br02InvoiceShallHaveInvoiceNumber() : FacturXBusinessRule("BR-02", "An Invoice shall have an Invoice number (BT-1).", FacturXProfileFlags.Minimum.AndHigher())
{
    public override bool Check(CrossIndustryInvoice invoice) => !string.IsNullOrWhiteSpace(invoice.ExchangedDocument.Id);
}
