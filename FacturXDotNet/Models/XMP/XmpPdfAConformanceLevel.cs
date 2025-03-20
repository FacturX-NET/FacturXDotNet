namespace FacturXDotNet.Models.XMP;

/// <summary>
///     PDF/A conformance level: A or B.
/// </summary>
/// <remarks>https://pdfa.org/wp-content/uploads/2011/08/tn0008_predefined_xmp_properties_in_pdfa-1_2008-03-20.pdf</remarks>
public enum XmpPdfAConformanceLevel
{
    /// <summary>
    ///     Conformance level A.
    /// </summary>
    A,

    /// <summary>
    ///     Conformance level B.
    /// </summary>
    B
}

/// <summary>
///     Mapping methods for the <see cref="XmpPdfAConformanceLevel" /> enumeration.
/// </summary>
public static class XmpPdfAConformanceMappingExtensions
{
    /// <summary>
    ///     Convert the <see cref="XmpPdfAConformanceLevel" /> to its string representation.
    /// </summary>
    public static ReadOnlySpan<char> ToXmpPdfAConformanceLevel(this XmpPdfAConformanceLevel conformanceLevel) =>
        conformanceLevel switch
        {
            XmpPdfAConformanceLevel.A => "A",
            XmpPdfAConformanceLevel.B => "B",
            _ => throw new ArgumentOutOfRangeException(nameof(conformanceLevel), conformanceLevel, null)
        };

    /// <summary>
    ///     Convert the string to its <see cref="XmpPdfAConformanceLevel" /> representation.
    /// </summary>
    /// <seealso cref="ToXmpPdfAConformanceLevel(ReadOnlySpan{char})" />
    public static XmpPdfAConformanceLevel? ToXmpPdfAConformanceLevelOrNull(this ReadOnlySpan<char> value) =>
        value switch
        {
            "A" => XmpPdfAConformanceLevel.A,
            "B" => XmpPdfAConformanceLevel.B,
            _ => null
        };

    /// <summary>
    ///     Convert the string to its <see cref="XmpPdfAConformanceLevel" /> representation.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not a valid <see cref="XmpPdfAConformanceLevel" />.</exception>
    public static XmpPdfAConformanceLevel ToXmpPdfAConformanceLevel(this ReadOnlySpan<char> value) =>
        ToXmpPdfAConformanceLevelOrNull(value) ?? throw new ArgumentOutOfRangeException(nameof(value), value.ToString(), null);
}
