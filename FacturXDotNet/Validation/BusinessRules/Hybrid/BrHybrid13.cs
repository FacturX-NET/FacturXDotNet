using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

/// <summary>
///     BR-HYBRID-13: The embedded file name SHALL be one of the values defined in the HybridDocumentFilename code list.
/// </summary>
public record BrHybrid13() : HybridBusinessRule("BR-HYBRID-13", "The embedded file name SHALL be one of the values defined in the HybridDocumentFilename code list.")
{
    /// <inheritdoc />
    public override bool Check(XmpMetadata? xmp, string? ciiAttachmentName, CrossIndustryInvoice? cii) => ciiAttachmentName is "factur-x.xml" or "xrechnung.xml" or "order-x.xml";
}
