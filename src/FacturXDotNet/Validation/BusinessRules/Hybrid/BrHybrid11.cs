using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

/// <summary>
///     BR-HYBRID-11: The relationship between the embedded document and the PDF file SHOULD follow the table defined in the current specification.
/// </summary>
public record BrHybrid11() : HybridBusinessRule(
    "BR-HYBRID-11",
    "The relationship between the embedded document and the PDF file SHOULD follow the table defined in the current specification.",
    BusinessRuleSeverity.Warning
)
{
    /// <inheritdoc />
    public override bool Check(XmpMetadata? xmp, string? ciiAttachmentName, CrossIndustryInvoice? cii, IBusinessRuleDetailsLogger? logger = null) =>
        // TODO: extract the relationship between the embedded document and the PDF file
        true;
}
