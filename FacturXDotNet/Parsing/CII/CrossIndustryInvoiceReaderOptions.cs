using Microsoft.Extensions.Logging;

namespace FacturXDotNet.Parsing.CII;

/// <summary>
///     The options that can be passed to the <see cref="CrossIndustryInvoiceReader" />.
/// </summary>
public class CrossIndustryInvoiceReaderOptions
{
    /// <summary>
    ///     The logger that should be used by the reader.
    ///     The reader logs the unknown paths it encounters at the WARN level.
    /// </summary>
    public ILogger? Logger { get; set; }
}
