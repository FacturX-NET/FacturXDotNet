namespace FacturXDotNet.Models.XMP;

/// <summary>
///     <b>PDF/A ValueType Value Type</b> - The PDF/A ValueType schema is required for all property value types which are not defined in the XMP 2004 specification [4], i.e. for value
///     types outside of the following list:
///     <list type="bullet">
///         <item>
///             Array types (these are container types which may contain one or more fields): Alt, Bag, Seq
///         </item>
///         <item>
///             Basic value types: Boolean, (open and closed) Choice, Date, Dimensions, Integer, Lang Alt, Locale, MIMEType, ProperName, Real, Text, Thumbnail, URI, URL, XPath
///         </item>
///         <item>
///             Media Management value types: AgentName, RenditionClass, Resource-Event, ResourceRef, Version
///         </item>
///         <item>
///             Basic Job/Workflow value type: Job
///         </item>
///         <item>
///             EXIF schema value types: Flash, CFAPattern, DeviceSettings, GPSCoordinate, OECF/SFR, Rational
///         </item>
///     </list>
/// </summary>
/// <remarks>See https://pdfa.org/wp-content/uploads/2011/09/tn0009_xmp_extension_schemas_in_pdfa-1_2008-03-20.pdf</remarks>
/// <URI>http://www.aiim.org/pdfa/ns/type#</URI>
/// <Prefix>pdfaType</Prefix>
public class XmpPdfATypeMetadata
{
    /// <summary>
    ///     Description of the property value type.
    /// </summary>
    /// <remarks>
    ///     Human-readable text.
    /// </remarks>
    /// <XmpTag>pdfaType:description</XmpTag>
    public string? Description { get; set; }

    /// <summary>
    ///     Optional description of the struc- tured fields.
    /// </summary>
    /// <remarks>
    ///     Separate entries are required for all fields in a structured type.
    /// </remarks>
    /// <XmpTag>pdfaType:field</XmpTag>
    public List<XmpPdfAFieldMetadata> Field { get; set; } = [];

    /// <summary>
    ///     Property value type field namespace URI.
    /// </summary>
    /// <XmpTag>pdfaType:namespaceURI</XmpTag>
    public string? NamespaceUri { get; set; }

    /// <summary>
    ///     Preferred value type field namespace prefix.
    /// </summary>
    /// <XmpTag>pdfaType:prefix</XmpTag>
    public string? Prefix { get; set; }

    /// <summary>
    ///     Property value type name.
    /// </summary>
    /// <XmpTag>pdfaType:type</XmpTag>
    public string? Type { get; set; }
}
