using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace FacturXDotNet.Generation.PDF.Generators.Standard;

public partial class StandardPdfGenerator
{
    class CellDrawer(PdfPage page, XRect rect)
    {
        XRect _rect = rect;

        public CellDrawer Background(XColor color)
        {
            using XGraphics gfx = XGraphics.FromPdfPage(page);
            gfx.DrawRectangle(new XSolidBrush(color), _rect);
            return this;
        }

        public CellDrawer Key(string text, XBrush? brush, XFont? font, bool appendColon = true)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return this;
            }

            if (appendColon)
            {
                text = text.EndsWith(": ") || text.EndsWith(":") ? text : $"{text}: ";
            }

            text = text.EndsWith(' ') ? text : $"{text} ";

            using XGraphics gfx = XGraphics.FromPdfPage(page);
            XSize keySize = gfx.MeasureString(text, font ?? NormalFont);
            gfx.DrawString(text, font ?? NormalFont, brush ?? BlackBrush, new XRect(_rect.X + MarginPadding, _rect.Y, keySize.Width, _rect.Height), XStringFormats.CenterLeft);

            _rect = new XRect(_rect.X + keySize.Width, _rect.Y, _rect.Width - keySize.Width, _rect.Height);
            return this;
        }

        public CellDrawer Text(string? text, XBrush? brush = null, XFont? font = null, XStringFormat? format = null)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return this;
            }

            using XGraphics gfx = XGraphics.FromPdfPage(page);
            gfx.DrawString(
                text,
                font ?? NormalFont,
                brush ?? BlackBrush,
                new XRect(_rect.X + MarginPadding, _rect.Y, _rect.Width - TwoMarginPaddings, _rect.Height),
                format ?? XStringFormats.CenterLeft
            );

            return this;
        }

        public CellDrawer DrawBorders(XBrush brush, int width = 1)
        {
            using XGraphics gfx = XGraphics.FromPdfPage(page);
            int twoWidths = 2 * width;
            gfx.DrawRectangle(brush, new XRect(_rect.X - width, _rect.Y - width, width, _rect.Height + twoWidths));
            gfx.DrawRectangle(brush, new XRect(_rect.X + _rect.Width, _rect.Y - width, width, _rect.Height + twoWidths));
            gfx.DrawRectangle(brush, new XRect(_rect.X - width, _rect.Y - width, _rect.Width + twoWidths, width));
            gfx.DrawRectangle(brush, new XRect(_rect.X - width, _rect.Y + _rect.Height, _rect.Width + twoWidths, width));
            return this;
        }

        public CellDrawer DrawTopBorder(XBrush brush, int width = 1)
        {
            using XGraphics gfx = XGraphics.FromPdfPage(page);
            gfx.DrawRectangle(brush, new XRect(_rect.X - width, _rect.Y - width, _rect.Width + 2 * width, width));
            return this;
        }

        public CellDrawer DrawLeftBorder(XBrush brush, int width = 1)
        {
            using XGraphics gfx = XGraphics.FromPdfPage(page);
            gfx.DrawRectangle(brush, new XRect(_rect.X - width, _rect.Y, width, _rect.Height));
            return this;
        }

        public static CellDrawer Create(PdfPage page, XRect rect) => new(page, rect);

        public static CellDrawer Create(PdfPage page, XRect rect, int line, double lineHeight = LineHeight) =>
            new(page, new XRect(rect.X, rect.Y + line * lineHeight, rect.Width, lineHeight));
    }
}
