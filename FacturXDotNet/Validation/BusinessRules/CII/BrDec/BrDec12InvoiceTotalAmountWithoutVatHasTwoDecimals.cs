using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;
using FacturXDotNet.Validation.Utils;

namespace FacturXDotNet.Validation.BusinessRules.CII.BrDec;

/// <summary>
///     BR-DEC-12: The allowed maximum number of decimals for the Invoice total amount without VAT (BT-109) is 2.
/// </summary>
public record BrDec12InvoiceTotalAmountWithoutVatHasTwoDecimals() : CrossIndustryInvoiceBusinessRule(
    "BR-DEC-12",
    "The allowed maximum number of decimals for the Invoice total amount without VAT (BT-109) is 2.",
    FacturXProfile.Minimum.AndHigher()
)
{
    /// <inheritdoc />
    public override bool Check(CrossIndustryInvoice? cii) =>
        cii?.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount == null
        || cii.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount.CountDecimals() <= 2;
}
