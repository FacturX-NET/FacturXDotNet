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
}
