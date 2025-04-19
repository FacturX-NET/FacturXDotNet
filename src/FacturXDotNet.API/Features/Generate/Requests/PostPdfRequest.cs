using FacturXDotNet.API.Features.Generate.Models;
using FacturXDotNet.Models.CII;

namespace FacturXDotNet.API.Features.Generate.Requests;

/// <summary>
///     Represents a request to generate a standard PDF file based on a Factur-X invoice.
///     Utilized in the generation process to customize the PDF output with specific invoice data and configuration options.
/// </summary>
public class PostPdfRequest
{
    /// <summary>
    ///     The Factur-X invoice, represented by the CrossIndustryInvoice class.
    ///     Provides the structured invoice data required for generating a Factur-X based PDF.
    /// </summary>
    public required CrossIndustryInvoice CrossIndustryInvoice { get; set; }

    /// <summary>
    ///     The options required to configure the generation of a standard PDF file.
    ///     Specifies various parameters such as the inclusion of logos, footers, and localized language packs for customization.
    /// </summary>
    public StandardPdfGeneratorOptionsDto? Options { get; set; }
}
