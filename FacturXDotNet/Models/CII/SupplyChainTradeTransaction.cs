namespace FacturXDotNet.Models.CII;

/// <summary>
///     <b>SUPPLY CHAIN TRADE TRANSACTION</b>
/// </summary>
/// <ID>BG-25-00</ID>
/// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction</CiiXPath>
/// <Profile>MINIMUM</Profile>
public sealed class SupplyChainTradeTransaction
{
    /// <inheritdoc cref="CII.ApplicableHeaderTradeAgreement" />
    public ApplicableHeaderTradeAgreement? ApplicableHeaderTradeAgreement { get; set; }

    /// <inheritdoc cref="CII.ApplicableHeaderTradeDelivery" />
    public ApplicableHeaderTradeDelivery? ApplicableHeaderTradeDelivery { get; set; }

    /// <inheritdoc cref="CII.ApplicableHeaderTradeSettlement" />
    public ApplicableHeaderTradeSettlement? ApplicableHeaderTradeSettlement { get; set; }
}
