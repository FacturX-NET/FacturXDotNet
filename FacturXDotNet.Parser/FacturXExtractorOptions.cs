namespace FacturXDotNet.Parser;

public class FacturXExtractorOptions
{
    /// <summary>
    ///     The password to use to open the PDF document if it is encrypted with standard encryption.
    /// </summary>
    public string? Password { get; set; }
}
