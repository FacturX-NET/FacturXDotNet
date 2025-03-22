using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace FacturXDotNet.Parsing;

/// <summary>
///     Extracts XMP metadata from a Factur-X PDF file.
/// </summary>
public class FacturXXmpExtractor(FacturXXmpExtractorOptions? options = null)
{
    readonly FacturXXmpExtractorOptions _options = options ?? new FacturXXmpExtractorOptions();
    readonly ExtractXmpFromFacturX _extractor = new();

    /// <summary>
    ///     Extracts the XMP metadata from a Factur-X PDF file.
    /// </summary>
    public Stream ExtractXmpAsync(Stream facturXStream)
    {
        using PdfDocument document = OpenPdfDocument(facturXStream);
        return _extractor.ExtractXmpMetadata(document);
    }

    PdfDocument OpenPdfDocument(Stream stream)
    {
        PdfDocument document;

        if (_options.Password != null)
        {
            document = PdfReader.Open(stream, PdfDocumentOpenMode.Import, args => args.Password = _options.Password);
        }
        else
        {
            document = PdfReader.Open(stream, PdfDocumentOpenMode.Import);
        }

        return document;
    }
}

/// <summary>
///     Options for the <see cref="FacturXXmpExtractor" />.
/// </summary>
public class FacturXXmpExtractorOptions
{
    /// <summary>
    ///     The password to open the PDF file.
    /// </summary>
    public string? Password { get; set; }
}
