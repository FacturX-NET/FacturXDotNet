﻿namespace FacturXDotNet.Validation.CII.BusinessRules;

record Br06InvoiceShallHaveSellerName() : FacturXBusinessRule("BR-06", "An Invoice shall contain the Seller name (BT-27).", FacturXProfileFlags.Minimum.AndHigher())
{
    public override bool Check(CrossIndustryInvoice invoice) =>
        !string.IsNullOrWhiteSpace(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.Name);
}
