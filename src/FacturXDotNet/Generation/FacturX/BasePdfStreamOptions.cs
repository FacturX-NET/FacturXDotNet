namespace FacturXDotNet.Generation.FacturX;

/// <summary>
///     Represents the options for configuring the base PDF stream used during the Factur-X document generation process.
/// </summary>
public class BasePdfStreamOptions
{
    /// <summary>
    ///     The password used to protect the base PDF stream in the Factur-X document generation process.
    /// </summary>
    public string? Password { get; set; } = null;

    /// <summary>
    ///     The flag indicating whether the base PDF stream should remain open after the Factur-X document generation process.
    /// </summary>
    public bool LeaveOpen { get; set; } = true;
}
