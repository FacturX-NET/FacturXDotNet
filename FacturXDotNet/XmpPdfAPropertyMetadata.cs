namespace FacturXDotNet;

/// <summary>
///     <b>PDF/A Property Value Type</b> - This schema describes a single property.
/// </summary>
/// <remarks>See https://pdfa.org/wp-content/uploads/2011/09/tn0009_xmp_extension_schemas_in_pdfa-1_2008-03-20.pdf</remarks>
/// <URI>http://www.aiim.org/pdfa/ns/property#</URI>
/// <Prefix>pdfaProperty</Prefix>
public class XmpPdfAPropertyMetadata
{
    /// <summary>
    ///     Property category: internal or external.
    /// </summary>
    /// <remarks>
    ///     Internal properties are created automatically from document content. External properties are based on user input.
    /// </remarks>
    /// <XmpTag>pdfaProperty:category</XmpTag>
    public XmpPdfAPropertyCategory? Category { get; set; }

    /// <summary>
    ///     Description of the property.
    /// </summary>
    /// <remarks>
    ///     Human-readable text.
    /// </remarks>
    /// <XmpTag>pdfaProperty:description</XmpTag>
    public string? Description { get; set; }

    /// <summary>
    ///     Property name.
    /// </summary>
    /// <remarks>
    ///     The property names comprise the vocabulary defined by the schema. Property names must be valid XML element names.
    /// </remarks>
    /// <XmpTag>pdfaProperty:name</XmpTag>
    public string? Name { get; set; }

    /// <summary>
    ///     Value type of the property, drawn from XMP Specification, or an embedded PDF/A extension schema value type.
    /// </summary>
    /// <remarks>
    ///     Predefined XMP type names or names of custom types according to section 4.5 can be used.
    /// </remarks>
    /// <XmpTag>pdfaProperty:valueType</XmpTag>
    public string? ValueType { get; set; }
}
