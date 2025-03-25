using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

/// <summary>
///     BR-HYBRID-06: The fx:DocumentType in the XMP instance SHALL be a value from the HybridDocumentType code list.
/// </summary>
public record BrHybrid06() : HybridBusinessRule("BR-HYBRID-06", "The fx:DocumentType in the XMP instance SHALL be a value from the HybridDocumentType code list.")
{
    /// <inheritdoc />
    public override bool Check(XmpMetadata? xmp, string? ciiAttachmentName, CrossIndustryInvoice? cii) =>
        xmp?.FacturX?.DocumentType is not null && Enum.IsDefined(xmp.FacturX.DocumentType.Value);
}
