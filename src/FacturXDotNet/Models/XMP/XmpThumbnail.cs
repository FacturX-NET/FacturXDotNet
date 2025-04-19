namespace FacturXDotNet.Models.XMP;

/// <summary>
///     <b>Thumbnail</b> - A thumbnail image for a file.
/// </summary>
/// <remarks>See https://developer.adobe.com/xmp/docs/XMPNamespaces/XMPDataTypes/Thumbnails/</remarks>
/// <URI>http://ns.adobe.com/xap/1.0/g/img/</URI>
/// <Prefix>xmpGImg</Prefix>
public class XmpThumbnail
{
    /// <summary>
    ///     The image encoding. Defined value: JPEG.
    /// </summary>
    /// <XmpTag>xmpGlmg:format</XmpTag>
    public XmpThumbnailFormat? Format { get; set; }

    /// <summary>
    ///     Height in pixels.
    /// </summary>
    /// <XmpTag>xmpGlmg:height</XmpTag>
    public int? Height { get; set; }

    /// <summary>
    ///     Width in pixels.
    /// </summary>
    /// <XmpTag>xmpGlmg:width</XmpTag>
    public int? Width { get; set; }

    /// <summary>
    ///     The full thumbnail image data, converted to base 64 notation(according to section 6.8 of RFC 2045). This is the thumbnail data typically found in a digital image, such as the
    ///     value of tag 513 in a JPEG stream.
    /// </summary>
    /// <XmpTag>xmpGlmg:image</XmpTag>
    public string? Image { get; set; }
}
