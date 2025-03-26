using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;

namespace FacturXDotNet.Validation.BusinessRules.CII.Br;

/// <summary>
///     BR-04: An Invoice shall have an Invoice type code (BT-3).
/// </summary>
public record Br04InvoiceShallHaveTypeCode() : CrossIndustryInvoiceBusinessRule("BR-04", "An Invoice shall have an Invoice type code (BT-3).", FacturXProfile.Minimum.AndHigher())
{
    /// <inheritdoc />
    public override bool Check(CrossIndustryInvoice? cii) => cii?.ExchangedDocument?.TypeCode is not null && Enum.IsDefined(cii.ExchangedDocument.TypeCode.Value);
}
