namespace FacturXDotNet.Models.XMP;

/// <summary>
///     <b>Adobe XMP Basic namespace</b> - The XMP basic namespace contains properties that provide basic descriptive information.
/// </summary>
/// <remarks>See https://developer.adobe.com/xmp/docs/XMPNamespaces/xmp/</remarks>
/// <URI>http://ns.adobe.com/xap/1.0/</URI>
/// <Prefix>xmp</Prefix>
public class XmpBasicMetadata
{
    /// <summary>
    ///     An unordered array of text strings that unambiguously identify the resource within a given context. An array item may be qualified with xmpidq:Scheme to denote the formal
    ///     identification system to which that identifier conforms.
    /// </summary>
    /// <remarks>
    ///     The xmp:Identifier property was added because dc:identifier has been defined in the original XMP specification
    ///     as a single identifier instead of as an array, and changing dc:identifier to an array would break compatibility with existing XMP processors.
    /// </remarks>
    /// <XmpTag>xmp:Identifier</XmpTag>
    public string? Identifier { get; set; }

    /// <summary>
    ///     The date and time the resource was created. For a digital file, this need not match a file-system creation time. For a freshly created resource, it should be close to that
    ///     time, modulo the time taken to write the file. Later file transfer, copying, and so on, can make the file-system time arbitrarily different.
    /// </summary>
    /// <XmpTag>xmp:CreateDate</XmpTag>
    public DateTime? CreateDate { get; set; }

    /// <summary>
    ///     The name of the first known tool used to create the resource.
    /// </summary>
    /// <XmpTag>xmp:CreatorTool</XmpTag>
    public string? CreatorTool { get; set; }

    /// <summary>
    ///     A word or short phrase that identifies a resource as a member of a userdefined collection.
    /// </summary>
    /// <remarks>
    ///     One anticipated usage is to organize resources in a file browser.
    /// </remarks>
    /// <XmpTag>xmp:Label</XmpTag>
    public string? Label { get; set; }

    /// <summary>
    ///     The date and time that any metadata for this resource was last changed. It should be the same as or more recent than xmp:ModifyDate.
    /// </summary>
    /// <XmpTag>xmp:MetadataDate</XmpTag>
    public DateTime? MetadataDate { get; set; }

    /// <summary>
    ///     The date and time the resource was last modified. NOTE: The value of this property is not necessarily the same as the file’s system modification date because it is typically
    ///     set before the file is saved.
    /// </summary>
    /// <XmpTag>xmp:ModifyDate</XmpTag>
    public DateTime? ModifyDate { get; set; }

    /// <summary>
    ///     A user-assigned rating for this file. The value shall be -1 or in the range [0..5], where -1 indicates “rejected” and 0 indicates “unrated”. If xmp:Rating is not present, a
    ///     value of 0 should be assumed.
    /// </summary>
    /// <remarks>
    ///     Anticipated usage is for a typical “star rating” UI, with the addition of a notion of rejection.
    /// </remarks>
    /// <XmpTag>xmp:Rating</XmpTag>
    public double Rating { get; set; }

    /// <summary>
    ///     The base URL for relative URLs in the document content. If this document contains Internet links, and those links are relative, they are relative to this base URL. This
    ///     property provides a standard way for embedded relative URLs to be interpreted by tools. Web authoring tools should set the value based on their notion of where URLs will be
    ///     interpreted.
    /// </summary>
    /// <XmpTag>xmp:BaseURL</XmpTag>
    public string? BaseUrl { get; set; }

    /// <summary>
    ///     A short informal name for the resource.
    /// </summary>
    /// <XmpTag>xmp:Nickname</XmpTag>
    public string? Nickname { get; set; }

    /// <summary>
    ///     An alternative array of thumbnail images for a file, which can differ in characteristics such as size or image encoding.
    /// </summary>
    /// <XmpTag>xmp:Thumbnails</XmpTag>
    public List<XmpThumbnail> Thumbnails { get; set; } = [];
}
