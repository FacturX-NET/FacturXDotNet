namespace FacturXDotNet.Models.CII;

/// <summary>
///     <b>Invoice type code</b> - A code specifying the functional type of the Invoice.
/// </summary>
/// <remarks>
///     Commercial invoices and credit notes are defined according the entries in UNTDID 1001. Other  entries of UNTDID 1001 with specific invoices or credit notes may be used if
///     applicable.
/// </remarks>
/// <ID>BT-3</ID>
/// <BR-4>An Invoice shall have an Invoice type code.</BR-4>
/// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:ExchangedDocument/ram:TypeCode</CiiXPath>
/// <Profile>MINIMUM</Profile>
/// <ChorusPro>
///     The types of documents used are:
///     <list type="bullet">
///         <item>380: Commercial Invoice</item>
///         <item>381: Credit note</item>
///         <item>384: Corrected invoice</item>
///         <item>389: Self-billed invoice (created by the buyer on behalf of the supplier)</item>
///         <item>261: Self billed credit note (not accepted by CHORUSPRO)</item>
///         <item>386: Prepayment invoice</item>
///         <item>751: Invoice information for accounting purposes (not accepted by CHORUSPRO)</item>
///     </list>
/// </ChorusPro>
public enum InvoiceTypeCode
{
    /// <summary>
    ///     71 - Request for payment
    /// </summary>
    RequestForPayment = 71,

    /// <summary>
    ///     80 - Debit note related to goods or services
    /// </summary>
    DebitNoteRelatedToGoodsOrServices = 80,

    /// <summary>
    ///     81 - Credit note related to goods or services
    /// </summary>
    CreditNoteRelatedToGoodsOrServices = 81,

    /// <summary>
    ///     82 - Metered services invoice
    /// </summary>
    MeteredServicesInvoice = 82,

    /// <summary>
    ///     83 - Credit note related to financial adjustments
    /// </summary>
    CreditNoteRelatedToFinancialAdjustments = 83,

    /// <summary>
    ///     84 - Debit note related to financial adjustments
    /// </summary>
    DebitNoteRelatedToFinancialAdjustments = 84,

    /// <summary>
    ///     102 - Tax notification
    /// </summary>
    TaxNotification = 102,

    /// <summary>
    ///     130 - Invoicing data sheet
    /// </summary>
    InvoicingDataSheet = 130,

    /// <summary>
    ///     202 - Direct payment valuation
    /// </summary>
    DirectPaymentValuation = 202,

    /// <summary>
    ///     203 - Provisional payment valuation
    /// </summary>
    ProvisionalPaymentValuation = 203,

    /// <summary>
    ///     204 - Payment valuation
    /// </summary>
    PaymentValuation = 204,

    /// <summary>
    ///     211 - Interim application for payment
    /// </summary>
    InterimApplicationForPayment = 211,

    /// <summary>
    ///     218 - Final payment request based on completion of work
    /// </summary>
    FinalPaymentRequestBasedOnCompletionOfWork = 218,

    /// <summary>
    ///     219 - Payment request for completed units
    /// </summary>
    PaymentRequestForCompletedUnits = 219,

    /// <summary>
    ///     261 - Self billed credit note
    /// </summary>
    SelfBilledCreditNote = 261,

    /// <summary>
    ///     262 - Consolidated credit note - goods and services
    /// </summary>
    ConsolidatedCreditNoteGoodsAndServices = 262,

    /// <summary>
    ///     295 - Price variation invoice
    /// </summary>
    PriceVariationInvoice = 295,

    /// <summary>
    ///     296 - Credit note for price variation
    /// </summary>
    CreditNoteForPriceVariation = 296,

    /// <summary>
    ///     308 - Delcredere credit note
    /// </summary>
    DelcredereCreditNote = 308,

    /// <summary>
    ///     325 - Proforma invoice
    /// </summary>
    ProformaInvoice = 325,

    /// <summary>
    ///     326 - Partial invoice
    /// </summary>
    PartialInvoice = 326,

    /// <summary>
    ///     331 - Commercial invoice which includes a packing list
    /// </summary>
    CommercialInvoiceWhichIncludesPackingList = 331,

    /// <summary>
    ///     380 - Commercial invoice
    /// </summary>
    CommercialInvoice = 380,

    /// <summary>
    ///     381 - Credit note
    /// </summary>
    CreditNote = 381,

    /// <summary>
    ///     382 - Commission note
    /// </summary>
    CommissionNote = 382,

    /// <summary>
    ///     383 - Debit note
    /// </summary>
    DebitNote = 383,

    /// <summary>
    ///     384 - Corrected invoice
    /// </summary>
    CorrectedInvoice = 384,

    /// <summary>
    ///     385 - Consolidated invoice
    /// </summary>
    ConsolidatedInvoice = 385,

    /// <summary>
    ///     386 - Prepayment invoice
    /// </summary>
    PrepaymentInvoice = 386,

    /// <summary>
    ///     387 - Hire invoice
    /// </summary>
    HireInvoice = 387,

    /// <summary>
    ///     388 - Tax invoice
    /// </summary>
    TaxInvoice = 388,

    /// <summary>
    ///     389 - Self-billed invoice
    /// </summary>
    SelfBilledInvoice = 389,

    /// <summary>
    ///     390 - Delcredere invoice
    /// </summary>
    DelcredereInvoice = 390,

    /// <summary>
    ///     393 - Factored invoice
    /// </summary>
    FactoredInvoice = 393,

    /// <summary>
    ///     394 - Lease invoice
    /// </summary>
    LeaseInvoice = 394,

    /// <summary>
    ///     395 - Consignment invoice
    /// </summary>
    ConsignmentInvoice = 395,

    /// <summary>
    ///     396 - Factored credit note
    /// </summary>
    FactoredCreditNote = 396,

    /// <summary>
    ///     420 - Optical Character Reading (OCR) payment credit note
    /// </summary>
    OcrPaymentCreditNote = 420,

    /// <summary>
    ///     456 - Debit advice
    /// </summary>
    DebitAdvice = 456,

    /// <summary>
    ///     457 - Reversal of debit
    /// </summary>
    ReversalOfDebit = 457,

    /// <summary>
    ///     458 - Reversal of credit
    /// </summary>
    ReversalOfCredit = 458,

    /// <summary>
    ///     527 - Self billed debit note
    /// </summary>
    SelfBilledDebitNote = 527,

    /// <summary>
    ///     532 - Forwarder's credit note
    /// </summary>
    ForwardersCreditNote = 532,

    /// <summary>
    ///     553 - Forwarder's invoice discrepancy report
    /// </summary>
    ForwardersInvoiceDiscrepancyReport = 553,

    /// <summary>
    ///     575 - Insurer's invoice
    /// </summary>
    InsurersInvoice = 575,

    /// <summary>
    ///     623 - Forwarder's invoice
    /// </summary>
    ForwardersInvoice = 623,

    /// <summary>
    ///     633 - Port charges documents
    /// </summary>
    PortChargesDocuments = 633,

    /// <summary>
    ///     751 - Invoice information for accounting purposes
    /// </summary>
    InvoiceInformationForAccountingPurposes = 751,

    /// <summary>
    ///     780 - Freight invoice
    /// </summary>
    FreightInvoice = 780,

    /// <summary>
    ///     817 - Claim notification
    /// </summary>
    ClaimNotification = 817,

    /// <summary>
    ///     870 - Consular invoice
    /// </summary>
    ConsularInvoice = 870,

    /// <summary>
    ///     875 - Partial construction invoice
    /// </summary>
    PartialConstructionInvoice = 875,

    /// <summary>
    ///     876 - Partial final construction invoice
    /// </summary>
    PartialFinalConstructionInvoice = 876,

    /// <summary>
    ///     877 - Final construction invoice
    /// </summary>
    FinalConstructionInvoice = 877,

    /// <summary>
    ///     935 - Customs invoice
    /// </summary>
    CustomsInvoice = 935
}

/// <summary>
///     Mapping methods for the <see cref="InvoiceTypeCode" /> enumeration.
/// </summary>
public static class FacturXTypeCodeMappingExtensions
{
    /// <summary>
    ///     Converts the <see cref="InvoiceTypeCode" /> to its integer representation.
    /// </summary>
    public static int ToSpecificationIdentifier(this InvoiceTypeCode value) =>
        value switch
        {
            InvoiceTypeCode.RequestForPayment => 71,
            InvoiceTypeCode.DebitNoteRelatedToGoodsOrServices => 80,
            InvoiceTypeCode.CreditNoteRelatedToGoodsOrServices => 81,
            InvoiceTypeCode.MeteredServicesInvoice => 82,
            InvoiceTypeCode.CreditNoteRelatedToFinancialAdjustments => 83,
            InvoiceTypeCode.DebitNoteRelatedToFinancialAdjustments => 84,
            InvoiceTypeCode.TaxNotification => 102,
            InvoiceTypeCode.InvoicingDataSheet => 130,
            InvoiceTypeCode.DirectPaymentValuation => 202,
            InvoiceTypeCode.ProvisionalPaymentValuation => 203,
            InvoiceTypeCode.PaymentValuation => 204,
            InvoiceTypeCode.InterimApplicationForPayment => 211,
            InvoiceTypeCode.FinalPaymentRequestBasedOnCompletionOfWork => 218,
            InvoiceTypeCode.PaymentRequestForCompletedUnits => 219,
            InvoiceTypeCode.SelfBilledCreditNote => 261,
            InvoiceTypeCode.ConsolidatedCreditNoteGoodsAndServices => 262,
            InvoiceTypeCode.PriceVariationInvoice => 295,
            InvoiceTypeCode.CreditNoteForPriceVariation => 296,
            InvoiceTypeCode.DelcredereCreditNote => 308,
            InvoiceTypeCode.ProformaInvoice => 325,
            InvoiceTypeCode.PartialInvoice => 326,
            InvoiceTypeCode.CommercialInvoiceWhichIncludesPackingList => 331,
            InvoiceTypeCode.CommercialInvoice => 380,
            InvoiceTypeCode.CreditNote => 381,
            InvoiceTypeCode.CommissionNote => 382,
            InvoiceTypeCode.DebitNote => 383,
            InvoiceTypeCode.CorrectedInvoice => 384,
            InvoiceTypeCode.ConsolidatedInvoice => 385,
            InvoiceTypeCode.PrepaymentInvoice => 386,
            InvoiceTypeCode.HireInvoice => 387,
            InvoiceTypeCode.TaxInvoice => 388,
            InvoiceTypeCode.SelfBilledInvoice => 389,
            InvoiceTypeCode.DelcredereInvoice => 390,
            InvoiceTypeCode.FactoredInvoice => 393,
            InvoiceTypeCode.LeaseInvoice => 394,
            InvoiceTypeCode.ConsignmentInvoice => 395,
            InvoiceTypeCode.FactoredCreditNote => 396,
            InvoiceTypeCode.OcrPaymentCreditNote => 420,
            InvoiceTypeCode.DebitAdvice => 456,
            InvoiceTypeCode.ReversalOfDebit => 457,
            InvoiceTypeCode.ReversalOfCredit => 458,
            InvoiceTypeCode.SelfBilledDebitNote => 527,
            InvoiceTypeCode.ForwardersCreditNote => 532,
            InvoiceTypeCode.ForwardersInvoiceDiscrepancyReport => 553,
            InvoiceTypeCode.InsurersInvoice => 575,
            InvoiceTypeCode.ForwardersInvoice => 623,
            InvoiceTypeCode.PortChargesDocuments => 633,
            InvoiceTypeCode.InvoiceInformationForAccountingPurposes => 751,
            InvoiceTypeCode.FreightInvoice => 780,
            InvoiceTypeCode.ClaimNotification => 817,
            InvoiceTypeCode.ConsularInvoice => 870,
            InvoiceTypeCode.PartialConstructionInvoice => 875,
            InvoiceTypeCode.PartialFinalConstructionInvoice => 876,
            InvoiceTypeCode.FinalConstructionInvoice => 877,
            InvoiceTypeCode.CustomsInvoice => 935,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };

    /// <summary>
    ///     Converts the integer to its <see cref="InvoiceTypeCode" /> representation.
    /// </summary>
    /// <seealso cref="ToSpecificationIdentifier(int)" />
    public static InvoiceTypeCode? ToSpecificationIdentifierOrNull(this int value) =>
        value switch
        {
            71 => InvoiceTypeCode.RequestForPayment,
            80 => InvoiceTypeCode.DebitNoteRelatedToGoodsOrServices,
            81 => InvoiceTypeCode.CreditNoteRelatedToGoodsOrServices,
            82 => InvoiceTypeCode.MeteredServicesInvoice,
            83 => InvoiceTypeCode.CreditNoteRelatedToFinancialAdjustments,
            84 => InvoiceTypeCode.DebitNoteRelatedToFinancialAdjustments,
            102 => InvoiceTypeCode.TaxNotification,
            130 => InvoiceTypeCode.InvoicingDataSheet,
            202 => InvoiceTypeCode.DirectPaymentValuation,
            203 => InvoiceTypeCode.ProvisionalPaymentValuation,
            204 => InvoiceTypeCode.PaymentValuation,
            211 => InvoiceTypeCode.InterimApplicationForPayment,
            218 => InvoiceTypeCode.FinalPaymentRequestBasedOnCompletionOfWork,
            219 => InvoiceTypeCode.PaymentRequestForCompletedUnits,
            261 => InvoiceTypeCode.SelfBilledCreditNote,
            262 => InvoiceTypeCode.ConsolidatedCreditNoteGoodsAndServices,
            295 => InvoiceTypeCode.PriceVariationInvoice,
            296 => InvoiceTypeCode.CreditNoteForPriceVariation,
            308 => InvoiceTypeCode.DelcredereCreditNote,
            325 => InvoiceTypeCode.ProformaInvoice,
            326 => InvoiceTypeCode.PartialInvoice,
            331 => InvoiceTypeCode.CommercialInvoiceWhichIncludesPackingList,
            380 => InvoiceTypeCode.CommercialInvoice,
            381 => InvoiceTypeCode.CreditNote,
            382 => InvoiceTypeCode.CommissionNote,
            383 => InvoiceTypeCode.DebitNote,
            384 => InvoiceTypeCode.CorrectedInvoice,
            385 => InvoiceTypeCode.ConsolidatedInvoice,
            386 => InvoiceTypeCode.PrepaymentInvoice,
            387 => InvoiceTypeCode.HireInvoice,
            388 => InvoiceTypeCode.TaxInvoice,
            389 => InvoiceTypeCode.SelfBilledInvoice,
            390 => InvoiceTypeCode.DelcredereInvoice,
            393 => InvoiceTypeCode.FactoredInvoice,
            394 => InvoiceTypeCode.LeaseInvoice,
            395 => InvoiceTypeCode.ConsignmentInvoice,
            396 => InvoiceTypeCode.FactoredCreditNote,
            420 => InvoiceTypeCode.OcrPaymentCreditNote,
            456 => InvoiceTypeCode.DebitAdvice,
            457 => InvoiceTypeCode.ReversalOfDebit,
            458 => InvoiceTypeCode.ReversalOfCredit,
            527 => InvoiceTypeCode.SelfBilledDebitNote,
            532 => InvoiceTypeCode.ForwardersCreditNote,
            553 => InvoiceTypeCode.ForwardersInvoiceDiscrepancyReport,
            575 => InvoiceTypeCode.InsurersInvoice,
            623 => InvoiceTypeCode.ForwardersInvoice,
            633 => InvoiceTypeCode.PortChargesDocuments,
            751 => InvoiceTypeCode.InvoiceInformationForAccountingPurposes,
            780 => InvoiceTypeCode.FreightInvoice,
            817 => InvoiceTypeCode.ClaimNotification,
            870 => InvoiceTypeCode.ConsularInvoice,
            875 => InvoiceTypeCode.PartialConstructionInvoice,
            876 => InvoiceTypeCode.PartialFinalConstructionInvoice,
            877 => InvoiceTypeCode.FinalConstructionInvoice,
            935 => InvoiceTypeCode.CustomsInvoice,
            _ => null
        };

    /// <summary>
    ///     Converts the integer to its <see cref="InvoiceTypeCode" /> representation.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not a valid <see cref="InvoiceTypeCode" />.</exception>
    public static InvoiceTypeCode ToSpecificationIdentifier(this int value) =>
        value.ToSpecificationIdentifierOrNull() ?? throw new ArgumentOutOfRangeException(nameof(InvoiceTypeCode), value, null);

    /// <summary>
    ///     Converts the <see cref="InvoiceTypeCode" /> to its corresponding document name as a string.
    /// </summary>
    /// <param name="invoiceTypeCode">The <see cref="InvoiceTypeCode" /> to be converted to a document name.</param>
    /// <returns>The document name corresponding to the specified <see cref="InvoiceTypeCode" />, or null if no match is found.</returns>
    public static string? ToDocumentName(this InvoiceTypeCode invoiceTypeCode) =>
        invoiceTypeCode switch
        {
            InvoiceTypeCode.RequestForPayment => "Request for payment",
            InvoiceTypeCode.DebitNoteRelatedToGoodsOrServices => "Debit note related to goods or services",
            InvoiceTypeCode.CreditNoteRelatedToGoodsOrServices => "Credit note related to goods or services",
            InvoiceTypeCode.MeteredServicesInvoice => "Metered services invoice",
            InvoiceTypeCode.CreditNoteRelatedToFinancialAdjustments => "Credit note related to financial adjustments",
            InvoiceTypeCode.DebitNoteRelatedToFinancialAdjustments => "Debit note related to financial adjustments",
            InvoiceTypeCode.TaxNotification => "Tax notification",
            InvoiceTypeCode.InvoicingDataSheet => "Invoicing data sheet",
            InvoiceTypeCode.DirectPaymentValuation => "Direct payment valuation",
            InvoiceTypeCode.ProvisionalPaymentValuation => "Provisional payment valuation",
            InvoiceTypeCode.PaymentValuation => "Payment valuation",
            InvoiceTypeCode.InterimApplicationForPayment => "Interim application for payment",
            InvoiceTypeCode.FinalPaymentRequestBasedOnCompletionOfWork => "Final payment request based on completion of work",
            InvoiceTypeCode.PaymentRequestForCompletedUnits => "Payment request for completed units",
            InvoiceTypeCode.SelfBilledCreditNote => "Self billed credit note",
            InvoiceTypeCode.ConsolidatedCreditNoteGoodsAndServices => "Consolidated credit note - goods and services",
            InvoiceTypeCode.PriceVariationInvoice => "Price variation invoice",
            InvoiceTypeCode.CreditNoteForPriceVariation => "Credit note for price variation",
            InvoiceTypeCode.DelcredereCreditNote => "Delcredere credit note",
            InvoiceTypeCode.ProformaInvoice => "Proforma invoice",
            InvoiceTypeCode.PartialInvoice => "Partial invoice",
            InvoiceTypeCode.CommercialInvoiceWhichIncludesPackingList => "Commercial invoice which includes a packing list",
            InvoiceTypeCode.CommercialInvoice => "Commercial invoice",
            InvoiceTypeCode.CreditNote => "Credit note",
            InvoiceTypeCode.CommissionNote => "Commission note",
            InvoiceTypeCode.DebitNote => "Debit note",
            InvoiceTypeCode.CorrectedInvoice => "Corrected invoice",
            InvoiceTypeCode.ConsolidatedInvoice => "Consolidated invoice",
            InvoiceTypeCode.PrepaymentInvoice => "Prepayment invoice",
            InvoiceTypeCode.HireInvoice => "Hire invoice",
            InvoiceTypeCode.TaxInvoice => "Tax invoice",
            InvoiceTypeCode.SelfBilledInvoice => "Self-billed invoice",
            InvoiceTypeCode.DelcredereInvoice => "Delcredere invoice",
            InvoiceTypeCode.FactoredInvoice => "Factored invoice",
            InvoiceTypeCode.LeaseInvoice => "Lease invoice",
            InvoiceTypeCode.ConsignmentInvoice => "Consignment invoice",
            InvoiceTypeCode.FactoredCreditNote => "Factored credit note",
            InvoiceTypeCode.OcrPaymentCreditNote => "Optical Character Reading (OCR) payment credit note",
            InvoiceTypeCode.DebitAdvice => "Debit advice",
            InvoiceTypeCode.ReversalOfDebit => "Reversal of debit",
            InvoiceTypeCode.ReversalOfCredit => "Reversal of credit",
            InvoiceTypeCode.SelfBilledDebitNote => "Self billed debit note",
            InvoiceTypeCode.ForwardersCreditNote => "Forwarder's credit note",
            InvoiceTypeCode.ForwardersInvoiceDiscrepancyReport => "Forwarder's invoice discrepancy report",
            InvoiceTypeCode.InsurersInvoice => "Insurer's invoice",
            InvoiceTypeCode.ForwardersInvoice => "Forwarder's invoice",
            InvoiceTypeCode.PortChargesDocuments => "Port charges documents",
            InvoiceTypeCode.InvoiceInformationForAccountingPurposes => "Invoice information for accounting purposes",
            InvoiceTypeCode.FreightInvoice => "Freight invoice",
            InvoiceTypeCode.ClaimNotification => "Claim notification",
            InvoiceTypeCode.ConsularInvoice => "Consular invoice",
            InvoiceTypeCode.PartialConstructionInvoice => "Partial construction invoice",
            InvoiceTypeCode.PartialFinalConstructionInvoice => "Partial final construction invoice",
            InvoiceTypeCode.FinalConstructionInvoice => "Final construction invoice",
            InvoiceTypeCode.CustomsInvoice => "Customs invoice",
            _ => null
        };
}
