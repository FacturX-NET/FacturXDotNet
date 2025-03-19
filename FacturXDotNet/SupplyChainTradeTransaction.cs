namespace FacturXDotNet;

/// <summary>
///     <b>SUPPLY CHAIN TRADE TRANSACTION</b>
/// </summary>
/// <ID>BG-25-00</ID>
/// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction</CiiXPath>
/// <Profile>MINIMUM</Profile>
public sealed class SupplyChainTradeTransaction
{
    /// <inheritdoc cref="FacturXDotNet.ApplicableHeaderTradeAgreement" />
    public required ApplicableHeaderTradeAgreement ApplicableHeaderTradeAgreement { get; set; }

    /// <inheritdoc cref="FacturXDotNet.ApplicableHeaderTradeDelivery" />
    public required ApplicableHeaderTradeDelivery ApplicableHeaderTradeDelivery { get; set; }

    /// <inheritdoc cref="FacturXDotNet.ApplicableHeaderTradeSettlement" />
    public ApplicableHeaderTradeSettlement? ApplicableHeaderTradeSettlement { get; set; }
}
