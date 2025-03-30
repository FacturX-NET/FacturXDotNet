using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;

namespace FacturXDotNet.Validation.BusinessRules.CII.Br;

/// <summary>
///     BR-06: An Invoice shall contain the Seller name (BT-27).
/// </summary>
public record Br06InvoiceShallHaveSellerName() : CrossIndustryInvoiceBusinessRule("BR-06", "An Invoice shall contain the Seller name (BT-27).", FacturXProfile.Minimum.AndHigher())
{
    /// <inheritdoc />
    public override bool Check(CrossIndustryInvoice? cii, IBusinessRuleDetailsLogger? logger = null) =>
        !string.IsNullOrWhiteSpace(cii?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.Name);
}
