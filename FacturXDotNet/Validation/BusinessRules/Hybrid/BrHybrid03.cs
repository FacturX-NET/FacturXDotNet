using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

record BrHybrid03() : HybridBusinessRule("BR-HYBRID-03", "A PDF/A extension schema (XMP) following the structure definition in the corresponding specification SHALL be used.")
{
    public override bool Check(FacturX invoice) => invoice.XmpMetadata.PdfAExtensions?.Schemas.SingleOrDefault(s => s.NamespaceUri == XmpFacturXMetadata.NamespaceUri) is not null;
}
