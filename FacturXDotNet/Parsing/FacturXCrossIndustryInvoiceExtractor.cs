using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace FacturXDotNet.Parsing;

/// <summary>
///     Extracts XMP metadata from a Factur-X PDF file.
/// </summary>
public class FacturXCrossIndustryInvoiceExtractor
{
    readonly FacturXCrossIndustryInvoiceExtractorOptions _options;
    readonly ExtractCiiFromFacturX _extractor;

    /// <summary>
    ///     Extracts XMP metadata from a Factur-X PDF file.
    /// </summary>
    public FacturXCrossIndustryInvoiceExtractor(FacturXCrossIndustryInvoiceExtractorOptions? options = null)
    {
        _options = options ?? new FacturXCrossIndustryInvoiceExtractorOptions();
        _extractor = new ExtractCiiFromFacturX(_options.CiiXmlAttachmentName);
    }

    /// <summary>
    ///     Extracts the XMP metadata from a Factur-X PDF file.
    /// </summary>
    public Stream ExtractCiiAsync(Stream facturXStream)
    {
        using PdfDocument document = OpenPdfDocument(facturXStream);
        return _extractor.ExtractFacturXAttachment(document, out _);
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
///     Options for the <see cref="FacturXCrossIndustryInvoiceExtractor" />.
/// </summary>
public class FacturXCrossIndustryInvoiceExtractorOptions
{
    /// <summary>
    ///     The password to open the PDF file.
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    ///     The name of the attachment containing the Cross-Industry Invoice XML file.
    /// </summary>
    public string? CiiXmlAttachmentName { get; set; } = "factur-x.xml";
}
