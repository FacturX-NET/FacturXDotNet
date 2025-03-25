namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

/// <summary>
///     BR-HYBRID-13: The embedded file name SHALL be one of the values defined in the HybridDocumentFilename code list.
/// </summary>
public record BrHybrid13() : HybridBusinessRule("BR-HYBRID-13", "The embedded file name SHALL be one of the values defined in the HybridDocumentFilename code list.")
{
    /// <inheritdoc />
    public override bool Check(XmpMetadata? xmp, string? ciiAttachmentName, CrossIndustryInvoice? cii) =>
        // At this point this is necessarily true because we must have found it in order to create the FacturX instance. 
        true;
}
