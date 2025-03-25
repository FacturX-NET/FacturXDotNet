using FacturXDotNet;
using FacturXDotNet.Models;
using FacturXDotNet.Validation.BusinessRules;

namespace Tests.FacturXDotNet.Validation.CII.Br;

record Br03InvoiceShallHaveIssueDate() : CrossIndustryInvoiceBusinessRule("BR-03", "An Invoice shall have an Invoice issue date (BT-2).", FacturXProfile.Minimum.AndHigher())
{
    public override bool Check(CrossIndustryInvoice? cii) => cii != null && cii.ExchangedDocument.IssueDateTime != default;
}
