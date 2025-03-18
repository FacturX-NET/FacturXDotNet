using FacturXDotNet.Models;
using FacturXDotNet.Parser.Exceptions;
using Microsoft.Extensions.Logging;

namespace FacturXDotNet.Parser;

public class FacturXCrossIndustryInvoiceParserOptions
{
    /// <summary>
    ///     The logger that should be used by the parser.
    ///     The parser logs the unknown paths it encounters at the WARN level.
    /// </summary>
    public ILogger? Logger { get; set; }
}

public class FacturXCrossIndustryInvoiceParser
{
    readonly FacturXCrossIndustryInvoiceParserOptions _options;
    readonly XmlParser<FacturXCrossIndustryInvoice> _parser;

    public FacturXCrossIndustryInvoiceParser(FacturXCrossIndustryInvoiceParserOptions? options = null)
    {
        _options = options ?? new FacturXCrossIndustryInvoiceParserOptions();
        _parser = InitializeParser();
    }

    public async Task<FacturXCrossIndustryInvoice> ParseAsync(Stream stream)
    {
        FacturXCrossIndustryInvoice result = InitializeResult();
        await _parser.ParseAsync(stream, result);

        List<string> errors = ValidateResult(result);
        if (errors.Count > 0)
        {
            throw new FacturXCrossIndustryInvoiceParserException(errors);
        }

        return result;
    }

    XmlParser<FacturXCrossIndustryInvoice> InitializeParser()
    {
        XmlParser<FacturXCrossIndustryInvoice> parser = new();
        parser.RegisterFallbackHandler((_, path, value) => _options.Logger?.LogWarning("Unknown element at '{Path}' with value '{Value}'.", path, value));
        return parser;
    }

    static FacturXCrossIndustryInvoice InitializeResult() =>
        new()
        {
            // We put null values for now, they will be filled by the parser.
            // If the non-null values are still null after the parser is done, we will throw: see Validate
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            ExchangedDocumentContext = null,
            ExchangedDocument = null,
            SupplyChainTradeTransaction = null
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        };

    /// <summary>
    ///     Check that all the required values have indeed been set.
    /// </summary>
    static List<string> ValidateResult(FacturXCrossIndustryInvoice result)
    {
        List<string> errors = new();

        // ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

        if (result.ExchangedDocumentContext == null)
        {
            errors.Add("required element /rsm:CrossIndustryInvoice/rsm:ExchangedDocumentContext is missing.");
        }

        if (result.ExchangedDocument == null)
        {
            errors.Add("required element /rsm:CrossIndustryInvoice/rsm:ExchangedDocument is missing.");
        }

        if (result.SupplyChainTradeTransaction == null)
        {
            errors.Add("required element /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction is missing.");
        }

        if (result.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement == null)
        {
            errors.Add("required element /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement is missing.");
        }

        if (result.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty == null)
        {
            errors.Add("required element /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerTradeParty is missing.");
        }

        if (result.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty == null)
        {
            errors.Add("required element /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty is missing.");
        }

        if (result.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress == null)
        {
            errors.Add(
                "required element /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:PostalTradeAddress is missing."
            );
        }

        if (result.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery == null)
        {
            errors.Add("required element /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeDelivery is missing.");
        }

        // ReSharper restore ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

        return errors;
    }
}
