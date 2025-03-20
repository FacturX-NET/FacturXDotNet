﻿using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.CII.BusinessRules;

record Br09InvoiceShallHaveSellerPostalAddressWithCountryCode() : FacturXBusinessRule(
    "BR-09",
    "The Seller postal address (BG-5) shall contain a Seller country code (BT-40).",
    FacturXProfile.Minimum.AndHigher()
)
{
    public override bool Check(CrossIndustryInvoice invoice) =>
        !string.IsNullOrWhiteSpace(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryId);
}
