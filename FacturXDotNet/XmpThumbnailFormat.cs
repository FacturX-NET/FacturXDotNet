namespace FacturXDotNet;

/// <summary>
///     The image encoding. Defined value: JPEG.
/// </summary>
/// <remarks>See https://developer.adobe.com/xmp/docs/XMPNamespaces/XMPDataTypes/Thumbnails/</remarks>
public enum XmpThumbnailFormat
{
    /// <summary>
    ///     JPEG format.
    /// </summary>
    Jpeg
}

/// <summary>
///     Mapping methods for the <see cref="XmpThumbnailFormat" /> enumeration.
/// </summary>
public static class XmpThumbnailFormatMappingExtensions
{
    /// <summary>
    ///     Convert the <see cref="XmpThumbnailFormat" /> to its string representation.
    /// </summary>
    public static string ToXmpThumbnailFormat(this XmpThumbnailFormat conformanceLevel) =>
        conformanceLevel switch
        {
            XmpThumbnailFormat.Jpeg => "JPEG",
            _ => throw new ArgumentOutOfRangeException(nameof(conformanceLevel), conformanceLevel, null)
        };

    /// <summary>
    ///     Convert the string to its <see cref="XmpThumbnailFormat" /> representation.
    /// </summary>
    /// <seealso cref="ToXmpThumbnailFormat(ReadOnlySpan{char})" />
    public static XmpThumbnailFormat? ToXmpThumbnailFormatOrNull(this ReadOnlySpan<char> value) =>
        value switch
        {
            "JPEG" => XmpThumbnailFormat.Jpeg,
            _ => null
        };

    /// <summary>
    ///     Convert the string to its <see cref="XmpThumbnailFormat" /> representation.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not a valid <see cref="XmpThumbnailFormat" />.</exception>
    public static XmpThumbnailFormat ToXmpThumbnailFormat(this ReadOnlySpan<char> value) =>
        ToXmpThumbnailFormatOrNull(value) ?? throw new ArgumentOutOfRangeException(nameof(value), value.ToString(), null);
}
