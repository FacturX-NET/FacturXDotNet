using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;

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
    public override bool Check(XmpMetadata? xmp, string? ciiAttachmentName, CrossIndustryInvoice? cii, IBusinessRuleDetailsLogger? logger = null) =>
        // TODO: extract the method of embedding the XML into the PDF 
        true;
}
