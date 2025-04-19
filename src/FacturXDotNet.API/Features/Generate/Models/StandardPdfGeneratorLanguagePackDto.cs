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
    ///     Specifies the language and regional settings used for the invoice. This affect region-specific formatting rules such as date formats, and number separators.
    /// </summary>
    public string? Culture { get; init; }

    /// <summary>
    ///     The label used for the invoice issue date.
    /// </summary>
    public string? DateLabel { get; init; }

    /// <summary>
    ///     The label for the contact person related to the invoice, such as a sales representative or account manager.
    /// </summary>
    public string? ContactLabel { get; init; }

    /// <summary>
    ///     The label used for displaying the contact email address.
    /// </summary>
    public string? EmailLabel { get; init; }

    /// <summary>
    ///     The label used when the legal identification scheme in the invoice is not recognized. While known schemes like SIREN or SIRET have specific labels, this default value is used
    ///     as a fallback for unrecognized or unmapped schemes.
    /// </summary>
    public string? DefaultLegalIdType { get; init; }

    /// <summary>
    ///     The label used to display your VAT (Value Added Tax) number on the invoice.
    /// </summary>
    public string? VatNumberLabel { get; init; }

    /// <summary>
    ///     The label for your internal reference or tracking number related to the invoice.
    /// </summary>
    public string? SellerReferencesLabel { get; init; }

    /// <summary>
    ///     The label for the identifier assigned to the client or customer in your system.
    /// </summary>
    public string? InvoicedObjectIdentifierLabel { get; init; }

    /// <summary>
    ///     The label for referencing a related sales order in the invoice.
    /// </summary>
    public string? SalesOrderReferenceLabel { get; init; }

    /// <summary>
    ///     The label for the client’s reference number or identifier for this transaction.
    /// </summary>
    public string? BuyerReferencesLabel { get; init; }

    /// <summary>
    ///     The label used when referencing a call for tender or bidding process associated with the invoice.
    /// </summary>
    public string? CallForTenderLabel { get; init; }

    /// <summary>
    ///     The label for identifying a specific project the invoice relates to.
    /// </summary>
    public string? ProjectReferenceLabel { get; init; }

    /// <summary>
    ///     The label for referencing internal accounting or financial tracking codes.
    /// </summary>
    public string? AccountingReferenceLabel { get; init; }

    /// <summary>
    ///     The label for referencing a related contract or agreement.
    /// </summary>
    public string? ContractReferenceLabel { get; init; }

    /// <summary>
    ///     The label used to display the client’s purchase order reference.
    /// </summary>
    public string? PurchaseOrderReferenceLabel { get; init; }

    /// <summary>
    ///     The label for any references specific to the invoice document itself.
    /// </summary>
    public string? InvoiceReferencesLabel { get; init; }

    /// <summary>
    ///     The label indicating the beginning of a billing or service period.
    /// </summary>
    public string? StartPeriodLabel { get; init; }

    /// <summary>
    ///     The label indicating the end of a billing or service period.
    /// </summary>
    public string? EndPeriodLabel { get; init; }

    /// <summary>
    ///     The label used to reference a previous invoice related to the current one (e.g., a correction or follow-up).
    /// </summary>
    public string? PrecedingInvoiceReferenceLabel { get; init; }

    /// <summary>
    ///     The label for the issue date of the preceding invoice referenced.
    /// </summary>
    public string? PrecedingInvoiceDateLabel { get; init; }

    /// <summary>
    ///     The label that describes the business process or transaction context (e.g., sales, service).
    /// </summary>
    public string? BusinessProcessLabel { get; init; }

    /// <summary>
    ///     The label used when the document is an invoice and the actual invoice type cannot be matched to a more specific name. Typically set to "Invoice", it serves as a fallback when
    ///     no precise document type label is available.
    /// </summary>
    public string? DefaultInvoiceDocumentsTypeName { get; init; }

    /// <summary>
    ///     The label used when the document is a credit note and the actual credit note type cannot be matched to a more specific name. Typically set to "Credit Note", it serves as a
    ///     fallback when no precise document type label is available.
    /// </summary>
    public string? DefaultCreditNoteDocumentsTypeName { get; init; }

    /// <summary>
    ///     The <see cref="DefaultInvoiceDocumentsTypeName" /> and <see cref="DefaultCreditNoteDocumentsTypeName" /> let you set the labels for common document types like invoices and
    ///     credit notes. However, there are many more specific variations of these documents.
    /// </summary>
    public Dictionary<InvoiceTypeCode, string?>? DocumentTypeNames { get; init; }

    /// <summary>
    ///     The label for the section showing the client’s billing address.
    /// </summary>
    public string? BuyerAddressLabel { get; init; }

    /// <summary>
    ///     The label for the client's identifiers, such as buyer number or account ID.
    /// </summary>
    public string? BuyerIdentifiersLabel { get; init; }

    /// <summary>
    ///     The label for the section containing shipping or delivery details.
    /// </summary>
    public string? DeliveryInformationLabel { get; init; }

    /// <summary>
    ///     The label used for referencing a despatch advice, which confirms the shipment of goods.
    /// </summary>
    public string? DespatchAdviceLabel { get; init; }

    /// <summary>
    ///     The label for the actual or expected date of delivery.
    /// </summary>
    public string? DeliveryDateLabel { get; init; }

    /// <summary>
    ///     The label used for referencing a receiving advice, confirming goods have been received.
    /// </summary>
    public string? ReceivingAdviceLabel { get; init; }

    /// <summary>
    ///     The label used to indicate the currency used on the invoice.
    /// </summary>
    public string? CurrencyLabel { get; init; }

    /// <summary>
    ///     The label for the total amount before VAT is applied.
    /// </summary>
    public string? TotalWithoutVatLabel { get; init; }

    /// <summary>
    ///     The label for the total VAT amount calculated on the invoice.
    /// </summary>
    public string? TotalVatLabel { get; init; }

    /// <summary>
    ///     The label for the total amount including VAT.
    /// </summary>
    public string? TotalWithVatLabel { get; init; }

    /// <summary>
    ///     The label for any amount already paid in advance.
    /// </summary>
    public string? PrepaidAmountLabel { get; set; }

    /// <summary>
    ///     The label for the payment due date.
    /// </summary>
    public string? DueDateLabel { get; set; }

    /// <summary>
    ///     The label for the remaining amount that needs to be paid.
    /// </summary>
    public string? DueAmountLabel { get; set; }

    /// <summary>
    ///     The label used to indicate page numbers in multi-page invoices.
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
