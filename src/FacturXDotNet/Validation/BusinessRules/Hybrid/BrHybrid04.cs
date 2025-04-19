using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

/// <summary>
///     BR-HYBRID-4: The URI in the extension schema SHALL be urn:factur-x:pdfa:CrossIndustryDocument:1p0#.
/// </summary>
public record BrHybrid04() : HybridBusinessRule("BR-HYBRID-4", "The URI in the extension schema SHALL be urn:factur-x:pdfa:CrossIndustryDocument:1p0#.")
{
    /// <inheritdoc />
    public override bool Check(XmpMetadata? xmp, string? ciiAttachmentName, CrossIndustryInvoice? cii, IBusinessRuleDetailsLogger? logger = null) =>
        // a bit of a tautology
        xmp?.PdfAExtensions?.Schemas.SingleOrDefault(s => s.NamespaceUri == XmpFacturXMetadata.NamespaceUri) is { NamespaceUri: XmpFacturXMetadata.NamespaceUri };
}
