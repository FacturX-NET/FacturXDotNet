using FacturXDotNet.Models.CII;
using NMoneys;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;

namespace FacturXDotNet.Generation.PDF.Generators.Standard;

/// <summary>
///     Generate PDFs that look like the model provided in the Factur-X specification.
/// </summary>
public partial class StandardPdfGenerator(StandardPdfGeneratorOptions? options = null) : IPdfGenerator
{
    readonly StandardPdfGeneratorOptions _options = options
                                                    ?? new StandardPdfGeneratorOptions
                                                    {
                                                        Footer = """
                                                                 Ma société. Société anonyme au capital de xx.xxx EUROS - R.C.S. MAVILLE 123 456 789 - NAF ZZZZZ
                                                                 136 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789
                                                                 """
                                                    };

    const PageSize PageSize = PdfSharp.PageSize.A4;
    const int Width = 595;
    const int Height = 842;
    const int TopMarginPt = 20;
    const int BottomMarginPt = 15;
    const int LeftMarginPt = 90;
    const int RightMarginPt = 90;

    const int MarginPadding = 4;
    const int TwoMarginPaddings = 2 * MarginPadding;

    const int ContentWidth = Width - LeftMarginPt - RightMarginPt;
    const int ContentHeight = Height - TopMarginPt - BottomMarginPt;

    const int LineHeight = 11;
    const int PageNumberWidth = 45;

    static readonly XColor BlueLineBg = new() { R = 221, G = 235, B = 247 };
    static readonly XColor GreenLineBg = new() { R = 235, G = 241, B = 222 };
    static readonly XSolidBrush BlackBrush = new(XColors.DarkSlateGray);
    static readonly XSolidBrush BlueBrush = new(new XColor { R = 68, G = 84, B = 106 });
    static readonly XSolidBrush RedBrush = new(new XColor { R = 192, G = 0, B = 0 });
    static readonly XSolidBrush BorderBrush = BlackBrush;

    static readonly XFont SmallFont;
    static readonly XFont NormalFont;
    static readonly XFont NormalBoldFont;
    static readonly XFont BigFont;
    static readonly XFont BigBoldFont;
    static readonly XFont HugeBoldFont;

    static readonly XRect TopMarginRect;
    static readonly XRect BottomMarginRect;
    static readonly XRect LeftMarginRect;
    static readonly XRect RightMarginRect;
    static readonly XRect TopLeftMarginRect;
    static readonly XRect TopRightMarginRect;
    static readonly XRect BottomLeftMarginRect;
    static readonly XRect BottomRightMarginRect;
    static readonly XRect ContentRect;
    static readonly XRect SellerLogoRect;
    static readonly XRect SellerInfoRect;
    static readonly XRect SellerTaxRepresentativeInfoRect;
    static readonly XRect SellerReferencesRect;
    static readonly XRect BuyerReferencesRect;
    static readonly XRect InvoiceReferencesRect;

    static readonly XRect DocumentInfoRect;
    static readonly XRect BuyerInfoRect;
    static readonly XRect BuyerContactInfoRect;
    static readonly XRect BuyerIdentifiersRect;
    static readonly XRect DeliveryInformationIdentifiersRect;
    static readonly XRect CurrencyRect;

    static readonly XRect LinesTableRect;
    static readonly XRect AdjustmentsTableRect;
    static readonly XRect VatBreakdownTableRect;
    static readonly XRect PaymentTermsRect;
    static readonly XRect TotalAmountsTableRect;
    static readonly XRect PrepaidAmountRect;
    static readonly XRect DueDateRect;
    static readonly XRect DueAmountRect;

    static readonly XRect PayeeRectColumn1;
    static readonly XRect PayeeRectColumn2;

    static readonly XRect FooterRect;
    static readonly XRect LegalMentionsRect;
    static readonly XRect PageRect;

    static StandardPdfGenerator()
    {
        GlobalFontSettings.FontResolver = new RobotoFontResolver();
        SmallFont = new XFont("Roboto", 4);
        NormalFont = new XFont("Roboto", 6);
        NormalBoldFont = new XFont("Roboto", 6, XFontStyleEx.Bold);
        BigFont = new XFont("Roboto", 8);
        BigBoldFont = new XFont("Roboto", 8, XFontStyleEx.Bold);
        HugeBoldFont = new XFont("Roboto", 11, XFontStyleEx.Bold);

        TopMarginRect = new XRect(LeftMarginPt, MarginPadding, ContentWidth, TopMarginPt - TwoMarginPaddings);
        BottomMarginRect = new XRect(LeftMarginPt, Height - BottomMarginPt + MarginPadding, ContentWidth, BottomMarginPt - TwoMarginPaddings);
        LeftMarginRect = new XRect(MarginPadding, TopMarginPt, LeftMarginPt - TwoMarginPaddings, ContentHeight);
        RightMarginRect = new XRect(Width - RightMarginPt + MarginPadding, TopMarginPt, RightMarginPt - TwoMarginPaddings, ContentHeight);
        TopLeftMarginRect = new XRect(MarginPadding, MarginPadding, LeftMarginPt - TwoMarginPaddings, TopMarginPt - TwoMarginPaddings);
        TopRightMarginRect = new XRect(Width - RightMarginPt + MarginPadding, MarginPadding, RightMarginPt - TwoMarginPaddings, TopMarginPt - TwoMarginPaddings);
        BottomLeftMarginRect = new XRect(MarginPadding, Height - BottomMarginPt + MarginPadding, LeftMarginPt - TwoMarginPaddings, BottomMarginPt - TwoMarginPaddings);
        BottomRightMarginRect = new XRect(
            Width - RightMarginPt + MarginPadding,
            Height - BottomMarginPt + MarginPadding,
            RightMarginPt - TwoMarginPaddings,
            BottomMarginPt - TwoMarginPaddings
        );
        ContentRect = new XRect(LeftMarginPt, TopMarginPt, ContentWidth, ContentHeight);

        const int columnWidth = ContentWidth / 2 - TwoMarginPaddings;

        SellerLogoRect = new XRect(LeftMarginPt, TopMarginPt, columnWidth, 3 * LineHeight);
        SellerInfoRect = new XRect(LeftMarginPt, TopMarginPt + 3 * LineHeight, columnWidth, 9 * LineHeight);
        SellerTaxRepresentativeInfoRect = new XRect(LeftMarginPt, TopMarginPt + 12 * LineHeight, columnWidth, 5 * LineHeight);
        SellerReferencesRect = new XRect(LeftMarginPt, TopMarginPt + 18 * LineHeight, columnWidth, 4 * LineHeight);
        BuyerReferencesRect = new XRect(LeftMarginPt, TopMarginPt + 23 * LineHeight, columnWidth, 7 * LineHeight);
        InvoiceReferencesRect = new XRect(LeftMarginPt, TopMarginPt + 31 * LineHeight, columnWidth, 6 * LineHeight);

        DocumentInfoRect = new XRect(LeftMarginPt + columnWidth + 2 * TwoMarginPaddings, TopMarginPt, columnWidth, 3 * LineHeight);
        BuyerInfoRect = new XRect(LeftMarginPt + columnWidth + 2 * TwoMarginPaddings, TopMarginPt + 4 * LineHeight, columnWidth, 9 * LineHeight);
        BuyerContactInfoRect = new XRect(LeftMarginPt + columnWidth + 2 * TwoMarginPaddings, TopMarginPt + 14 * LineHeight, columnWidth, LineHeight);
        BuyerIdentifiersRect = new XRect(LeftMarginPt + columnWidth + 2 * TwoMarginPaddings, TopMarginPt + 18 * LineHeight, columnWidth, 4 * LineHeight);
        DeliveryInformationIdentifiersRect = new XRect(LeftMarginPt + columnWidth + 2 * TwoMarginPaddings, TopMarginPt + 23 * LineHeight, columnWidth, 11 * LineHeight);
        CurrencyRect = new XRect(LeftMarginPt + columnWidth + 2 * TwoMarginPaddings, TopMarginPt + 36 * LineHeight, columnWidth, LineHeight);

        LinesTableRect = new XRect(LeftMarginPt, TopMarginPt + 38 * LineHeight, ContentWidth, 10 * LineHeight);
        AdjustmentsTableRect = new XRect(LeftMarginPt, TopMarginPt + 49 * LineHeight, ContentWidth, 2 * LineHeight);
        VatBreakdownTableRect = new XRect(LeftMarginPt, TopMarginPt + 52 * LineHeight, ContentWidth, 5 * LineHeight);
        PaymentTermsRect = new XRect(LeftMarginPt, TopMarginPt + 58 * LineHeight, columnWidth, 4 * LineHeight);
        TotalAmountsTableRect = new XRect(LeftMarginPt + columnWidth + 2 * TwoMarginPaddings, TopMarginPt + 58 * LineHeight, columnWidth, 4 * LineHeight);
        PrepaidAmountRect = new XRect(LeftMarginPt + columnWidth + 2 * TwoMarginPaddings, TopMarginPt + 62 * LineHeight, columnWidth, LineHeight);
        DueDateRect = new XRect(LeftMarginPt, TopMarginPt + 63 * LineHeight, columnWidth, 2 * LineHeight);
        DueAmountRect = new XRect(LeftMarginPt + columnWidth + 2 * TwoMarginPaddings, TopMarginPt + 63 * LineHeight, columnWidth, 2 * LineHeight);

        PayeeRectColumn1 = new XRect(LeftMarginPt, TopMarginPt + 66 * LineHeight, columnWidth, 4 * LineHeight);
        PayeeRectColumn2 = new XRect(LeftMarginPt + columnWidth, TopMarginPt + 66 * LineHeight, columnWidth + 2 * TwoMarginPaddings, 4 * LineHeight);

        FooterRect = new XRect(LeftMarginPt, TopMarginPt + 71 * LineHeight, ContentWidth, 2 * LineHeight);
        LegalMentionsRect = new XRect(LeftMarginPt, TopMarginPt + 71 * LineHeight + 1, ContentWidth - PageNumberWidth, 2 * LineHeight);
        PageRect = new XRect(LeftMarginPt + ContentWidth - PageNumberWidth, TopMarginPt + 71 * LineHeight + 1, PageNumberWidth, 2 * LineHeight);
    }

    /// <inheritdoc />
    public PdfDocument Build(CrossIndustryInvoice invoice)
    {
        PdfDocument document = new();

        PdfPage page = document.AddPage();
        page.Size = PageSize;

        CellDrawer.Create(page, SellerLogoRect).Image(_options.Logo);

        CellDrawer.Create(page, SellerInfoRect, 0).Background(BlueLineBg);
        CellDrawer.Create(page, SellerInfoRect, 1)
            .Background(GreenLineBg)
            .Text(invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.Name, RedBrush);
        CellDrawer.Create(page, SellerInfoRect, 2).Background(BlueLineBg);
        CellDrawer.Create(page, SellerInfoRect, 3)
            .Background(GreenLineBg)
            .Text(invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress?.CountryId, RedBrush);
        CellDrawer.Create(page, SellerInfoRect, 4).Key(_options.LanguagePack.ContactLabel);
        CellDrawer.Create(page, SellerInfoRect, 5).Key(_options.LanguagePack.EmailLabel);
        CellDrawer.Create(page, SellerInfoRect, 6).Background(BlueLineBg);
        string sellerLegalIdType = GetLegalIdType(invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization?.IdSchemeId);
        string? sellerLegalId = FormatLegalId(
            invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization?.Id,
            invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization?.IdSchemeId
        );
        CellDrawer.Create(page, SellerInfoRect, 7).Background(GreenLineBg).Key(sellerLegalIdType).Text(sellerLegalId, RedBrush, NormalFont);
        string? sellerVatId = FormatVatId(invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.Id);
        CellDrawer.Create(page, SellerInfoRect, 8).Background(GreenLineBg).Key(_options.LanguagePack.VatNumberLabel).Text(sellerVatId, RedBrush, NormalFont);
        CellDrawer.Create(page, SellerInfoRect).DrawLeftBorder(BorderBrush);

        CellDrawer.Create(page, SellerReferencesRect, 0).Text(_options.LanguagePack.SellerReferencesLabel, font: NormalBoldFont);
        CellDrawer.Create(page, SellerReferencesRect, 1).Key(_options.LanguagePack.InvoicedObjectIdentifierLabel);
        CellDrawer.Create(page, SellerReferencesRect, 2).Key(_options.LanguagePack.SalesOrderReferenceLabel);
        CellDrawer.Create(page, SellerReferencesRect).DrawLeftBorder(BorderBrush);

        CellDrawer.Create(page, BuyerReferencesRect, 0).Text(_options.LanguagePack.BuyerReferencesLabel, font: NormalBoldFont);
        CellDrawer.Create(page, BuyerReferencesRect, 1)
            .Background(GreenLineBg)
            .Text(invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerReference, BlueBrush);
        CellDrawer.Create(page, BuyerReferencesRect, 2).Key(_options.LanguagePack.CallForTenderLabel);
        CellDrawer.Create(page, BuyerReferencesRect, 3).Key(_options.LanguagePack.ProjectReferenceLabel);
        CellDrawer.Create(page, BuyerReferencesRect, 4).Background(BlueLineBg).Key(_options.LanguagePack.AccountingReferenceLabel);
        CellDrawer.Create(page, BuyerReferencesRect, 5).Background(BlueLineBg).Key(_options.LanguagePack.ContractReferenceLabel);
        CellDrawer.Create(page, BuyerReferencesRect, 6)
            .Background(GreenLineBg)
            .Key(_options.LanguagePack.PurchaseOrderReferenceLabel)
            .Text(invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerOrderReferencedDocument?.IssuerAssignedId, BlueBrush, NormalFont);
        CellDrawer.Create(page, BuyerReferencesRect).DrawLeftBorder(BorderBrush);

        CellDrawer.Create(page, InvoiceReferencesRect, 0).Text(_options.LanguagePack.InvoiceReferencesLabel, font: NormalBoldFont);
        CellDrawer.Create(page, InvoiceReferencesRect, 1).Background(BlueLineBg).Key(_options.LanguagePack.StartPeriodLabel);
        CellDrawer.Create(page, InvoiceReferencesRect, 2).Background(BlueLineBg).Key(_options.LanguagePack.EndPeriodLabel);
        CellDrawer.Create(page, InvoiceReferencesRect, 3).Background(BlueLineBg).Key(_options.LanguagePack.PrecedingInvoiceReferenceLabel);
        CellDrawer.Create(page, InvoiceReferencesRect, 4).Background(BlueLineBg).Key(_options.LanguagePack.PrecedingInvoiceDateLabel);
        CellDrawer.Create(page, InvoiceReferencesRect, 5)
            .Background(GreenLineBg)
            .Key(_options.LanguagePack.BusinessProcessLabel)
            .Text(invoice.ExchangedDocumentContext?.BusinessProcessSpecifiedDocumentContextParameterId, BlueBrush, NormalFont);
        CellDrawer.Create(page, InvoiceReferencesRect).DrawLeftBorder(BorderBrush);

        string documentTypeName = GetDocumentName(invoice);
        CellDrawer.Create(page, DocumentInfoRect, 0).Background(GreenLineBg).Text(documentTypeName, font: BigBoldFont);
        CellDrawer.Create(page, DocumentInfoRect, 1).Background(GreenLineBg).Key("N° ", appendColon: false).Text(invoice.ExchangedDocument?.Id, RedBrush, BigFont);
        CellDrawer.Create(page, DocumentInfoRect, 2)
            .Key(_options.LanguagePack.DateLabel)
            .Text(invoice.ExchangedDocument?.IssueDateTime?.ToString("d", _options.LanguagePack.Culture), RedBrush, BigFont);
        CellDrawer.Create(page, DocumentInfoRect).DrawLeftBorder(BorderBrush);

        CellDrawer.Create(page, BuyerInfoRect, 0).Text(_options.LanguagePack.BuyerAddressLabel, font: NormalBoldFont);
        CellDrawer.Create(page, BuyerInfoRect, 1).Background(BlueLineBg).Key(_options.LanguagePack.EmailLabel);
        CellDrawer.Create(page, BuyerInfoRect, 2)
            .Background(GreenLineBg)
            .Text(invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.Name, RedBrush);
        CellDrawer.Create(page, BuyerInfoRect, 4).Background(BlueLineBg);
        CellDrawer.Create(page, BuyerInfoRect, 5).Background(BlueLineBg);
        CellDrawer.Create(page, BuyerInfoRect, 6).Background(BlueLineBg);
        CellDrawer.Create(page, BuyerInfoRect, 7).Background(BlueLineBg);
        CellDrawer.Create(page, BuyerInfoRect, 8).Background(GreenLineBg).Key(_options.LanguagePack.ContactLabel);
        CellDrawer.Create(page, BuyerInfoRect).DrawLeftBorder(BorderBrush);

        CellDrawer.Create(page, BuyerIdentifiersRect, 0).Text(_options.LanguagePack.BuyerIdentifiersLabel, font: NormalBoldFont);
        string buyerLegalIdType = GetLegalIdType(invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization?.IdSchemeId);
        string? buyerLegalId = FormatLegalId(
            invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization?.Id,
            invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization?.IdSchemeId
        );
        CellDrawer.Create(page, BuyerIdentifiersRect, 2).Background(GreenLineBg).Key(buyerLegalIdType).Text(buyerLegalId, RedBrush, NormalFont);
        CellDrawer.Create(page, BuyerIdentifiersRect, 3).Background(BlueLineBg).Key(_options.LanguagePack.VatNumberLabel);
        CellDrawer.Create(page, BuyerIdentifiersRect).DrawLeftBorder(BorderBrush);

        CellDrawer.Create(page, DeliveryInformationIdentifiersRect, 0).Text(_options.LanguagePack.DeliveryInformationLabel, font: NormalBoldFont);
        CellDrawer.Create(page, DeliveryInformationIdentifiersRect, 8).Background(BlueLineBg).Key(_options.LanguagePack.DespatchAdviceLabel);
        CellDrawer.Create(page, DeliveryInformationIdentifiersRect, 9).Background(BlueLineBg).Key(_options.LanguagePack.DeliveryDateLabel);
        CellDrawer.Create(page, DeliveryInformationIdentifiersRect, 10).Key(_options.LanguagePack.ReceivingAdviceLabel);
        CellDrawer.Create(page, DeliveryInformationIdentifiersRect).DrawLeftBorder(BorderBrush);

        CellDrawer.Create(page, CurrencyRect)
            .Background(GreenLineBg)
            .Key(_options.LanguagePack.CurrencyLabel)
            .Text(invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode, RedBrush, NormalFont);
        CellDrawer.Create(page, CurrencyRect).DrawLeftBorder(BorderBrush);

        GridDrawer vatAmountsTable = GridDrawer.Create(page, TotalAmountsTableRect, 2, 3);
        vatAmountsTable.Cell(0, 0).Text(_options.LanguagePack.TotalWithoutVatLabel, font: BigBoldFont, format: XStringFormats.Center);
        vatAmountsTable.Cell(1, 0)
            .Background(GreenLineBg)
            .Text(
                FormatMoney(
                    invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxBasisTotalAmount,
                    invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode
                ),
                RedBrush,
                BigBoldFont,
                XStringFormats.Center
            );
        vatAmountsTable.Cell(0, 1).Text(_options.LanguagePack.TotalVatLabel, font: BigBoldFont, format: XStringFormats.Center);
        vatAmountsTable.Cell(1, 1)
            .Background(GreenLineBg)
            .Text(
                FormatMoney(
                    invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount,
                    invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmountCurrencyId
                ),
                RedBrush,
                BigBoldFont,
                XStringFormats.Center
            );
        vatAmountsTable.Cell(0, 2).Text(_options.LanguagePack.TotalWithVatLabel, font: BigBoldFont, format: XStringFormats.Center);
        vatAmountsTable.Cell(1, 2)
            .Background(GreenLineBg)
            .Text(
                FormatMoney(
                    invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount,
                    invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode
                ),
                RedBrush,
                BigBoldFont,
                XStringFormats.Center
            );
        vatAmountsTable.DrawBorders(BlackBrush, 0.5);

        CellDrawer.Create(page, PrepaidAmountRect).Key(_options.LanguagePack.PrepaidAmountLabel).Text("");

        CellDrawer.Create(page, DueDateRect)
            .Background(BlueLineBg)
            .Key(_options.LanguagePack.DueDateLabel, BlackBrush, BigBoldFont)
            .Text("", BlackBrush, HugeBoldFont, XStringFormats.Center);
        CellDrawer.Create(page, DueDateRect).DrawLeftBorder(BorderBrush, 2);

        string? value = FormatMoney(
            invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount,
            invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode
        );
        CellDrawer.Create(page, DueAmountRect)
            .Background(GreenLineBg)
            .Key(_options.LanguagePack.DueAmountLabel, BlackBrush, BigBoldFont)
            .Text(value, RedBrush, HugeBoldFont, XStringFormats.Center);
        CellDrawer.Create(page, DueAmountRect).DrawLeftBorder(BorderBrush, 2);

        if (!string.IsNullOrWhiteSpace(_options.Footer))
        {
            string[] footerLines = _options.Footer.ReplaceLineEndings().Split(Environment.NewLine);
            for (int i = 0; i < footerLines.Length; i++)
            {
                CellDrawer.Create(page, LegalMentionsRect, i, 6).Text(footerLines[i].Trim(), font: SmallFont, format: XStringFormats.Center);
            }
        }

        CellDrawer.Create(page, PageRect).Text($"{_options.LanguagePack.PageLabel} 1 / 1", format: XStringFormats.TopRight);
        CellDrawer.Create(page, FooterRect).DrawTopBorder(BorderBrush);

        CellDrawer.Create(page, BottomMarginRect)
            .Text(
                $"This document has been generated using FacturX.NET v{BuildInformation.Version.WithoutPrereleaseOrMetadata()} and is still a work in progress.",
                font: SmallFont,
                format: XStringFormats.Center
            );

        return document;
    }

    string GetDocumentName(CrossIndustryInvoice invoice)
    {
        if (invoice.ExchangedDocument?.TypeCode == null)
        {
            return _options.LanguagePack.DefaultInvoiceDocumentsTypeName;
        }

        InvoiceTypeCode typeCode = invoice.ExchangedDocument.TypeCode.Value;
        string? typeName = _options.LanguagePack.DocumentTypeNames.GetValueOrDefault(typeCode);
        if (!string.IsNullOrWhiteSpace(typeName))
        {
            return typeName;
        }

        return typeCode switch
        {
            InvoiceTypeCode.CreditNote or InvoiceTypeCode.CreditNoteForPriceVariation or InvoiceTypeCode.CreditNoteRelatedToFinancialAdjustments
                or InvoiceTypeCode.CreditNoteRelatedToGoodsOrServices => _options.LanguagePack.DefaultCreditNoteDocumentsTypeName,
            _ => _options.LanguagePack.DefaultInvoiceDocumentsTypeName
        };
    }

    string GetLegalIdType(string? idSchemeId) =>
        idSchemeId switch
        {
            "0002" => "SIREN",
            "0009" => "SIRET",
            _ => _options.LanguagePack.DefaultLegalIdType
        };

    static string? FormatLegalId(string? id, string? idSchemeId)
    {
        if (id == null)
        {
            return null;
        }

        return idSchemeId switch
        {
            "0002" => FormatSiren(id),
            "0009" => FormatSiret(id),
            _ => id
        };
    }

    static string? FormatSiren(string? id) =>
        id == null
            ? null
            : id.Length != 9
                ? id
                : $"{id[..3]} {id[3..6]} {id[6..9]}";

    static string? FormatSiret(string? id) =>
        id == null
            ? null
            : id.Length != 14
                ? id
                : $"{id[..3]} {id[3..5]} {id[5..8]} {id[8..14]}";

    static string? FormatVatId(string? id) =>
        id == null
            ? null
            : id[..2] switch
            {
                "FR" => $"{id[..2]} {id[2..4]} {id[4..7]} {id[7..10]} {id[10..13]}",
                _ => id
            };

    static string? FormatMoney(decimal? amount, string? currencyCode)
    {
        if (!amount.HasValue)
        {
            return null;
        }

        Money money = new(amount.Value, currencyCode ?? "XXX");
        return money.ToString();
    }
}
