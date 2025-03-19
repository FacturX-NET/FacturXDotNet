namespace FacturXDotNet.Models;

/// <summary>
///     <b>BUYER LEGAL REGISTRATION IDENTIFIER</b> - Details about the organization
/// </summary>
/// <ID>BT-47-00</ID>
/// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerTradeParty/ram:SpecifiedLegalOrganization</CiiXPath>
/// <Profile>MINIMUM</Profile>
public class BuyerTradePartySpecifiedLegalOrganization
{
    /// <summary>
    ///     <b>Buyer legal registration identifier</b> - An identifier issued by an official registrar that identifies the Buyer as a legal entity or person.
    /// </summary>
    /// <remarks>
    ///     If no identification scheme is specified, it should be known by Buyer and Seller, e.g. the identifier that is exclusively used in the applicable legal environment.
    /// </remarks>
    /// <ID>BT-47</ID>
    /// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerTradeParty/ram:SpecifiedLegalOrganization/ram:ID</CiiXPath>
    /// <Profile>MINIMUM</Profile>
    /// <ChorusPro>The identifier of the buyer (public entity) is mandatory and is always a SIRET number.</ChorusPro>
    public string? Id { get; set; }

    /// <summary>
    ///     <b>Scheme identifier</b> - The identification scheme identifier of the Buyer legal registration identifier.
    /// </summary>
    /// <remarks>
    ///     If used, the identification scheme shall be chosen from the entries of the list published by the ISO 6523 maintenance agency. For a SIREN or a SIRET, the value of this field
    ///     is "0002".
    /// </remarks>
    /// <ID>BT-47-1</ID>
    /// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerTradeParty/ram:SpecifiedLegalOrganization/ram:ID/@schemeID</CiiXPath>
    /// <Profile>MINIMUM</Profile>
    public string? IdSchemeId { get; set; }
}
