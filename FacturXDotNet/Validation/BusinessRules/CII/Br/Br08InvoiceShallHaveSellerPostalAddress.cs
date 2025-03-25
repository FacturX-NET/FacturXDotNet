using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.BusinessRules.CII.Br;

record Br08InvoiceShallHaveSellerPostalAddress() : CrossIndustryInvoiceBusinessRule(
    "BR-08",
    "An Invoice shall contain the Seller postal address (BG-5).",
    FacturXProfile.Minimum.AndHigher()
)
{
    public override bool Check(CrossIndustryInvoice? cii) =>
        // Nullability analysis should guarantee that this is always true, however it is still a BT so we check it anyway
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        cii?.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress != null;
}
