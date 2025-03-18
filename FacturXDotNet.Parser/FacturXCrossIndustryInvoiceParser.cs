using FacturXDotNet.Models;
using FacturXDotNet.Parser.Exceptions;

namespace FacturXDotNet.Parser;

public class FacturXCrossIndustryInvoiceParser(FacturXCrossIndustryInvoiceParserOptions options)
{
    public async Task ParseAsync(Stream stream)
    {
        FacturXCrossIndustryInvoice invoice = CreateDefaultInvoice();

        Validate(invoice);
    }

    static FacturXCrossIndustryInvoice CreateDefaultInvoice() =>
        // We put null values for now, they will be filled by the parser.
        // If the non-null values are still null after the parser is done, we will throw: see Validate
        new()
        {
            ExchangedDocumentContext = null,
            ExchangedDocument = null,
            SupplyChainTradeTransaction = null
        };

    /// <summary>
    ///     Check that all the required values have indeed been set.
    /// </summary>
    static void Validate(FacturXCrossIndustryInvoice invoice)
    {
        // ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

        if (invoice.ExchangedDocumentContext == null)
        {
            throw new FacturXCrossIndustryInvoiceParserException(
                """
                The provided file is not a valid Factur-X document: the required element at 
                /rsm:CrossIndustryInvoice/rsm:ExchangedDocumentContext is missing.
                """
            );
        }
        if (invoice.ExchangedDocument == null)
        {
            throw new FacturXCrossIndustryInvoiceParserException(
                """
                The provided file is not a valid Factur-X document: the required element at 
                /rsm:CrossIndustryInvoice/rsm:ExchangedDocument is missing.
                """
            );
        }
        if (invoice.SupplyChainTradeTransaction == null)
        {
            throw new FacturXCrossIndustryInvoiceParserException(
                """
                The provided file is not a valid Factur-X document: the required element at 
                /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction is missing.
                """
            );
        }
        if (invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement == null)
        {
            throw new FacturXCrossIndustryInvoiceParserException(
                """
                The provided file is not a valid Factur-X document: the required element at 
                /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement is missing.
                """
            );
        }
        if (invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty == null)
        {
            throw new FacturXCrossIndustryInvoiceParserException(
                """
                The provided file is not a valid Factur-X document: the required element at 
                /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerTradeParty is missing.
                """
            );
        }
        if (invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty == null)
        {
            throw new FacturXCrossIndustryInvoiceParserException(
                """
                The provided file is not a valid Factur-X document: the required element at 
                /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty is missing.
                """
            );
        }
        if (invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress == null)
        {
            throw new FacturXCrossIndustryInvoiceParserException(
                """
                The provided file is not a valid Factur-X document: the required element at 
                /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:PostalTradeAddress is missing.
                """
            );
        }
        if (invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeDelivery == null)
        {
            throw new FacturXCrossIndustryInvoiceParserException(
                """
                The provided file is not a valid Factur-X document: the required element at 
                /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeDelivery is missing.
                """
            );
        }

        // ReSharper restore ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
    }
}

public class FacturXCrossIndustryInvoiceParserOptions
{
}
