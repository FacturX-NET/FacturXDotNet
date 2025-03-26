using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;

namespace FacturXDotNet.Validation.BusinessRules.CII.Br;

/// <summary>
///     BR-07: An Invoice shall contain the Buyer name (BT-44).
/// </summary>
public record Br07InvoiceShallHaveBuyerName() : CrossIndustryInvoiceBusinessRule("BR-07", "An Invoice shall contain the Buyer name (BT-44).", FacturXProfile.Minimum.AndHigher())
{
    /// <inheritdoc />
    public override bool Check(CrossIndustryInvoice? cii) => !string.IsNullOrWhiteSpace(cii?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.Name);
}
