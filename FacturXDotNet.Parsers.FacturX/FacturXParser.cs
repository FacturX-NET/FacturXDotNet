using FacturXDotNet.Parsers.CII;

namespace FacturXDotNet.Parsers.FacturX;

public class FacturXParser
{
    readonly FacturXExtractor _extractor;
    readonly CrossIndustryInvoiceParser _parser;

    public FacturXParser(FacturXParserOptions? options = null)
    {
        _extractor = new FacturXExtractor(options?.Extraction ?? new FacturXExtractorOptions());
        _parser = new CrossIndustryInvoiceParser(options?.Cii ?? new CrossIndustryInvoiceParserOptions());
    }

    /// <summary>
    ///     Parse the Cross-Industry Invoice XML file in a Factur-X PDF file.
    /// </summary>
    /// <param name="stream">The stream containing the Factur-X PDF file.</param>
    /// <returns>The Factur-X Cross-Industry Invoice.</returns>
    public async Task<CrossIndustryInvoice> ParseCiiXmlInFacturXPdfAsync(Stream stream)
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
    public CrossIndustryInvoiceParserOptions Cii { get; } = new();
}
