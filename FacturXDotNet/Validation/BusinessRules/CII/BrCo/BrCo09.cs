using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;
using FacturXDotNet.Validation.Utils;

namespace FacturXDotNet.Validation.BusinessRules.CII.BrCo;

/// <summary>
///     BR-CO-09: The Seller VAT identifier (BT-31), the Seller tax representative VAT identifier (BT-63) and the Buyer VAT identifier (BT-48) shall have a prefix in accordance with
///     ISO code ISO 3166-1 alpha-2 by which the country of issue may be identified. Nevertheless, Greece may use the prefix ‘EL’.
/// </summary>
public record BrCo09() : CrossIndustryInvoiceBusinessRule(
    "BR-CO-09",
    """
    The Seller VAT identifier (BT-31), the Seller tax representative VAT identifier (BT-63) and the Buyer VAT identifier (BT-48) shall have a prefix in accordance 
    with ISO code ISO 3166-1 alpha-2 by which the country of issue may be identified. Nevertheless, Greece may use the prefix ‘EL’.
    """,
    FacturXProfile.Minimum.AndHigher()
)
{
    /// <inheritdoc />
    public override bool Check(CrossIndustryInvoice? cii) =>
        // TODO: also check BT-63 and BT-48
        cii?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.Id is not null
        && CheckPrefix(cii.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.Id.AsSpan(0, 2));

    static bool CheckPrefix(ReadOnlySpan<char> prefix) => Iso31661CountryCodesUtils.IsValidCountryCode(prefix) || prefix is "el" || prefix is "El" || prefix is "EL";
}
