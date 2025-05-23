using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;
using FacturXDotNet.Validation.Utils;

namespace FacturXDotNet.Validation.BusinessRules.CII.BrDec;

/// <summary>
///     BR-DEC-13: The allowed maximum number of decimals for the Invoice total VAT amount (BT-110) is 2.
/// </summary>
public record BrDec13InvoiceTotalVatAmountHasTwoDecimals() : CrossIndustryInvoiceBusinessRule(
    "BR-DEC-13",
    "The allowed maximum number of decimals for the Invoice total VAT amount (BT-110) is 2.",
    FacturXProfile.Minimum.AndHigher(),
    ["BT-110"]
)
{
    /// <inheritdoc />
    public override bool Check(CrossIndustryInvoice? cii, IBusinessRuleDetailsLogger? logger = null) =>
        cii?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount is null
        || cii.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.Value.CountDecimals() <= 2;
}
