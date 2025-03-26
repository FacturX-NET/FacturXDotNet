namespace FacturXDotNet.Models.CII.Minimum;

/// <inheritdoc cref="CII.SellerTradeParty" />
public class MinimumSellerTradeParty
{
    MinimumSellerTradePartySpecifiedLegalOrganization? _specifiedLegalOrganization;
    MinimumSellerTradePartyPostalTradeAddress _postalTradeAddress;
    MinimumSellerTradePartySpecifiedTaxRegistration? _specifiedTaxRegistration;

    internal MinimumSellerTradeParty(SellerTradeParty sellerTradeParty)
    {
        SellerTradeParty = sellerTradeParty;
        _specifiedLegalOrganization = sellerTradeParty.SpecifiedLegalOrganization == null
            ? null
            : new MinimumSellerTradePartySpecifiedLegalOrganization(sellerTradeParty.SpecifiedLegalOrganization);
        _postalTradeAddress = new MinimumSellerTradePartyPostalTradeAddress(sellerTradeParty.PostalTradeAddress!);
        _specifiedTaxRegistration = sellerTradeParty.SpecifiedTaxRegistration == null
            ? null
            : new MinimumSellerTradePartySpecifiedTaxRegistration(sellerTradeParty.SpecifiedTaxRegistration);
    }

    internal SellerTradeParty SellerTradeParty { get; }

    /// <inheritdoc cref="CII.SellerTradeParty.Name" />
    public string Name { get => SellerTradeParty.Name!; set => SellerTradeParty.Name = value; }

    /// <inheritdoc cref="CII.SellerTradeParty.SpecifiedLegalOrganization" />
    public MinimumSellerTradePartySpecifiedLegalOrganization? SpecifiedLegalOrganization {
        get => _specifiedLegalOrganization;

        set {
            _specifiedLegalOrganization = value;
            SellerTradeParty.SpecifiedLegalOrganization = value?.SpecifiedLegalOrganization;
        }
    }

    /// <inheritdoc cref="CII.SellerTradeParty.PostalTradeAddress" />
    public MinimumSellerTradePartyPostalTradeAddress PostalTradeAddress {
        get => _postalTradeAddress;

        set {
            _postalTradeAddress = value;
            SellerTradeParty.PostalTradeAddress = value.PostalTradeAddress;
        }
    }

    /// <inheritdoc cref="CII.SellerTradeParty.SpecifiedTaxRegistration" />
    public MinimumSellerTradePartySpecifiedTaxRegistration? SpecifiedTaxRegistration {
        get => _specifiedTaxRegistration;

        set {
            _specifiedTaxRegistration = value;
            SellerTradeParty.SpecifiedTaxRegistration = value?.SpecifiedTaxRegistration;
        }
    }

    /// <summary>
    ///     Return the <see cref="CII.SellerTradeParty" /> that this class is a view of.
    /// </summary>
    /// <remarks>
    ///     The <see cref="MinimumSellerTradeParty" /> view is useful when manipulating the invoice because it has better nullability checks, but some methods still
    ///     use the <see cref="CII.SellerTradeParty" /> class. This method allows you to get the original <see cref="CII.SellerTradeParty" /> back.
    /// </remarks>
    /// <returns>The <see cref="CII.SellerTradeParty" /> that this class is a view of.</returns>
    public SellerTradeParty ToSellerTradeParty() => SellerTradeParty;

    /// <summary>
    ///     Implicitly convert a <see cref="MinimumSellerTradeParty" /> to a <see cref="CII.SellerTradeParty" />.
    /// </summary>
    /// <param name="sellerTradeParty">The <see cref="MinimumSellerTradeParty" /> to convert.</param>
    /// <returns>The <see cref="CII.SellerTradeParty" /> that this class is a view of.</returns>
    public static implicit operator SellerTradeParty(MinimumSellerTradeParty sellerTradeParty) => sellerTradeParty.ToSellerTradeParty();
}
