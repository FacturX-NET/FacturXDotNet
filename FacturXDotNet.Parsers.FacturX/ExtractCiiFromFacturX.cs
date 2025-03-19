using System.Diagnostics.CodeAnalysis;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.Filters;
using PdfSharp.Pdf.IO;

namespace FacturXDotNet.Parsers.FacturX;

/// <summary>
///     Extract the Cross-Industry Invoice XML attachment from a Factur-X PDF document.
/// </summary>
class ExtractCiiFromFacturX(FacturXParserOptions? options = null)
{
    readonly FacturXParserOptions _options = options ?? new FacturXParserOptions();

    public Stream ExtractFacturXAttachment(Stream facturXStream) =>
        TryExtractFacturXAttachment(facturXStream, out Stream? result)
            ? result
            : throw new InvalidOperationException($"The Cross-Industry Invoice XML attachment with name '{_options.CiiXmlAttachmentName}' could not be found.");

    /// <summary>
    ///     Return the Cross-Industry Invoice XML attachment named <c>factur-x.xml</c> from the Factur-X PDF document.
    /// </summary>
    /// <param name="facturXStream">The Factur-X PDF document to parse.</param>
    /// <param name="facturXAttachment">The Cross-Industry Invoice document.</param>
    public bool TryExtractFacturXAttachment(Stream facturXStream, [NotNullWhen(true)] out Stream? facturXAttachment)
    {
        PdfDocument document;

        if (_options.Password != null)
        {
            document = PdfReader.Open(facturXStream, PdfDocumentOpenMode.Import, args => args.Password = _options.Password);
        }
        else
        {
            document = PdfReader.Open(facturXStream, PdfDocumentOpenMode.Import);
        }

        using PdfDocument _ = document;

        PdfCatalog catalog = document.Internals.Catalog;
        PdfArray? attachedFiles = catalog.Elements.GetArray("/AF");

        if (attachedFiles == null)
        {
            facturXAttachment = null;
            return false;
        }

        foreach (PdfItem? attachedFile in attachedFiles.Elements)
        {
            if (attachedFile is not PdfReference { Value: PdfDictionary fileSpec })
            {
                continue;
            }

            string attachedFileName = fileSpec.Elements.GetString("/F");
            if (attachedFileName != _options.CiiXmlAttachmentName)
            {
                continue;
            }

            if (fileSpec.Elements.GetDictionary("/EF") is not { } embeddedFile)
            {
                facturXAttachment = null;
                return false;
            }

            if (embeddedFile.Elements.GetReference("/F") is not { } pdfStreamReference)
            {
                facturXAttachment = null;
                return false;
            }

            if (pdfStreamReference.Value is not PdfDictionary pdfStreamDictionary)
            {
                facturXAttachment = null;
                return false;
            }

            PdfDictionary.PdfStream pdfStream = pdfStreamDictionary.Stream;
            if (pdfStream.Length == 0)
            {
                facturXAttachment = null;
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
            return true;
        }

        facturXAttachment = null;
        return false;
    }
}
