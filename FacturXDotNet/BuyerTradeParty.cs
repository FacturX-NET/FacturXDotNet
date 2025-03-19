namespace FacturXDotNet;

/// <summary>
///     <b>BUYER</b> - A group of business terms providing information about the Buyer.
/// </summary>
/// <ID>BG-7</ID>
/// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerTradeParty</CiiXPath>
/// <Profile>MINIMUM</Profile>
public class BuyerTradeParty
{
    /// <summary>
    ///     <b>Buyer name</b> - The full name of the Buyer.
    /// </summary>
    /// <ID>BT-44</ID>
    /// <BR-7>An Invoice shall contain the Buyer name.</BR-7>
    /// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerTradeParty/ram:Name</CiiXPath>
    /// <Profile>MINIMUM</Profile>
    /// <ChorusPro>This field is limited to 99 characters.</ChorusPro>
    public required string Name { get; set; }

    /// <inheritdoc cref="BuyerTradePartySpecifiedLegalOrganization" />
    public BuyerTradePartySpecifiedLegalOrganization? SpecifiedLegalOrganization { get; set; }
}
