namespace FacturXDotNet.Models.CII.Minimum;

/// <inheritdoc cref="CII.ApplicableHeaderTradeDelivery" />
public class MinimumApplicableHeaderTradeDelivery
{
    internal MinimumApplicableHeaderTradeDelivery(ApplicableHeaderTradeDelivery applicableHeaderTradeDelivery)
    {
        ApplicableHeaderTradeDelivery = applicableHeaderTradeDelivery;
    }

    internal ApplicableHeaderTradeDelivery ApplicableHeaderTradeDelivery { get; }

    /// <summary>
    ///     Return the <see cref="CII.ApplicableHeaderTradeDelivery" /> that this class is a view of.
    /// </summary>
    /// <remarks>
    ///     The <see cref="MinimumApplicableHeaderTradeDelivery" /> view is useful when manipulating the invoice because it has better nullability checks, but some methods still
    ///     use the <see cref="CII.ApplicableHeaderTradeDelivery" /> class. This method allows you to get the original <see cref="CII.ApplicableHeaderTradeDelivery" /> back.
    /// </remarks>
    /// <returns>The <see cref="CII.ApplicableHeaderTradeDelivery" /> that this class is a view of.</returns>
    public ApplicableHeaderTradeDelivery ToApplicableHeaderTradeDelivery() => ApplicableHeaderTradeDelivery;

    /// <summary>
    ///     Implicitly convert a <see cref="MinimumApplicableHeaderTradeDelivery" /> to a <see cref="CII.ApplicableHeaderTradeDelivery" />.
    /// </summary>
    /// <param name="applicableHeaderTradeDelivery">The <see cref="MinimumApplicableHeaderTradeDelivery" /> to convert.</param>
    /// <returns>The <see cref="CII.ApplicableHeaderTradeDelivery" /> that this class is a view of.</returns>
    public static implicit operator ApplicableHeaderTradeDelivery(MinimumApplicableHeaderTradeDelivery applicableHeaderTradeDelivery) =>
        applicableHeaderTradeDelivery.ToApplicableHeaderTradeDelivery();
}
