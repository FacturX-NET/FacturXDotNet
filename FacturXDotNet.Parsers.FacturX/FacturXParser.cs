using System.Xml.Linq;
using FacturXDotNet.Parsers.CII;

namespace FacturXDotNet.Parsers.FacturX;

public class FacturXParser
{
    readonly ExtractXmpFromFacturX _xmpExtractor;
    readonly ExtractCiiFromFacturX _ciiExtractor;
    readonly CrossIndustryInvoiceParser _parser;

    public FacturXParser(FacturXParserOptions? options = null)
    {
        _xmpExtractor = new ExtractXmpFromFacturX(options);
        _ciiExtractor = new ExtractCiiFromFacturX(options);
        _parser = new CrossIndustryInvoiceParser(options?.Cii ?? new CrossIndustryInvoiceParserOptions());
    }

    /// <summary>
    ///     Parse the Cross-Industry Invoice XML file in a Factur-X PDF file.
    /// </summary>
    /// <param name="stream">The stream containing the Factur-X PDF file.</param>
    /// <returns>The Factur-X Cross-Industry Invoice.</returns>
    public async Task<XDocument> ParseXmpMetadataInFacturXPdfAsync(Stream stream)
    {
        await using Stream xmpXmlStream = _xmpExtractor.ExtractXmpAttachment(stream);
        return XDocument.Load(xmpXmlStream);
    }

    /// <summary>
    ///     Parse the Cross-Industry Invoice XML file in a Factur-X PDF file.
    /// </summary>
    /// <param name="stream">The stream containing the Factur-X PDF file.</param>
    /// <returns>The Factur-X Cross-Industry Invoice.</returns>
    public async Task<CrossIndustryInvoice> ParseCiiXmlInFacturXPdfAsync(Stream stream)
    {
        await using Stream ciiXmlStream = _ciiExtractor.ExtractFacturXAttachment(stream);
        return _parser.ParseCiiXml(ciiXmlStream);
    }
}
