using Microsoft.Extensions.Logging;

namespace FacturXDotNet.Parsers.CII;

public class CrossIndustryInvoiceParserOptions
{
    /// <summary>
    ///     The logger that should be used by the parser.
    ///     The parser logs the unknown paths it encounters at the WARN level.
    /// </summary>
    public ILogger? Logger { get; set; }
}
