using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.BusinessRules;

class Br03InvoiceShallHaveIssueDate() : FacturXBusinessRule("BR-03", "An Invoice shall have an Invoice issue date (BT-2).", FacturXProfileFlags.Minimum.AndHigher())
{
    public override bool Check(FacturXCrossIndustryInvoice invoice) => invoice.ExchangedDocument.IssueDateTime != default;
}
