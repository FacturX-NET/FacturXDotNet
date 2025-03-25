namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

/// <summary>
///     BR-HYBRID-11: The relationship between the embedded document and the PDF file SHOULD follow the table defined in the current specification.
/// </summary>
public record BrHybrid11() : HybridBusinessRule(
    "BR-HYBRID-11",
    "The relationship between the embedded document and the PDF file SHOULD follow the table defined in the current specification.",
    FacturXBusinessRuleSeverity.Warning
)
{
    /// <inheritdoc />
    public override bool Check(XmpMetadata? xmp, string? ciiAttachmentName, CrossIndustryInvoice? cii) =>
        // At this point this is necessarily true because we must have found it in order to create the FacturX instance. 
        true;
}
