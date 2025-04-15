using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace FacturXDotNet.Generation.PDF.Generators.Standard;

public partial class StandardPdfGenerator
{
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

        public CellDrawer KeyValue(string key, string? value, XStringFormat? format = null) => KeyValue(key, value, BlackBrush, BlackBrush, NormalFont, NormalFont, format);

        public CellDrawer KeyValue(string key, string? value, XBrush brush, XStringFormat? format = null) =>
            KeyValue(key, value, BlackBrush, brush, NormalFont, NormalFont, format);

        public CellDrawer KeyValue(string key, string? value, XFont font, XStringFormat? format = null) => KeyValue(key, value, BlackBrush, BlackBrush, NormalFont, font, format);

        public CellDrawer KeyValue(string key, string? value, XFont keyFont, XFont valueFont, XStringFormat? format = null) =>
            KeyValue(key, value, BlackBrush, BlackBrush, keyFont, valueFont, format);

        public CellDrawer KeyValue(string key, string? value, XBrush? brush, XFont? font, XStringFormat? format = null) =>
            KeyValue(key, value, BlackBrush, brush ?? BlackBrush, NormalFont, font ?? NormalFont, format);

        public CellDrawer KeyValue(string key, string? value, XBrush keyBrush, XBrush valueBrush, XFont keyFont, XFont valueFont, XStringFormat? format = null)
        {
            if (value is not null)
            {
                key = key.EndsWith(' ') ? key : $"{key} ";

                XSize keySize;
                using (XGraphics gfx = XGraphics.FromPdfPage(page))
                {
                    keySize = gfx.MeasureString(key, keyFont);
                    gfx.DrawString(key, keyFont, keyBrush, new XRect(rect.X + MarginPadding, rect.Y, keySize.Width, rect.Height), XStringFormats.CenterLeft);
                }

                using (XGraphics gfx = XGraphics.FromPdfPage(page))
                {
                    gfx.DrawString(
                        value,
                        valueFont,
                        valueBrush,
                        new XRect(rect.X + MarginPadding + keySize.Width, rect.Y, rect.Width - keySize.Width - TwoMarginPaddings, rect.Height),
                        format ?? XStringFormats.CenterLeft
                    );
                }
            }

            return this;
        }

        public CellDrawer DrawBorders(XBrush brush, int width = 1)
        {
            using XGraphics gfx = XGraphics.FromPdfPage(page);
            int twoWidths = 2 * width;
            gfx.DrawRectangle(brush, new XRect(rect.X - width, rect.Y - width, width, rect.Height + twoWidths));
            gfx.DrawRectangle(brush, new XRect(rect.X + rect.Width, rect.Y - width, width, rect.Height + twoWidths));
            gfx.DrawRectangle(brush, new XRect(rect.X - width, rect.Y - width, rect.Width + twoWidths, width));
            gfx.DrawRectangle(brush, new XRect(rect.X - width, rect.Y + rect.Height, rect.Width + twoWidths, width));
            return this;
        }

        public CellDrawer DrawLeftBorder(XBrush brush, int width = 1)
        {
            using XGraphics gfx = XGraphics.FromPdfPage(page);
            gfx.DrawRectangle(brush, new XRect(rect.X - width, rect.Y, width, rect.Height));
            return this;
        }

        public static CellDrawer Create(PdfPage page, XRect rect) => new(page, rect);

        public static CellDrawer Create(PdfPage page, XRect rect, int line, double lineHeight = LineHeight) =>
            new(page, new XRect(rect.X, rect.Y + line * lineHeight, rect.Width, lineHeight));
    }
}
