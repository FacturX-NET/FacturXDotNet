﻿using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.BusinessRules.CII.Br;

/// <summary>
///     BR-13: An Invoice shall have the Invoice total amount without VAT (BT-109).
/// </summary>
public record Br13InvoiceShallHaveTotalAmountWithoutVat() : CrossIndustryInvoiceBusinessRule(
    "BR-13",
    "An Invoice shall have the Invoice total amount without VAT (BT-109).",
    FacturXProfile.Minimum.AndHigher()
)
{
    /// <inheritdoc />
    public override bool Check(CrossIndustryInvoice? cii) =>
        cii?.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement != null
        && cii.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount != 0;
}
