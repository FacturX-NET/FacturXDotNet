using Microsoft.Extensions.Logging;

namespace FacturXDotNet.Parsers.XMP;

/// <summary>
///     The options that can be passed to the <see cref="XmpMetadataParser" />.
/// </summary>
public class XmpMetadataParserOptions
{
    /// <summary>
    ///     The logger that should be used by the parser.
    ///     The parser logs the unknown paths it encounters at the WARN level.
    /// </summary>
    public ILogger? Logger { get; set; }
}
