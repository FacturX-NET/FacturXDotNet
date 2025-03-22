using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.BusinessRules.CII;

record Br02InvoiceShallHaveInvoiceNumber() : CrossIndustryInvoiceBusinessRule("BR-02", "An Invoice shall have an Invoice number (BT-1).", FacturXProfile.Minimum.AndHigher())
{
    public override bool Check(CrossIndustryInvoice? cii) => !string.IsNullOrWhiteSpace(cii?.ExchangedDocument.Id);
}
