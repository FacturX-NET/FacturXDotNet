﻿namespace FacturXDotNet.Models.CII.Minimum;

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
}
