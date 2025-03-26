﻿using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;
using FacturXDotNet.Validation.Utils;

namespace FacturXDotNet.Validation.BusinessRules.CII.Br;

/// <summary>
///     BR-09: The Seller postal address (BG-5) shall contain a Seller country code (BT-40).
/// </summary>
public record Br09InvoiceShallHaveSellerPostalAddressWithCountryCode() : CrossIndustryInvoiceBusinessRule(
    "BR-09",
    "The Seller postal address (BG-5) shall contain a Seller country code (BT-40).",
    FacturXProfile.Minimum.AndHigher()
)
{
    /// <inheritdoc />
    public override bool Check(CrossIndustryInvoice? cii) =>
        Iso31661CountryCodesUtils.IsValidCountryCode(cii?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress?.CountryId);
}
