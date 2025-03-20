using FacturXDotNet.Validation.BusinessRules.CII.VatRelated.StandardAndReducedRate;

namespace FacturXDotNet.Validation.BusinessRules.CII;

/// <summary>
///     The business rules for validating a Cross-Industry Invoice.
/// </summary>
static class CrossIndustryInvoiceBusinessRules
{
    public static readonly CrossIndustryInvoiceBusinessRule[] Rules =
    [
        new Br01InvoiceShallHaveSpecificationIdentifier(),
        new Br02InvoiceShallHaveInvoiceNumber(),
        new Br03InvoiceShallHaveIssueDate(),
        new Br04InvoiceShallHaveTypeCode(),
        new Br05InvoiceShallHaveCurrencyCode(),
        new Br06InvoiceShallHaveSellerName(),
        new Br07InvoiceShallHaveBuyerName(),
        new Br08InvoiceShallHaveSellerPostalAddress(),
        new Br09InvoiceShallHaveSellerPostalAddressWithCountryCode(),
        new Br13InvoiceShallHaveTotalAmountWithoutVat(),
        new Br14InvoiceShallHaveTotalAmountWithVat(),
        new Br15InvoiceShallHaveAmountDueForPayment(),
        new BrCo09(),
        new BrCo26(),
        new BrDec12InvoiceTotalAmountWithoutVatHasTwoDecimals(),
        new BrDec13InvoiceTotalVatAmountHasTwoDecimals(),
        new BrDec14InvoiceTotalAmountHasTwoDecimals(),
        new BrDec18InvoiceDueAmountHasTwoDecimals(),
        new BrS01()
    ];
}
