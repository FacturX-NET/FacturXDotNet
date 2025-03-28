using CommunityToolkit.HighPerformance;
using FacturXDotNet.Extensions;
using FacturXDotNet.Generation.PDF;
using FacturXDotNet.Models.CII;
using FacturXDotNet.Parsing.CII;
using Microsoft.Extensions.Logging;
using PdfSharp.Pdf;

namespace FacturXDotNet.Generation.FacturX.Internals;

static class FacturXDocumentBuilderAddCrossIndustryInvoiceStep
{
    public static async Task<CrossIndustryInvoice> RunAsync(PdfDocument pdfDocument, FacturXDocumentBuildArgs args)
    {
        CrossIndustryInvoiceReader ciiReader = new();

        if (args.Cii == null)
        {
            // assume that there is already a CII attachment in the base PDF, and keep it as-is
            await using Stream ciiStream = pdfDocument.ExtractAttachment(args.CiiAttachmentName);
            return ciiReader.Read(ciiStream);
        }

        PdfAttachmentData ciiAttachment = PdfAttachmentData.LoadFromStream(args.CiiAttachmentName, args.Cii);
        ciiAttachment.Description = "CII XML - FacturX";
        ciiAttachment.Relationship = AfRelationship.Alternative;
        ciiAttachment.MimeType = "text/xml";

        FacturXDocumentBuilderAddAttachmentsStep.AddAttachment(pdfDocument, ciiAttachment, FacturXDocumentBuilderAttachmentConflictResolution.Overwrite, args);
        args.Logger?.LogInformation("Added CII attachment to the PDF document.");

        if (!args.CiiLeaveOpen)
        {
            await args.Cii.DisposeAsync();
        }

        // create a new memory buffer in case the CII stream is not seekable
        await using Stream attachmentStream = ciiAttachment.Content.AsStream();
        CrossIndustryInvoice cii = ciiReader.Read(attachmentStream);

        return cii;
    }
}
