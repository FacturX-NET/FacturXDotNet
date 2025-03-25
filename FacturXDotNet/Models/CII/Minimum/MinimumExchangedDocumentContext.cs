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
}
