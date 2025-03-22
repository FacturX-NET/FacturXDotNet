namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

record BrHybrid02() : HybridBusinessRule(
    "BR-HYBRID-02",
    "The PDF envelope of a hybrid document SHALL use the PDF/A-3 standard.Optionally, a PDF/A-4f file (ISO 19005-4, based on PDF 2.0 ISO 32000-2:2020) is allowed."
)
{
    public override bool Check(FacturXDocument invoice) => invoice.XmpMetadata.PdfAIdentification is { Part: >= 3 };
}
