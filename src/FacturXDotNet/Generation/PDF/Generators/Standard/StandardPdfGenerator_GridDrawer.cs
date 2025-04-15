using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace FacturXDotNet.Generation.PDF.Generators.Standard;

public partial class StandardPdfGenerator
{
    class GridDrawer(PdfPage page, XRect rect, int lines, int columns)
    {
        public CellDrawer Cell(int line, int column) => CellDrawer.Create(page, GetCellRect(line, column));

        public GridDrawer DrawBorders(XBrush brush, double width = 1)
        {
            (double cellWidth, double cellHeight) = GetCellSize();
            double halfWidth = width / 2d;

            using XGraphics gfx = XGraphics.FromPdfPage(page);

            for (int i = 0; i <= lines; i++)
            {
                gfx.DrawRectangle(brush, new XRect(rect.X - halfWidth, rect.Y - halfWidth + i * cellHeight, rect.Width + width, width));
            }

            for (int i = 0; i <= columns; i++)
            {
                gfx.DrawRectangle(brush, new XRect(rect.X - halfWidth + i * cellWidth, rect.Y - halfWidth, width, rect.Height + width));
            }

            return this;
        }

        public static GridDrawer Create(PdfPage page, XRect rect, int lines, int columns) => new(page, rect, lines, columns);

        XRect GetCellRect(int line, int column)
        {
            (double cellWidth, double cellHeight) = GetCellSize();
            return new XRect(rect.X + column * cellWidth, rect.Y + line * cellHeight, cellWidth, cellHeight);
        }

        (double cellWidth, double cellHeight) GetCellSize()
        {
            double cellWidth = rect.Width / columns;
            double cellHeight = rect.Height / lines;
            return (cellWidth, cellHeight);
        }
    }
}
