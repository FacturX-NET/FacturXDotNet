namespace FacturXDotNet.Generation.FacturX;

/// <summary>
///     Represents the options for configuring an XMP Metadata stream during Factur-X document generation.
/// </summary>
public class XmpMetadataStreamOptions
{
    /// <summary>
    ///     The flag indicating whether the provided stream should remain open after Factur-X document generation operations are completed.
    /// </summary>
    public bool LeaveOpen { get; set; } = true;
}
