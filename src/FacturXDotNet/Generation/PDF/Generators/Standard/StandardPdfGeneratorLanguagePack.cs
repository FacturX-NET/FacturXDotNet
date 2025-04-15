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
        OurReferencesLabel = pack.OurReferencesLabel;
        YourReferencesLabel = pack.YourReferencesLabel;
        OrderLabel = pack.OrderLabel;
        InvoiceReferencesLabel = pack.InvoiceReferencesLabel;
        BusinessProcessLabel = pack.BusinessProcessLabel;
        DocumentTypeNames = pack.DocumentTypeNames;
        DefaultDocumentTypeName = pack.DefaultDocumentTypeName;
        DateLabel = pack.DateLabel;
        ClientAddressLabel = pack.ClientAddressLabel;
        YourIdentifiersLabel = pack.YourIdentifiersLabel;
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
        WipLabel = pack.WipLabel;
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
    ///     The label representing "Our References" in the localized language pack, typically used to identify the document references associated with the sender.
    /// </summary>
    public required string OurReferencesLabel { get; init; }

    /// <summary>
    ///     The label used for the "Your references" field in the localized resources of the StandardPdfGenerator.
    /// </summary>
    public required string YourReferencesLabel { get; init; }

    /// <summary>
    ///     The label used for the order reference in the StandardPdfGenerator.
    /// </summary>
    public required string OrderLabel { get; init; }

    /// <summary>
    ///     The label representing invoice references, used to identify and localize these references in the generated PDF.
    /// </summary>
    public required string InvoiceReferencesLabel { get; init; }

    /// <summary>
    ///     The label used to represent a business process in the generated PDF.
    /// </summary>
    public required string BusinessProcessLabel { get; init; }

    /// <summary>
    ///     The collection of names for document types, keyed by their invoice type codes.
    /// </summary>
    public required Dictionary<InvoiceTypeCode, string> DocumentTypeNames { get; init; }

    /// <summary>
    ///     The name of the document type used in the StandardPdfGenerator.
    /// </summary>
    public required string DefaultDocumentTypeName { get; init; }

    /// <summary>
    ///     The label representing the date field in the language pack used by the StandardPdfGenerator.
    /// </summary>
    public required string DateLabel { get; init; }

    /// <summary>
    ///     The label used to identify the client's address in the generated PDF.
    /// </summary>
    public required string ClientAddressLabel { get; init; }

    /// <summary>
    ///     The label associated with the recipient's identifiers in the context of generating a PDF.
    /// </summary>
    public required string YourIdentifiersLabel { get; init; }

    /// <summary>
    ///     The label for delivery information, used to display or identify delivery-related details in the PDF document.
    /// </summary>
    public required string DeliveryInformationLabel { get; init; }

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
    ///     The label indicating that the PDF document is in progress, generated with FacturX.NET, and subject to further changes.
    /// </summary>
    public required string WipLabel { get; init; }

    /// <summary>
    ///     The empty instance of the StandardPdfGeneratorLanguagePack with all properties initialized to their default values.
    /// </summary>
    public static StandardPdfGeneratorLanguagePack Empty { get; } = new()
    {
        Culture = CultureInfo.InvariantCulture,
        VatNumberLabel = "",
        OurReferencesLabel = "",
        YourReferencesLabel = "",
        OrderLabel = "",
        InvoiceReferencesLabel = "",
        BusinessProcessLabel = "",
        DocumentTypeNames = [],
        DefaultDocumentTypeName = "",
        DateLabel = "",
        ClientAddressLabel = "",
        YourIdentifiersLabel = "",
        DeliveryInformationLabel = "",
        CurrencyLabel = "",
        TotalWithoutVatLabel = "",
        TotalVatLabel = "",
        TotalWithVatLabel = "",
        PrepaidAmountLabel = "",
        DueDateLabel = "",
        DueAmountLabel = "",
        DefaultLegalIdType = "",
        PageLabel = "",
        WipLabel = ""
    };

    /// <summary>
    ///     The predefined English language pack for the StandardPdfGenerator containing labels and culture-specific settings.
    /// </summary>
    public static StandardPdfGeneratorLanguagePack English { get; } = new()
    {
        Culture = CultureInfo.GetCultureInfo("en-EN"),
        VatNumberLabel = "VAT N°",
        OurReferencesLabel = "Our references",
        YourReferencesLabel = "Your references",
        OrderLabel = "Order",
        InvoiceReferencesLabel = "Invoice references",
        BusinessProcessLabel = "Business process",
        DocumentTypeNames = new Dictionary<InvoiceTypeCode, string>
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
        DefaultDocumentTypeName = "Invoice",
        DateLabel = "Date",
        ClientAddressLabel = "Client address",
        YourIdentifiersLabel = "Your identifiers",
        DeliveryInformationLabel = "Delivery information",
        CurrencyLabel = "Currency",
        TotalWithoutVatLabel = "Total net",
        TotalVatLabel = "Total VAT",
        TotalWithVatLabel = "Total gross",
        PrepaidAmountLabel = "Prepaid",
        DueDateLabel = "Due date",
        DueAmountLabel = "Due for payment",
        DefaultLegalIdType = "Legal ID",
        PageLabel = "Page",
        WipLabel = $"This PDF has been generated using FacturX.NET v{BuildInformation.Version.WithoutPrereleaseOrMetadata()} and is still a work in progress."
    };

    /// <summary>
    ///     The predefined French language pack for the StandardPdfGenerator containing labels and culture-specific settings.
    /// </summary>
    public static StandardPdfGeneratorLanguagePack French { get; } = new()
    {
        Culture = CultureInfo.GetCultureInfo("fr-FR"),
        VatNumberLabel = "N° TVA",
        OurReferencesLabel = "Nos references",
        YourReferencesLabel = "Vos references",
        OrderLabel = "Commande",
        InvoiceReferencesLabel = "Références sur la facture",
        BusinessProcessLabel = "Type de processus",
        DocumentTypeNames = new Dictionary<InvoiceTypeCode, string>
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
        DefaultDocumentTypeName = "Facture",
        DateLabel = "Date",
        ClientAddressLabel = "Adresse du client",
        YourIdentifiersLabel = "Vos identifiants",
        DeliveryInformationLabel = "Livraison",
        CurrencyLabel = "Devise",
        TotalWithoutVatLabel = "Total HT",
        TotalVatLabel = "Total TVA",
        TotalWithVatLabel = "Total TTC",
        PrepaidAmountLabel = "Acompte",
        DueDateLabel = "Date d'échéance",
        DueAmountLabel = "Net à payer",
        DefaultLegalIdType = "Identifiant",
        PageLabel = "Page",
        WipLabel = $"Ce PDF a été généré par FacturX.NET v{BuildInformation.Version.WithoutPrereleaseOrMetadata()} et est toujours en cours de construction."
    };
}
