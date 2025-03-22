using System.Diagnostics.CodeAnalysis;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.Filters;

namespace FacturXDotNet.Parsing;

/// <summary>
///     Extract the Cross-Industry Invoice XML attachment from a Factur-X PDF document.
/// </summary>
class ExtractCiiFromFacturX
{
    readonly string? _ciiAttachmentName;

    /// <summary>
    ///     Extract the Cross-Industry Invoice XML attachment from a Factur-X PDF document.
    /// </summary>
    /// <param name="ciiAttachmentName">The name of the attachment containing the Cross-Industry Invoice XML file. If not specified, the default name 'factur-x.xml' will be used.</param>
    public ExtractCiiFromFacturX(string? ciiAttachmentName = null)
    {
        _ciiAttachmentName = ciiAttachmentName ?? "factur-x.xml";
    }

    /// <summary>
    ///     Extract the Cross-Industry Invoice XML attachment from a Factur-X PDF document.
    /// </summary>
    public Stream ExtractFacturXAttachment(PdfDocument document, out string attachmentFileName)
    {
        if (TryExtractFacturXAttachment(document, out Stream? result, out string? attachmentFileNameOrNull))
        {
            attachmentFileName = attachmentFileNameOrNull;
            return result;
        }
        throw new InvalidOperationException($"The Cross-Industry Invoice XML attachment with name '{_ciiAttachmentName}' could not be found.");
    }

    /// <summary>
    ///     Extract the Cross-Industry Invoice XML attachment from a Factur-X PDF document.
    /// </summary>
    /// <param name="document">The Factur-X PDF document to parse.</param>
    /// <param name="facturXAttachment">The Cross-Industry Invoice document.</param>
    /// <param name="attachmentFileName">The name of the attachment containing the Cross-Industry Invoice XML file.</param>
    public bool TryExtractFacturXAttachment(PdfDocument document, [NotNullWhen(true)] out Stream? facturXAttachment, [NotNullWhen(true)] out string? attachmentFileName)
    {
        PdfCatalog catalog = document.Internals.Catalog;
        PdfArray? attachedFiles = catalog.Elements.GetArray("/AF");

        if (attachedFiles == null)
        {
            facturXAttachment = null;
            attachmentFileName = null;
            return false;
        }

        foreach (PdfItem? attachedFile in attachedFiles.Elements)
        {
            if (attachedFile is not PdfReference { Value: PdfDictionary fileSpec })
            {
                continue;
            }

            string attachedFileName = fileSpec.Elements.GetString("/F");
            if (attachedFileName != _ciiAttachmentName)
            {
                continue;
            }

            if (fileSpec.Elements.GetDictionary("/EF") is not { } embeddedFile)
            {
                facturXAttachment = null;
                attachmentFileName = null;
                return false;
            }

            if (embeddedFile.Elements.GetReference("/F") is not { } pdfStreamReference)
            {
                facturXAttachment = null;
                attachmentFileName = null;
                return false;
            }

            if (pdfStreamReference.Value is not PdfDictionary pdfStreamDictionary)
            {
                facturXAttachment = null;
                attachmentFileName = null;
                return false;
            }

            PdfDictionary.PdfStream pdfStream = pdfStreamDictionary.Stream;
            if (pdfStream.Length == 0)
            {
                facturXAttachment = null;
                attachmentFileName = null;
                return false;
            }

            byte[] bytes;
            if (pdfStream.TryUnfilter())
            {
                bytes = pdfStream.Value;
            }
            else
            {
                FlateDecode flate = new();
                bytes = flate.Decode(pdfStream.Value, new PdfDictionary());
            }

            facturXAttachment = new MemoryStream(bytes);
            attachmentFileName = attachedFileName;
            return true;
        }

        facturXAttachment = null;
        attachmentFileName = null;
        return false;
    }
}
