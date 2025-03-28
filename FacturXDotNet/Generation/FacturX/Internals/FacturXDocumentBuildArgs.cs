using FacturXDotNet.Generation.FacturX.Internals.PostProcess;
using FacturXDotNet.Generation.PDF;
using Microsoft.Extensions.Logging;

namespace FacturXDotNet.Generation.FacturX.Internals;

class FacturXDocumentBuildArgs
{
    public Stream? BasePdf { get; set; }
    public string? BasePdfPassword { get; set; }
    public bool BasePdfLeaveOpen { get; set; }
    public Stream? Cii { get; set; }
    public string CiiAttachmentName { get; set; } = "factur-x.xml";
    public bool CiiLeaveOpen { get; set; }
    public Stream? Xmp { get; set; }
    public bool XmpLeaveOpen { get; set; }
    public bool DisableXmpMetadataAutoGeneration { get; set; }
    public FacturXBuilderPostProcess PostProcess { get; set; } = new();
    public List<(PdfAttachmentData Name, FacturXDocumentBuilderAttachmentConflictResolution ConflictResolution)> Attachments { get; set; } = [];
    public ILogger? Logger { get; set; }
}
