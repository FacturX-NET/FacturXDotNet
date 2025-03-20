namespace FacturXDotNet;

/// <summary>
///     <b>Adobe PDF namespace</b> - This namespace specifies properties used with Adobe PDF documents.
/// </summary>
/// <remarks>See https://developer.adobe.com/xmp/docs/XMPNamespaces/pdf/</remarks>
/// <URI>http://ns.adobe.com/pdf/1.3/</URI>
/// <Prefix>pdf</Prefix>
public class XmpPdfMetadata
{
    /// <summary>
    ///     Keywords.
    /// </summary>
    /// <XmpTag>pdf:Keywords</XmpTag>
    public string? Keywords { get; set; }

    /// <summary>
    ///     The PDF file version (for example: 1.0, 1.3, and so on).
    /// </summary>
    /// <XmpTag>pdf:PDFVersion</XmpTag>
    public string? PdfVersion { get; set; }

    /// <summary>
    ///     The name of the tool that created the PDF document.
    /// </summary>
    /// <XmpTag>pdf:Producer</XmpTag>
    public string? Producer { get; set; }

    /// <summary>
    ///     True when the document has been trapped.
    /// </summary>
    /// <XmpTag>pdf:Trapped</XmpTag>
    public bool Trapped { get; set; }
}
