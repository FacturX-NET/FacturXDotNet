using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

/// <summary>
///     BR-HYBRID-3: A PDF/A extension schema (XMP) following the structure definition in the corresponding specification SHALL be used.
/// </summary>
public record BrHybrid03() : HybridBusinessRule(
    "BR-HYBRID-3",
    "A PDF/A extension schema (XMP) following the structure definition in the corresponding specification SHALL be used."
)
{
    /// <inheritdoc />
    public override bool Check(XmpMetadata? xmp, string? ciiAttachmentName, CrossIndustryInvoice? cii, IBusinessRuleDetailsLogger? logger = null) =>
        xmp?.PdfAExtensions?.Schemas.SingleOrDefault(s => s.NamespaceUri == XmpFacturXMetadata.NamespaceUri) is not null;
}
