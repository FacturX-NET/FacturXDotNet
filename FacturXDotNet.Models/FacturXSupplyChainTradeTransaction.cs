namespace FacturXDotNet.Models;

/// <summary>
///     <b>SUPPLY CHAIN TRADE TRANSACTION</b>
/// </summary>
/// <ID>BG-25-00</ID>
/// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction</CiiXPath>
/// <Profile>MINIMUM</Profile>
public sealed class FacturXSupplyChainTradeTransaction
{
    /// <inheritdoc cref="FacturXApplicableHeaderTradeAgreement" />
    public required FacturXApplicableHeaderTradeAgreement ApplicableHeaderTradeAgreement { get; set; }

    /// <inheritdoc cref="FacturXApplicableHeaderTradeDelivery" />
    public required FacturXApplicableHeaderTradeDelivery ApplicableHeaderTradeDelivery { get; set; }

    /// <inheritdoc cref="FacturXApplicableHeaderTradeSettlement" />
    public FacturXApplicableHeaderTradeSettlement? ApplicableHeaderTradeSettlement { get; set; }
}
