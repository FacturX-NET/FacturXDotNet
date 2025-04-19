using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

/// <summary>
///     BR-HYBRID-2: The PDF envelope of a hybrid document SHALL use the PDF/A-3 standard.Optionally, a PDF/A-4f file (ISO 19005-4, based on PDF 2.0 ISO 32000-2:2020) is allowed.
/// </summary>
public record BrHybrid02() : HybridBusinessRule(
    "BR-HYBRID-2",
    "The PDF envelope of a hybrid document SHALL use the PDF/A-3 standard.Optionally, a PDF/A-4f file (ISO 19005-4, based on PDF 2.0 ISO 32000-2:2020) is allowed."
)
{
    /// <inheritdoc />
    public override bool Check(XmpMetadata? xmp, string? ciiAttachmentName, CrossIndustryInvoice? cii, IBusinessRuleDetailsLogger? logger = null)
    {
        if (xmp?.PdfAIdentification is not { Part: >= 3 })
        {
            return false;
        }

        logger?.LogWarning("The PDF/A rules were not actually checked, the only thing checked was the presence of a PDF/A identification schema with a part >= 3.");
        return true;

    }
}
