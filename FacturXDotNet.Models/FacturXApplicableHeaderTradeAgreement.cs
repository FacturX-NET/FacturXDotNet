namespace FacturXDotNet.Models;

/// <summary>
///     <b>HEADER TRADE AGREEMENT</b>
/// </summary>
/// <ID>BT-10-00</ID>
/// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement</CiiXPath>
/// <Profile>MINIMUM</Profile>
public class FacturXApplicableHeaderTradeAgreement
{
    /// <summary>
    ///     <b>Buyer reference</b> - An identifier assigned by the Buyer used for internal routing purposes.
    /// </summary>
    /// <remarks>
    ///     The identifier is defined by the Buyer (e.g. contact ID, department, office id, project code), but provided by the Seller in the Invoice.
    /// </remarks>
    /// <ID>BT-10</ID>
    /// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerReference</CiiXPath>
    /// <Profile>MINIMUM</Profile>
    /// <ChorusPro>For the public sector, it is the "Service Exécutant". It is mandatory for some buyers. It must belong to the Chorus Pro repository. It is limited to 100 characters.</ChorusPro>
    public string? BuyerReference { get; set; }

    /// <inheritdoc cref="FacturXSellerTradeParty" />
    public required FacturXSellerTradeParty SellerTradeParty { get; set; }

    /// <inheritdoc cref="FacturXBuyerTradeParty" />
    public required FacturXBuyerTradeParty BuyerTradeParty { get; set; }

    /// <inheritdoc cref="FacturXBuyerOrderReferencedDocument" />
    public FacturXBuyerOrderReferencedDocument? BuyerOrderReferencedDocument { get; set; }
}
