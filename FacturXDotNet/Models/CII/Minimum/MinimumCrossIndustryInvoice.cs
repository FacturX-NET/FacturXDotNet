namespace FacturXDotNet.Models.CII.Minimum;

/// <inheritdoc cref="CII.CrossIndustryInvoice" />
/// <remarks>
///     This class cannot be constructed directly. To create an instance of this class, use the <see cref="CrossIndustryInvoiceMinimumProfileExtensions.AsMinimumInvoice" /> extension.
/// </remarks>
public class MinimumCrossIndustryInvoice
{
    MinimumExchangedDocumentContext _exchangedDocumentContext;
    MinimumExchangedDocument _exchangedDocument;
    MinimumSupplyChainTradeTransaction _supplyChainTradeTransaction;

    internal MinimumCrossIndustryInvoice(CrossIndustryInvoice crossIndustryInvoice)
    {
        CrossIndustryInvoice = crossIndustryInvoice;
        _exchangedDocumentContext = new MinimumExchangedDocumentContext(crossIndustryInvoice.ExchangedDocumentContext!);
        _exchangedDocument = new MinimumExchangedDocument(crossIndustryInvoice.ExchangedDocument!);
        _supplyChainTradeTransaction = new MinimumSupplyChainTradeTransaction(crossIndustryInvoice.SupplyChainTradeTransaction!);
    }

    internal CrossIndustryInvoice CrossIndustryInvoice { get; }

    /// <inheritdoc cref="CII.CrossIndustryInvoice.ExchangedDocumentContext" />
    public MinimumExchangedDocumentContext ExchangedDocumentContext {
        get => _exchangedDocumentContext;

        set {
            _exchangedDocumentContext = value;
            CrossIndustryInvoice.ExchangedDocumentContext = value.ExchangedDocumentContext;
        }
    }

    /// <inheritdoc cref="CII.CrossIndustryInvoice.ExchangedDocument" />
    public MinimumExchangedDocument ExchangedDocument {
        get => _exchangedDocument;

        set {
            _exchangedDocument = value;
            CrossIndustryInvoice.ExchangedDocument = value.ExchangedDocument;
        }
    }

    /// <inheritdoc cref="CII.CrossIndustryInvoice.SupplyChainTradeTransaction" />
    public MinimumSupplyChainTradeTransaction SupplyChainTradeTransaction {
        get => _supplyChainTradeTransaction;

        set {
            _supplyChainTradeTransaction = value;
            CrossIndustryInvoice.SupplyChainTradeTransaction = value.SupplyChainTradeTransaction;
        }
    }

    /// <summary>
    ///     Return the <see cref="CII.CrossIndustryInvoice" /> that this class is a view of.
    /// </summary>
    /// <remarks>
    ///     The <see cref="MinimumCrossIndustryInvoice" /> view is useful when manipulating the invoice because it has better nullability checks, but some methods still
    ///     use the <see cref="CII.CrossIndustryInvoice" /> class. This method allows you to get the original <see cref="CII.CrossIndustryInvoice" /> back.
    /// </remarks>
    /// <returns>The <see cref="CII.CrossIndustryInvoice" /> that this class is a view of.</returns>
    public CrossIndustryInvoice ToCrossIndustryInvoice() => CrossIndustryInvoice;

    /// <summary>
    ///     Implicitly convert a <see cref="MinimumCrossIndustryInvoice" /> to a <see cref="CII.CrossIndustryInvoice" />.
    /// </summary>
    /// <param name="minimumCrossIndustryInvoice">The <see cref="MinimumCrossIndustryInvoice" /> to convert.</param>
    /// <returns>The <see cref="CII.CrossIndustryInvoice" /> that this class is a view of.</returns>
    public static implicit operator CrossIndustryInvoice(MinimumCrossIndustryInvoice minimumCrossIndustryInvoice) => minimumCrossIndustryInvoice.ToCrossIndustryInvoice();
}

/// <summary>
///     Extension methods for <see cref="CrossIndustryInvoice" />.
/// </summary>
public static class CrossIndustryInvoiceMinimumProfileExtensions
{
    /// <summary>
    ///     Get a view of the <see cref="CrossIndustryInvoice" /> as a <see cref="MinimumCrossIndustryInvoice" />.
    ///     The difference between the two is that the <see cref="MinimumCrossIndustryInvoice" /> has the correct nullability annotations according to the MINIMUM profile.
    /// </summary>
    /// <remarks>
    ///     This method does not perform any validation. It is the responsibility of the caller to ensure that the <see cref="CrossIndustryInvoice" /> is valid according to the MINIMUM
    ///     profile. Using this method on a <see cref="CrossIndustryInvoice" /> that is not valid according to the MINIMUM profile will result in null-ref exceptions at runtime.
    /// </remarks>
    /// <param name="cii">The <see cref="CrossIndustryInvoice" /> to convert.</param>
    /// <returns>A view of the <see cref="CrossIndustryInvoice" /> as a <see cref="MinimumCrossIndustryInvoice" />.</returns>
    public static MinimumCrossIndustryInvoice AsMinimumInvoice(this CrossIndustryInvoice cii) => new(cii);
}
