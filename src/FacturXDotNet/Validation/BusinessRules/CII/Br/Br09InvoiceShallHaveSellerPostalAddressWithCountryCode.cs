using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;
using FacturXDotNet.Validation.Utils;

namespace FacturXDotNet.Validation.BusinessRules.CII.Br;

/// <summary>
///     BR-9: The Seller postal address (BG-5) shall contain a Seller country code (BT-40).
/// </summary>
public record Br09InvoiceShallHaveSellerPostalAddressWithCountryCode() : CrossIndustryInvoiceBusinessRule(
    "BR-9",
    "The Seller postal address (BG-5) shall contain a Seller country code (BT-40).",
    FacturXProfile.Minimum.AndHigher(),
    ["BT-40"]
)
{
    /// <inheritdoc />
    public override bool Check(CrossIndustryInvoice? cii, IBusinessRuleDetailsLogger? logger = null) =>
        Iso31661CountryCodesUtils.IsValidCountryCode(cii?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress?.CountryId);
}
