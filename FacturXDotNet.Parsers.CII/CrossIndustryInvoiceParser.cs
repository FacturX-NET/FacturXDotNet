﻿using FacturXDotNet.Parsers.CII.Exceptions;
using TurboXml;

namespace FacturXDotNet.Parsers.CII;

/// <summary>
///     Parse a CrossIndustryInvoice from an XML stream.
/// </summary>
public class CrossIndustryInvoiceParser
{
    readonly CrossIndustryInvoiceParserOptions _options;

    public CrossIndustryInvoiceParser(CrossIndustryInvoiceParserOptions? options = null)
    {
        _options = options ?? new CrossIndustryInvoiceParserOptions();
    }

    /// <summary>
    ///     Parse the given stream into a <see cref="CrossIndustryInvoice" />.
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    /// <exception cref="CrossIndustryInvoiceInvalidResultException"></exception>
    public CrossIndustryInvoice ParseCiiXml(Stream stream)
    {
        CrossIndustryInvoice result = InitializeResult();
        CrossIndustryInvoiceXmlReadHandler handler = new(result, _options.Logger);

        XmlParser.Parse(stream, ref handler);

        List<string> errors = ValidateResult(result);
        if (errors.Count > 0)
        {
            throw new CrossIndustryInvoiceInvalidResultException(errors);
        }

        return result;
    }

    static CrossIndustryInvoice InitializeResult() =>
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
    static List<string> ValidateResult(CrossIndustryInvoice result)
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
