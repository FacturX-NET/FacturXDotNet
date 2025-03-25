namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

/// <summary>
///     BR-HYBRID-08: The fx:DocumentFileName in the XMP instance SHALL be a value defined in the HybridDocumentFilename code list.
/// </summary>
public record BrHybrid08() : HybridBusinessRule("BR-HYBRID-08", "The fx:DocumentFileName in the XMP instance SHALL be a value defined in the HybridDocumentFilename code list.")
{
    /// <inheritdoc />
    public override bool Check(XmpMetadata? xmp, string? ciiAttachmentName, CrossIndustryInvoice? cii) =>
        xmp?.FacturX is { DocumentFileName: "factur-x.xml" or "xrechnung.xml" or "order-x.xml" };
}
