namespace FacturXDotNet.Models.CII.Minimum;

/// <inheritdoc cref="CII.SellerTradePartySpecifiedLegalOrganization" />
public class MinimumSellerTradePartySpecifiedLegalOrganization
{
    internal MinimumSellerTradePartySpecifiedLegalOrganization(SellerTradePartySpecifiedLegalOrganization specifiedLegalOrganization)
    {
        SpecifiedLegalOrganization = specifiedLegalOrganization;

    }

    internal SellerTradePartySpecifiedLegalOrganization SpecifiedLegalOrganization { get; }

    /// <inheritdoc cref="CII.SellerTradePartySpecifiedLegalOrganization.Id" />
    public string? Id { get => SpecifiedLegalOrganization.Id; set => SpecifiedLegalOrganization.Id = value; }

    /// <inheritdoc cref="CII.SellerTradePartySpecifiedLegalOrganization.IdSchemeId" />
    public string? IdSchemeId { get => SpecifiedLegalOrganization.IdSchemeId; set => SpecifiedLegalOrganization.IdSchemeId = value; }
}
