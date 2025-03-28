using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

/// <summary>
///     BR-HYBRID-05: The schema namespace prefix in the XMP extension schema SHALL be fx.
/// </summary>
public record BrHybrid05() : HybridBusinessRule("BR-HYBRID-05", "The schema namespace prefix in the XMP extension schema SHALL be fx.")
{
    /// <inheritdoc />
    public override bool Check(XmpMetadata? xmp, string? ciiAttachmentName, CrossIndustryInvoice? cii, IBusinessRuleDetailsLogger? logger = null) =>
        xmp?.PdfAExtensions?.Schemas.SingleOrDefault(s => s.NamespaceUri == XmpFacturXMetadata.NamespaceUri) is { Prefix: "fx" };
}
