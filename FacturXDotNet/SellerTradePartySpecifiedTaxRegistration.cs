namespace FacturXDotNet;

/// <summary>
///     <b>SELLER VAT IDENTIFIER</b> - Detailed information on tax information of the seller
/// </summary>
/// <ID>BT-31-00</ID>
/// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:SpecifiedTaxRegistration</CiiXPath>
/// <Profile>MINIMUM</Profile>
public class SellerTradePartySpecifiedTaxRegistration
{
    /// <summary>
    ///     <b>Seller VAT identifier</b> - The Seller's VAT identifier (also known as Seller VAT identification number).
    /// </summary>
    /// <remarks>
    ///     VAT number prefixed by a country code. A VAT registered Supplier shall include his VAT ID, except when he uses a tax representative.
    /// </remarks>
    /// <ID>BT-31</ID>
    /// <BR-CO-9>
    ///     The Seller VAT identifier, the Seller tax representative VAT identifier (BT-63) and the Buyer VAT identifier (BT-48) shall have a prefix in accordance with ISO
    ///     code ISO 3166-1 alpha-2 by which the country of issue may be identified. Nevertheless, Greece may use the prefix ‘EL’.
    /// </BR-CO-9>
    /// <BR-CO-26>
    ///     In order for the buyer to automatically identify a supplier, the Seller identifier (BT-29), the Seller legal registration identifier (BT-30) and/or the Seller VAT
    ///     identifier shall be present.
    /// </BR-CO-26>
    /// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:SpecifiedTaxRegistration/ram:ID</CiiXPath>
    /// <Profile>MINIMUM</Profile>
    public string? Id { get; set; }

    /// <summary>
    ///     <b>Tax Scheme identifier</b> - Scheme identifier for supplier VAT identifier.
    /// </summary>
    /// <remarks>
    ///     Value = VA
    /// </remarks>
    /// <ID>BT-31-0</ID>
    /// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:SpecifiedTaxRegistration/ram:ID/@schemeID</CiiXPath>
    /// <Profile>MINIMUM</Profile>
    public VatOnlyTaxSchemeIdentifier IdSchemeId { get; set; }
}
