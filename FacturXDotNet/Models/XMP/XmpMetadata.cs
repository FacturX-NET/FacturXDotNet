namespace FacturXDotNet.Models.XMP;

/// <summary>
///     XMP metadata of a Factur-X invoice.
/// </summary>
/// <remarks>
///     See
///     <list type="bullet">
///         <item>https://pdfa.org/wp-content/uploads/2011/08/tn0008_predefined_xmp_properties_in_pdfa-1_2008-03-20.pdf</item>
///         <item>https://pdfa.org/wp-content/uploads/2011/09/tn0009_xmp_extension_schemas_in_pdfa-1_2008-03-20.pdf</item>
///         <item>https://developer.adobe.com/xmp/docs/XMPNamespaces/</item>
///     </list>
/// </remarks>
public class XmpMetadata
{
    /// <summary>
    ///     The PDF/A ID metadata of the invoice.
    /// </summary>
    public XmpPdfAIdentificationMetadata? PdfAIdentification { get; set; }

    /// <summary>
    ///     The basic descriptive information of the document.
    /// </summary>
    public XmpBasicMetadata? Basic { get; set; }

    /// <summary>
    ///     The properties of the PDF document.
    /// </summary>
    public XmpPdfMetadata? Pdf { get; set; }

    /// <summary>
    ///     The commonly used properties from the Dublin Core Metadata Initiative (DCMI).
    /// </summary>
    public XmpDublinCoreMetadata? DublinCore { get; set; }

    /// <summary>
    ///     The PDF/A metadata of the invoice.
    /// </summary>
    public XmpPdfAExtensionsMetadata? PdfAExtensions { get; set; }

    /// <summary>
    ///     The Factur-X metadata of the invoice.
    /// </summary>
    public XmpFacturXMetadata? FacturX { get; set; }
}
