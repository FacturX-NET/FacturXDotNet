namespace FacturXDotNet.Models.XMP;

/// <summary>
///     <b>Factur-X PDFA Extension Schema</b>
/// </summary>
/// <URI>urn:factur-x:pdfa:CrossIndustryDocument:invoice:1p0#</URI>
/// <Prefix>fx</Prefix>
public class XmpFacturXMetadata
{
    /// <summary>
    ///     The namespace URI for the Factur-X PDFA Extension Schema.
    /// </summary>
    public const string NamespaceUri = "urn:factur-x:pdfa:CrossIndustryDocument:invoice:1p0#";

    /// <summary>
    ///     The name of the embedded XML document.
    /// </summary>
    /// <XmlTag>fx:DocumentFileName</XmlTag>
    public string? DocumentFileName { get; set; }

    /// <summary>
    ///     The type of the hybrid document in capital letters, e.g. INVOICE or ORDER.
    /// </summary>
    /// <XmlTag>fx:DocumentType</XmlTag>
    public XmpFacturXDocumentType? DocumentType { get; set; }

    /// <summary>
    ///     The actual version of the standard applying to the embedded XML document.
    /// </summary>
    /// <XmlTag>fx:Version</XmlTag>
    public string? Version { get; set; }

    /// <summary>
    ///     The conformance level of the embedded XML document.
    /// </summary>
    /// <XmlTag>fx:ConformanceLevel</XmlTag>
    public XmpFacturXConformanceLevel? ConformanceLevel { get; set; }
}
