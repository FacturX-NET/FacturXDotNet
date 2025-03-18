namespace FacturXDotNet.Models;

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
public enum FacturXGuidelineSpecifiedDocumentContextParameterId
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

public static class FacturXGuidelineSpecifiedDocumentContextParameterIdMappingExtensions
{
    public static string ToSpecificationIdentifierString(this FacturXGuidelineSpecifiedDocumentContextParameterId value) =>
        value switch
        {
            FacturXGuidelineSpecifiedDocumentContextParameterId.Minimum => "urn:factur-x.eu:1p0:minimum",
            FacturXGuidelineSpecifiedDocumentContextParameterId.BasicWl => "urn:factur-x.eu:1p0:basicwl",
            FacturXGuidelineSpecifiedDocumentContextParameterId.Basic => "urn:cen.eu:en16931:2017#compliant#urn:factur-x.eu:1p0:basic",
            FacturXGuidelineSpecifiedDocumentContextParameterId.En16931 => "urn:cen.eu:en16931:2017",
            FacturXGuidelineSpecifiedDocumentContextParameterId.Extended => "urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };

    public static FacturXGuidelineSpecifiedDocumentContextParameterId? ToSpecificationIdentifier(this string value) =>
        value switch
        {
            "urn:factur-x.eu:1p0:minimum" => FacturXGuidelineSpecifiedDocumentContextParameterId.Minimum,
            "urn:factur-x.eu:1p0:basicwl" => FacturXGuidelineSpecifiedDocumentContextParameterId.BasicWl,
            "urn:cen.eu:en16931:2017#compliant#urn:factur-x.eu:1p0:basic" => FacturXGuidelineSpecifiedDocumentContextParameterId.Basic,
            "urn:cen.eu:en16931:2017" => FacturXGuidelineSpecifiedDocumentContextParameterId.En16931,
            "urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended" => FacturXGuidelineSpecifiedDocumentContextParameterId.Extended,
            _ => null
        };
}
