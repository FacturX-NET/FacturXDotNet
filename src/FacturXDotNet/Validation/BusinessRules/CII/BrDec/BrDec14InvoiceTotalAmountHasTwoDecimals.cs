using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;
using FacturXDotNet.Validation.Utils;

namespace FacturXDotNet.Validation.BusinessRules.CII.BrDec;

/// <summary>
///     BR-DEC-14: The allowed maximum number of decimals for the Invoice total amount with VAT (BT-112) is 2.
/// </summary>
public record BrDec14InvoiceTotalAmountHasTwoDecimals() : CrossIndustryInvoiceBusinessRule(
    "BR-DEC-14",
    "The allowed maximum number of decimals for the Invoice total amount with VAT (BT-112) is 2.",
    FacturXProfile.Minimum.AndHigher(),
    ["BT-112"]
)
{
    /// <inheritdoc />
    public override bool Check(CrossIndustryInvoice? cii, IBusinessRuleDetailsLogger? logger = null) =>
        cii?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount is null
        || cii.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount.Value.CountDecimals() <= 2;
}
