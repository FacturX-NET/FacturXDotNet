using FacturXDotNet.Generation.PDF;
using Microsoft.Extensions.Logging;
using PdfSharp.Pdf;

namespace FacturXDotNet.Generation.Internals;

static class FacturXBuilderCrossIndustryInvoice
{
    public static async Task AddCrossIndustryInvoiceAttachmentAsync(PdfDocument pdfDocument, FacturXDocumentBuildArgs args)
    {
        if (args.Cii is null)
        {
            return;
        }

        string ciiAttachmentName = args.CiiAttachmentName ?? "factur-x.xml";

        PdfAttachmentData ciiAttachment = PdfAttachmentData.LoadFromStream(ciiAttachmentName, args.Cii);
        ciiAttachment.Description = "CII XML - FacturX";
        ciiAttachment.Relationship = AfRelationship.Alternative;
        ciiAttachment.MimeType = "application/xml";

        if (!args.CiiLeaveOpen)
        {
            await args.Cii.DisposeAsync();
        }

        FacturXBuilderAttachments.AddAttachment(pdfDocument, ciiAttachment, FacturXDocumentBuilderAttachmentConflictResolution.Overwrite, args);
        args.Logger?.LogInformation("Added CII attachment to the PDF document.");
    }
}
