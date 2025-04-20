namespace FacturXDotNet.Models.CII;

/// <summary>
///     <b>Specification identifier</b> - An identification of the specification containing the total set of rules regarding semantic content, cardinalities and business rules to
///     which the data contained in the instance document conforms.
/// </summary>
/// <remarks>
///     This identifies compliance or conformance to the specification. Conformant invoices specify: urn:cen.eu:en16931:2017. Invoices, compliant to a user specification may identify
///     that user specification here. No identification scheme is to be used.
/// </remarks>
/// <ID>BT-24</ID>
/// <BR-1>An Invoice shall have a Specification identifier.</BR-1>
/// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:ExchangedDocumentContext/ram:GuidelineSpecifiedDocumentContextParameter/ram:ID</CiiXPath>
/// <Profile>MINIMUM</Profile>
public enum GuidelineSpecifiedDocumentContextParameterId
{
    /// <summary>
    ///     urn:factur-x.eu:1p0:minimum
    /// </summary>
    Minimum = 1,

    /// <summary>
    ///     urn:factur-x.eu:1p0:basicwl
    /// </summary>
    BasicWl = 2,

    /// <summary>
    ///     urn:cen.eu:en16931:2017#compliant#urn:factur-x.eu:1p0:basic
    /// </summary>
    Basic = 3,

    /// <summary>
    ///     urn:cen.eu:en16931:2017
    /// </summary>
    En16931 = 4,

    /// <summary>
    ///     urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended
    /// </summary>
    Extended = 5
}

/// <summary>
///     Mapping methods for <see cref="GuidelineSpecifiedDocumentContextParameterId" /> enumeration.
/// </summary>
public static class GuidelineSpecifiedDocumentContextParameterIdMappingExtensions
{
    /// <summary>
    ///     Converts the <see cref="GuidelineSpecifiedDocumentContextParameterId" /> enumeration to its string representation.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static ReadOnlySpan<char> ToGuidelineSpecifiedDocumentContextParameterId(this GuidelineSpecifiedDocumentContextParameterId value) =>
        value switch
        {
            GuidelineSpecifiedDocumentContextParameterId.Minimum => "urn:factur-x.eu:1p0:minimum",
            GuidelineSpecifiedDocumentContextParameterId.BasicWl => "urn:factur-x.eu:1p0:basicwl",
            GuidelineSpecifiedDocumentContextParameterId.Basic => "urn:cen.eu:en16931:2017#compliant#urn:factur-x.eu:1p0:basic",
            GuidelineSpecifiedDocumentContextParameterId.En16931 => "urn:cen.eu:en16931:2017",
            GuidelineSpecifiedDocumentContextParameterId.Extended => "urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };

    /// <summary>
    ///     Converts the string to its <see cref="GuidelineSpecifiedDocumentContextParameterId" /> representation.
    /// </summary>
    /// <seealso cref="ToGuidelineSpecifiedDocumentContextParameterId(ReadOnlySpan{char})" />
    public static GuidelineSpecifiedDocumentContextParameterId? ToGuidelineSpecifiedDocumentContextParameterIdOrNull(this ReadOnlySpan<char> value) =>
        value switch
        {
            "urn:factur-x.eu:1p0:minimum" => GuidelineSpecifiedDocumentContextParameterId.Minimum,
            "urn:factur-x.eu:1p0:basicwl" => GuidelineSpecifiedDocumentContextParameterId.BasicWl,
            "urn:cen.eu:en16931:2017#compliant#urn:factur-x.eu:1p0:basic" => GuidelineSpecifiedDocumentContextParameterId.Basic,
            "urn:cen.eu:en16931:2017" => GuidelineSpecifiedDocumentContextParameterId.En16931,
            "urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended" => GuidelineSpecifiedDocumentContextParameterId.Extended,
            _ => null
        };

    /// <summary>
    ///     Converts the string to its <see cref="GuidelineSpecifiedDocumentContextParameterId" /> representation.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not a valid <see cref="GuidelineSpecifiedDocumentContextParameterId" />.</exception>
    public static GuidelineSpecifiedDocumentContextParameterId ToGuidelineSpecifiedDocumentContextParameterId(this ReadOnlySpan<char> value) =>
        value.ToGuidelineSpecifiedDocumentContextParameterIdOrNull()
        ?? throw new ArgumentOutOfRangeException(nameof(GuidelineSpecifiedDocumentContextParameterId), value.ToString(), null);
}
