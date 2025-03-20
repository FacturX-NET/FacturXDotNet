namespace FacturXDotNet;

/// <summary>
///     A file attached to the FacturX PDF that contains the Cross-Industry Invoice XML.
/// </summary>
public class CrossIndustryInvoiceFileInformation
{
    /// <summary>
    ///     The name of the attachment containing the Cross-Industry Invoice XML file.
    /// </summary>
    public required string Name { get; set; }
}
