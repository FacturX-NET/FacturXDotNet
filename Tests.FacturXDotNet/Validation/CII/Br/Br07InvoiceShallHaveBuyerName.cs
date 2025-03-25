using FacturXDotNet;
using FacturXDotNet.Models;
using FacturXDotNet.Validation.BusinessRules;

namespace Tests.FacturXDotNet.Validation.CII.Br;

record Br07InvoiceShallHaveBuyerName() : CrossIndustryInvoiceBusinessRule("BR-07", "An Invoice shall contain the Buyer name (BT-44).", FacturXProfile.Minimum.AndHigher())
{
    public override bool Check(CrossIndustryInvoice? cii) => !string.IsNullOrWhiteSpace(cii?.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name);
}
