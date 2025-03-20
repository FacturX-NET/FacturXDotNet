using FacturXDotNet.Parsing.CII;
using FacturXDotNet.Parsing.XMP;

namespace FacturXDotNet.Parsing;

/// <summary>
///     The options that can be passed to the <see cref="FacturXParser" />.
/// </summary>
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
    ///     The options for parsing the XMP metadata.
    /// </summary>
    public XmpMetadataParserOptions Xmp { get; set; } = new();

    /// <summary>
    ///     The options for parsing the Cross-Industry Invoice.
    /// </summary>
    public CrossIndustryInvoiceParserOptions Cii { get; } = new();
}
