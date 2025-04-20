using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Models;

/// <summary>
///     Factur-X profile.
/// </summary>
/// <seealso cref="GuidelineSpecifiedDocumentContextParameterId" />
public enum FacturXProfile
{
    /// <summary>
    ///     No profile.
    /// </summary>
    None = 0,

    /// <summary>
    ///     The minimum profile.
    /// </summary>
    /// <seealso cref="GuidelineSpecifiedDocumentContextParameterId.Minimum" />
    Minimum = 1,

    /// <summary>
    ///     The BASIC WL profile.
    /// </summary>
    /// <seealso cref="GuidelineSpecifiedDocumentContextParameterId.BasicWl" />
    BasicWl = 2,

    /// <summary>
    ///     The BASIC profile.
    /// </summary>
    /// <seealso cref="GuidelineSpecifiedDocumentContextParameterId.Basic" />
    Basic = 3,

    /// <summary>
    ///     The EN 16931 profile.
    /// </summary>
    /// <seealso cref="GuidelineSpecifiedDocumentContextParameterId.En16931" />
    En16931 = 4,

    /// <summary>
    ///     The EXTENDED profile.
    /// </summary>
    /// <seealso cref="GuidelineSpecifiedDocumentContextParameterId.Extended" />
    Extended = 5
}

/// <summary>
///     Mapping methods for <see cref="FacturXProfile" />.
/// </summary>
public static class FacturXProfileMappingExtensions
{
    /// <summary>
    ///     Convert the <see cref="FacturXProfile" /> to its string representation.
    /// </summary>
    public static ReadOnlySpan<char> ToFacturXProfile(this FacturXProfile profile) =>
        profile switch
        {
            FacturXProfile.None => string.Empty,
            FacturXProfile.Minimum => "MINIMUM",
            FacturXProfile.BasicWl => "BASIC WL",
            FacturXProfile.Basic => "BASIC",
            FacturXProfile.En16931 => "EN16931",
            FacturXProfile.Extended => "EXTENDED",
            _ => throw new ArgumentOutOfRangeException(nameof(profile), profile, null)
        };

    /// <summary>
    ///     Convert the string to its <see cref="FacturXProfile" /> representation.
    /// </summary>
    /// <seealso cref="ToFacturXProfileOrNull(ReadOnlySpan{char})" />
    public static FacturXProfile? ToFacturXProfileOrNull(this ReadOnlySpan<char> profile) =>
        profile switch
        {
            "MINIMUM" => FacturXProfile.Minimum,
            "BASIC WL" => FacturXProfile.BasicWl,
            "BASIC" => FacturXProfile.Basic,
            "EN16931" => FacturXProfile.En16931,
            "EXTENDED" => FacturXProfile.Extended,
            _ => null
        };

    /// <summary>
    ///     Convert the string to its <see cref="FacturXProfile" /> representation.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not a valid <see cref="FacturXProfile" />.</exception>
    public static FacturXProfile ToFacturXProfile(this ReadOnlySpan<char> profile) =>
        ToFacturXProfileOrNull(profile) ?? throw new ArgumentOutOfRangeException(nameof(profile), profile.ToString(), null);

    /// <summary>
    ///     Convert the <see cref="FacturXProfile" /> to its <see cref="GuidelineSpecifiedDocumentContextParameterId" /> representation.
    /// </summary>
    public static GuidelineSpecifiedDocumentContextParameterId ToGuideLineSpecifiedDocumentContextParameterId(this FacturXProfile profile) =>
        profile switch
        {
            FacturXProfile.Minimum => GuidelineSpecifiedDocumentContextParameterId.Minimum,
            FacturXProfile.BasicWl => GuidelineSpecifiedDocumentContextParameterId.BasicWl,
            FacturXProfile.Basic => GuidelineSpecifiedDocumentContextParameterId.Basic,
            FacturXProfile.En16931 => GuidelineSpecifiedDocumentContextParameterId.En16931,
            FacturXProfile.Extended => GuidelineSpecifiedDocumentContextParameterId.Extended,
            _ => throw new ArgumentOutOfRangeException(nameof(profile), profile, null)
        };

    /// <summary>
    ///     Convert the <see cref="GuidelineSpecifiedDocumentContextParameterId" /> to its <see cref="FacturXProfile" /> representation.
    /// </summary>
    /// <seealso cref="ToFacturXProfileOrNull(GuidelineSpecifiedDocumentContextParameterId)" />
    public static FacturXProfile? ToFacturXProfileOrNull(this GuidelineSpecifiedDocumentContextParameterId profile) =>
        profile switch
        {
            GuidelineSpecifiedDocumentContextParameterId.Minimum => FacturXProfile.Minimum,
            GuidelineSpecifiedDocumentContextParameterId.BasicWl => FacturXProfile.BasicWl,
            GuidelineSpecifiedDocumentContextParameterId.Basic => FacturXProfile.Basic,
            GuidelineSpecifiedDocumentContextParameterId.En16931 => FacturXProfile.En16931,
            GuidelineSpecifiedDocumentContextParameterId.Extended => FacturXProfile.Extended,
            _ => null
        };

    /// <summary>
    ///     Convert the <see cref="GuidelineSpecifiedDocumentContextParameterId" /> to its <see cref="FacturXProfile" /> representation.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not a valid <see cref="FacturXProfile" />.</exception>
    public static FacturXProfile ToFacturXProfile(this GuidelineSpecifiedDocumentContextParameterId profile) =>
        ToFacturXProfileOrNull(profile) ?? throw new ArgumentOutOfRangeException(nameof(profile), profile, null);

    /// <summary>
    ///     Convert the <see cref="FacturXProfile" /> to its <see cref="XmpFacturXConformanceLevel" /> representation.
    /// </summary>
    public static XmpFacturXConformanceLevel ToXmpFacturXConformanceLevel(this FacturXProfile profile) =>
        profile switch
        {
            FacturXProfile.Minimum => XmpFacturXConformanceLevel.Minimum,
            FacturXProfile.BasicWl => XmpFacturXConformanceLevel.BasicWl,
            FacturXProfile.Basic => XmpFacturXConformanceLevel.Basic,
            FacturXProfile.En16931 => XmpFacturXConformanceLevel.En16931,
            FacturXProfile.Extended => XmpFacturXConformanceLevel.Extended,
            _ => throw new ArgumentOutOfRangeException(nameof(profile), profile, null)
        };

    /// <summary>
    ///     Convert the <see cref="XmpFacturXConformanceLevel" /> to its <see cref="FacturXProfile" /> representation.
    /// </summary>
    /// <seealso cref="ToFacturXProfile(XmpFacturXConformanceLevel)" />
    public static FacturXProfile? ToFacturXProfileOrNull(this XmpFacturXConformanceLevel profile) =>
        profile switch
        {
            XmpFacturXConformanceLevel.Minimum => FacturXProfile.Minimum,
            XmpFacturXConformanceLevel.BasicWl => FacturXProfile.BasicWl,
            XmpFacturXConformanceLevel.Basic => FacturXProfile.Basic,
            XmpFacturXConformanceLevel.En16931 => FacturXProfile.En16931,
            XmpFacturXConformanceLevel.Extended => FacturXProfile.Extended,
            _ => null
        };

    /// <summary>
    ///     Convert the <see cref="XmpFacturXConformanceLevel" /> to its <see cref="FacturXProfile" /> representation.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not a valid <see cref="FacturXProfile" />.</exception>
    public static FacturXProfile ToFacturXProfile(this XmpFacturXConformanceLevel profile) =>
        ToFacturXProfileOrNull(profile) ?? throw new ArgumentOutOfRangeException(nameof(profile), profile, null);
}
