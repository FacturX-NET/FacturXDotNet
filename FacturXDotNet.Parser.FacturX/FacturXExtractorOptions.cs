namespace FacturXDotNet.Parser.FacturX;

public class FacturXExtractorOptions
{
    /// <summary>
    ///     The name of the attachment containing the Cross-Industry Invoice XML file.
    /// </summary>
    public string CiiXmlAttachmentName { get; set; } = "factur-x.xml";

    /// <summary>
    ///     The password to use to open the PDF document if it is encrypted with standard encryption.
    /// </summary>
    public string? Password { get; set; }
}
