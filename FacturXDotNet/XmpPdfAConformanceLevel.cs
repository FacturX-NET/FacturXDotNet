namespace FacturXDotNet;

/// <summary>
///     PDF/A conformance level: A or B.
/// </summary>
/// <remarks>https://pdfa.org/wp-content/uploads/2011/08/tn0008_predefined_xmp_properties_in_pdfa-1_2008-03-20.pdf</remarks>
public enum XmpPdfAConformanceLevel
{
    A,
    B
}

public static class XmpPdfAConformanceMappingExtensions
{
    /// <summary>
    ///     Convert a <see cref="XmpPdfAConformanceLevel" /> to a string.
    /// </summary>
    /// <param name="conformanceLevel">The conformance level to convert.</param>
    /// <returns>The string representation of the conformance level.</returns>
    public static string ToXmpPdfAConformanceString(this XmpPdfAConformanceLevel conformanceLevel) =>
        conformanceLevel switch
        {
            XmpPdfAConformanceLevel.A => "A",
            XmpPdfAConformanceLevel.B => "B",
            _ => throw new ArgumentOutOfRangeException(nameof(conformanceLevel), conformanceLevel, null)
        };

    public static XmpPdfAConformanceLevel? ToXmpPdfAConformanceLevelOrNull(this string value) =>
        value switch
        {
            "A" => XmpPdfAConformanceLevel.A,
            "B" => XmpPdfAConformanceLevel.B,
            _ => null
        };

    public static XmpPdfAConformanceLevel ToXmpPdfAConformanceLevel(this string value) =>
        ToXmpPdfAConformanceLevelOrNull(value) ?? throw new ArgumentOutOfRangeException(nameof(value), value, null);
}
