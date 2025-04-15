using FacturXDotNet.Models.XMP;
using FacturXDotNet.Parsing.XMP;

namespace FacturXDotNet.Generation.XMP.Internals.Providers;

class XmpFromStreamDataProvider(Stream stream, bool leaveOpen = true) : IXmpDataProvider, IDisposable
{
    readonly long _startPosition = stream.Position;

    public XmpMetadata GetXmpMetadata()
    {
        stream.Seek(_startPosition, SeekOrigin.Begin);
        XmpMetadataReader reader = new();
        return reader.Read(stream);
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
