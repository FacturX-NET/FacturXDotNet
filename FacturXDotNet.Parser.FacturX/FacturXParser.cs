using FacturXDotNet.Models;
using FacturXDotNet.Parser.CII;

namespace FacturXDotNet.Parser.FacturX;

public class FacturXParser
{
    readonly FacturXExtractor _extractor;
    readonly FacturXCrossIndustryInvoiceParser _parser;

    public FacturXParser(FacturXParserOptions? options = null)
    {
        _extractor = new FacturXExtractor(options?.Extraction ?? new FacturXExtractorOptions());
        _parser = new FacturXCrossIndustryInvoiceParser(options?.Cii ?? new FacturXCrossIndustryInvoiceParserOptions());
    }

    /// <summary>
    ///     Parse the Cross-Industry Invoice XML file in a Factur-X PDF file.
    /// </summary>
    /// <param name="stream">The stream containing the Factur-X PDF file.</param>
    /// <returns>The Factur-X Cross-Industry Invoice.</returns>
    public async Task<FacturXCrossIndustryInvoice> ParseCiiXmlInFacturXPdfAsync(Stream stream)
    {
        await using Stream ciiXmlStream = _extractor.ExtractFacturXAttachment(stream);
        return await _parser.ParseCiiXmlAsync(ciiXmlStream);
    }
}

public class FacturXParserOptions
{
    /// <summary>
    ///     The options for extracting the Cross-Industry Invoice XML from the PDF.
    /// </summary>
    public FacturXExtractorOptions Extraction { get; } = new();

    /// <summary>
    ///     The options for parsing the Cross-Industry Invoice.
    /// </summary>
    public FacturXCrossIndustryInvoiceParserOptions Cii { get; } = new();
}
