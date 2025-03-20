using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.CII.BusinessRules;

record Br03InvoiceShallHaveIssueDate() : FacturXBusinessRule("BR-03", "An Invoice shall have an Invoice issue date (BT-2).", FacturXProfile.Minimum.AndHigher())
{
    public override bool Check(CrossIndustryInvoice invoice) => invoice.ExchangedDocument.IssueDateTime != default;
}
