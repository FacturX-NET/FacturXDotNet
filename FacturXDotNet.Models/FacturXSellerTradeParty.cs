namespace FacturXDotNet.Models;

/// <summary>
///     <b>SELLER</b> - A group of business terms providing information about the Seller.
/// </summary>
/// <ID>BG-4</ID>
/// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty</CiiXPath>
/// <Profile>MINIMUM</Profile>
public class FacturXSellerTradeParty
{
    /// <summary>
    ///     <b>Seller name</b> - The full formal name by which the Seller is registered in the national registry of legal entities or as a Taxable person or otherwise trades as a person
    ///     or persons.
    /// </summary>
    /// <ID>BT-27</ID>
    /// <BR-6>An Invoice shall contain the Seller name.</BR-6>
    /// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:Name</CiiXPath>
    /// <Profile>MINIMUM</Profile>
    public required string Name { get; set; }

    /// <inheritdoc cref="FacturXSellerTradePartySpecifiedLegalOrganization" />
    public FacturXSellerTradePartySpecifiedLegalOrganization? SpecifiedLegalOrganization { get; set; }

    /// <inheritdoc cref="FacturXSellerTradePartyPostalTradeAddress" />
    public required FacturXSellerTradePartyPostalTradeAddress PostalTradeAddress { get; set; }

    /// <inheritdoc cref="FacturXSellerTradePartySpecifiedTaxRegistration" />
    public FacturXSellerTradePartySpecifiedTaxRegistration? SpecifiedTaxRegistration { get; set; }
}
