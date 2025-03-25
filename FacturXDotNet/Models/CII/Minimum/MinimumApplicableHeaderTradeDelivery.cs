namespace FacturXDotNet.Models.CII.Minimum;

/// <inheritdoc cref="CII.ApplicableHeaderTradeDelivery" />
public class MinimumApplicableHeaderTradeDelivery
{
    internal MinimumApplicableHeaderTradeDelivery(ApplicableHeaderTradeDelivery applicableHeaderTradeDelivery)
    {
        ApplicableHeaderTradeDelivery = applicableHeaderTradeDelivery;
    }

    internal ApplicableHeaderTradeDelivery ApplicableHeaderTradeDelivery { get; }
}
