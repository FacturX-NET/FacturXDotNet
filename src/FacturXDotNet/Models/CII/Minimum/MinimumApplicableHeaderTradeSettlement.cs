namespace FacturXDotNet.Models.CII.Minimum;

/// <inheritdoc cref="CII.ApplicableHeaderTradeSettlement" />
public class MinimumApplicableHeaderTradeSettlement
{
    MinimumSpecifiedTradeSettlementHeaderMonetarySummation _specifiedTradeSettlementHeaderMonetarySummation;

    internal MinimumApplicableHeaderTradeSettlement(ApplicableHeaderTradeSettlement applicableHeaderTradeSettlement)
    {
        ApplicableHeaderTradeSettlement = applicableHeaderTradeSettlement;
        _specifiedTradeSettlementHeaderMonetarySummation =
            new MinimumSpecifiedTradeSettlementHeaderMonetarySummation(applicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation!);
    }

    internal ApplicableHeaderTradeSettlement ApplicableHeaderTradeSettlement { get; }

    /// <inheritdoc cref="CII.ApplicableHeaderTradeSettlement.InvoiceCurrencyCode" />
    public string InvoiceCurrencyCode { get => ApplicableHeaderTradeSettlement.InvoiceCurrencyCode!; set => ApplicableHeaderTradeSettlement.InvoiceCurrencyCode = value; }

    /// <inheritdoc cref="CII.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation" />
    public MinimumSpecifiedTradeSettlementHeaderMonetarySummation SpecifiedTradeSettlementHeaderMonetarySummation {
        get => _specifiedTradeSettlementHeaderMonetarySummation;

        set {
            _specifiedTradeSettlementHeaderMonetarySummation = value;
            ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation = value.SpecifiedTradeSettlementHeaderMonetarySummation;
        }
    }

    /// <summary>
    ///     Return the <see cref="CII.ApplicableHeaderTradeSettlement" /> that this class is a view of.
    /// </summary>
    /// <remarks>
    ///     The <see cref="MinimumApplicableHeaderTradeSettlement" /> view is useful when manipulating the invoice because it has better nullability checks, but some methods still
    ///     use the <see cref="CII.ApplicableHeaderTradeSettlement" /> class. This method allows you to get the original <see cref="CII.ApplicableHeaderTradeSettlement" /> back.
    /// </remarks>
    /// <returns>The <see cref="CII.ApplicableHeaderTradeSettlement" /> that this class is a view of.</returns>
    public ApplicableHeaderTradeSettlement ToApplicableHeaderTradeSettlement() => ApplicableHeaderTradeSettlement;

    /// <summary>
    ///     Implicitly convert a <see cref="MinimumApplicableHeaderTradeSettlement" /> to a <see cref="CII.ApplicableHeaderTradeSettlement" />.
    /// </summary>
    /// <param name="applicableHeaderTradeSettlement">The <see cref="MinimumApplicableHeaderTradeSettlement" /> to convert.</param>
    /// <returns>The <see cref="CII.ApplicableHeaderTradeSettlement" /> that this class is a view of.</returns>
    public static implicit operator ApplicableHeaderTradeSettlement(MinimumApplicableHeaderTradeSettlement applicableHeaderTradeSettlement) =>
        applicableHeaderTradeSettlement.ToApplicableHeaderTradeSettlement();
}
