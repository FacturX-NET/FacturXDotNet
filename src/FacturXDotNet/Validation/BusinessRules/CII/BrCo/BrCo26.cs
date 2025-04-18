using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;

namespace FacturXDotNet.Validation.BusinessRules.CII.BrCo;

/// <summary>
///     BR-CO-26: In order for the buyer to automatically identify a supplier, the Seller identifier (BT-29), the Seller legal registration identifier (BT-30) and/or the Seller VAT
///     identifier (BT-31) shall be present.
/// </summary>
public record BrCo26() : CrossIndustryInvoiceBusinessRule(
    "BR-CO-26",
    """
    In order for the buyer to automatically identify a supplier, the Seller identifier (BT-29), the Seller legal registration identifier (BT-30) 
    and/or the Seller VAT identifier (BT-31) shall be present.
    """,
    FacturXProfile.Minimum.AndHigher(),
    ["BT-30", "BT-31"]
)
{
    /// <inheritdoc />
    public override bool Check(CrossIndustryInvoice? cii, IBusinessRuleDetailsLogger? logger = null) =>
        // TODO: check BT-29
        !string.IsNullOrWhiteSpace(cii?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization?.Id)
        || !string.IsNullOrWhiteSpace(cii?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.Id);
}
