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
        SmallFont = new XFont(FontName, 6);
        NormalFont = new XFont(FontName, 8);
        NormalBoldFont = new XFont(FontName, 8, XFontStyleEx.Bold);
        BigFont = new XFont(FontName, 11);

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
            .KeyValue("VAT ID: ", invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.Id, RedBrush);

        CellDrawer.Create(page, SellerReferencesRect, 0).Text("Our references", font: NormalBoldFont);

        CellDrawer.Create(page, BuyerReferencesRect, 0).Text("Your references", font: NormalBoldFont);
        CellDrawer.Create(page, BuyerReferencesRect, 1)
            .Background(GreenLineBg)
            .Text(invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerReference, BlueBrush);
        CellDrawer.Create(page, BuyerReferencesRect, 4).Background(BlueLineBg);
        CellDrawer.Create(page, BuyerReferencesRect, 5).Background(BlueLineBg);
        CellDrawer.Create(page, BuyerReferencesRect, 6)
            .Background(GreenLineBg)
            .KeyValue("Order: ", invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerOrderReferencedDocument?.IssuerAssignedId, BlueBrush);

        CellDrawer.Create(page, InvoiceReferencesRect, 0).Text("Invoice references", font: NormalBoldFont);
        CellDrawer.Create(page, InvoiceReferencesRect, 3).Background(BlueLineBg);
        CellDrawer.Create(page, InvoiceReferencesRect, 4).Background(BlueLineBg);
        CellDrawer.Create(page, InvoiceReferencesRect, 5)
            .Background(GreenLineBg)
            .KeyValue("Business process: ", invoice.ExchangedDocumentContext?.BusinessProcessSpecifiedDocumentContextParameterId, BlueBrush);

        string documentName = invoice.ExchangedDocument?.TypeCode?.ToDocumentName() ?? "Invoice";
        int? typeCode = invoice.ExchangedDocument?.TypeCode?.ToSpecificationIdentifier();
        string documentNameAndTypeCode = typeCode is not null ? $"{documentName} ({typeCode})" : documentName;
        CellDrawer.Create(page, DocumentInfoRect, 0).Background(GreenLineBg).Text($"{documentNameAndTypeCode}", font: NormalBoldFont);
        CellDrawer.Create(page, DocumentInfoRect, 1).Background(GreenLineBg).KeyValue("N° ", invoice.ExchangedDocument?.Id, RedBrush);

        CellDrawer.Create(page, DocumentInfoRect, 2).KeyValue("Date: ", invoice.ExchangedDocument?.IssueDateTime?.ToString("d"), RedBrush);

        CellDrawer.Create(page, BuyerInfoRect, 0).Text("Client address", font: NormalBoldFont);
        CellDrawer.Create(page, BuyerInfoRect, 1).Background(BlueLineBg);
        CellDrawer.Create(page, BuyerInfoRect, 2)
            .Background(GreenLineBg)
            .Text(invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.Name, RedBrush);
        CellDrawer.Create(page, BuyerInfoRect, 4).Background(BlueLineBg);
        CellDrawer.Create(page, BuyerInfoRect, 5).Background(BlueLineBg);
        CellDrawer.Create(page, BuyerInfoRect, 6).Background(BlueLineBg);
        CellDrawer.Create(page, BuyerInfoRect, 7).Background(BlueLineBg);
        CellDrawer.Create(page, BuyerInfoRect, 8).Background(GreenLineBg);

        CellDrawer.Create(page, BuyerIdentifiersRect, 0).Text("Your identifiers", font: NormalBoldFont);
        string buyerLegalIdType = GetLegalIdType(invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization?.IdSchemeId);
        CellDrawer.Create(page, BuyerIdentifiersRect, 2)
            .Background(GreenLineBg)
            .KeyValue($"{buyerLegalIdType}: ", invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization?.Id, BrownBrush);
        CellDrawer.Create(page, BuyerIdentifiersRect, 3).Background(BlueLineBg);

        CellDrawer.Create(page, DeliveryInformationIdentifiersRect, 0).Text("Delivery information", font: NormalBoldFont);
        CellDrawer.Create(page, DeliveryInformationIdentifiersRect, 8).Background(BlueLineBg);
        CellDrawer.Create(page, DeliveryInformationIdentifiersRect, 9).Background(BlueLineBg);

        CellDrawer.Create(page, CurrencyRect)
            .Background(GreenLineBg)
            .KeyValue("Currency: ", invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode, RedBrush);

        CellDrawer.Create(page, BottomMarginRect)
            .Text(
                $"This PDF has been generated using FacturX.NET v{BuildInformation.Version.WithoutPrereleaseOrMetadata()} and is still a work in progress.",
                font: SmallFont,
                format: XStringFormats.Center
            );

        return document;
    }

    static string GetLegalIdType(string? idSchemeId) =>
        idSchemeId switch
        {
            "0002" => "SIREN",
            "0009" => "SIRET",
            _ => "Legal ID"
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
                gfx.DrawString(text, font ?? NormalFont, brush ?? BlackBrush, rect, format ?? XStringFormats.CenterLeft);
            }

            return this;
        }

        public CellDrawer KeyValue(string key, string? value, XBrush? brush = null, XFont? font = null, XStringFormat? format = null)
        {
            if (value is not null)
            {
                XSize keySize;
                using (XGraphics gfx = XGraphics.FromPdfPage(page))
                {
                    keySize = gfx.MeasureString(key, font ?? NormalFont);
                    gfx.DrawString(key, font ?? NormalFont, BlackBrush, new XRect(rect.X, rect.Y, keySize.Width, rect.Height), format ?? XStringFormats.CenterLeft);
                }

                using (XGraphics gfx = XGraphics.FromPdfPage(page))
                {
                    gfx.DrawString(
                        value,
                        font ?? NormalFont,
                        brush ?? BlackBrush,
                        new XRect(rect.X + keySize.Width, rect.Y, rect.Width - keySize.Width, rect.Height),
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
}
