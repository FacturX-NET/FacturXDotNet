using Microsoft.Extensions.Logging;

namespace FacturXDotNet.Generation.CII;

/// <summary>
///     The options that can be passed to the <see cref="CrossIndustryInvoiceWriter" />.
/// </summary>
public class CrossIndustryInvoiceWriterOptions
{
    /// <summary>
    ///     The logger that should be used by the writer.
    /// </summary>
    public ILogger? Logger { get; set; }
}
