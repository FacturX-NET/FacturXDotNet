using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

/// <summary>
///     BR-HYBRID-02: The PDF envelope of a hybrid document SHALL use the PDF/A-3 standard.Optionally, a PDF/A-4f file (ISO 19005-4, based on PDF 2.0 ISO 32000-2:2020) is allowed.
/// </summary>
public record BrHybrid02() : HybridBusinessRule(
    "BR-HYBRID-02",
    "The PDF envelope of a hybrid document SHALL use the PDF/A-3 standard.Optionally, a PDF/A-4f file (ISO 19005-4, based on PDF 2.0 ISO 32000-2:2020) is allowed."
)
{
    /// <inheritdoc />
    public override bool Check(XmpMetadata? xmp, string? ciiAttachmentName, CrossIndustryInvoice? cii) => xmp?.PdfAIdentification is { Part: >= 3 };
}
