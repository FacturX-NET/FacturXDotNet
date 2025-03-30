namespace FacturXDotNet.Models.CII;

/// <summary>
///     <b>SELLER POSTAL ADDRESS</b> - A group of business terms providing information about the address of the Seller.
/// </summary>
/// <remarks>
///     Sufficient components of the address are to be filled in order to comply to legal requirements.
///     Like any address, the fields necessary to define the address must appear. The country code is mandatory.
/// </remarks>
/// <ID>BG-5</ID>
/// <BR-8>An Invoice shall contain the Seller postal address.</BR-8>
/// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:PostalTradeAddress</CiiXPath>
/// <Profile>MINIMUM</Profile>
public class SellerTradePartyPostalTradeAddress
{
    /// <summary>
    ///     <b>Seller country code</b> - A code that identifies the country.
    /// </summary>
    /// <remarks>
    ///     If no tax representative is specified, this is the country where VAT is liable. The lists of valid countries are registered with the ISO 3166-1 Maintenance agency, "Codes for
    ///     the representation of names of countries and their subdivisions".
    /// </remarks>
    /// <ID>BT-40</ID>
    /// <BR-9>The Seller postal address (BG-5) shall contain a Seller country code.</BR-9>
    /// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:PostalTradeAddress/ram:CountryID</CiiXPath>
    /// <Profile>MINIMUM</Profile>
    public string? CountryId { get; set; }
}
