namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

record BrHybridFr01() : HybridBusinessRule(
    "BR-HYBRID-FR-01",
    "If the Buyer Country BT-40 is France and the Seller Country BT-55 is France the XRECHNUNG profile SHALL NOT be used."
)
{
    public override bool Check(FacturXDocument invoice) =>
        // TODO
        true;
}
