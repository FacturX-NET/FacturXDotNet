using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.CII.BusinessRules;

record Br02InvoiceShallHaveInvoiceNumber() : FacturXBusinessRule("BR-02", "An Invoice shall have an Invoice number (BT-1).", FacturXProfile.Minimum.AndHigher())
{
    public override bool Check(CrossIndustryInvoice invoice) => !string.IsNullOrWhiteSpace(invoice.ExchangedDocument.Id);
}
