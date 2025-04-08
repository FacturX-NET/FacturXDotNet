using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;

namespace FacturXDotNet.Validation.BusinessRules.CII.Br;

/// <summary>
///     BR-02: An Invoice shall have an Invoice number (BT-1).
/// </summary>
public record Br02InvoiceShallHaveInvoiceNumber() : CrossIndustryInvoiceBusinessRule(
    "BR-02",
    "An Invoice shall have an Invoice number (BT-1).",
    FacturXProfile.Minimum.AndHigher(),
    [$"{nameof(CrossIndustryInvoice.ExchangedDocument)}.{nameof(ExchangedDocument.Id)}"]
)
{
    /// <inheritdoc />
    public override bool Check(CrossIndustryInvoice? cii, IBusinessRuleDetailsLogger? logger = null) => !string.IsNullOrWhiteSpace(cii?.ExchangedDocument?.Id);
}
