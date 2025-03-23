using FacturXDotNet.Parsing.CII;

namespace FacturXDotNet;

/// <summary>
///     The Cross-Industry Invoice attachment.
/// </summary>
public class CrossIndustryInvoiceAttachment : FacturXDocumentAttachment
{
    /// <summary>
    ///     The Cross-Industry Invoice attachment.
    /// </summary>
    /// <param name="facturX">The Factur-X document.</param>
    /// <param name="name">The name of the attachment in the Factur-X document.</param>
    internal CrossIndustryInvoiceAttachment(FacturXDocument facturX, string name) : base(facturX, name)
    {
    }

    /// <summary>
    ///     Get the parsed Cross-Industry Invoice.
    /// </summary>
    /// <param name="password">The password to open the PDF document.</param>
    /// <param name="options">The options to parse the Cross-Industry Invoice.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The parsed Cross-Industry Invoice.</returns>
    public async Task<CrossIndustryInvoice> GetCrossIndustryInvoiceAsync(
        string? password = null,
        CrossIndustryInvoiceReaderOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        options ??= new CrossIndustryInvoiceReaderOptions();

        await using Stream stream = await FindAttachmentStreamAsync(password, cancellationToken);

        CrossIndustryInvoiceReader reader = new(options);
        return reader.Read(stream);
    }
}
