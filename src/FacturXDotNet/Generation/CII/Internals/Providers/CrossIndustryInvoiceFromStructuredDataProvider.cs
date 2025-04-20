using FacturXDotNet.Models.CII;

namespace FacturXDotNet.Generation.CII.Internals.Providers;

/// <summary>
///     Provides functionality to generate a Cross-Industry Invoice data stream from structured data.
/// </summary>
class CrossIndustryInvoiceFromStructuredDataProvider(CrossIndustryInvoice cii) : ICrossIndustryInvoiceDataProvider
{
    public Task<CrossIndustryInvoice> GetCrossIndustryInvoiceAsync() => Task.FromResult(cii);

    public async Task<Stream> GetCrossIndustryInvoiceStreamAsync()
    {
        MemoryStream result = new();
        CrossIndustryInvoiceWriter writer = new();
        await writer.WriteAsync(result, cii);
        result.Seek(0, SeekOrigin.Begin);
        return result;
    }
}
