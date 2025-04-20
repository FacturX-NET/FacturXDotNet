namespace FacturXDotNet.Models.CII.Minimum;

/// <inheritdoc cref="CII.BuyerOrderReferencedDocument" />
public class MinimumBuyerOrderReferencedDocument
{
    internal MinimumBuyerOrderReferencedDocument(BuyerOrderReferencedDocument buyerOrderReferencedDocument)
    {
        BuyerOrderReferencedDocument = buyerOrderReferencedDocument;
    }

    internal BuyerOrderReferencedDocument BuyerOrderReferencedDocument { get; }

    /// <inheritdoc cref="CII.BuyerOrderReferencedDocument.IssuerAssignedId" />
    public string? IssuerAssignedId { get => BuyerOrderReferencedDocument.IssuerAssignedId; set => BuyerOrderReferencedDocument.IssuerAssignedId = value; }

    /// <summary>
    ///     Return the <see cref="CII.BuyerOrderReferencedDocument" /> that this class is a view of.
    /// </summary>
    /// <remarks>
    ///     The <see cref="MinimumBuyerOrderReferencedDocument" /> view is useful when manipulating the invoice because it has better nullability checks, but some methods still
    ///     use the <see cref="CII.BuyerOrderReferencedDocument" /> class. This method allows you to get the original <see cref="CII.BuyerOrderReferencedDocument" /> back.
    /// </remarks>
    /// <returns>The <see cref="CII.BuyerOrderReferencedDocument" /> that this class is a view of.</returns>
    public BuyerOrderReferencedDocument ToBuyerOrderReferencedDocument() => BuyerOrderReferencedDocument;

    /// <summary>
    ///     Implicitly convert a <see cref="MinimumBuyerOrderReferencedDocument" /> to a <see cref="CII.BuyerOrderReferencedDocument" />.
    /// </summary>
    /// <param name="buyerOrderReferencedDocument">The <see cref="MinimumBuyerOrderReferencedDocument" /> to convert.</param>
    /// <returns>The <see cref="CII.BuyerOrderReferencedDocument" /> that this class is a view of.</returns>
    public static implicit operator BuyerOrderReferencedDocument(MinimumBuyerOrderReferencedDocument buyerOrderReferencedDocument) =>
        buyerOrderReferencedDocument.ToBuyerOrderReferencedDocument();
}
