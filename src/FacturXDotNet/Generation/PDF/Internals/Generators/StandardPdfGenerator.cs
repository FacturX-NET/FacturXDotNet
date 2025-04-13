using FacturXDotNet.Models.CII;
using PdfSharp.Pdf;

namespace FacturXDotNet.Generation.PDF.Internals.Generators;

/// <summary>
///     Generate PDFs that look like the model provided in the Factur-X specification.
/// </summary>
class StandardPdfGenerator : IPdfGenerator
{
    public PdfDocument Build(CrossIndustryInvoice invoice) => throw new NotImplementedException();
}
