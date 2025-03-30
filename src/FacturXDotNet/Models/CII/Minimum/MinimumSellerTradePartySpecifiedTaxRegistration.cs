namespace FacturXDotNet.Models.CII.Minimum;

/// <inheritdoc cref="CII.SellerTradePartySpecifiedTaxRegistration" />
public class MinimumSellerTradePartySpecifiedTaxRegistration
{
    internal MinimumSellerTradePartySpecifiedTaxRegistration(SellerTradePartySpecifiedTaxRegistration specifiedTaxRegistration)
    {
        SpecifiedTaxRegistration = specifiedTaxRegistration;
    }

    internal SellerTradePartySpecifiedTaxRegistration SpecifiedTaxRegistration { get; }

    /// <inheritdoc cref="CII.SellerTradePartySpecifiedTaxRegistration.Id" />
    public string? Id { get => SpecifiedTaxRegistration.Id; set => SpecifiedTaxRegistration.Id = value; }

    /// <inheritdoc cref="CII.SellerTradePartySpecifiedTaxRegistration.IdSchemeId" />
    public VatOnlyTaxSchemeIdentifier IdSchemeId { get => SpecifiedTaxRegistration.IdSchemeId!.Value; set => SpecifiedTaxRegistration.IdSchemeId = value; }

    /// <summary>
    ///     Return the <see cref="CII.SellerTradePartySpecifiedTaxRegistration" /> that this class is a view of.
    /// </summary>
    /// <remarks>
    ///     The <see cref="MinimumSellerTradePartySpecifiedTaxRegistration" /> view is useful when manipulating the invoice because it has better nullability checks, but some methods
    ///     still
    ///     use the <see cref="CII.SellerTradePartySpecifiedTaxRegistration" /> class. This method allows you to get the original
    ///     <see cref="CII.SellerTradePartySpecifiedTaxRegistration" /> back.
    /// </remarks>
    /// <returns>The <see cref="CII.SellerTradePartySpecifiedTaxRegistration" /> that this class is a view of.</returns>
    public SellerTradePartySpecifiedTaxRegistration ToSellerTradePartySpecifiedTaxRegistration() => SpecifiedTaxRegistration;

    /// <summary>
    ///     Implicitly convert a <see cref="MinimumSellerTradePartySpecifiedTaxRegistration" /> to a <see cref="CII.SellerTradePartySpecifiedTaxRegistration" />.
    /// </summary>
    /// <param name="sellerTradePartySpecifiedTaxRegistration">The <see cref="MinimumSellerTradePartySpecifiedTaxRegistration" /> to convert.</param>
    /// <returns>The <see cref="CII.SellerTradePartySpecifiedTaxRegistration" /> that this class is a view of.</returns>
    public static implicit operator SellerTradePartySpecifiedTaxRegistration(MinimumSellerTradePartySpecifiedTaxRegistration sellerTradePartySpecifiedTaxRegistration) =>
        sellerTradePartySpecifiedTaxRegistration.ToSellerTradePartySpecifiedTaxRegistration();
}
