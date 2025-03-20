﻿using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.CII.BusinessRules;

record Br08InvoiceShallHaveSellerPostalAddress() : FacturXBusinessRule("BR-08", "An Invoice shall contain the Seller postal address (BG-5).", FacturXProfile.Minimum.AndHigher())
{
    public override bool Check(CrossIndustryInvoice invoice) =>
        // Nullability analysis should guarantee that this is always true, however it is still a BT so we check it anyway
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress != null;
}
