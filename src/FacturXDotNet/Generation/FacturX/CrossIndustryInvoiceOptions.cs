namespace FacturXDotNet.Generation.FacturX;

/// <summary>
///     Represents the options for configuring the Cross Industry Invoice (CII) during Factur-X document generation.
/// </summary>
public class CrossIndustryInvoiceOptions
{
    /// <summary>
    ///     The name of the Cross Industry Invoice (CII) attachment file used in the Factur-X document generation process.
    /// </summary>
    public string? CiiAttachmentName { get; set; } = null;
}
