using FacturXDotNet.Models.CII;
using FacturXDotNet.Parsing.CII;

namespace FacturXDotNet.Generation.CII.Internals.Providers;

class CrossIndustryInvoiceFromStreamDataProvider(Stream stream, bool leaveOpen = true) : ICrossIndustryInvoiceDataProvider, IDisposable, IAsyncDisposable
{
    readonly long _startPosition = stream.Position;

    public Task<CrossIndustryInvoice> GetCrossIndustryInvoiceAsync()
    {
        stream.Seek(_startPosition, SeekOrigin.Begin);
        CrossIndustryInvoiceReader reader = new();
        return Task.FromResult(reader.Read(stream));
    }

    public Task<Stream> GetCrossIndustryInvoiceStreamAsync()
    {
        stream.Seek(_startPosition, SeekOrigin.Begin);
        return Task.FromResult(stream);
    }

    public void Dispose()
    {
        if (!leaveOpen)
        {
            stream.Dispose();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (!leaveOpen)
        {
            await stream.DisposeAsync();
        }
    }
}
