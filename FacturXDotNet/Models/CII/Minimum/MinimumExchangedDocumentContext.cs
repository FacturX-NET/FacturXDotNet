namespace FacturXDotNet.Models.CII.Minimum;

/// <inheritdoc cref="CII.ExchangedDocumentContext" />
public class MinimumExchangedDocumentContext
{
    internal MinimumExchangedDocumentContext(ExchangedDocumentContext exchangedDocumentContext)
    {
        ExchangedDocumentContext = exchangedDocumentContext;
    }

    internal ExchangedDocumentContext ExchangedDocumentContext { get; }

    /// <inheritdoc cref="CII.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameterId" />
    public string? BusinessProcessSpecifiedDocumentContextParameterId {
        get => ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameterId;
        set => ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameterId = value;
    }

    /// <inheritdoc cref="CII.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameterId" />
    public GuidelineSpecifiedDocumentContextParameterId GuidelineSpecifiedDocumentContextParameterId {
        get => ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameterId!.Value;
        set => ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameterId = value;
    }

    /// <summary>
    ///     Return the <see cref="CII.ExchangedDocumentContext" /> that this class is a view of.
    /// </summary>
    /// <remarks>
    ///     The <see cref="MinimumExchangedDocumentContext" /> view is useful when manipulating the invoice because it has better nullability checks, but some methods still
    ///     use the <see cref="CII.ExchangedDocumentContext" /> class. This method allows you to get the original <see cref="CII.ExchangedDocumentContext" /> back.
    /// </remarks>
    /// <returns>The <see cref="CII.ExchangedDocumentContext" /> that this class is a view of.</returns>
    public ExchangedDocumentContext ToExchangedDocumentContext() => ExchangedDocumentContext;

    /// <summary>
    ///     Implicitly convert a <see cref="MinimumExchangedDocumentContext" /> to a <see cref="CII.ExchangedDocumentContext" />.
    /// </summary>
    /// <param name="exchangedDocumentContext">The <see cref="MinimumExchangedDocumentContext" /> to convert.</param>
    /// <returns>The <see cref="CII.ExchangedDocumentContext" /> that this class is a view of.</returns>
    public static implicit operator ExchangedDocumentContext(MinimumExchangedDocumentContext exchangedDocumentContext) => exchangedDocumentContext.ToExchangedDocumentContext();
}
