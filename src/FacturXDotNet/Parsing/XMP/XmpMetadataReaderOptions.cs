using Microsoft.Extensions.Logging;

namespace FacturXDotNet.Parsing.XMP;

/// <summary>
///     The options that can be passed to the <see cref="XmpMetadataReader" />.
/// </summary>
public class XmpMetadataReaderOptions
{
    /// <summary>
    ///     The logger that should be used by the reader.
    ///     The reader logs the unknown paths it encounters at the WARN level.
    /// </summary>
    public ILogger? Logger { get; set; }
}
