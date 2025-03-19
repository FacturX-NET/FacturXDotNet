using FacturXDotNet.Parsers.CII;

namespace FacturXDotNet.Parsers.FacturX;

public class FacturXParserOptions
{
    /// <summary>
    ///     The password to use to open the PDF document if it is encrypted with standard encryption.
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    ///     The name of the attachment containing the Cross-Industry Invoice XML file.
    /// </summary>
    public string CiiXmlAttachmentName { get; set; } = "factur-x.xml";

    /// <summary>
    ///     The options for parsing the Cross-Industry Invoice.
    /// </summary>
    public CrossIndustryInvoiceParserOptions Cii { get; } = new();
}
