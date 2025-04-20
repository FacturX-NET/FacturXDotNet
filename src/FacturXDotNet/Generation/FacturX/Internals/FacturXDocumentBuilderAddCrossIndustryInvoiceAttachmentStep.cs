using FacturXDotNet.Generation.CII.Internals.Providers;
using FacturXDotNet.Generation.PDF;
using Microsoft.Extensions.Logging;
using PdfSharp.Pdf;

namespace FacturXDotNet.Generation.FacturX.Internals;

static class FacturXDocumentBuilderAddCrossIndustryInvoiceAttachmentStep
{
    public static async Task AttachCiiStreamToPdf(PdfDocument pdfDocument, ICrossIndustryInvoiceDataProvider ciiProvider, string ciiAttachmentName, ILogger? logger)
    {
        Stream ciiStream = await ciiProvider.GetCrossIndustryInvoiceStreamAsync();
        PdfAttachmentData ciiAttachment = PdfAttachmentData.LoadFromStream(ciiAttachmentName, ciiStream);
        ciiAttachment.Description = "CII XML - FacturX";
        ciiAttachment.Relationship = AfRelationship.Alternative;
        ciiAttachment.MimeType = "text/xml";

        FacturXDocumentBuilderAddAttachmentsStep.AddAttachment(pdfDocument, ciiAttachment, FacturXDocumentBuilderAttachmentConflictResolution.Overwrite, logger);
        logger?.LogInformation("Added CII attachment to the PDF document.");
    }
}
