namespace FacturXDotNet.Models.CII.Minimum;

/// <inheritdoc cref="CII.ApplicableHeaderTradeAgreement" />
public class MinimumApplicableHeaderTradeAgreement
{
    MinimumSellerTradeParty _sellerTradeParty;
    MinimumBuyerTradeParty _buyerTradeParty;
    MinimumBuyerOrderReferencedDocument? _buyerOrderReferencedDocument;

    internal MinimumApplicableHeaderTradeAgreement(ApplicableHeaderTradeAgreement applicableHeaderTradeAgreement)
    {
        ApplicableHeaderTradeAgreement = applicableHeaderTradeAgreement;
        _sellerTradeParty = new MinimumSellerTradeParty(applicableHeaderTradeAgreement.SellerTradeParty!);
        _buyerTradeParty = new MinimumBuyerTradeParty(applicableHeaderTradeAgreement.BuyerTradeParty!);
        _buyerOrderReferencedDocument = applicableHeaderTradeAgreement.BuyerOrderReferencedDocument == null
            ? null
            : new MinimumBuyerOrderReferencedDocument(applicableHeaderTradeAgreement.BuyerOrderReferencedDocument);
    }

    internal ApplicableHeaderTradeAgreement ApplicableHeaderTradeAgreement { get; }

    /// <inheritdoc cref="CII.ApplicableHeaderTradeAgreement.BuyerReference" />
    public string? BuyerReference { get => ApplicableHeaderTradeAgreement.BuyerReference; set => ApplicableHeaderTradeAgreement.BuyerReference = value; }

    /// <inheritdoc cref="CII.ApplicableHeaderTradeAgreement.SellerTradeParty" />
    public MinimumSellerTradeParty SellerTradeParty {
        get => _sellerTradeParty;

        set {
            _sellerTradeParty = value;
            ApplicableHeaderTradeAgreement.SellerTradeParty = value.SellerTradeParty;
        }
    }

    /// <inheritdoc cref="CII.ApplicableHeaderTradeAgreement.BuyerTradeParty" />
    public MinimumBuyerTradeParty BuyerTradeParty {
        get => _buyerTradeParty;

        set {
            _buyerTradeParty = value;
            ApplicableHeaderTradeAgreement.BuyerTradeParty = value.BuyerTradeParty;
        }
    }

    /// <inheritdoc cref="CII.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument" />
    public MinimumBuyerOrderReferencedDocument? BuyerOrderReferencedDocument {
        get => _buyerOrderReferencedDocument;

        set {
            _buyerOrderReferencedDocument = value;
            ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument = value?.BuyerOrderReferencedDocument;
        }
    }

    /// <summary>
    ///     Return the <see cref="CII.ApplicableHeaderTradeAgreement" /> that this class is a view of.
    /// </summary>
    /// <remarks>
    ///     The <see cref="MinimumApplicableHeaderTradeAgreement" /> view is useful when manipulating the invoice because it has better nullability checks, but some methods still
    ///     use the <see cref="CII.ApplicableHeaderTradeAgreement" /> class. This method allows you to get the original <see cref="CII.ApplicableHeaderTradeAgreement" /> back.
    /// </remarks>
    /// <returns>The <see cref="CII.ApplicableHeaderTradeAgreement" /> that this class is a view of.</returns>
    public ApplicableHeaderTradeAgreement ToApplicableHeaderTradeAgreement() => ApplicableHeaderTradeAgreement;

    /// <summary>
    ///     Implicitly convert a <see cref="MinimumApplicableHeaderTradeAgreement" /> to a <see cref="CII.ApplicableHeaderTradeAgreement" />.
    /// </summary>
    /// <param name="applicableHeaderTradeAgreement">The <see cref="MinimumApplicableHeaderTradeAgreement" /> to convert.</param>
    /// <returns>The <see cref="CII.ApplicableHeaderTradeAgreement" /> that this class is a view of.</returns>
    public static implicit operator ApplicableHeaderTradeAgreement(MinimumApplicableHeaderTradeAgreement applicableHeaderTradeAgreement) =>
        applicableHeaderTradeAgreement.ToApplicableHeaderTradeAgreement();
}
