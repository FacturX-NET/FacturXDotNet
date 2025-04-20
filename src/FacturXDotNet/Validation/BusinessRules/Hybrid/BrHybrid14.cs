using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

/// <summary>
///     BR-HYBRID-14: The embedded file name SHOULD match the fx:DocumentFileName.
/// </summary>
public record BrHybrid14() : HybridBusinessRule("BR-HYBRID-14", "The embedded file name SHOULD match the fx:DocumentFileName.", BusinessRuleSeverity.Warning)
{
    /// <inheritdoc />
    public override bool Check(XmpMetadata? xmp, string? ciiAttachmentName, CrossIndustryInvoice? cii, IBusinessRuleDetailsLogger? logger = null) =>
        xmp?.FacturX?.DocumentFileName is not null && ciiAttachmentName == xmp.FacturX.DocumentFileName;
}
