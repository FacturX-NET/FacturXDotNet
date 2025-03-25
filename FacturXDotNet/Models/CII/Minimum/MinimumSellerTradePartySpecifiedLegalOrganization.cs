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

    /// <summary>
    ///     Return the <see cref="CII.SellerTradePartySpecifiedLegalOrganization" /> that this class is a view of.
    /// </summary>
    /// <remarks>
    ///     The <see cref="MinimumSellerTradePartySpecifiedLegalOrganization" /> view is useful when manipulating the invoice because it has better nullability checks, but some methods
    ///     still
    ///     use the <see cref="CII.SellerTradePartySpecifiedLegalOrganization" /> class. This method allows you to get the original
    ///     <see cref="CII.SellerTradePartySpecifiedLegalOrganization" /> back.
    /// </remarks>
    /// <returns>The <see cref="CII.SellerTradePartySpecifiedLegalOrganization" /> that this class is a view of.</returns>
    public SellerTradePartySpecifiedLegalOrganization ToSellerTradePartySpecifiedLegalOrganization() => SpecifiedLegalOrganization;
}
