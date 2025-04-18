using System.Globalization;
using FacturXDotNet.Generation.PDF.Generators.Standard;
using FacturXDotNet.Models.CII;

namespace FacturXDotNet.API.Features.Generate.Models;

/// <summary>
///     Represents a data transfer object for a language pack used in the standard PDF generator.
/// </summary>
public class StandardPdfGeneratorLanguagePackDto
{
    /// <summary>
    ///     The culture associated with the language pack, which defines language and regional settings for the generator.
    /// </summary>
    public string? Culture { get; init; }

    /// <summary>
    ///     The label used to denote the VAT (Value Added Tax) number in the generated PDF.
    /// </summary>
    public string? VatNumberLabel { get; init; }

    /// <summary>
    ///     The label representing the contact information in the standard PDF generator.
    /// </summary>
    public string? ContactLabel { get; init; }

    /// <summary>
    ///     The label used for email in the standard PDF generator language pack.
    /// </summary>
    public string? EmailLabel { get; init; }

    /// <summary>
    ///     The label representing "Our References" in the localized language pack, typically used to identify the document references associated with the sender.
    /// </summary>
    public string? SellerReferencesLabel { get; init; }

    /// <summary>
    ///     The label used to identify the invoiced object, which provides a description or reference for the object being invoiced.
    /// </summary>
    public string? InvoicedObjectIdentifierLabel { get; init; }

    /// <summary>
    ///     The label associated with the sales order reference, typically displayed in a PDF document to identify the related sales order.
    /// </summary>
    public string? SalesOrderReferenceLabel { get; init; }

    /// <summary>
    ///     The label used for the "Your references" field in the localized resources of the generator.
    /// </summary>
    public string? BuyerReferencesLabel { get; init; }

    /// <summary>
    ///     The label used to represent a call for tender within the context of the language pack.
    /// </summary>
    public string? CallForTenderLabel { get; init; }

    /// <summary>
    ///     The label representing the project reference in the language pack, used for identifying a referenced project.
    /// </summary>
    public string? ProjectReferenceLabel { get; init; }

    /// <summary>
    ///     The label associated with the accounting reference, used for standardized naming in the generator output.
    /// </summary>
    public string? AccountingReferenceLabel { get; init; }

    /// <summary>
    ///     The label for the contract reference field in the language pack.
    /// </summary>
    public string? ContractReferenceLabel { get; init; }

    /// <summary>
    ///     The label used for the order reference in the generator.
    /// </summary>
    public string? PurchaseOrderReferenceLabel { get; init; }

    /// <summary>
    ///     The label representing invoice references, used to identify and localize these references in the generated PDF.
    /// </summary>
    public string? InvoiceReferencesLabel { get; init; }

    /// <summary>
    ///     The label representing the starting period in a given language pack for the PDF generator.
    /// </summary>
    public string? StartPeriodLabel { get; init; }

    /// <summary>
    ///     The label representing the end of a specified period in the document.
    /// </summary>
    public string? EndPeriodLabel { get; init; }

    /// <summary>
    ///     The label associated with the reference to a preceding invoice.
    /// </summary>
    public string? PrecedingInvoiceReferenceLabel { get; init; }

    /// <summary>
    ///     The label text used for displaying the date of the preceding invoice in the generated PDF document.
    /// </summary>
    public string? PrecedingInvoiceDateLabel { get; init; }

    /// <summary>
    ///     The label used to represent a business process in the generated PDF.
    /// </summary>
    public string? BusinessProcessLabel { get; init; }

    /// <summary>
    ///     The collection of names for document types, keyed by their invoice type codes.
    /// </summary>
    public Dictionary<InvoiceTypeCode, string?>? DocumentTypeNames { get; init; }

    /// <summary>
    ///     The default name assigned to the invoice document type in the language pack.
    /// </summary>
    public string? DefaultInvoiceDocumentsTypeName { get; init; }

    /// <summary>
    ///     The default name of the document type used for credit notes in the PDF generator.
    /// </summary>
    public string? DefaultCreditNoteDocumentsTypeName { get; init; }

    /// <summary>
    ///     The label representing the date field in the language pack used by the generator.
    /// </summary>
    public string? DateLabel { get; init; }

    /// <summary>
    ///     The label used to identify the client's address in the generated PDF.
    /// </summary>
    public string? BuyerAddressLabel { get; init; }

    /// <summary>
    ///     The label associated with the recipient's identifiers in the context of generating a PDF.
    /// </summary>
    public string? BuyerIdentifiersLabel { get; init; }

    /// <summary>
    ///     The label for delivery information, used to display or identify delivery-related details in the PDF document.
    /// </summary>
    public string? DeliveryInformationLabel { get; init; }

    /// <summary>
    ///     The label associated with the despatch advice, typically used to represent shipping or dispatch information in documents.
    /// </summary>
    public string? DespatchAdviceLabel { get; init; }

    /// <summary>
    ///     The label used to identify the delivery date in the generated document.
    /// </summary>
    public string? DeliveryDateLabel { get; init; }

    /// <summary>
    ///     The reference identifier for a receiving advice document, typically used to associate deliveries
    ///     with related documentation in the business process.
    /// </summary>
    public string? ReceivingAdviceLabel { get; init; }

    /// <summary>
    ///     The label used to represent or display the currency in the generator's output.
    /// </summary>
    public string? CurrencyLabel { get; init; }

    /// <summary>
    ///     The label representing the total amount excluding VAT, used in the generator.
    /// </summary>
    public string? TotalWithoutVatLabel { get; init; }

    /// <summary>
    ///     The label used to represent the total VAT amount in the generator.
    /// </summary>
    public string? TotalVatLabel { get; init; }

    /// <summary>
    ///     The label representing the total amount including VAT in the generator.
    /// </summary>
    public string? TotalWithVatLabel { get; init; }

    /// <summary>
    ///     The label representing the prepaid amount in the generated PDF, used for localization purposes in the generator.
    /// </summary>
    public string? PrepaidAmountLabel { get; set; }

    /// <summary>
    ///     The label representing the due date text in the generated PDF document.
    /// </summary>
    public string? DueDateLabel { get; set; }

    /// <summary>
    ///     The label representing the due amount in the PDF document.
    /// </summary>
    public string? DueAmountLabel { get; set; }

    /// <summary>
    ///     The default label or identifier used to represent the legal ID type in the language pack for the generator.
    /// </summary>
    public string? DefaultLegalIdType { get; init; }

    /// <summary>
    ///     The label used to denote or identify a page within the generator.
    /// </summary>
    public string? PageLabel { get; init; }
}

static class StandardPdfGeneratorLanguagePackMappingExtensions
{
    public static StandardPdfGeneratorLanguagePack ToStandardPdfGeneratorLanguagePack(this StandardPdfGeneratorLanguagePackDto pack) =>
        new(StandardPdfGeneratorLanguagePack.Empty)
        {
            Culture = string.IsNullOrWhiteSpace(pack.Culture) ? CultureInfo.InvariantCulture : new CultureInfo(pack.Culture),
            VatNumberLabel = pack.VatNumberLabel ?? "",
            ContactLabel = pack.ContactLabel ?? "",
            EmailLabel = pack.EmailLabel ?? "",
            SellerReferencesLabel = pack.SellerReferencesLabel ?? "",
            InvoicedObjectIdentifierLabel = pack.InvoicedObjectIdentifierLabel ?? "",
            SalesOrderReferenceLabel = pack.SalesOrderReferenceLabel ?? "",
            BuyerReferencesLabel = pack.BuyerReferencesLabel ?? "",
            CallForTenderLabel = pack.CallForTenderLabel ?? "",
            ProjectReferenceLabel = pack.ProjectReferenceLabel ?? "",
            AccountingReferenceLabel = pack.AccountingReferenceLabel ?? "",
            ContractReferenceLabel = pack.ContractReferenceLabel ?? "",
            PurchaseOrderReferenceLabel = pack.PurchaseOrderReferenceLabel ?? "",
            InvoiceReferencesLabel = pack.InvoiceReferencesLabel ?? "",
            StartPeriodLabel = pack.StartPeriodLabel ?? "",
            EndPeriodLabel = pack.EndPeriodLabel ?? "",
            PrecedingInvoiceReferenceLabel = pack.PrecedingInvoiceReferenceLabel ?? "",
            PrecedingInvoiceDateLabel = pack.PrecedingInvoiceDateLabel ?? "",
            BusinessProcessLabel = pack.BusinessProcessLabel ?? "",
            DocumentTypeNames = pack.DocumentTypeNames ?? [],
            DefaultInvoiceDocumentsTypeName = pack.DefaultInvoiceDocumentsTypeName ?? "",
            DefaultCreditNoteDocumentsTypeName = pack.DefaultCreditNoteDocumentsTypeName ?? "",
            DateLabel = pack.DateLabel ?? "",
            BuyerAddressLabel = pack.BuyerAddressLabel ?? "",
            BuyerIdentifiersLabel = pack.BuyerIdentifiersLabel ?? "",
            DeliveryInformationLabel = pack.DeliveryInformationLabel ?? "",
            DespatchAdviceLabel = pack.DespatchAdviceLabel ?? "",
            DeliveryDateLabel = pack.DeliveryDateLabel ?? "",
            ReceivingAdviceLabel = pack.ReceivingAdviceLabel ?? "",
            CurrencyLabel = pack.CurrencyLabel ?? "",
            TotalWithoutVatLabel = pack.TotalWithoutVatLabel ?? "",
            TotalVatLabel = pack.TotalVatLabel ?? "",
            TotalWithVatLabel = pack.TotalWithVatLabel ?? "",
            PrepaidAmountLabel = pack.PrepaidAmountLabel ?? "",
            DueDateLabel = pack.DueDateLabel ?? "",
            DueAmountLabel = pack.DueAmountLabel ?? "",
            DefaultLegalIdType = pack.DefaultLegalIdType ?? "",
            PageLabel = pack.PageLabel ?? ""
        };

    public static StandardPdfGeneratorLanguagePackDto ToStandardPdfGeneratorLanguagePackDto(this StandardPdfGeneratorLanguagePack pack) =>
        new()
        {
            Culture = pack.Culture.Name,
            VatNumberLabel = pack.VatNumberLabel,
            ContactLabel = pack.ContactLabel,
            EmailLabel = pack.EmailLabel,
            SellerReferencesLabel = pack.SellerReferencesLabel,
            InvoicedObjectIdentifierLabel = pack.InvoicedObjectIdentifierLabel,
            SalesOrderReferenceLabel = pack.SalesOrderReferenceLabel,
            BuyerReferencesLabel = pack.BuyerReferencesLabel,
            CallForTenderLabel = pack.CallForTenderLabel,
            ProjectReferenceLabel = pack.ProjectReferenceLabel,
            AccountingReferenceLabel = pack.AccountingReferenceLabel,
            ContractReferenceLabel = pack.ContractReferenceLabel,
            PurchaseOrderReferenceLabel = pack.PurchaseOrderReferenceLabel,
            InvoiceReferencesLabel = pack.InvoiceReferencesLabel,
            StartPeriodLabel = pack.StartPeriodLabel,
            EndPeriodLabel = pack.EndPeriodLabel,
            PrecedingInvoiceReferenceLabel = pack.PrecedingInvoiceReferenceLabel,
            PrecedingInvoiceDateLabel = pack.PrecedingInvoiceDateLabel,
            BusinessProcessLabel = pack.BusinessProcessLabel,
            DocumentTypeNames = pack.DocumentTypeNames ?? [],
            DefaultInvoiceDocumentsTypeName = pack.DefaultInvoiceDocumentsTypeName,
            DefaultCreditNoteDocumentsTypeName = pack.DefaultCreditNoteDocumentsTypeName,
            DateLabel = pack.DateLabel,
            BuyerAddressLabel = pack.BuyerAddressLabel,
            BuyerIdentifiersLabel = pack.BuyerIdentifiersLabel,
            DeliveryInformationLabel = pack.DeliveryInformationLabel,
            DespatchAdviceLabel = pack.DespatchAdviceLabel,
            DeliveryDateLabel = pack.DeliveryDateLabel,
            ReceivingAdviceLabel = pack.ReceivingAdviceLabel,
            CurrencyLabel = pack.CurrencyLabel,
            TotalWithoutVatLabel = pack.TotalWithoutVatLabel,
            TotalVatLabel = pack.TotalVatLabel,
            TotalWithVatLabel = pack.TotalWithVatLabel,
            PrepaidAmountLabel = pack.PrepaidAmountLabel,
            DueDateLabel = pack.DueDateLabel,
            DueAmountLabel = pack.DueAmountLabel,
            DefaultLegalIdType = pack.DefaultLegalIdType,
            PageLabel = pack.PageLabel
        };
}
