namespace FacturXDotNet;

/// <summary>
///     <b>PDF/A Schema Value Type</b> - This schema describes a single extension schema which may comprise an arbitrary number of properties.
/// </summary>
/// <remarks>See https://pdfa.org/wp-content/uploads/2011/09/tn0009_xmp_extension_schemas_in_pdfa-1_2008-03-20.pdf</remarks>
/// <URI>http://www.aiim.org/pdfa/ns/schema#</URI>
/// <Prefix>pdfaSchema</Prefix>
public class XmpPdfASchemaMetadata
{
    /// <summary>
    ///     Schema namespace URI.
    /// </summary>
    /// <remarks>
    ///     Unique URI which describes the schema.
    /// </remarks>
    /// <XmpTag>pdfaSchema:namespaceURI</XmpTag>
    public string? NamespaceUri { get; set; }

    /// <summary>
    ///     Preferred schema namespace prefix.
    /// </summary>
    /// <remarks>
    ///     This prefix can be used in addition to the predefined XMP namespace prefixes.
    /// </remarks>
    /// <XmpTag>pdfaSchema:prefix</XmpTag>
    public string? Prefix { get; set; }

    /// <summary>
    ///     Description of schema properties.
    /// </summary>
    /// <remarks>
    ///     All properties in the extension schema must be defined here according to section 4.4.
    /// </remarks>
    /// <XmpTag>pdfaSchema:property</XmpTag>
    public List<XmpPdfAPropertyMetadata> Property { get; set; } = [];

    /// <summary>
    ///     Optional description of schema.
    /// </summary>
    /// <remarks>
    ///     Human-readable text.
    /// </remarks>
    /// <XmpTag>pdfaSchema:schema</XmpTag>
    public string? Schema { get; set; }

    /// <summary>
    ///     Description of schema-specific value types.
    /// </summary>
    /// <remarks>
    ///     All types which are used in the extension schema, but are not defined in the XMP Specification must be defined here according to section 4.5. This property is required if
    ///     custom types are used in the extension schema. If no custom types are used it may be absent or empty.
    /// </remarks>
    /// <XmpTag>pdfaSchema:valueType</XmpTag>
    public List<XmpPdfAValueTypeMetadata> ValueType { get; set; } = [];
}
