using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;

namespace FacturXDotNet.Validation.BusinessRules.CII.Br;

/// <summary>
///     BR-08: An Invoice shall contain the Seller postal address (BG-5).
/// </summary>
public record Br08InvoiceShallHaveSellerPostalAddress() : CrossIndustryInvoiceBusinessRule(
    "BR-08",
    "An Invoice shall contain the Seller postal address (BG-5).",
    FacturXProfile.Minimum.AndHigher()
)
{
    /// <inheritdoc />
    public override bool Check(CrossIndustryInvoice? cii, IBusinessRuleDetailsLogger? logger = null) =>
        // Nullability analysis should guarantee that this is always true, however it is still a BT so we check it anyway
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        cii?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress is not null;
}
