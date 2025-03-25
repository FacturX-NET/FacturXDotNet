using FacturXDotNet;
using FacturXDotNet.Models;
using FacturXDotNet.Validation.BusinessRules;

namespace Tests.FacturXDotNet.Validation.CII.Br;

record Br02InvoiceShallHaveInvoiceNumber() : CrossIndustryInvoiceBusinessRule("BR-02", "An Invoice shall have an Invoice number (BT-1).", FacturXProfile.Minimum.AndHigher())
{
    public override bool Check(CrossIndustryInvoice? cii) => !string.IsNullOrWhiteSpace(cii?.ExchangedDocument.Id);
}
