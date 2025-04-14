using System.Globalization;
using FacturXDotNet.Models.CII;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;

namespace FacturXDotNet.Generation.PDF.Generators;

/// <summary>
///     Generate PDFs that look like the model provided in the Factur-X specification.
/// </summary>
public class StandardPdfGenerator(StandardPdfGeneratorOptions? options = null) : IPdfGenerator
{
    StandardPdfGeneratorOptions _options = options ?? new StandardPdfGeneratorOptions();

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
    const int PageNumberWidth = 90;

    static readonly XColor DebugFrameColor = new() { R = 255, G = 226, B = 226 };
    static readonly XColor BlueLineBg = new() { R = 221, G = 235, B = 247 };
    static readonly XColor GreenLineBg = new() { R = 235, G = 241, B = 222 };
    static readonly XSolidBrush BlackBrush = new(XColors.DarkSlateGray);
    static readonly XSolidBrush BlueBrush = new(new XColor { R = 68, G = 84, B = 106 });
    static readonly XSolidBrush RedBrush = new(new XColor { R = 192, G = 0, B = 0 });
    static readonly XSolidBrush BrownBrush = new(new XColor { R = 131, G = 60, B = 12 });

    const string FontName = "Verdana";
    static readonly XFont SmallFont;
    static readonly XFont NormalFont;
    static readonly XFont NormalBoldFont;
    static readonly XFont BigFont;

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
    static readonly XRect LegalMentionsTableRect;
    static readonly XRect TotalAmountsTableRect;
    static readonly XRect PrepaidAmountRect;
    static readonly XRect DueDateRect;
    static readonly XRect DueAmountRect;

    static readonly XRect PayeeRectColumn1;
    static readonly XRect PayeeRectColumn2;

    static readonly XRect FooterRect;
    static readonly XRect PageRect;

    static StandardPdfGenerator()
    {
        GlobalFontSettings.UseWindowsFontsUnderWindows = true;
        SmallFont = new XFont(FontName, 4);
        NormalFont = new XFont(FontName, 6);
        NormalBoldFont = new XFont(FontName, 6, XFontStyleEx.Bold);
        BigFont = new XFont(FontName, 9);

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
        LegalMentionsTableRect = new XRect(LeftMarginPt, TopMarginPt + 58 * LineHeight, columnWidth, 4 * LineHeight);
        TotalAmountsTableRect = new XRect(LeftMarginPt + columnWidth + 2 * TwoMarginPaddings, TopMarginPt + 58 * LineHeight, columnWidth, 4 * LineHeight);
        PrepaidAmountRect = new XRect(LeftMarginPt + columnWidth + 2 * TwoMarginPaddings, TopMarginPt + 62 * LineHeight, columnWidth, LineHeight);
        DueDateRect = new XRect(LeftMarginPt, TopMarginPt + 63 * LineHeight, columnWidth, 2 * LineHeight);
        DueAmountRect = new XRect(LeftMarginPt + columnWidth + 2 * TwoMarginPaddings, TopMarginPt + 63 * LineHeight, columnWidth, 2 * LineHeight);

        PayeeRectColumn1 = new XRect(LeftMarginPt, TopMarginPt + 66 * LineHeight, columnWidth, 4 * LineHeight);
        PayeeRectColumn2 = new XRect(LeftMarginPt + columnWidth, TopMarginPt + 66 * LineHeight, columnWidth + 2 * TwoMarginPaddings, 4 * LineHeight);

        FooterRect = new XRect(LeftMarginPt, TopMarginPt + 71 * LineHeight, ContentWidth - PageNumberWidth, 2 * LineHeight);
        PageRect = new XRect(LeftMarginPt + ContentWidth - PageNumberWidth, TopMarginPt + 71 * LineHeight, PageNumberWidth, 2 * LineHeight);
    }

    /// <inheritdoc />
    public PdfDocument Build(CrossIndustryInvoice invoice)
    {
        PdfDocument document = new();

        PdfPage page = document.AddPage();
        page.Size = PageSize;

#if DEBUG
        DrawDebugFrames(page);
#endif
        CellDrawer.Create(page, SellerInfoRect, 0).Background(BlueLineBg);
        CellDrawer.Create(page, SellerInfoRect, 1)
            .Background(GreenLineBg)
            .Text(invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.Name, RedBrush);
        CellDrawer.Create(page, SellerInfoRect, 2).Background(BlueLineBg);
        CellDrawer.Create(page, SellerInfoRect, 3)
            .Background(GreenLineBg)
            .Text(invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress?.CountryId, RedBrush);
        CellDrawer.Create(page, SellerInfoRect, 6).Background(BlueLineBg);
        string sellerLegalIdType = GetLegalIdType(invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization?.IdSchemeId);
        CellDrawer.Create(page, SellerInfoRect, 7)
            .Background(GreenLineBg)
            .KeyValue($"{sellerLegalIdType}: ", invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization?.Id, RedBrush);
        CellDrawer.Create(page, SellerInfoRect, 8)
            .Background(GreenLineBg)
            .KeyValue(
                $"{_options.LanguagePack.VatNumberLabel}: ",
                invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.Id,
                RedBrush
            );

        CellDrawer.Create(page, SellerReferencesRect, 0).Text(_options.LanguagePack.OurReferencesLabel, font: NormalBoldFont);

        CellDrawer.Create(page, BuyerReferencesRect, 0).Text(_options.LanguagePack.YourReferencesLabel, font: NormalBoldFont);
        CellDrawer.Create(page, BuyerReferencesRect, 1)
            .Background(GreenLineBg)
            .Text(invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerReference, BlueBrush);
        CellDrawer.Create(page, BuyerReferencesRect, 4).Background(BlueLineBg);
        CellDrawer.Create(page, BuyerReferencesRect, 5).Background(BlueLineBg);
        CellDrawer.Create(page, BuyerReferencesRect, 6)
            .Background(GreenLineBg)
            .KeyValue(
                $"{_options.LanguagePack.OrderLabel}: ",
                invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerOrderReferencedDocument?.IssuerAssignedId,
                BlueBrush
            );

        CellDrawer.Create(page, InvoiceReferencesRect, 0).Text(_options.LanguagePack.InvoiceReferencesLabel, font: NormalBoldFont);
        CellDrawer.Create(page, InvoiceReferencesRect, 3).Background(BlueLineBg);
        CellDrawer.Create(page, InvoiceReferencesRect, 4).Background(BlueLineBg);
        CellDrawer.Create(page, InvoiceReferencesRect, 5)
            .Background(GreenLineBg)
            .KeyValue($"{_options.LanguagePack.BusinessProcessLabel}: ", invoice.ExchangedDocumentContext?.BusinessProcessSpecifiedDocumentContextParameterId, BlueBrush);

        string documentTypeName = invoice.ExchangedDocument?.TypeCode == null
            ? _options.LanguagePack.DefaultDocumentTypeName
            : _options.LanguagePack.DocumentTypeNames.GetValueOrDefault(invoice.ExchangedDocument.TypeCode.Value) ?? _options.LanguagePack.DefaultDocumentTypeName;
        int? typeCode = invoice.ExchangedDocument?.TypeCode?.ToSpecificationIdentifier();
        string documentNameAndTypeCode = typeCode is not null ? $"{documentTypeName} ({typeCode})" : documentTypeName;
        CellDrawer.Create(page, DocumentInfoRect, 0).Background(GreenLineBg).Text($"{documentNameAndTypeCode}", font: NormalBoldFont);
        CellDrawer.Create(page, DocumentInfoRect, 1).Background(GreenLineBg).KeyValue("N° ", invoice.ExchangedDocument?.Id, RedBrush);

        CellDrawer.Create(page, DocumentInfoRect, 2)
            .KeyValue($"{_options.LanguagePack.DateLabel}: ", invoice.ExchangedDocument?.IssueDateTime?.ToString("d", _options.LanguagePack.Culture), RedBrush);

        CellDrawer.Create(page, BuyerInfoRect, 0).Text(_options.LanguagePack.ClientAddressLabel, font: NormalBoldFont);
        CellDrawer.Create(page, BuyerInfoRect, 1).Background(BlueLineBg);
        CellDrawer.Create(page, BuyerInfoRect, 2)
            .Background(GreenLineBg)
            .Text(invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.Name, RedBrush);
        CellDrawer.Create(page, BuyerInfoRect, 4).Background(BlueLineBg);
        CellDrawer.Create(page, BuyerInfoRect, 5).Background(BlueLineBg);
        CellDrawer.Create(page, BuyerInfoRect, 6).Background(BlueLineBg);
        CellDrawer.Create(page, BuyerInfoRect, 7).Background(BlueLineBg);
        CellDrawer.Create(page, BuyerInfoRect, 8).Background(GreenLineBg);

        CellDrawer.Create(page, BuyerIdentifiersRect, 0).Text(_options.LanguagePack.YourIdentifiersLabel, font: NormalBoldFont);
        string buyerLegalIdType = GetLegalIdType(invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization?.IdSchemeId);
        CellDrawer.Create(page, BuyerIdentifiersRect, 2)
            .Background(GreenLineBg)
            .KeyValue($"{buyerLegalIdType}: ", invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization?.Id, BrownBrush);
        CellDrawer.Create(page, BuyerIdentifiersRect, 3).Background(BlueLineBg);

        CellDrawer.Create(page, DeliveryInformationIdentifiersRect, 0).Text(_options.LanguagePack.DeliveryInformationLabel, font: NormalBoldFont);
        CellDrawer.Create(page, DeliveryInformationIdentifiersRect, 8).Background(BlueLineBg);
        CellDrawer.Create(page, DeliveryInformationIdentifiersRect, 9).Background(BlueLineBg);

        CellDrawer.Create(page, CurrencyRect)
            .Background(GreenLineBg)
            .KeyValue($"{_options.LanguagePack.CurrencyLabel}: ", invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode, RedBrush);

        CellDrawer.Create(page, BottomMarginRect).Text(_options.LanguagePack.WipLabel, font: SmallFont, format: XStringFormats.Center);

        return document;
    }

    string GetLegalIdType(string? idSchemeId) =>
        idSchemeId switch
        {
            "0002" => "SIREN",
            "0009" => "SIRET",
            _ => _options.LanguagePack.DefaultLegalIdType
        };

    static void DrawDebugFrames(PdfPage page)
    {
        using XGraphics gfx = XGraphics.FromPdfPage(page);

        DrawDebugFrame(gfx, TopMarginRect);
        DrawDebugFrame(gfx, BottomMarginRect);
        DrawDebugFrame(gfx, LeftMarginRect);
        DrawDebugFrame(gfx, RightMarginRect);
        DrawDebugFrame(gfx, TopLeftMarginRect);
        DrawDebugFrame(gfx, TopRightMarginRect);
        DrawDebugFrame(gfx, BottomLeftMarginRect);
        DrawDebugFrame(gfx, BottomRightMarginRect);
        DrawDebugFrame(gfx, ContentRect);

        DrawDebugFrame(gfx, SellerLogoRect);
        DrawDebugFrame(gfx, SellerInfoRect);
        DrawDebugFrame(gfx, SellerTaxRepresentativeInfoRect);
        DrawDebugFrame(gfx, SellerReferencesRect);
        DrawDebugFrame(gfx, BuyerReferencesRect);
        DrawDebugFrame(gfx, InvoiceReferencesRect);

        DrawDebugFrame(gfx, DocumentInfoRect);
        DrawDebugFrame(gfx, BuyerInfoRect);
        DrawDebugFrame(gfx, BuyerContactInfoRect);
        DrawDebugFrame(gfx, BuyerIdentifiersRect);
        DrawDebugFrame(gfx, DeliveryInformationIdentifiersRect);
        DrawDebugFrame(gfx, CurrencyRect);

        DrawDebugFrame(gfx, LinesTableRect);
        DrawDebugFrame(gfx, AdjustmentsTableRect);
        DrawDebugFrame(gfx, VatBreakdownTableRect);
        DrawDebugFrame(gfx, LegalMentionsTableRect);
        DrawDebugFrame(gfx, TotalAmountsTableRect);
        DrawDebugFrame(gfx, PrepaidAmountRect);
        DrawDebugFrame(gfx, DueDateRect);
        DrawDebugFrame(gfx, DueAmountRect);

        DrawDebugFrame(gfx, PayeeRectColumn1);
        DrawDebugFrame(gfx, PayeeRectColumn2);

        DrawDebugFrame(gfx, FooterRect);
        DrawDebugFrame(gfx, PageRect);
    }

    static void DrawDebugFrame(XGraphics gfx, XRect rect) => gfx.DrawRectangle(new XPen(DebugFrameColor, 1), rect);

    class CellDrawer(PdfPage page, XRect rect)
    {
        public CellDrawer Background(XColor color)
        {
            using XGraphics gfx = XGraphics.FromPdfPage(page);
            gfx.DrawRectangle(new XSolidBrush(color), rect);
            return this;
        }

        public CellDrawer Text(string? text, XBrush? brush = null, XFont? font = null, XStringFormat? format = null)
        {
            if (text is not null)
            {
                using XGraphics gfx = XGraphics.FromPdfPage(page);
                gfx.DrawString(
                    text,
                    font ?? NormalFont,
                    brush ?? BlackBrush,
                    new XRect(rect.X + MarginPadding, rect.Y, rect.Width - TwoMarginPaddings, rect.Height),
                    format ?? XStringFormats.CenterLeft
                );
            }

            return this;
        }

        public CellDrawer KeyValue(string key, string? value, XBrush? brush = null, XFont? font = null, XStringFormat? format = null)
        {
            if (value is not null)
            {
                key = key.EndsWith(' ') ? key : $"{key} ";

                XSize keySize;
                using (XGraphics gfx = XGraphics.FromPdfPage(page))
                {
                    keySize = gfx.MeasureString(key, font ?? NormalFont);
                    gfx.DrawString(key, font ?? NormalFont, BlackBrush, new XRect(rect.X + MarginPadding, rect.Y, keySize.Width, rect.Height), format ?? XStringFormats.CenterLeft);
                }

                using (XGraphics gfx = XGraphics.FromPdfPage(page))
                {
                    gfx.DrawString(
                        value,
                        font ?? NormalFont,
                        brush ?? BlackBrush,
                        new XRect(rect.X + MarginPadding + keySize.Width, rect.Y, rect.Width - keySize.Width - TwoMarginPaddings, rect.Height),
                        format ?? XStringFormats.CenterLeft
                    );
                }
            }

            return this;
        }

        public static CellDrawer Create(PdfPage page, XRect rect) => new(page, rect);

        public static CellDrawer Create(PdfPage page, XRect rect, int line, double lineHeight = LineHeight) =>
            new(page, new XRect(rect.X, rect.Y + line * lineHeight, rect.Width, lineHeight));
    }
}

/// <summary>
///     Provides configuration options for generating standard PDFs.
/// </summary>
public class StandardPdfGeneratorOptions
{
    /// <summary>
    ///     The logo to be displayed in the generated PDF document as a byte array.
    /// </summary>
    public ReadOnlyMemory<byte>? Logo { get; set; }

    /// <summary>
    ///     The language pack that contains localized resources for generating the standard PDF.
    /// </summary>
    public StandardPdfGeneratorLanguagePack LanguagePack { get; set; } = StandardPdfGeneratorLanguagePack.English;
}

/// <summary>
///     Represents a language pack containing localized resources used by the StandardPdfGenerator.
/// </summary>
public class StandardPdfGeneratorLanguagePack
{
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
    ///     The default label or identifier used to represent the legal ID type in the language pack for the StandardPdfGenerator.
    /// </summary>
    public required string DefaultLegalIdType { get; init; }

    /// <summary>
    ///     The label indicating that the PDF document is in progress, generated with FacturX.NET, and subject to further changes.
    /// </summary>
    public required string WipLabel { get; init; }

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
        DefaultLegalIdType = "Legal ID",
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
        DefaultLegalIdType = "Identifiant",
        WipLabel = $"Ce PDF a été généré par FacturX.NET v{BuildInformation.Version.WithoutPrereleaseOrMetadata()} et est toujours en cours de construction."
    };
}
