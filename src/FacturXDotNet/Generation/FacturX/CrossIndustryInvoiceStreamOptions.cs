namespace FacturXDotNet.Generation.FacturX;

/// <summary>
///     Represents the options for configuring a Cross Industry Invoice (CII) stream during Factur-X document generation.
/// </summary>
public class CrossIndustryInvoiceStreamOptions
{
    /// <summary>
    ///     The name of the Cross Industry Invoice (CII) attachment file used in the Factur-X document generation process.
    /// </summary>
    public string? CiiAttachmentName { get; set; } = null;

    /// <summary>
    ///     The flag indicating whether the provided stream should remain open after Factur-X document generation operations are completed.
    /// </summary>
    public bool LeaveOpen { get; set; } = true;
}
