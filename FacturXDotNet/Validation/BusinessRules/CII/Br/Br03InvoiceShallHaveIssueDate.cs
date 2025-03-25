using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.BusinessRules.CII.Br;

/// <summary>
///     BR-03: An Invoice shall have an Invoice issue date (BT-2).
/// </summary>
public record Br03InvoiceShallHaveIssueDate() : CrossIndustryInvoiceBusinessRule("BR-03", "An Invoice shall have an Invoice issue date (BT-2).", FacturXProfile.Minimum.AndHigher())
{
    /// <inheritdoc />
    public override bool Check(CrossIndustryInvoice? cii) => cii != null && cii.ExchangedDocument.IssueDateTime != default;
}
