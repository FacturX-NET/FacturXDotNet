using FacturXDotNet.Validation.CII.Utils;

namespace FacturXDotNet.Validation.CII.BusinessRules;

record BrCo09() : FacturXBusinessRule(
    "BR-CO-09",
    """
    The Seller VAT identifier (BT-31), the Seller tax representative VAT identifier (BT-63) and the Buyer VAT identifier (BT-48) shall have a prefix in accordance 
    with ISO code ISO 3166-1 alpha-2 by which the country of issue may be identified. Nevertheless, Greece may use the prefix ‘EL’.
    """,
    FacturXProfileFlags.Minimum.AndHigher()
)
{
    public override bool Check(CrossIndustryInvoice invoice) =>
        // TODO: also check BT-63 and BT-48
        invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration is { Id: not null }
        && CheckPrefix(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.Id.AsSpan(0, 2));

    static bool CheckPrefix(ReadOnlySpan<char> prefix) => Iso31661CountryCodesUtils.IsValidCountryCode(prefix) || prefix is "el" || prefix is "El" || prefix is "EL";
}
