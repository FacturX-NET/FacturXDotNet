using Microsoft.Extensions.Logging;

namespace FacturXDotNet.Parser.CII;

public class FacturXCrossIndustryInvoiceParserOptions
{
    /// <summary>
    ///     The logger that should be used by the parser.
    ///     The parser logs the unknown paths it encounters at the WARN level.
    /// </summary>
    public ILogger? Logger { get; set; }
}