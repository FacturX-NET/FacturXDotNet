using System.Globalization;
using FacturXDotNet.Models.CII;

namespace FacturXDotNet.Generation.PDF.Generators.Standard;

/// <summary>
///     Represents a language pack containing localized resources used by the StandardPdfGenerator.
/// </summary>
public class StandardPdfGeneratorLanguagePack
{
    StandardPdfGeneratorLanguagePack() { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="StandardPdfGeneratorLanguagePack" /> class
    ///     by copying values from an existing language pack. Fields can be further customized using
    ///     their respective <c>init</c> setters after initialization.
    /// </summary>
    /// <param name="pack">The language pack from which to copy the field values.</param>
    public StandardPdfGeneratorLanguagePack(StandardPdfGeneratorLanguagePack pack)
    {
        Culture = pack.Culture;
        VatNumberLabel = pack.VatNumberLabel;
        ContactLabel = pack.ContactLabel;
        EmailLabel = pack.EmailLabel;
        SellerReferencesLabel = pack.SellerReferencesLabel;
        BuyerReferencesLabel = pack.BuyerReferencesLabel;
        PurchaseOrderReferenceLabel = pack.PurchaseOrderReferenceLabel;
        InvoiceReferencesLabel = pack.InvoiceReferencesLabel;
        BusinessProcessLabel = pack.BusinessProcessLabel;
        DocumentTypeNames = pack.DocumentTypeNames;
        DefaultInvoiceDocumentsTypeName = pack.DefaultInvoiceDocumentsTypeName;
        DateLabel = pack.DateLabel;
        BuyerAddressLabel = pack.BuyerAddressLabel;
        BuyerIdentifiersLabel = pack.BuyerIdentifiersLabel;
        DeliveryInformationLabel = pack.DeliveryInformationLabel;
        CurrencyLabel = pack.CurrencyLabel;
        TotalWithoutVatLabel = pack.TotalWithoutVatLabel;
        TotalVatLabel = pack.TotalVatLabel;
        TotalWithVatLabel = pack.TotalWithVatLabel;
        PrepaidAmountLabel = pack.PrepaidAmountLabel;
        DueDateLabel = pack.DueDateLabel;
        DueAmountLabel = pack.DueAmountLabel;
        DefaultLegalIdType = pack.DefaultLegalIdType;
        PageLabel = pack.PageLabel;
    }

    /// <summary>
    ///     The culture associated with the language pack, which defines language and regional settings for the StandardPdfGenerator.
    /// </summary>
    public required CultureInfo Culture { get; init; }

    /// <summary>
    ///     The label used to denote the VAT (Value Added Tax) number in the generated PDF.
    /// </summary>
    public required string VatNumberLabel { get; init; }

    /// <summary>
    ///     The label text used to represent a contact section in the StandardPdfGenerator.
    /// </summary>
    public required string ContactLabel { get; init; }

    /// <summary>
    ///     The label used to denote the email field in the StandardPdfGenerator's language pack.
    /// </summary>
    public required string EmailLabel { get; init; }

    /// <summary>
    ///     The label representing "Our References" in the localized language pack, typically used to identify the document references associated with the sender.
    /// </summary>
    public required string SellerReferencesLabel { get; init; }

    /// <summary>
    ///     The label used to denote the sales order reference in the StandardPdfGenerator.
    /// </summary>
    public required string InvoicedObjectIdentifierLabel { get; init; }

    /// <summary>
    ///     The label used to denote the sales order reference in the StandardPdfGenerator.
    /// </summary>
    public required string SalesOrderReferenceLabel { get; init; }

    /// <summary>
    ///     The label used for the "Your references" field in the localized resources of the StandardPdfGenerator.
    /// </summary>
    public required string BuyerReferencesLabel { get; init; }

    /// <summary>
    ///     The label for the call for tender section in the generated PDF, used for identifying tender-related references or information.
    /// </summary>
    public required string CallForTenderLabel { get; init; }

    /// <summary>
    ///     The label used for accounting references in the StandardPdfGenerator language pack.
    /// </summary>
    public required string ProjectReferenceLabel { get; init; }

    /// <summary>
    ///     The label used for accounting references in the StandardPdfGenerator language pack.
    /// </summary>
    public required string AccountingReferenceLabel { get; init; }

    /// <summary>
    ///     The label for contracts used in the StandardPdfGenerator, representing text associated with contractual terms.
    /// </summary>
    public required string ContractReferenceLabel { get; init; }

    /// <summary>
    ///     The label used for the order reference in the StandardPdfGenerator.
    /// </summary>
    public required string PurchaseOrderReferenceLabel { get; init; }

    /// <summary>
    ///     The label representing invoice references, used to identify and localize these references in the generated PDF.
    /// </summary>
    public required string InvoiceReferencesLabel { get; init; }

    /// <summary>
    ///     The label representing the start of a specific period in a document or report within the StandardPdfGenerator.
    /// </summary>
    public required string StartPeriodLabel { get; init; }

    /// <summary>
    ///     The label representing the end period in the StandardPdfGenerator.
    /// </summary>
    public required string EndPeriodLabel { get; init; }

    /// <summary>
    ///     The label representing the reference to a previous invoice in the StandardPdfGenerator.
    /// </summary>
    public required string PrecedingInvoiceReferenceLabel { get; init; }

    /// <summary>
    ///     The label used to represent the date of a previous invoice in generated documents.
    /// </summary>
    public required string PrecedingInvoiceDateLabel { get; init; }

    /// <summary>
    ///     The label used to represent a business process in the generated PDF.
    /// </summary>
    public required string BusinessProcessLabel { get; init; }

    /// <summary>
    ///     The collection of names for document types, keyed by their invoice type codes.
    /// </summary>
    public required Dictionary<InvoiceTypeCode, string?> DocumentTypeNames { get; init; }

    /// <summary>
    ///     The default document type name used for invoices in the StandardPdfGeneratorLanguagePack.
    /// </summary>
    public required string DefaultInvoiceDocumentsTypeName { get; init; }

    /// <summary>
    ///     The default document type name used for credit notes in the StandardPdfGenerator.
    /// </summary>
    public required string DefaultCreditNoteDocumentsTypeName { get; init; }

    /// <summary>
    ///     The label representing the date field in the language pack used by the StandardPdfGenerator.
    /// </summary>
    public required string DateLabel { get; init; }

    /// <summary>
    ///     The label used to identify the client's address in the generated PDF.
    /// </summary>
    public required string BuyerAddressLabel { get; init; }

    /// <summary>
    ///     The label associated with the recipient's identifiers in the context of generating a PDF.
    /// </summary>
    public required string BuyerIdentifiersLabel { get; init; }

    /// <summary>
    ///     The label for delivery information, used to display or identify delivery-related details in the PDF document.
    /// </summary>
    public required string DeliveryInformationLabel { get; init; }

    /// <summary>
    ///     The label used to represent the despatch advice information within the StandardPdfGenerator.
    /// </summary>
    public required string DespatchAdviceLabel { get; init; }

    /// <summary>
    ///     The label used to represent the delivery date in the StandardPdfGenerator.
    /// </summary>
    public required string DeliveryDateLabel { get; init; }

    /// <summary>
    ///     The label indicating the reference to the receiving advice, typically used in invoicing or delivery documentation contexts.
    /// </summary>
    public required string ReceivingAdviceLabel { get; init; }

    /// <summary>
    ///     The label used to represent or display the currency in the StandardPdfGenerator's output.
    /// </summary>
    public required string CurrencyLabel { get; init; }

    /// <summary>
    ///     The label representing the total amount excluding VAT, used in the StandardPdfGenerator.
    /// </summary>
    public required string TotalWithoutVatLabel { get; init; }

    /// <summary>
    ///     The label used to represent the total VAT amount in the StandardPdfGenerator.
    /// </summary>
    public required string TotalVatLabel { get; init; }

    /// <summary>
    ///     The label representing the total amount including VAT in the StandardPdfGenerator.
    /// </summary>
    public required string TotalWithVatLabel { get; init; }

    /// <summary>
    ///     The label representing the prepaid amount in the generated PDF, used for localization purposes in the StandardPdfGenerator.
    /// </summary>
    public required string PrepaidAmountLabel { get; set; }

    /// <summary>
    ///     The label representing the due date text in the generated PDF document.
    /// </summary>
    public required string DueDateLabel { get; set; }

    /// <summary>
    ///     The label representing the due amount in the PDF document.
    /// </summary>
    public required string DueAmountLabel { get; set; }

    /// <summary>
    ///     The default label or identifier used to represent the legal ID type in the language pack for the StandardPdfGenerator.
    /// </summary>
    public required string DefaultLegalIdType { get; init; }

    /// <summary>
    ///     The label used to denote or identify a page within the StandardPdfGenerator.
    /// </summary>
    public required string PageLabel { get; init; }

    /// <summary>
    ///     The empty instance of the StandardPdfGeneratorLanguagePack with all properties initialized to their default values.
    /// </summary>
    public static StandardPdfGeneratorLanguagePack Empty { get; } = new()
    {
        Culture = CultureInfo.InvariantCulture,
        VatNumberLabel = "",
        ContactLabel = "",
        EmailLabel = "",
        SellerReferencesLabel = "",
        InvoicedObjectIdentifierLabel = "",
        SalesOrderReferenceLabel = "",
        BuyerReferencesLabel = "",
        CallForTenderLabel = "",
        ProjectReferenceLabel = "",
        AccountingReferenceLabel = "",
        ContractReferenceLabel = "",
        PurchaseOrderReferenceLabel = "",
        InvoiceReferencesLabel = "",
        StartPeriodLabel = "",
        EndPeriodLabel = "",
        PrecedingInvoiceReferenceLabel = "",
        PrecedingInvoiceDateLabel = "",
        BusinessProcessLabel = "",
        DocumentTypeNames = [],
        DefaultInvoiceDocumentsTypeName = "",
        DefaultCreditNoteDocumentsTypeName = "",
        DateLabel = "",
        BuyerAddressLabel = "",
        BuyerIdentifiersLabel = "",
        DeliveryInformationLabel = "",
        DespatchAdviceLabel = "",
        DeliveryDateLabel = "",
        ReceivingAdviceLabel = "",
        CurrencyLabel = "",
        TotalWithoutVatLabel = "",
        TotalVatLabel = "",
        TotalWithVatLabel = "",
        PrepaidAmountLabel = "",
        DueDateLabel = "",
        DueAmountLabel = "",
        DefaultLegalIdType = "",
        PageLabel = ""
    };

    /// <summary>
    ///     The default language pack associated with the StandardPdfGenerator, currently set to English.
    /// </summary>
    public static StandardPdfGeneratorLanguagePack Default => English;

    /// <summary>
    ///     The predefined English language pack for the StandardPdfGenerator containing labels and culture-specific settings.
    /// </summary>
    public static StandardPdfGeneratorLanguagePack English { get; } = new()
    {
        Culture = CultureInfo.GetCultureInfo("en-EN"),
        VatNumberLabel = "VAT N°",
        ContactLabel = "Contact",
        EmailLabel = "Email",
        SellerReferencesLabel = "Our references",
        InvoicedObjectIdentifierLabel = "Client ID",
        SalesOrderReferenceLabel = "Sales order",
        BuyerReferencesLabel = "Your references",
        CallForTenderLabel = "Tender",
        ProjectReferenceLabel = "Project",
        AccountingReferenceLabel = "Accounting",
        ContractReferenceLabel = "Contract",
        PurchaseOrderReferenceLabel = "Purchase order",
        InvoiceReferencesLabel = "Invoice references",
        StartPeriodLabel = "Period start",
        EndPeriodLabel = "Period end",
        PrecedingInvoiceReferenceLabel = "Preceding invoice",
        PrecedingInvoiceDateLabel = "Preceding invoice date",
        BusinessProcessLabel = "Business process",
        DocumentTypeNames = new Dictionary<InvoiceTypeCode, string?>
        {
            { InvoiceTypeCode.RequestForPayment, "Request for payment" },
            { InvoiceTypeCode.DebitNoteRelatedToGoodsOrServices, "Debit note related to goods or services" },
            { InvoiceTypeCode.CreditNoteRelatedToGoodsOrServices, "Credit note related to goods or services" },
            { InvoiceTypeCode.MeteredServicesInvoice, "Metered services invoice" },
            { InvoiceTypeCode.CreditNoteRelatedToFinancialAdjustments, "Credit note related to financial adjustments" },
            { InvoiceTypeCode.DebitNoteRelatedToFinancialAdjustments, "Debit note related to financial adjustments" },
            { InvoiceTypeCode.TaxNotification, "Tax notification" },
            { InvoiceTypeCode.InvoicingDataSheet, "Invoicing data sheet" },
            { InvoiceTypeCode.DirectPaymentValuation, "Direct payment valuation" },
            { InvoiceTypeCode.ProvisionalPaymentValuation, "Provisional payment valuation" },
            { InvoiceTypeCode.PaymentValuation, "Payment valuation" },
            { InvoiceTypeCode.InterimApplicationForPayment, "Interim application for payment" },
            { InvoiceTypeCode.FinalPaymentRequestBasedOnCompletionOfWork, "Final payment request based on completion of work" },
            { InvoiceTypeCode.PaymentRequestForCompletedUnits, "Payment request for completed units" },
            { InvoiceTypeCode.SelfBilledCreditNote, "Self billed credit note" },
            { InvoiceTypeCode.ConsolidatedCreditNoteGoodsAndServices, "Consolidated credit note - goods and services" },
            { InvoiceTypeCode.PriceVariationInvoice, "Price variation invoice" },
            { InvoiceTypeCode.CreditNoteForPriceVariation, "Credit note for price variation" },
            { InvoiceTypeCode.DelcredereCreditNote, "Delcredere credit note" },
            { InvoiceTypeCode.ProformaInvoice, "Proforma invoice" },
            { InvoiceTypeCode.PartialInvoice, "Partial invoice" },
            { InvoiceTypeCode.CommercialInvoiceWhichIncludesPackingList, "Commercial invoice which includes a packing list" },
            { InvoiceTypeCode.CommercialInvoice, "Commercial invoice" },
            { InvoiceTypeCode.CreditNote, "Credit note" },
            { InvoiceTypeCode.CommissionNote, "Commission note" },
            { InvoiceTypeCode.DebitNote, "Debit note" },
            { InvoiceTypeCode.CorrectedInvoice, "Corrected invoice" },
            { InvoiceTypeCode.ConsolidatedInvoice, "Consolidated invoice" },
            { InvoiceTypeCode.PrepaymentInvoice, "Prepayment invoice" },
            { InvoiceTypeCode.HireInvoice, "Hire invoice" },
            { InvoiceTypeCode.TaxInvoice, "Tax invoice" },
            { InvoiceTypeCode.SelfBilledInvoice, "Self-billed invoice" },
            { InvoiceTypeCode.DelcredereInvoice, "Delcredere invoice" },
            { InvoiceTypeCode.FactoredInvoice, "Factored invoice" },
            { InvoiceTypeCode.LeaseInvoice, "Lease invoice" },
            { InvoiceTypeCode.ConsignmentInvoice, "Consignment invoice" },
            { InvoiceTypeCode.FactoredCreditNote, "Factored credit note" },
            { InvoiceTypeCode.OcrPaymentCreditNote, "Optical Character Reading (OCR) payment credit note" },
            { InvoiceTypeCode.DebitAdvice, "Debit advice" },
            { InvoiceTypeCode.ReversalOfDebit, "Reversal of debit" },
            { InvoiceTypeCode.ReversalOfCredit, "Reversal of credit" },
            { InvoiceTypeCode.SelfBilledDebitNote, "Self billed debit note" },
            { InvoiceTypeCode.ForwardersCreditNote, "Forwarder's credit note" },
            { InvoiceTypeCode.ForwardersInvoiceDiscrepancyReport, "Forwarder's invoice discrepancy report" },
            { InvoiceTypeCode.InsurersInvoice, "Insurer's invoice" },
            { InvoiceTypeCode.ForwardersInvoice, "Forwarder's invoice" },
            { InvoiceTypeCode.PortChargesDocuments, "Port charges documents" },
            { InvoiceTypeCode.InvoiceInformationForAccountingPurposes, "Invoice information for accounting purposes" },
            { InvoiceTypeCode.FreightInvoice, "Freight invoice" },
            { InvoiceTypeCode.ClaimNotification, "Claim notification" },
            { InvoiceTypeCode.ConsularInvoice, "Consular invoice" },
            { InvoiceTypeCode.PartialConstructionInvoice, "Partial construction invoice" },
            { InvoiceTypeCode.PartialFinalConstructionInvoice, "Partial final construction invoice" },
            { InvoiceTypeCode.FinalConstructionInvoice, "Final construction invoice" },
            { InvoiceTypeCode.CustomsInvoice, "Customs invoice" }
        },
        DefaultInvoiceDocumentsTypeName = "Invoice",
        DefaultCreditNoteDocumentsTypeName = "Credit Note",
        DateLabel = "Date",
        BuyerAddressLabel = "Client address",
        BuyerIdentifiersLabel = "Your identifiers",
        DeliveryInformationLabel = "Delivery information",
        DespatchAdviceLabel = "Despatch advice",
        DeliveryDateLabel = "Delivery date",
        ReceivingAdviceLabel = "Receiving advice",
        CurrencyLabel = "Currency",
        TotalWithoutVatLabel = "Total (Net)",
        TotalVatLabel = "Total VAT",
        TotalWithVatLabel = "Total (Gross)",
        PrepaidAmountLabel = "Prepaid",
        DueDateLabel = "Due date",
        DueAmountLabel = "Due for payment",
        DefaultLegalIdType = "Legal ID",
        PageLabel = "Page"
    };

    /// <summary>
    ///     The predefined French language pack for the StandardPdfGenerator containing labels and culture-specific settings.
    /// </summary>
    public static StandardPdfGeneratorLanguagePack French { get; } = new()
    {
        Culture = CultureInfo.GetCultureInfo("fr-FR"),
        VatNumberLabel = "N° TVA",
        ContactLabel = "Contact",
        EmailLabel = "Mèl",
        SellerReferencesLabel = "Nos references",
        InvoicedObjectIdentifierLabel = "Client",
        SalesOrderReferenceLabel = "Bon de vente",
        BuyerReferencesLabel = "Vos references",
        CallForTenderLabel = "Ref appel d'offre",
        ProjectReferenceLabel = "Ref projet",
        AccountingReferenceLabel = "Ref comptable",
        ContractReferenceLabel = "Ref contrat",
        PurchaseOrderReferenceLabel = "Bon de commande",
        InvoiceReferencesLabel = "Références sur la facture",
        StartPeriodLabel = "Début période",
        EndPeriodLabel = "Fin période",
        PrecedingInvoiceReferenceLabel = "Facture associée",
        PrecedingInvoiceDateLabel = "Date de facture associée",
        BusinessProcessLabel = "Type de processus",
        DocumentTypeNames = new Dictionary<InvoiceTypeCode, string?>
        {

            { InvoiceTypeCode.RequestForPayment, "Demande de paiement" },
            { InvoiceTypeCode.DebitNoteRelatedToGoodsOrServices, "Note de débit liée aux biens ou services" },
            { InvoiceTypeCode.CreditNoteRelatedToGoodsOrServices, "Avoir liée aux biens ou services" },
            { InvoiceTypeCode.MeteredServicesInvoice, "Facture de services mesurés" },
            { InvoiceTypeCode.CreditNoteRelatedToFinancialAdjustments, "Avoir liée aux ajustements financiers" },
            { InvoiceTypeCode.DebitNoteRelatedToFinancialAdjustments, "Note de débit liée aux ajustements financiers" },
            { InvoiceTypeCode.TaxNotification, "Notification fiscale" },
            { InvoiceTypeCode.InvoicingDataSheet, "Fiche de données de facturation" },
            { InvoiceTypeCode.DirectPaymentValuation, "Évaluation de paiement direct" },
            { InvoiceTypeCode.ProvisionalPaymentValuation, "Évaluation de paiement provisoire" },
            { InvoiceTypeCode.PaymentValuation, "Évaluation de paiement" },
            { InvoiceTypeCode.InterimApplicationForPayment, "Demande de paiement intermédiaire" },
            { InvoiceTypeCode.FinalPaymentRequestBasedOnCompletionOfWork, "Demande finale de paiement basée sur l'achèvement des travaux" },
            { InvoiceTypeCode.PaymentRequestForCompletedUnits, "Demande de paiement pour unités terminées" },
            { InvoiceTypeCode.SelfBilledCreditNote, "Avoir auto-facturée" },
            { InvoiceTypeCode.ConsolidatedCreditNoteGoodsAndServices, "Avoir consolidée - biens et services" },
            { InvoiceTypeCode.PriceVariationInvoice, "Facture de variation de prix" },
            { InvoiceTypeCode.CreditNoteForPriceVariation, "Avoir pour variation de prix" },
            { InvoiceTypeCode.DelcredereCreditNote, "Avoir delcredere" },
            { InvoiceTypeCode.ProformaInvoice, "Facture proforma" },
            { InvoiceTypeCode.PartialInvoice, "Facture partielle" },
            { InvoiceTypeCode.CommercialInvoiceWhichIncludesPackingList, "Facture commerciale incluant la liste de colisage" },
            { InvoiceTypeCode.CommercialInvoice, "Facture commerciale" },
            { InvoiceTypeCode.CreditNote, "Avoir" },
            { InvoiceTypeCode.CommissionNote, "Note de commission" },
            { InvoiceTypeCode.DebitNote, "Note de débit" },
            { InvoiceTypeCode.CorrectedInvoice, "Facture corrigée" },
            { InvoiceTypeCode.ConsolidatedInvoice, "Facture consolidée" },
            { InvoiceTypeCode.PrepaymentInvoice, "Facture d'acompte" },
            { InvoiceTypeCode.HireInvoice, "Facture de location" },
            { InvoiceTypeCode.TaxInvoice, "Facture fiscale" },
            { InvoiceTypeCode.SelfBilledInvoice, "Facture auto-facturée" },
            { InvoiceTypeCode.DelcredereInvoice, "Facture delcredere" },
            { InvoiceTypeCode.FactoredInvoice, "Facture factorisée" },
            { InvoiceTypeCode.LeaseInvoice, "Facture de leasing" },
            { InvoiceTypeCode.ConsignmentInvoice, "Facture de consignation" },
            { InvoiceTypeCode.FactoredCreditNote, "Avoir factorisée" },
            { InvoiceTypeCode.OcrPaymentCreditNote, "Avoir de paiement OCR" },
            { InvoiceTypeCode.DebitAdvice, "Avis de débit" },
            { InvoiceTypeCode.ReversalOfDebit, "Annulation de débit" },
            { InvoiceTypeCode.ReversalOfCredit, "Annulation de crédit" },
            { InvoiceTypeCode.SelfBilledDebitNote, "Note de débit auto-facturée" },
            { InvoiceTypeCode.ForwardersCreditNote, "Avoir du transitaire" },
            { InvoiceTypeCode.ForwardersInvoiceDiscrepancyReport, "Rapport de disparité de facture du transitaire" },
            { InvoiceTypeCode.InsurersInvoice, "Facture de l'assureur" },
            { InvoiceTypeCode.ForwardersInvoice, "Facture du transitaire" },
            { InvoiceTypeCode.PortChargesDocuments, "Documents de frais portuaires" },
            { InvoiceTypeCode.InvoiceInformationForAccountingPurposes, "Informations de facturation à des fins comptables" },
            { InvoiceTypeCode.FreightInvoice, "Facture de fret" },
            { InvoiceTypeCode.ClaimNotification, "Notification de réclamation" },
            { InvoiceTypeCode.ConsularInvoice, "Facture consulaire" },
            { InvoiceTypeCode.PartialConstructionInvoice, "Facture partielle de construction" },
            { InvoiceTypeCode.PartialFinalConstructionInvoice, "Facture finale partielle de construction" },
            { InvoiceTypeCode.FinalConstructionInvoice, "Facture finale de construction" },
            { InvoiceTypeCode.CustomsInvoice, "Facture douanière" }
        },
        DefaultInvoiceDocumentsTypeName = "Facture",
        DefaultCreditNoteDocumentsTypeName = "Avoir",
        DateLabel = "Date",
        BuyerAddressLabel = "Adresse du client",
        BuyerIdentifiersLabel = "Vos identifiants",
        DeliveryInformationLabel = "Livraison",
        DespatchAdviceLabel = "Bon de livraison",
        DeliveryDateLabel = "Date de livraison",
        ReceivingAdviceLabel = "Bon de réception",
        CurrencyLabel = "Devise",
        TotalWithoutVatLabel = "Total HT",
        TotalVatLabel = "Total TVA",
        TotalWithVatLabel = "Total TTC",
        PrepaidAmountLabel = "Acompte",
        DueDateLabel = "Date d'échéance",
        DueAmountLabel = "Net à payer",
        DefaultLegalIdType = "Identifiant",
        PageLabel = "Page"
    };
}
