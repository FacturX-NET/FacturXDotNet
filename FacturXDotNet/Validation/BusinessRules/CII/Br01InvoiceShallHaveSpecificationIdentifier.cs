﻿using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.BusinessRules.CII;

record Br01InvoiceShallHaveSpecificationIdentifier() : CrossIndustryInvoiceBusinessRule(
    "BR-01",
    "An Invoice shall have a Specification identifier (BT-24).",
    FacturXProfile.Minimum.AndHigher()
)
{
    public override bool Check(CrossIndustryInvoice invoice) => Enum.IsDefined(invoice.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameterId);
}
