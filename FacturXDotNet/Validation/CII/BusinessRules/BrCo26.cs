using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.CII.BusinessRules;

record BrCo26() : FacturXBusinessRule(
    "BR-CO-26",
    """
    In order for the buyer to automatically identify a supplier, the Seller identifier (BT-29), the Seller legal registration identifier (BT-30) 
    and/or the Seller VAT identifier (BT-31) shall be present.
    """,
    FacturXProfile.Minimum.AndHigher()
)
{
    public override bool Check(CrossIndustryInvoice invoice) =>
        // TODO: check BT-29
        !string.IsNullOrWhiteSpace(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization?.Id)
        || !string.IsNullOrWhiteSpace(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration?.Id);
}
