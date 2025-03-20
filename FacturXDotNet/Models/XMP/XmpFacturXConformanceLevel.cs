namespace FacturXDotNet.Models.XMP;

/// <summary>
///     The conformance level of the embedded XML document.
/// </summary>
public enum XmpFacturXConformanceLevel
{
    /// <summary>
    ///     The included document uses a MINIMUM profile
    /// </summary>
    /// <remarks>
    ///     Only applicable in Factur-X/ZUGFeRD. Not allowed in Germany from 2025-01-01.
    /// </remarks>
    /// <seealso cref="FacturXProfile.Minimum" />
    Minimum,

    /// <summary>
    ///     The included document uses a Basic Without Lines profile
    /// </summary>
    /// <remarks>
    ///     Only applicable in Factur-X/ZUGFeRD. Not allowed in Germany from 2025-01-01.
    /// </remarks>
    /// <seealso cref="FacturXProfile.BasicWl" />
    BasicWl,

    /// <summary>
    ///     The included document uses a Basic profile
    /// </summary>
    /// <remarks>
    ///     Applicable in Factur-X/ZUGFeRD and Order-X. For Factur-X/ZUGFeRD the BASIC profile is compliant to the EN16931.
    /// </remarks>
    /// <seealso cref="FacturXProfile.Basic" />
    Basic,

    /// <summary>
    ///     The included document uses a Comfort profile
    /// </summary>
    /// <remarks>
    ///     Only applicable in Order-X.
    /// </remarks>
    Comfort,

    /// <summary>
    ///     The included document uses a EN 16931 profile
    /// </summary>
    /// <remarks>
    ///     Only applicable in Factur-X/ZUGFeRD. This profile is compliant to the EN16931.
    /// </remarks>
    /// <seealso cref="FacturXProfile.En16931" />
    En16931,

    /// <summary>
    ///     The included document uses a Comfort profile
    /// </summary>
    /// <remarks>
    ///     Applicable in Factur-X/ZUGFeRD and Order-X. For Factur-X/ZUGFeRD the EXTENDED profile is compliant to and conformant extension of the EN16931.
    /// </remarks>
    /// <seealso cref="FacturXProfile.Extended" />
    Extended,

    /// <summary>
    ///     The included document uses an XRECHNUNG profile
    /// </summary>
    /// <remarks>
    ///     Only applicable in Factur-X/ZUGFeRD. Not applicable in France.
    /// </remarks>
    XRechnung
}

/// <summary>
///     Mapping methods for <see cref="XmpFacturXConformanceLevel" />.
/// </summary>
public static class XmpFacturXConformanceLevelMappingExtensions
{
    /// <summary>
    ///     Convert the <see cref="XmpFacturXConformanceLevel" /> to its string representation.
    /// </summary>
    public static ReadOnlySpan<char> ToXmpFacturXConformanceLevel(this XmpFacturXConformanceLevel conformanceLevel) =>
        conformanceLevel switch
        {
            XmpFacturXConformanceLevel.Minimum => "MINIMUM",
            XmpFacturXConformanceLevel.BasicWl => "BASIC WL",
            XmpFacturXConformanceLevel.Basic => "BASIC",
            XmpFacturXConformanceLevel.Comfort => "COMFORT",
            XmpFacturXConformanceLevel.En16931 => "EN 16931",
            XmpFacturXConformanceLevel.Extended => "EXTENDED",
            XmpFacturXConformanceLevel.XRechnung => "XRECHNUNG",
            _ => throw new ArgumentOutOfRangeException(nameof(conformanceLevel), conformanceLevel, null)
        };

    /// <summary>
    ///     Convert the string to its <see cref="XmpFacturXConformanceLevel" /> representation.
    /// </summary>
    /// <seealso cref="ToXmpFacturXConformanceLevel(ReadOnlySpan{char})" />
    public static XmpFacturXConformanceLevel? ToXmpFacturXConformanceLevelOrNull(this ReadOnlySpan<char> value) =>
        value switch
        {
            "MINIMUM" => XmpFacturXConformanceLevel.Minimum,
            "BASIC WL" => XmpFacturXConformanceLevel.BasicWl,
            "BASIC" => XmpFacturXConformanceLevel.Basic,
            "COMFORT" => XmpFacturXConformanceLevel.Comfort,
            "EN 16931" => XmpFacturXConformanceLevel.En16931,
            "EXTENDED" => XmpFacturXConformanceLevel.Extended,
            "XRECHNUNG" => XmpFacturXConformanceLevel.XRechnung,
            _ => null
        };

    /// <summary>
    ///     Convert the string to its <see cref="XmpFacturXConformanceLevel" /> representation.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not a valid <see cref="XmpFacturXConformanceLevel" />.</exception>
    public static XmpFacturXConformanceLevel ToXmpFacturXConformanceLevel(this ReadOnlySpan<char> value) =>
        ToXmpFacturXConformanceLevelOrNull(value) ?? throw new ArgumentOutOfRangeException(nameof(value), value.ToString(), null);
}
