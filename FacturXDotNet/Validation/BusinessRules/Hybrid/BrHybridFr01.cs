namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

/// <summary>
///     BR-HYBRID-FR-01: If the Buyer Country BT-40 is France and the Seller Country BT-55 is France the XRECHNUNG profile SHALL NOT be used.
/// </summary>
public record BrHybridFr01() : HybridBusinessRule(
    "BR-HYBRID-FR-01",
    "If the Buyer Country BT-40 is France and the Seller Country BT-55 is France the XRECHNUNG profile SHALL NOT be used."
)
{
    /// <inheritdoc />
    public override bool Check(XmpMetadata? xmp, string? ciiAttachmentName, CrossIndustryInvoice? cii) =>
        // TODO
        true;
}
