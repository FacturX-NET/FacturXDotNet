namespace FacturXDotNet.Models.CII.Minimum;

/// <inheritdoc cref="CII.SellerTradePartyPostalTradeAddress" />
public class MinimumSellerTradePartyPostalTradeAddress
{
    internal MinimumSellerTradePartyPostalTradeAddress(SellerTradePartyPostalTradeAddress postalTradeAddress)
    {
        PostalTradeAddress = postalTradeAddress;
    }

    internal SellerTradePartyPostalTradeAddress PostalTradeAddress { get; }

    /// <inheritdoc cref="CII.SellerTradePartyPostalTradeAddress.CountryId" />
    public string CountryId { get => PostalTradeAddress.CountryId!; set => PostalTradeAddress.CountryId = value; }

    /// <summary>
    ///     Return the <see cref="CII.SellerTradePartyPostalTradeAddress" /> that this class is a view of.
    /// </summary>
    /// <remarks>
    ///     The <see cref="MinimumSellerTradePartyPostalTradeAddress" /> view is useful when manipulating the invoice because it has better nullability checks, but some methods still
    ///     use the <see cref="CII.SellerTradePartyPostalTradeAddress" /> class. This method allows you to get the original <see cref="CII.SellerTradePartyPostalTradeAddress" /> back.
    /// </remarks>
    /// <returns>The <see cref="CII.SellerTradePartyPostalTradeAddress" /> that this class is a view of.</returns>
    public SellerTradePartyPostalTradeAddress ToSellerTradePartyPostalTradeAddress() => PostalTradeAddress;

    /// <summary>
    ///     Implicitly convert a <see cref="MinimumSellerTradePartyPostalTradeAddress" /> to a <see cref="CII.SellerTradePartyPostalTradeAddress" />.
    /// </summary>
    /// <param name="sellerTradePartyPostalTradeAddress">The <see cref="MinimumSellerTradePartyPostalTradeAddress" /> to convert.</param>
    /// <returns>The <see cref="CII.SellerTradePartyPostalTradeAddress" /> that this class is a view of.</returns>
    public static implicit operator SellerTradePartyPostalTradeAddress(MinimumSellerTradePartyPostalTradeAddress sellerTradePartyPostalTradeAddress) =>
        sellerTradePartyPostalTradeAddress.ToSellerTradePartyPostalTradeAddress();
}
