using FacturXDotNet.Models.CII;
using FacturXDotNet.Parsing.CII.Exceptions;
using TurboXml;

namespace FacturXDotNet.Parsing.CII;

/// <summary>
///     Parse a <see cref="CrossIndustryInvoice" /> from an XML stream.
/// </summary>
public class CrossIndustryInvoiceReader(CrossIndustryInvoiceReaderOptions? options = null)
{
    readonly CrossIndustryInvoiceReaderOptions _options = options ?? new CrossIndustryInvoiceReaderOptions();

    /// <summary>
    ///     Parse the given stream into a <see cref="CrossIndustryInvoice" />.
    /// </summary>
    public CrossIndustryInvoice Read(Stream stream)
    {
        CrossIndustryInvoice result = InitializeResult();
        CrossIndustryInvoiceXmlReadHandler handler = new(result, _options.Logger);

        XmlParser.Parse(stream, ref handler);

        if (handler is { FoundXmlDeclaration: false, AtLeastOneTag: false })
        {
            throw CrossIndustryInvoiceReaderException.ValidationError("The provided stream does not contain a valid XML document.");
        }

        return result;
    }

    static CrossIndustryInvoice InitializeResult() =>
        new()
        {
            ExchangedDocumentContext = null,
            ExchangedDocument = null,
            SupplyChainTradeTransaction = null
        };
}
