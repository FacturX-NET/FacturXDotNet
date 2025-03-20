﻿using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.CII.BusinessRules;

record Br05InvoiceShallHaveCurrencyCode() : FacturXBusinessRule("BR-05", "An Invoice shall have an Invoice currency code (BT-5).", FacturXProfile.Minimum.AndHigher())
{
    public override bool Check(CrossIndustryInvoice invoice) =>
        invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement != null
        && !string.IsNullOrWhiteSpace(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.InvoiceCurrencyCode);
}
