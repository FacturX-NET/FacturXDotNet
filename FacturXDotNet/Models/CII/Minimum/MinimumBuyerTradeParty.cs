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
}
