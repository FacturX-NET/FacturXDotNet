namespace FacturXDotNet.Models.Validation.BusinessRules;

class Br02InvoiceShallHaveInvoiceNumber() : FacturXBusinessRule("BR-02", "An Invoice shall have an Invoice number (BT-1).", FacturXProfileFlags.Minimum.AndHigher())
{
    public override bool Check(FacturXCrossIndustryInvoice invoice) => !string.IsNullOrWhiteSpace(invoice.ExchangedDocument.Id);
}
