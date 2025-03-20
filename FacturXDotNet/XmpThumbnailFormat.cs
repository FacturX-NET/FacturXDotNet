namespace FacturXDotNet;

/// <summary>
///     The image encoding. Defined value: JPEG.
/// </summary>
/// <remarks>See https://developer.adobe.com/xmp/docs/XMPNamespaces/XMPDataTypes/Thumbnails/</remarks>
public enum XmpThumbnailFormat
{
    Jpeg
}

public static class XmpThumbnailFormatMappingExtensions
{
    public static string ToXmpThumbnailFormatString(this XmpThumbnailFormat conformanceLevel) =>
        conformanceLevel switch
        {
            XmpThumbnailFormat.Jpeg => "JPEG",
            _ => throw new ArgumentOutOfRangeException(nameof(conformanceLevel), conformanceLevel, null)
        };

    public static XmpThumbnailFormat? ToXmpThumbnailFormatOrNull(this ReadOnlySpan<char> value) =>
        value switch
        {
            "JPEG" => XmpThumbnailFormat.Jpeg,
            _ => null
        };

    public static XmpThumbnailFormat ToXmpThumbnailFormat(this ReadOnlySpan<char> value) =>
        ToXmpThumbnailFormatOrNull(value) ?? throw new ArgumentOutOfRangeException(nameof(value), value.ToString(), null);
}
