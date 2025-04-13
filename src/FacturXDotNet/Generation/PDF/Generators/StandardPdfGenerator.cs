using FacturXDotNet.Models.CII;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;

namespace FacturXDotNet.Generation.PDF.Generators;

/// <summary>
///     Generate PDFs that look like the model provided in the Factur-X specification.
/// </summary>
public class StandardPdfGenerator : IPdfGenerator
{
    /// <inheritdoc />
    public PdfDocument Build(CrossIndustryInvoice invoice)
    {
        PdfDocument document = new();

        GlobalFontSettings.UseWindowsFontsUnderWindows = true;
        XFont font = new("Verdana", 12);

        PdfPage page = document.AddPage();
        XGraphics gfx = XGraphics.FromPdfPage(page);

        gfx.DrawString("Hello world!", font, XBrushes.Black, 10, 10);

        return document;
    }
}
