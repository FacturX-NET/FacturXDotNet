using Microsoft.Extensions.Logging;

namespace FacturXDotNet.Generation.XMP;

/// <summary>
///     The options that can be passed to the <see cref="XmpMetadataWriter" />.
/// </summary>
public class XmpMetadataWriterOptions
{
    /// <summary>
    ///     The logger that should be used by the writer.
    /// </summary>
    public ILogger? Logger { get; set; }
}
