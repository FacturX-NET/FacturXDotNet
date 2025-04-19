namespace FacturXDotNet.Models.CII;

/// <summary>
///     <b>PURCHASE ORDER REFERENCE</b>
/// </summary>
/// <ID>BT-13-00</ID>
/// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerOrderReferencedDocument</CiiXPath>
/// <Profile>MINIMUM</Profile>
public class BuyerOrderReferencedDocument
{
    /// <summary>
    ///     <b>Purchase order reference</b> - An identifier of a referenced purchase order, issued by the Buyer.
    /// </summary>
    /// <ID>BT-13</ID>
    /// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerOrderReferencedDocument/ram:IssuerAssignedID</CiiXPath>
    /// <Profile>MINIMUM</Profile>
    /// <ChorusPro>
    ///     For the public sector, this is the "Engagement Juridique" (Legal Commitment). It is mandatory for some buyers. You should refer to the ChorusPro Directory
    ///     to identify these public entity buyers that make it mandatory.
    /// </ChorusPro>
    public string? IssuerAssignedId { get; set; }
}
