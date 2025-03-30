namespace FacturXDotNet.Models.CII.Minimum;

/// <inheritdoc cref="CII.ExchangedDocument" />
public class MinimumExchangedDocument
{
    internal MinimumExchangedDocument(ExchangedDocument exchangedDocument)
    {
        ExchangedDocument = exchangedDocument;
    }

    internal ExchangedDocument ExchangedDocument { get; }

    /// <inheritdoc cref="CII.ExchangedDocument.Id" />
    public string Id { get => ExchangedDocument.Id!; set => ExchangedDocument.Id = value; }

    /// <inheritdoc cref="CII.ExchangedDocument.TypeCode" />
    public InvoiceTypeCode TypeCode { get => ExchangedDocument.TypeCode!.Value; set => ExchangedDocument.TypeCode = value; }

    /// <inheritdoc cref="CII.ExchangedDocument.IssueDateTime" />
    public DateOnly IssueDateTime { get => ExchangedDocument.IssueDateTime!.Value; set => ExchangedDocument.IssueDateTime = value; }

    /// <inheritdoc cref="CII.ExchangedDocument.IssueDateTimeFormat" />
    public DateOnlyFormat IssueDateTimeFormat { get => ExchangedDocument.IssueDateTimeFormat!.Value; set => ExchangedDocument.IssueDateTimeFormat = value; }

    /// <summary>
    ///     Return the <see cref="CII.ExchangedDocument" /> that this class is a view of.
    /// </summary>
    /// <remarks>
    ///     The <see cref="MinimumExchangedDocument" /> view is useful when manipulating the invoice because it has better nullability checks, but some methods still
    ///     use the <see cref="CII.ExchangedDocument" /> class. This method allows you to get the original <see cref="CII.ExchangedDocument" /> back.
    /// </remarks>
    /// <returns>The <see cref="CII.ExchangedDocument" /> that this class is a view of.</returns>
    public ExchangedDocument ToExchangedDocument() => ExchangedDocument;

    /// <summary>
    ///     Implicitly convert a <see cref="MinimumExchangedDocument" /> to a <see cref="CII.ExchangedDocument" />.
    /// </summary>
    /// <param name="exchangedDocument">The <see cref="MinimumExchangedDocument" /> to convert.</param>
    /// <returns>The <see cref="CII.ExchangedDocument" /> that this class is a view of.</returns>
    public static implicit operator ExchangedDocument(MinimumExchangedDocument exchangedDocument) => exchangedDocument.ToExchangedDocument();
}
