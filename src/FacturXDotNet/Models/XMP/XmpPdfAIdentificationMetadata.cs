namespace FacturXDotNet.Models.XMP;

/// <summary>
///     <b>PDF/A Identification Schema</b> - The only mandatory XMP entries are those which indicate that the file is a PDF/A-1 document and its conformance level. The table below
///     lists all properties in the PDF/A identification schema. The namespace URI is incorrectly described in ISO 19005-1 [1], and corrected in [2]. Unlike predefined XMP schemas,
///     the namespace prefix is not only preferred, but required.
/// </summary>
/// <remarks>See https://pdfa.org/wp-content/uploads/2011/08/tn0008_predefined_xmp_properties_in_pdfa-1_2008-03-20.pdf</remarks>
/// <URI>http://www.aiim.org/pdfa/ns/id/</URI>
/// <Prefix>pdfaid</Prefix>
public class XmpPdfAIdentificationMetadata
{
    /// <summary>
    ///     Optional PDF/A amendment identifier
    /// </summary>
    /// <XmpTag>pdfaid:amd</XmpTag>
    public string? Amendment { get; set; }

    /// <summary>
    ///     PDF/A conformance level: A or B.
    /// </summary>
    /// <remarks>
    ///     Required.
    /// </remarks>
    /// <XmpTag>pdfaid:conformance</XmpTag>
    public XmpPdfAConformanceLevel? Conformance { get; set; }

    /// <summary>
    ///     PDF/A version identifier.
    /// </summary>
    /// <remarks>
    ///     Required.
    /// </remarks>
    /// <XmpTag>pdfaid:part</XmpTag>
    public int? Part { get; set; }
}
