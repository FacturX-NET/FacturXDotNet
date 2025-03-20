namespace FacturXDotNet;

/// <summary>
///     <b>PDF/A Field Value Type</b> - This schema describes a field in a structured type. It is very similar to the PDF/A Property Value Type schema (see chapter 4.4), but defines a
///     field in a structure instead of a property.
/// </summary>
/// <remarks>See https://pdfa.org/wp-content/uploads/2011/09/tn0009_xmp_extension_schemas_in_pdfa-1_2008-03-20.pdf</remarks>
/// <URI>http://www.aiim.org/pdfa/ns/field#</URI>
/// <Prefix>pdfaField</Prefix>
public class XmpPdfAFieldMetadata
{
    /// <summary>
    ///     Field description.
    /// </summary>
    /// <remarks>
    ///     Human-readable text.
    /// </remarks>
    /// <XmpTag>pdfaField:description</XmpTag>
    public string? Description { get; set; }

    /// <summary>
    ///     Field name.
    /// </summary>
    /// <remarks>
    ///     Field names must be valid XML element names.
    /// </remarks>
    /// <XmpTag>pdfaField:name</XmpTag>
    public string? Name { get; set; }

    /// <summary>
    ///     Field value type, drawn from XMP Specification 2004, or an embedded PDF/A value type extension schema.
    /// </summary>
    /// <remarks>
    ///     Predefined XMP type names or names of custom types according to section 4.5 can be used.
    /// </remarks>
    /// <XmpTag>pdfaField:valueType</XmpTag>
    public string? ValueType { get; set; }
}
