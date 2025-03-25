namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

/// <summary>
///     BR-HYBRID-12: The method of embedding the XML into the PDF SHALL conform as defined in the current specification in order to assure easy extraction of the machine readable XML
///     file.
/// </summary>
public record BrHybrid12() : HybridBusinessRule(
    "BR-HYBRID-12",
    "The method of embedding the XML into the PDF SHALL conform as defined in the current specification in order to assure easy extraction of the machine readable XML file."
)
{
    /// <inheritdoc />
    public override bool Check(XmpMetadata? xmp, string? ciiAttachmentName, CrossIndustryInvoice? cii) =>
        // At this point this is necessarily true because we must have found it in order to create the FacturX instance. 
        true;
}
