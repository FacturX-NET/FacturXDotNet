using System.Text.RegularExpressions;
using FacturXDotNet.Parsers.CII;
using FacturXDotNet.Parsers.XMP;

namespace FacturXDotNet.Parsers.FacturX;

public partial class FacturXParser
{
    readonly ExtractXmpFromFacturX _xmpExtractor;
    readonly ExtractCiiFromFacturX _ciiExtractor;
    readonly XmpMetadataParser _xmpParser;
    readonly CrossIndustryInvoiceParser _ciiParser;

    public FacturXParser(FacturXParserOptions? options = null)
    {
        _xmpExtractor = new ExtractXmpFromFacturX(options);
        _ciiExtractor = new ExtractCiiFromFacturX(options);
        _xmpParser = new XmpMetadataParser(options?.Xmp ?? new XmpMetadataParserOptions());
        _ciiParser = new CrossIndustryInvoiceParser(options?.Cii ?? new CrossIndustryInvoiceParserOptions());
    }

    /// <summary>
    ///     Parse the Cross-Industry Invoice XML file in a Factur-X PDF file.
    /// </summary>
    /// <param name="stream">The stream containing the Factur-X PDF file.</param>
    /// <returns>The Factur-X Cross-Industry Invoice.</returns>
    public async Task<XmpMetadata> ParseXmpMetadataInFacturXPdfAsync(Stream stream)
    {
        await using Stream xmpXmlStream = _xmpExtractor.ExtractXmpMetadata(stream);

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

    /// <summary>
    ///     Parse the Cross-Industry Invoice XML file in a Factur-X PDF file.
    /// </summary>
    /// <param name="stream">The stream containing the Factur-X PDF file.</param>
    /// <returns>The Factur-X Cross-Industry Invoice.</returns>
    public async Task<CrossIndustryInvoice> ParseCiiXmlInFacturXPdfAsync(Stream stream)
    {
        await using Stream ciiXmlStream = _ciiExtractor.ExtractFacturXAttachment(stream);
        return _ciiParser.ParseCiiXml(ciiXmlStream);
    }

    [GeneratedRegex("<\\?xpacket.*?\\?>")]
    private static partial Regex PacketInstructions();
}
