namespace FacturXDotNet.Models.XMP;

/// <summary>
///     Property category: internal or external
/// </summary>
/// <remarks>
///     This is the type of the <see cref="XmpPdfAPropertyMetadata.Category" /> property of the PDF/A metadata.
/// </remarks>
public enum XmpPdfAPropertyCategory
{
    /// <summary>
    ///     Created automatically from document content.
    /// </summary>
    Internal,

    /// <summary>
    ///     Based on user input.
    /// </summary>
    External
}

/// <summary>
///     Mapping methods for the <see cref="XmpPdfAPropertyCategory" /> enumeration.
/// </summary>
public static class XmpPdfAPropertyCategoryMappingExtensions
{
    /// <summary>
    ///     Convert the <see cref="XmpPdfAPropertyCategory" /> to its string representation.
    /// </summary>
    public static string ToXmpPdfAPropertyCategoryString(this XmpPdfAPropertyCategory category) =>
        category switch
        {
            XmpPdfAPropertyCategory.Internal => "internal",
            XmpPdfAPropertyCategory.External => "external",
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };

    /// <summary>
    ///     Convert the string to its <see cref="XmpPdfAPropertyCategory" /> representation.
    /// </summary>
    /// <seealso cref="ToXmpPdfAPropertyCategory(ReadOnlySpan{char})" />
    public static XmpPdfAPropertyCategory? ToXmpPdfAPropertyCategoryOrNull(this ReadOnlySpan<char> value) =>
        value switch
        {
            "internal" => XmpPdfAPropertyCategory.Internal,
            "external" => XmpPdfAPropertyCategory.External,
            _ => null
        };

    /// <summary>
    ///     Convert the string to its <see cref="XmpPdfAPropertyCategory" /> representation.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not a valid <see cref="XmpPdfAPropertyCategory" />.</exception>
    public static XmpPdfAPropertyCategory ToXmpPdfAPropertyCategory(this ReadOnlySpan<char> value) =>
        ToXmpPdfAPropertyCategoryOrNull(value) ?? throw new ArgumentOutOfRangeException(nameof(value), value.ToString(), null);
}
