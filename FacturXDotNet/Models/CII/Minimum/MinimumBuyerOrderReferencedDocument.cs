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
}
