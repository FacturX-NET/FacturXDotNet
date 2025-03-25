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
}
