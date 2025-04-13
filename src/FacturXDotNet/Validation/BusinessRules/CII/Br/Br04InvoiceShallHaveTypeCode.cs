using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;

namespace FacturXDotNet.Validation.BusinessRules.CII.Br;

/// <summary>
///     BR-4: An Invoice shall have an Invoice type code (BT-3).
/// </summary>
public record Br04InvoiceShallHaveTypeCode() : CrossIndustryInvoiceBusinessRule(
    "BR-4",
    "An Invoice shall have an Invoice type code (BT-3).",
    FacturXProfile.Minimum.AndHigher(),
    ["BT-3"]
)
{
    /// <inheritdoc />
    public override bool Check(CrossIndustryInvoice? cii, IBusinessRuleDetailsLogger? logger = null) =>
        cii?.ExchangedDocument?.TypeCode is not null && Enum.IsDefined(cii.ExchangedDocument.TypeCode.Value);
}
