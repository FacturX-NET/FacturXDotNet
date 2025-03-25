﻿using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.BusinessRules.CII.Br;

/// <summary>
///     BR-05: An Invoice shall have an Invoice currency code (BT-5).
/// </summary>
public record Br05InvoiceShallHaveCurrencyCode() : CrossIndustryInvoiceBusinessRule(
    "BR-05",
    "An Invoice shall have an Invoice currency code (BT-5).",
    FacturXProfile.Minimum.AndHigher()
)
{
    /// <inheritdoc />
    public override bool Check(CrossIndustryInvoice? cii) =>
        cii?.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement != null
        && !string.IsNullOrWhiteSpace(cii.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.InvoiceCurrencyCode);
}
