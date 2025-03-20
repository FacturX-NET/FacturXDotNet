namespace FacturXDotNet;

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

public static class XmpPdfAPropertyCategoryMappingExtensions
{
    public static string ToXmpPdfAPropertyCategoryString(this XmpPdfAPropertyCategory category) =>
        category switch
        {
            XmpPdfAPropertyCategory.Internal => "internal",
            XmpPdfAPropertyCategory.External => "external",
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };

    public static XmpPdfAPropertyCategory? ToXmpPdfAPropertyCategoryOrNull(this string value) =>
        value switch
        {
            "internal" => XmpPdfAPropertyCategory.Internal,
            "external" => XmpPdfAPropertyCategory.External,
            _ => null
        };

    public static XmpPdfAPropertyCategory ToXmpPdfAPropertyCategory(this string value) =>
        ToXmpPdfAPropertyCategoryOrNull(value) ?? throw new ArgumentOutOfRangeException(nameof(value), value, null);
}
