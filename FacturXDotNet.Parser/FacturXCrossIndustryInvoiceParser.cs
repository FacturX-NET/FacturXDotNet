using System.Globalization;
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
    readonly FacturXCrossIndustryInvoiceParserImpl<FacturXCrossIndustryInvoice> _parser;

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
            throw new FacturXCrossIndustryInvoiceInvalidResultException(errors);
        }

        return result;
    }

    FacturXCrossIndustryInvoiceParserImpl<FacturXCrossIndustryInvoice> InitializeParser()
    {
        FacturXCrossIndustryInvoiceParserImpl<FacturXCrossIndustryInvoice> parser = new();

        if (_options.Logger != null)
        {
            parser.RegisterFallbackValueHandler((_, path, value) => _options.Logger?.LogWarning("Unknown element '{Path}' with value '{Value}'.", path, value));
        }

        // EXCHANGED DOCUMENT CONTEXT

        parser.RegisterElementHandler(
            "/rsm:CrossIndustryInvoice/rsm:ExchangedDocumentContext",
            invoice => invoice.ExchangedDocumentContext = new FacturXExchangedDocumentContext
            {
                GuidelineSpecifiedDocumentContextParameterId = (FacturXGuidelineSpecifiedDocumentContextParameterId)(-1)
            }
        );

        parser.RegisterValueHandler(
            "/rsm:CrossIndustryInvoice/rsm:ExchangedDocumentContext/ram:BusinessProcessSpecifiedDocumentContextParameter/ram:ID",
            (invoice, value) => invoice.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameterId = value.ToString()
        );

        parser.RegisterValueHandler(
            "/rsm:CrossIndustryInvoice/rsm:ExchangedDocumentContext/ram:GuidelineSpecifiedDocumentContextParameter/ram:ID",
            (invoice, value) => invoice.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameterId = ParseGuidelineSpecifiedDocumentContextParameterId(value)
        );

        // EXCHANGED DOCUMENT

        parser.RegisterElementHandler(
            "/rsm:CrossIndustryInvoice/rsm:ExchangedDocument",
            invoice => invoice.ExchangedDocument = new FacturXExchangedDocument { Id = string.Empty, TypeCode = (FacturXTypeCode)(-1), IssueDateTime = default }
        );

        parser.RegisterValueHandler("/rsm:CrossIndustryInvoice/rsm:ExchangedDocument/ram:ID", (invoice, value) => invoice.ExchangedDocument.Id = value.ToString());

        parser.RegisterValueHandler(
            "/rsm:CrossIndustryInvoice/rsm:ExchangedDocument/ram:TypeCode",
            (invoice, value) => invoice.ExchangedDocument.TypeCode = ParseFacturXTypeCode(value)
        );

        parser.RegisterValueHandler(
            "/rsm:CrossIndustryInvoice/rsm:ExchangedDocument/ram:IssueDateTime/udt:DateTimeString",
            (invoice, value) => invoice.ExchangedDocument.IssueDateTime = ParseDateOnly(value)
        );

        // EXCHANGED DOCUMENT

        parser.RegisterElementHandler(
            "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction",
            invoice => invoice.SupplyChainTradeTransaction = new FacturXSupplyChainTradeTransaction
            {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                ApplicableHeaderTradeAgreement = null,
                ApplicableHeaderTradeDelivery = null,
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            }
        );

        // EXCHANGED DOCUMENT - APPLICABLE HEADER TRADE AGREEMENT

        parser.RegisterElementHandler(
            "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement",
            invoice => invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement = new FacturXApplicableHeaderTradeAgreement
            {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                SellerTradeParty = null,
                BuyerTradeParty = null,
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            }
        );

        parser.RegisterValueHandler(
            "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerReference",
            (invoice, value) => invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference = value.ToString()
        );

        // EXCHANGED DOCUMENT - APPLICABLE HEADER TRADE AGREEMENT - SELLER TRADE PARTY

        parser.RegisterElementHandler(
            "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty",
            invoice => invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty = new FacturXSellerTradeParty
            {

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                Name = string.Empty,
                PostalTradeAddress = null
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            }
        );

        parser.RegisterValueHandler(
            "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:Name",
            (invoice, value) => invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.Name = value.ToString()
        );

        // EXCHANGED DOCUMENT - APPLICABLE HEADER TRADE AGREEMENT - SELLER TRADE PARTY - POSTAL TRADE ADDRESS

        parser.RegisterElementHandler(
            "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:PostalTradeAddress",
            invoice => invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress = new FacturXSellerTradePartyPostalTradeAddress
            {
                CountryId = string.Empty
            }
        );

        parser.RegisterValueHandler(
            "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:PostalTradeAddress/ram:CountryID",
            (invoice, value) => invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryId = value.ToString()
        );

        // EXCHANGED DOCUMENT - APPLICABLE HEADER TRADE AGREEMENT - SELLER TRADE PARTY - SPECIFIED LEGAL ORGANIZATION

        parser.RegisterElementHandler(
            "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:SpecifiedLegalOrganization",
            invoice => invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization =
                new FacturXSellerTradePartySpecifiedLegalOrganization()
        );

        parser.RegisterValueHandler(
            "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:SpecifiedLegalOrganization/ram:ID",
            (invoice, value) => invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization!.Id = value.ToString()
        );

        parser.RegisterValueHandler(
            "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:SpecifiedLegalOrganization/ram:ID@schemeID",
            (invoice, value) => invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization!.IdSchemeId = value.ToString()
        );

        // EXCHANGED DOCUMENT - APPLICABLE HEADER TRADE AGREEMENT - SELLER TRADE PARTY - SPECIFIED TAX REGISTRATION

        parser.RegisterElementHandler(
            "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:SpecifiedTaxRegistration",
            invoice => invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration =
                new FacturXSellerTradePartySpecifiedTaxRegistration()
        );

        parser.RegisterValueHandler(
            "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:SpecifiedTaxRegistration/ram:ID",
            (invoice, value) => invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration!.Id = value.ToString()
        );

        // EXCHANGED DOCUMENT - APPLICABLE HEADER TRADE AGREEMENT - BUYER TRADE PARTY

        parser.RegisterElementHandler(
            "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerTradeParty",
            invoice => invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty = new FacturXBuyerTradeParty
            {
                Name = string.Empty
            }
        );

        parser.RegisterValueHandler(
            "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerTradeParty/ram:Name",
            (invoice, value) => invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name = value.ToString()
        );

        // EXCHANGED DOCUMENT - APPLICABLE HEADER TRADE AGREEMENT - BUYEr TRADE PARTY - SPECIFIED LEGAL ORGANIZATION

        parser.RegisterElementHandler(
            "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerTradeParty/ram:SpecifiedLegalOrganization",
            invoice => invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization =
                new FacturXBuyerTradePartySpecifiedLegalOrganization()
        );

        parser.RegisterValueHandler(
            "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerTradeParty/ram:SpecifiedLegalOrganization/ram:ID",
            (invoice, value) => invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization!.Id = value.ToString()
        );

        parser.RegisterValueHandler(
            "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerTradeParty/ram:SpecifiedLegalOrganization/ram:ID@schemeID",
            (invoice, value) => invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization!.IdSchemeId = value.ToString()
        );

        // EXCHANGED DOCUMENT - APPLICABLE HEADER TRADE AGREEMENT - BUYER ORDER REFERENCED DOCUMENT

        parser.RegisterElementHandler(
            "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerOrderReferencedDocument",
            invoice => invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument = new FacturXBuyerOrderReferencedDocument()
        );

        parser.RegisterValueHandler(
            "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerOrderReferencedDocument/ram:IssuerAssignedID",
            (invoice, value) => invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument!.IssuerAssignedId = value.ToString()
        );

        // EXCHANGED DOCUMENT - APPLICABLE HEADER TRADE DELIVERY

        parser.RegisterElementHandler(
            "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeDelivery",
            invoice => invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeDelivery = new FacturXApplicableHeaderTradeDelivery()
        );

        // EXCHANGED DOCUMENT - APPLICABLE HEADER TRADE SETTLEMENT

        parser.RegisterElementHandler(
            "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement",
            invoice => invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement = new FacturXApplicableHeaderTradeSettlement
            {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                InvoiceCurrencyCode = string.Empty,
                SpecifiedTradeSettlementHeaderMonetarySummation = null
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            }
        );

        parser.RegisterValueHandler(
            "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:InvoiceCurrencyCode",
            (invoice, value) => invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement!.InvoiceCurrencyCode = value.ToString()
        );

        // EXCHANGED DOCUMENT - APPLICABLE HEADER TRADE SETTLEMENT - SPECIFIED TRADE SETTLEMENT HEADER MONETARY SUMMATION

        parser.RegisterElementHandler(
            "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementHeaderMonetarySummation",
            invoice => invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement!.SpecifiedTradeSettlementHeaderMonetarySummation =
                new FacturXSpecifiedTradeSettlementHeaderMonetarySummation
                {
                    TaxBasisTotalAmount = 0,
                    TaxTotalAmountCurrencyId = string.Empty,
                    GrandTotalAmount = 0,
                    DuePayableAmount = 0
                }
        );

        parser.RegisterValueHandler(
            "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:TaxBasisTotalAmount",
            (invoice, value) => invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement!.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount =
                ParseDecimal(value)
        );

        parser.RegisterValueHandler(
            "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:TaxTotalAmount",
            (invoice, value) => invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement!.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount =
                ParseDecimal(value)
        );

        parser.RegisterValueHandler(
            "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:TaxTotalAmount@currencyID",
            (invoice, value) => invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement!.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmountCurrencyId =
                value.ToString()
        );

        parser.RegisterValueHandler(
            "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:GrandTotalAmount",
            (invoice, value) => invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement!.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount =
                ParseDecimal(value)
        );

        parser.RegisterValueHandler(
            "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:DuePayableAmount",
            (invoice, value) => invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement!.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount =
                ParseDecimal(value)
        );

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

    static FacturXGuidelineSpecifiedDocumentContextParameterId ParseGuidelineSpecifiedDocumentContextParameterId(ReadOnlyMemory<char> value) =>
        value.ToString().ToGuidelineSpecifiedDocumentContextParameterId();

    static FacturXTypeCode ParseFacturXTypeCode(ReadOnlyMemory<char> value)
    {
        if (!int.TryParse(value.Span, out int valueInt))
        {
            throw new FormatException($"Expected value to be an integer, but found {value}.");
        }

        return valueInt.ToSpecificationIdentifier();
    }

    static DateOnly ParseDateOnly(ReadOnlyMemory<char> value)
    {
        if (!DateOnly.TryParseExact(value.Span, "yyyyMMdd", out DateOnly valueDate))
        {
            throw new FormatException($"Expected date to be in format yyyyMMdd, but found {value}.");
        }

        return valueDate;
    }

    decimal ParseDecimal(ReadOnlyMemory<char> value) =>
        decimal.TryParse(value.Span, CultureInfo.InvariantCulture, out decimal d) ? d : throw new FormatException($"Expected value to be a decimal, but found {value}.");
}
