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
}
