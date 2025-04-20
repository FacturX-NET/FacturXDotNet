namespace FacturXDotNet.Models.CII.Minimum;

/// <inheritdoc cref="CII.SpecifiedTradeSettlementHeaderMonetarySummation" />
public class MinimumSpecifiedTradeSettlementHeaderMonetarySummation
{
    internal MinimumSpecifiedTradeSettlementHeaderMonetarySummation(SpecifiedTradeSettlementHeaderMonetarySummation specifiedTradeSettlementHeaderMonetarySummation)
    {
        SpecifiedTradeSettlementHeaderMonetarySummation = specifiedTradeSettlementHeaderMonetarySummation;

    }

    internal SpecifiedTradeSettlementHeaderMonetarySummation SpecifiedTradeSettlementHeaderMonetarySummation { get; }

    /// <inheritdoc cref="CII.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount" />
    public decimal TaxBasisTotalAmount {
        get => SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount!.Value;
        set => SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount = value;
    }

    /// <inheritdoc cref="CII.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount" />
    public decimal? TaxTotalAmount {
        get => SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount;
        set => SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount = value;
    }

    /// <inheritdoc cref="CII.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmountCurrencyId" />
    public string TaxTotalAmountCurrencyId {
        get => SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmountCurrencyId!;
        set => SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmountCurrencyId = value;
    }

    /// <inheritdoc cref="CII.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount" />
    public decimal GrandTotalAmount {
        get => SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount!.Value;
        set => SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount = value;
    }

    /// <inheritdoc cref="CII.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount" />
    public decimal DuePayableAmount {
        get => SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount!.Value;
        set => SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount = value;
    }

    /// <summary>
    ///     Return the <see cref="CII.SpecifiedTradeSettlementHeaderMonetarySummation" /> that this class is a view of.
    /// </summary>
    /// <remarks>
    ///     The <see cref="MinimumSpecifiedTradeSettlementHeaderMonetarySummation" /> view is useful when manipulating the invoice because it has better nullability checks, but some
    ///     methods still
    ///     use the <see cref="CII.SpecifiedTradeSettlementHeaderMonetarySummation" /> class. This method allows you to get the original
    ///     <see cref="CII.SpecifiedTradeSettlementHeaderMonetarySummation" /> back.
    /// </remarks>
    /// <returns>The <see cref="CII.SpecifiedTradeSettlementHeaderMonetarySummation" /> that this class is a view of.</returns>
    public SpecifiedTradeSettlementHeaderMonetarySummation ToSpecifiedTradeSettlementHeaderMonetarySummation() => SpecifiedTradeSettlementHeaderMonetarySummation;

    /// <summary>
    ///     Implicitly convert a <see cref="MinimumSpecifiedTradeSettlementHeaderMonetarySummation" /> to a <see cref="CII.SpecifiedTradeSettlementHeaderMonetarySummation" />.
    /// </summary>
    /// <param name="specifiedTradeSettlementHeaderMonetarySummation">The <see cref="MinimumSpecifiedTradeSettlementHeaderMonetarySummation" /> to convert.</param>
    /// <returns>The <see cref="CII.SpecifiedTradeSettlementHeaderMonetarySummation" /> that this class is a view of.</returns>
    public static implicit operator SpecifiedTradeSettlementHeaderMonetarySummation(
        MinimumSpecifiedTradeSettlementHeaderMonetarySummation specifiedTradeSettlementHeaderMonetarySummation
    ) =>
        specifiedTradeSettlementHeaderMonetarySummation.ToSpecifiedTradeSettlementHeaderMonetarySummation();
}
