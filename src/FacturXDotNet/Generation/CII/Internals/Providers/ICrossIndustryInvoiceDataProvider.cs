using FacturXDotNet.Models.CII;

namespace FacturXDotNet.Generation.CII.Internals.Providers;

/// <summary>
///     Encapsulate the generation of a Cross-Industry Invoice data stream.
/// </summary>
interface ICrossIndustryInvoiceDataProvider
{
    Task<CrossIndustryInvoice> GetCrossIndustryInvoiceAsync();
    Task<Stream> GetCrossIndustryInvoiceStreamAsync();
}
