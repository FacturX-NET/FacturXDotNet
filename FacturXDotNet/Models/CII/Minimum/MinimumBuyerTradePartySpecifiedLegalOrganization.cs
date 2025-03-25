namespace FacturXDotNet.Models.CII.Minimum;

/// <inheritdoc cref="CII.BuyerTradePartySpecifiedLegalOrganization" />
public class MinimumBuyerTradePartySpecifiedLegalOrganization
{
    internal MinimumBuyerTradePartySpecifiedLegalOrganization(BuyerTradePartySpecifiedLegalOrganization specifiedLegalOrganization)
    {
        SpecifiedLegalOrganization = specifiedLegalOrganization;
    }

    internal BuyerTradePartySpecifiedLegalOrganization SpecifiedLegalOrganization { get; }

    /// <inheritdoc cref="CII.BuyerTradePartySpecifiedLegalOrganization.Id" />
    public string? Id { get => SpecifiedLegalOrganization.Id; set => SpecifiedLegalOrganization.Id = value; }

    /// <inheritdoc cref="CII.BuyerTradePartySpecifiedLegalOrganization.IdSchemeId" />
    public string? IdSchemeId { get => SpecifiedLegalOrganization.IdSchemeId; set => SpecifiedLegalOrganization.IdSchemeId = value; }

    /// <summary>
    ///     Return the <see cref="CII.BuyerTradePartySpecifiedLegalOrganization" /> that this class is a view of.
    /// </summary>
    /// <remarks>
    ///     The <see cref="MinimumBuyerTradePartySpecifiedLegalOrganization" /> view is useful when manipulating the invoice because it has better nullability checks, but some methods
    ///     still
    ///     use the <see cref="CII.BuyerTradePartySpecifiedLegalOrganization" /> class. This method allows you to get the original
    ///     <see cref="CII.BuyerTradePartySpecifiedLegalOrganization" /> back.
    /// </remarks>
    /// <returns>The <see cref="CII.BuyerTradePartySpecifiedLegalOrganization" /> that this class is a view of.</returns>
    public BuyerTradePartySpecifiedLegalOrganization ToBuyerTradePartySpecifiedLegalOrganization() => SpecifiedLegalOrganization;
}
