using System.Diagnostics.CodeAnalysis;
using System.Text;
using iText.Kernel.Pdf;

namespace FacturXDotNet.Parser;

/// <summary>
///     Extract the Cross-Industry Invoice XML attachment from a Factur-X PDF document.
/// </summary>
public class FacturXExtractor(FacturXExtractorOptions? options = null)
{
    readonly FacturXExtractorOptions _options = options ?? new FacturXExtractorOptions();

    /// <summary>
    ///     Return the Cross-Industry Invoice XML attachment named <c>factur-x.xml</c> from the Factur-X PDF document.
    /// </summary>
    /// <param name="facturXStream">The Factur-X PDF document to parse.</param>
    /// <param name="facturXAttachment">The Cross-Industry Invoice document.</param>
    public bool TryExtractFacturXAttachment(Stream facturXStream, [NotNullWhen(true)] out Stream? facturXAttachment)
    {
        // See https://kb.itextpdf.com/itext/embedded-files#Embeddedfiles-removeembeddedfile

        ReaderProperties properties = CreateITextPdfReaderProperties();
        using PdfReader reader = new(facturXStream, properties);
        using PdfDocument document = new(reader);

        PdfDictionary? catalog = document.GetCatalog()?.GetPdfObject();
        PdfDictionary? names = catalog?.GetAsDictionary(PdfName.Names);
        PdfDictionary? embeddedFiles = names?.GetAsDictionary(PdfName.EmbeddedFiles);
        PdfArray? fileNames = embeddedFiles?.GetAsArray(PdfName.Names);

        if (fileNames != null)
        {
            for (int i = 0; i < fileNames.Size() - 1; i += 2)
            {
                PdfString? name = fileNames.GetAsString(i);
                if (name.ToString() != "factur-x.xml")
                {
                    continue;
                }

                PdfDictionary? fileSpec = fileNames.GetAsDictionary(i + 1);
                PdfDictionary? stream = fileSpec?.GetAsDictionary(PdfName.EF);
                if (stream == null)
                {
                    facturXAttachment = null;
                    return false;
                }

                PdfStream? pdfStream = stream.GetAsStream(PdfName.F);
                byte[]? bytes = pdfStream?.GetBytes(true);

                if (bytes == null)
                {
                    facturXAttachment = null;
                    return false;
                }

                facturXAttachment = new MemoryStream(bytes);
                return true;
            }
        }

        facturXAttachment = null;
        return false;
    }

    ReaderProperties CreateITextPdfReaderProperties()
    {
        ReaderProperties result = new();

        if (_options.Password != null)
        {
            result.SetPassword(Encoding.UTF8.GetBytes(_options.Password));
        }

        return result;
    }

    static bool StartsWith(byte[] buffer, params ReadOnlySpan<char> match)
    {
        if (match.Length > buffer.Length)
        {
            return false;
        }

        for (int i = 0; i < match.Length; i++)
        {
            if (buffer[i] != match[i])
            {
                return false;
            }
        }

        return true;
    }
}
