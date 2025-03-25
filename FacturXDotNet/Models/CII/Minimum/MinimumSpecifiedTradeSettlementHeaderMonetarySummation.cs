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
}
