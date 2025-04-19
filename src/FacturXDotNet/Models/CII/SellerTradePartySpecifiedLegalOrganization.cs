namespace FacturXDotNet.Models.CII;

/// <summary>
///     <b>SELLER LEGAL ORGANIZATION</b> - Details about the organization.
/// </summary>
/// <ID>BT-30-00</ID>
/// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:SpecifiedLegalOrganization</CiiXPath>
/// <Profile>MINIMUM</Profile>
public class SellerTradePartySpecifiedLegalOrganization
{
    /// <summary>
    ///     <b>Seller legal registration identifier</b> - An identifier issued by an official registrar that identifies the Seller as a legal entity or person.
    /// </summary>
    /// <remarks>
    ///     If no identification scheme is specified, it must be known by Buyer and Seller.
    /// </remarks>
    /// <ID>BT-30</ID>
    /// <BR-CO-26>
    ///     In order for the buyer to automatically identify a supplier, the Seller identifier (BT-29), the Seller legal registration identifier (BT-30)
    ///     and/or the Seller VAT identifier (BT-31) shall be present.
    /// </BR-CO-26>
    /// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:SpecifiedLegalOrganization/ram:ID</CiiXPath>
    /// <Profile>MINIMUM</Profile>
    public string? Id { get; set; }

    /// <summary>
    ///     <b>Scheme identifier</b> - The identification scheme identifier of the Seller legal registration identifier.
    /// </summary>
    /// <remarks>
    ///     If used, the identification scheme shall be chosen from the entries of the list published by the ISO/IEC 6523 maintenance agency. For a SIREN or a SIRET, the value of this
    ///     field is "0002".
    /// </remarks>
    /// <ID>BT-30-1</ID>
    /// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:SpecifiedLegalOrganization/ram:ID@schemeID</CiiXPath>
    /// <Profile>MINIMUM</Profile>
    public string? IdSchemeId { get; set; }
}
