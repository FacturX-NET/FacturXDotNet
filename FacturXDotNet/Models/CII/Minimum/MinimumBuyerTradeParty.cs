namespace FacturXDotNet.Models.CII.Minimum;

/// <inheritdoc cref="CII.BuyerTradeParty" />
public class MinimumBuyerTradeParty
{
    MinimumBuyerTradePartySpecifiedLegalOrganization? _specifiedLegalOrganization;

    internal MinimumBuyerTradeParty(BuyerTradeParty buyerTradeParty)
    {
        BuyerTradeParty = buyerTradeParty;
        _specifiedLegalOrganization = buyerTradeParty.SpecifiedLegalOrganization == null
            ? null
            : new MinimumBuyerTradePartySpecifiedLegalOrganization(buyerTradeParty.SpecifiedLegalOrganization);
    }

    internal BuyerTradeParty BuyerTradeParty { get; }

    /// <inheritdoc cref="CII.BuyerTradeParty.Name" />
    public string Name { get => BuyerTradeParty.Name!; set => BuyerTradeParty.Name = value; }

    /// <inheritdoc cref="CII.BuyerTradeParty.SpecifiedLegalOrganization" />
    public MinimumBuyerTradePartySpecifiedLegalOrganization? SpecifiedLegalOrganization {
        get => _specifiedLegalOrganization;

        set {
            _specifiedLegalOrganization = value;
            BuyerTradeParty.SpecifiedLegalOrganization = value?.SpecifiedLegalOrganization;
        }
    }

    /// <summary>
    ///     Return the <see cref="CII.BuyerTradeParty" /> that this class is a view of.
    /// </summary>
    /// <remarks>
    ///     The <see cref="MinimumBuyerTradeParty" /> view is useful when manipulating the invoice because it has better nullability checks, but some methods still
    ///     use the <see cref="CII.BuyerTradeParty" /> class. This method allows you to get the original <see cref="CII.BuyerTradeParty" /> back.
    /// </remarks>
    /// <returns>The <see cref="CII.BuyerTradeParty" /> that this class is a view of.</returns>
    public BuyerTradeParty ToBuyerTradeParty() => BuyerTradeParty;
}
