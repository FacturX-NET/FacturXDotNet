using System.Text.RegularExpressions;
using FacturXDotNet.Parsing.CII;
using FacturXDotNet.Parsing.XMP;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace FacturXDotNet.Parsing;

/// <summary>
///     Parse Factur-X from a PDF stream.
/// </summary>
public partial class FacturXParser
{
    readonly FacturXParserOptions _options;
    readonly ExtractXmpFromFacturX _xmpExtractor;
    readonly ExtractCiiFromFacturX _ciiExtractor;
    readonly XmpMetadataParser _xmpParser;
    readonly CrossIndustryInvoiceParser _ciiParser;

    /// <summary>
    ///     Initializes a new instance of the <see cref="FacturXParser" /> class.
    /// </summary>
    public FacturXParser(FacturXParserOptions? options = null)
    {
        _options = options ?? new FacturXParserOptions();
        _xmpExtractor = new ExtractXmpFromFacturX();
        _ciiExtractor = new ExtractCiiFromFacturX(_options.CiiXmlAttachmentName);
        _xmpParser = new XmpMetadataParser(_options.Xmp);
        _ciiParser = new CrossIndustryInvoiceParser(_options.Cii);
    }

    /// <summary>
    ///     Parse the Factur-X PDF file.
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public async Task<FacturX> ParseFacturXPdfAsync(Stream stream)
    {
        using PdfDocument document = OpenPdfDocument(stream);
        XmpMetadata xmpMetadata = await ParseXmpMetadataAsync(document);
        await using Stream ciiXmlStream = _ciiExtractor.ExtractFacturXAttachment(document, out string attachmentFileName);
        CrossIndustryInvoice ciiXml = _ciiParser.ParseCiiXml(ciiXmlStream);

        return new FacturX
        {
            XmpMetadata = xmpMetadata,
            CrossIndustryInvoiceFileInformation = new CrossIndustryInvoiceFileInformation { Name = attachmentFileName },
            CrossIndustryInvoice = ciiXml
        };
    }

    async Task<XmpMetadata> ParseXmpMetadataAsync(PdfDocument document)
    {
        await using Stream xmpXmlStream = _xmpExtractor.ExtractXmpMetadata(document);

        // TODO: avoid these two extra copies, it is only required because TurboXML doesn't support the <?xpacket...?> processing instructions
        using StreamReader reader = new(xmpXmlStream);
        string content = await reader.ReadToEndAsync();
        string transformedContent = PacketInstructions().Replace(content, string.Empty);

        await using MemoryStream transformedStream = new(transformedContent.Length + 54);
        await using StreamWriter writer = new(transformedStream);
        await writer.WriteAsync("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>");
        await writer.WriteAsync(transformedContent);
        await writer.FlushAsync();
        transformedStream.Seek(0, SeekOrigin.Begin);

        return _xmpParser.ParseXmpMetadata(transformedStream);
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

    [GeneratedRegex("<\\?xpacket.*?\\?>")]
    private static partial Regex PacketInstructions();
}
