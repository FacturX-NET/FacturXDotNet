namespace FacturXDotNet;

/// <summary>
///     A Factur-X document.
/// </summary>
public class FacturX
{
    /// <summary>
    ///     The XMP metadata of the Factur-X document.
    /// </summary>
    public required XmpMetadata XmpMetadata { get; set; }

    /// <summary>
    ///     Information about the Cross-Industry Invoice file that was found in the Factur-X document.
    /// </summary>
    public required CrossIndustryInvoiceFileInformation CrossIndustryInvoiceFileInformation { get; set; }

    /// <summary>
    ///     The Cross-Industry Invoice of the Factur-X document.
    /// </summary>
    public required CrossIndustryInvoice CrossIndustryInvoice { get; set; }
}
