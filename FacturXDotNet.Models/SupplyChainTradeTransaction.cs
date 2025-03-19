namespace FacturXDotNet.Models;

/// <summary>
///     <b>SUPPLY CHAIN TRADE TRANSACTION</b>
/// </summary>
/// <ID>BG-25-00</ID>
/// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction</CiiXPath>
/// <Profile>MINIMUM</Profile>
public sealed class SupplyChainTradeTransaction
{
    /// <inheritdoc cref="Models.ApplicableHeaderTradeAgreement" />
    public required ApplicableHeaderTradeAgreement ApplicableHeaderTradeAgreement { get; set; }

    /// <inheritdoc cref="Models.ApplicableHeaderTradeDelivery" />
    public required ApplicableHeaderTradeDelivery ApplicableHeaderTradeDelivery { get; set; }

    /// <inheritdoc cref="Models.ApplicableHeaderTradeSettlement" />
    public ApplicableHeaderTradeSettlement? ApplicableHeaderTradeSettlement { get; set; }
}
