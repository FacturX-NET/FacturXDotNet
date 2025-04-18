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
    ///     The label representing "Our References" in the localized language pack, typically used to identify the document references associated with the sender.
    /// </summary>
    public string? SupplierReferencesLabel { get; init; }

    /// <summary>
    ///     The label used for the "Your references" field in the localized resources of the generator.
    /// </summary>
    public string? CustomerReferencesLabel { get; init; }

    /// <summary>
    ///     The label used for the order reference in the generator.
    /// </summary>
    public string? OrderLabel { get; init; }

    /// <summary>
    ///     The label representing invoice references, used to identify and localize these references in the generated PDF.
    /// </summary>
    public string? InvoiceReferencesLabel { get; init; }

    /// <summary>
    ///     The label used to represent a business process in the generated PDF.
    /// </summary>
    public string? BusinessProcessLabel { get; init; }

    /// <summary>
    ///     The collection of names for document types, keyed by their invoice type codes.
    /// </summary>
    public Dictionary<InvoiceTypeCode, string?>? DocumentTypeNames { get; init; }

    /// <summary>
    ///     The name of the document type used in the generator.
    /// </summary>
    public string? DefaultDocumentTypeName { get; init; }

    /// <summary>
    ///     The label representing the date field in the language pack used by the generator.
    /// </summary>
    public string? DateLabel { get; init; }

    /// <summary>
    ///     The label used to identify the client's address in the generated PDF.
    /// </summary>
    public string? CustomerAddressLabel { get; init; }

    /// <summary>
    ///     The label associated with the recipient's identifiers in the context of generating a PDF.
    /// </summary>
    public string? CustomerIdentifiersLabel { get; init; }

    /// <summary>
    ///     The label for delivery information, used to display or identify delivery-related details in the PDF document.
    /// </summary>
    public string? DeliveryInformationLabel { get; init; }

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
            SupplierReferencesLabel = pack.SupplierReferencesLabel ?? "",
            CustomerReferencesLabel = pack.CustomerReferencesLabel ?? "",
            OrderLabel = pack.OrderLabel ?? "",
            InvoiceReferencesLabel = pack.InvoiceReferencesLabel ?? "",
            BusinessProcessLabel = pack.BusinessProcessLabel ?? "",
            DocumentTypeNames = pack.DocumentTypeNames ?? [],
            DefaultDocumentTypeName = pack.DefaultDocumentTypeName ?? "",
            DateLabel = pack.DateLabel ?? "",
            CustomerAddressLabel = pack.CustomerAddressLabel ?? "",
            CustomerIdentifiersLabel = pack.CustomerIdentifiersLabel ?? "",
            DeliveryInformationLabel = pack.DeliveryInformationLabel ?? "",
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
            SupplierReferencesLabel = pack.SupplierReferencesLabel,
            CustomerReferencesLabel = pack.CustomerReferencesLabel,
            OrderLabel = pack.OrderLabel,
            InvoiceReferencesLabel = pack.InvoiceReferencesLabel,
            BusinessProcessLabel = pack.BusinessProcessLabel,
            DocumentTypeNames = pack.DocumentTypeNames,
            DefaultDocumentTypeName = pack.DefaultDocumentTypeName,
            DateLabel = pack.DateLabel,
            CustomerAddressLabel = pack.CustomerAddressLabel,
            CustomerIdentifiersLabel = pack.CustomerIdentifiersLabel,
            DeliveryInformationLabel = pack.DeliveryInformationLabel,
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
