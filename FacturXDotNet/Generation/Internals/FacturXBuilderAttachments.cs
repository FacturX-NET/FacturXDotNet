using FacturXDotNet.Extensions;
using FacturXDotNet.Generation.PDF;
using Microsoft.Extensions.Logging;
using PdfSharp.Pdf;

namespace FacturXDotNet.Generation.Internals;

static class FacturXBuilderAttachments
{
    public static void AddAttachments(PdfDocument pdfDocument, FacturXDocumentBuildArgs args)
    {
        foreach ((PdfAttachmentData attachment, FacturXDocumentBuilderAttachmentConflictResolution conflictResolution) in args.Attachments)
        {
            AddAttachment(pdfDocument, attachment, conflictResolution, args);
            args.Logger?.LogInformation("Added attachment {AttachmentName} to the PDF document.", attachment.Name);
        }
    }

    public static void AddAttachment(
        PdfDocument document,
        PdfAttachmentData attachment,
        FacturXDocumentBuilderAttachmentConflictResolution conflictResolution,
        FacturXDocumentBuildArgs args
    )
    {
        if (document.ListAttachments().Contains(attachment.Name))
        {
            switch (conflictResolution)
            {
                case FacturXDocumentBuilderAttachmentConflictResolution.KeepOld:
                    // nothing to do, we keep the old attachment
                    args.Logger?.LogWarning("An attachment with the name {AttachmentName} already exists in the PDF document, will keep the old attachment.", attachment.Name);
                    return;
                case FacturXDocumentBuilderAttachmentConflictResolution.Overwrite:
                    // we remove the old attachment and add the new one
                    document.RemoveAttachment(attachment.Name);
                    args.Logger?.LogWarning("An attachment with the name {AttachmentName} already exists in the PDF document, will overwrite the old attachment.", attachment.Name);
                    break;
                case FacturXDocumentBuilderAttachmentConflictResolution.KeepBoth:
                    // we add the new attachment in addition to the old one
                    args.Logger?.LogWarning("An attachment with the name {AttachmentName} already exists in the PDF document, will keep both attachments.", attachment.Name);
                    break;
                case FacturXDocumentBuilderAttachmentConflictResolution.Throw:
                    throw new InvalidOperationException($"An attachment with the name {attachment.Name} already exists in the PDF document.");
                default:
                    throw new ArgumentOutOfRangeException(nameof(conflictResolution), conflictResolution, null);
            }
        }

        document.AddAttachment(attachment);
    }
}
