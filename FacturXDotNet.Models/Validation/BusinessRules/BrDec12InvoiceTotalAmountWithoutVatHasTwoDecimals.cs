﻿using FacturXDotNet.Models.Validation.Utils;

namespace FacturXDotNet.Models.Validation.BusinessRules;

class BrDec12InvoiceTotalAmountWithoutVatHasTwoDecimals() : FacturXBusinessRule(
    "BR-DEC-12",
    "The allowed maximum number of decimals for the Invoice total amount without VAT (BT-109) is 2.",
    FacturXProfileFlags.Minimum.AndHigher()
)
{
    public override bool Check(FacturXCrossIndustryInvoice invoice) =>
        invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount == null
        || invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount.CountDecimals() <= 2;
}
