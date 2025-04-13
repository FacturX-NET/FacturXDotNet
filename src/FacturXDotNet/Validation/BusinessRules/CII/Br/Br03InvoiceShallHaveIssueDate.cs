using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;

namespace FacturXDotNet.Validation.BusinessRules.CII.Br;

/// <summary>
///     BR-3: An Invoice shall have an Invoice issue date (BT-2).
/// </summary>
public record Br03InvoiceShallHaveIssueDate() : CrossIndustryInvoiceBusinessRule(
    "BR-3",
    "An Invoice shall have an Invoice issue date (BT-2).",
    FacturXProfile.Minimum.AndHigher(),
    ["BT-2"]
)
{
    /// <inheritdoc />
    public override bool Check(CrossIndustryInvoice? cii, IBusinessRuleDetailsLogger? logger = null) =>
        cii?.ExchangedDocument?.IssueDateTime is not null && cii.ExchangedDocument?.IssueDateTime != DateOnly.MinValue;
}
