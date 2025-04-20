using FacturXDotNet.Models.CII;
using PdfSharp.Pdf;

namespace FacturXDotNet.Generation.PDF;

/// <summary>
///     Represents a service responsible for generating PDF documents from provided invoice data.
/// </summary>
public interface IPdfGenerator
{
    /// <summary>
    ///     Generates a PDF stream based on the provided CrossIndustryInvoice object.
    /// </summary>
    /// <param name="invoice">The CrossIndustryInvoice object containing the invoice data to be used for generating the PDF.</param>
    /// <returns>The stream representing the generated PDF document.</returns>
    PdfDocument Build(CrossIndustryInvoice invoice);
}
