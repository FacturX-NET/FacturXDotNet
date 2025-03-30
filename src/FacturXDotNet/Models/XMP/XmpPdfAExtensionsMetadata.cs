namespace FacturXDotNet.Models.XMP;

/// <summary>
///     <b>PDF/A Extension Schema Container Schema</b> - This schema is required for defining XMP extension schemas. It is a container with one or more extension schemas. The
///     description of this schema is missing in ISO 19005-1, and was added in [2].
/// </summary>
/// <remarks>See https://pdfa.org/wp-content/uploads/2011/09/tn0009_xmp_extension_schemas_in_pdfa-1_2008-03-20.pdf</remarks>
/// <URI>http://www.aiim.org/pdfa/ns/extension/</URI>
/// <Prefix>pdfaExtension</Prefix>
public class XmpPdfAExtensionsMetadata
{
    /// <summary>
    ///     Container for all embedded extension schema descriptions.
    /// </summary>
    /// <remarks>
    ///     All extension schemas used in the document must be defined here according to section 4.3.
    /// </remarks>
    /// <XmpTag>pdfaExtension:schemas</XmpTag>
    public List<XmpPdfASchemaMetadata> Schemas { get; set; } = [];
}
