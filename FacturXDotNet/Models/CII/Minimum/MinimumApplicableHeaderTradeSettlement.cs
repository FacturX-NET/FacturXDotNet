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
}
