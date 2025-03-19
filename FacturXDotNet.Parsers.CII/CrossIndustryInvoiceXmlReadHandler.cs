using System.Globalization;
using FacturXDotNet.Parsers.CII.Exceptions;
using Microsoft.Extensions.Logging;
using TurboXml;

namespace FacturXDotNet.Parsers.CII;

/// <summary>
///     XML handler that parses the content of a Cross-Industry Invoice (CII) XML.
///     See https://github.com/xoofx/TurboXml?tab=readme-ov-file for more details.
/// </summary>
/// <param name="result">The <see cref="CrossIndustryInvoice" /> to fill.</param>
readonly struct CrossIndustryInvoiceXmlReadHandler(CrossIndustryInvoice result, ILogger? logger) : IXmlReadHandler
{
    readonly Stack<ReadOnlyMemory<char>> _pathStack = [];

    public void OnBeginTag(ReadOnlySpan<char> name, int line, int column)
    {
        ReadOnlyMemory<char> parentPath = _pathStack.TryPeek(out ReadOnlyMemory<char> p) ? p : ReadOnlyMemory<char>.Empty;
        string newPath = $"{parentPath}/{name}";
        _pathStack.Push(newPath.AsMemory());

        switch (newPath)
        {
            case "/rsm:CrossIndustryInvoice/rsm:ExchangedDocumentContext":
                result.ExchangedDocumentContext = new ExchangedDocumentContext
                {
                    GuidelineSpecifiedDocumentContextParameterId = (GuidelineSpecifiedDocumentContextParameterId)(-1)
                };
                break;

            case "/rsm:CrossIndustryInvoice/rsm:ExchangedDocument":
                result.ExchangedDocument = new ExchangedDocument
                    { Id = string.Empty, TypeCode = (InvoiceTypeCode)(-1), IssueDateTime = default, IssueDateTimeFormat = (DateOnlyFormat)(-1) };
                break;

            case "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction":
                result.SupplyChainTradeTransaction = new SupplyChainTradeTransaction
                {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                    ApplicableHeaderTradeAgreement = null,
                    ApplicableHeaderTradeDelivery = null,
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                };
                break;

            case "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement":
                result.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement = new ApplicableHeaderTradeAgreement
                {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                    SellerTradeParty = null,
                    BuyerTradeParty = null,
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                };
                break;

            case "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty":
                result.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty = new SellerTradeParty
                {

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                    Name = string.Empty,
                    PostalTradeAddress = null
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                };
                break;

            case "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:PostalTradeAddress":
                result.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress = new SellerTradePartyPostalTradeAddress
                {
                    CountryId = string.Empty
                };
                break;

            case "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:SpecifiedLegalOrganization":
                result.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization = new SellerTradePartySpecifiedLegalOrganization();
                break;

            case "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:SpecifiedTaxRegistration":
                result.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration = new SellerTradePartySpecifiedTaxRegistration();
                break;

            case "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerTradeParty":
                result.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty = new BuyerTradeParty
                {
                    Name = string.Empty
                };
                break;

            case "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerTradeParty/ram:SpecifiedLegalOrganization":
                result.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization = new BuyerTradePartySpecifiedLegalOrganization();
                break;

            case "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerOrderReferencedDocument":
                result.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument = new BuyerOrderReferencedDocument();
                break;

            case "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeDelivery":
                result.SupplyChainTradeTransaction.ApplicableHeaderTradeDelivery = new ApplicableHeaderTradeDelivery();
                break;

            case "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement":
                result.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement = new ApplicableHeaderTradeSettlement
                {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                    InvoiceCurrencyCode = string.Empty,
                    SpecifiedTradeSettlementHeaderMonetarySummation = null
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                };
                break;

            case "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementHeaderMonetarySummation":
                result.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement!.SpecifiedTradeSettlementHeaderMonetarySummation =
                    new SpecifiedTradeSettlementHeaderMonetarySummation
                    {
                        TaxBasisTotalAmount = 0,
                        TaxTotalAmountCurrencyId = string.Empty,
                        GrandTotalAmount = 0,
                        DuePayableAmount = 0
                    };
                break;
        }
    }

    public void OnEndTagEmpty() => _pathStack.Pop();

    public void OnEndTag(ReadOnlySpan<char> name, int line, int column) => _pathStack.Pop();

    public void OnAttribute(ReadOnlySpan<char> name, ReadOnlySpan<char> value, int nameLine, int nameColumn, int valueLine, int valueColumn)
    {
        if (!_pathStack.TryPeek(out ReadOnlyMemory<char> path))
        {
            return;
        }

        switch (path.Span)
        {
            // EXCHANGED DOCUMENT

            case "/rsm:CrossIndustryInvoice/rsm:ExchangedDocument/ram:IssueDateTime/udt:DateTimeString":
                if (name is "format")
                {
                    result.ExchangedDocument.IssueDateTimeFormat = ParseDateOnlyFormat(value);
                }
                break;

            // SUPPLY CHAIN TRADE TRANSACTION - APPLICABLE HEADER TRADE AGREEMENT - SELLER TRADE PARTY - SPECIFIED LEGAL ORGANIZATION

            case "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:SpecifiedLegalOrganization/ram:ID":
                if (name is "schemeID")
                {
                    result.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization!.IdSchemeId = value.ToString();
                }
                break;

            // SUPPLY CHAIN TRADE TRANSACTION - APPLICABLE HEADER TRADE AGREEMENT - SELLER TRADE PARTY - SPECIFIED TAX REGISTRATION

            case "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:SpecifiedTaxRegistration/ram:ID":
                if (name is "schemeID")
                {
                    result.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration!.IdSchemeId =
                        ParseVatOnlyTaxSchemeIdentifier(value.ToString());
                }
                break;

            // SUPPLY CHAIN TRADE TRANSACTION - APPLICABLE HEADER TRADE AGREEMENT - BUYER TRADE PARTY - SPECIFIED LEGAL ORGANIZATION

            case "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerTradeParty/ram:SpecifiedLegalOrganization/ram:ID":
                if (name is "schemeID")
                {
                    result.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization!.IdSchemeId = value.ToString();
                }
                break;

            // SUPPLY CHAIN TRADE TRANSACTION - APPLICABLE HEADER TRADE SETTLEMENT - SPECIFIED TRADE SETTLEMENT HEADER MONETARY SUMMATION

            case
                "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:TaxTotalAmount"
                :
                if (name is "currencyID")
                {
                    result.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement!.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmountCurrencyId = value.ToString();
                }
                break;

            default:
                logger?.LogWarning("Unknown element '{Path}@{Attribute}' with value '{Value}'.", path.Span.ToString(), name.ToString(), value.ToString());
                break;
        }
    }

    public void OnText(ReadOnlySpan<char> value, int line, int column)
    {
        if (!_pathStack.TryPeek(out ReadOnlyMemory<char> path) || value.IsWhiteSpace())
        {
            return;
        }

        switch (path.Span)
        {
            // EXCHANGED DOCUMENT CONTEXT

            case "/rsm:CrossIndustryInvoice/rsm:ExchangedDocumentContext/ram:BusinessProcessSpecifiedDocumentContextParameter/ram:ID":
                result.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameterId = value.ToString();
                break;

            case "/rsm:CrossIndustryInvoice/rsm:ExchangedDocumentContext/ram:GuidelineSpecifiedDocumentContextParameter/ram:ID":
                result.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameterId = ParseGuidelineSpecifiedDocumentContextParameterId(value);
                break;

            // EXCHANGED DOCUMENT

            case "/rsm:CrossIndustryInvoice/rsm:ExchangedDocument/ram:ID":
                result.ExchangedDocument.Id = value.ToString();
                break;

            case "/rsm:CrossIndustryInvoice/rsm:ExchangedDocument/ram:TypeCode":
                result.ExchangedDocument.TypeCode = ParseFacturXTypeCode(value);
                break;

            case "/rsm:CrossIndustryInvoice/rsm:ExchangedDocument/ram:IssueDateTime/udt:DateTimeString":
                result.ExchangedDocument.IssueDateTime = ParseDateOnly(value);
                break;

            // SUPPLY CHAIN TRADE TRANSACTION

            // SUPPLY CHAIN TRADE TRANSACTION - APPLICABLE HEADER TRADE AGREEMENT

            case "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerReference":
                result.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference = value.ToString();
                break;

            // SUPPLY CHAIN TRADE TRANSACTION - APPLICABLE HEADER TRADE AGREEMENT - SELLER TRADE PARTY

            case "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:Name":
                result.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.Name = value.ToString();
                break;

            // SUPPLY CHAIN TRADE TRANSACTION - APPLICABLE HEADER TRADE AGREEMENT - SELLER TRADE PARTY - POSTAL TRADE ADDRESS

            case "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:PostalTradeAddress/ram:CountryID":
                result.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryId = value.ToString();
                break;

            // SUPPLY CHAIN TRADE TRANSACTION - APPLICABLE HEADER TRADE AGREEMENT - SELLER TRADE PARTY - SPECIFIED LEGAL ORGANIZATION

            case "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:SpecifiedLegalOrganization/ram:ID":
                result.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization!.Id = value.ToString();
                break;

            // SUPPLY CHAIN TRADE TRANSACTION - APPLICABLE HEADER TRADE AGREEMENT - SELLER TRADE PARTY - SPECIFIED TAX REGISTRATION

            case "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:SpecifiedTaxRegistration/ram:ID":
                result.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration!.Id = value.ToString();
                break;

            // SUPPLY CHAIN TRADE TRANSACTION - APPLICABLE HEADER TRADE AGREEMENT - BUYER TRADE PARTY

            case "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerTradeParty/ram:Name":
                result.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name = value.ToString();
                break;

            // SUPPLY CHAIN TRADE TRANSACTION - APPLICABLE HEADER TRADE AGREEMENT - BUYER TRADE PARTY - SPECIFIED LEGAL ORGANIZATION

            case "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerTradeParty/ram:SpecifiedLegalOrganization/ram:ID":
                result.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization!.Id = value.ToString();
                break;

            // SUPPLY CHAIN TRADE TRANSACTION - APPLICABLE HEADER TRADE AGREEMENT - BUYER ORDER REFERENCED DOCUMENT

            case "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerOrderReferencedDocument/ram:IssuerAssignedID":
                result.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument!.IssuerAssignedId = value.ToString();
                break;

            // SUPPLY CHAIN TRADE TRANSACTION - APPLICABLE HEADER TRADE DELIVERY

            // SUPPLY CHAIN TRADE TRANSACTION - APPLICABLE HEADER TRADE SETTLEMENT

            case "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:InvoiceCurrencyCode":
                result.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement!.InvoiceCurrencyCode = value.ToString();
                break;

            // SUPPLY CHAIN TRADE TRANSACTION - APPLICABLE HEADER TRADE SETTLEMENT - SPECIFIED TRADE SETTLEMENT HEADER MONETARY SUMMATION

            case
                "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:TaxBasisTotalAmount"
                :
                result.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement!.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount = ParseDecimal(value);
                break;

            case
                "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:TaxTotalAmount"
                :
                result.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement!.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount = ParseDecimal(value);
                break;

            case
                "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:GrandTotalAmount"
                :
                result.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement!.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount = ParseDecimal(value);
                break;

            case
                "/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:DuePayableAmount"
                :
                result.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement!.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount = ParseDecimal(value);
                break;

            default:
                logger?.LogWarning("Unknown element '{Path}' with value '{Value}'.", path.Span.ToString(), value.ToString());
                break;
        }
    }

    public void OnError(string message, int line, int column)
    {
        string path = _pathStack.TryPeek(out ReadOnlyMemory<char> p) ? p.ToString() : string.Empty;
        throw new CrossIndustryInvoiceParsingException(path, message, line, column);
    }

    static GuidelineSpecifiedDocumentContextParameterId ParseGuidelineSpecifiedDocumentContextParameterId(ReadOnlySpan<char> value) =>
        value.ToString().ToGuidelineSpecifiedDocumentContextParameterId();

    static InvoiceTypeCode ParseFacturXTypeCode(ReadOnlySpan<char> value)
    {
        if (!int.TryParse(value, out int valueInt))
        {
            throw new FormatException($"Expected value to be an integer, but found {value}.");
        }

        return valueInt.ToSpecificationIdentifier();
    }

    static DateOnlyFormat ParseDateOnlyFormat(ReadOnlySpan<char> value)
    {
        if (!int.TryParse(value, out int valueInt))
        {
            throw new FormatException($"Expected value to be an integer, but found {value}.");
        }

        return valueInt.ToDateOnlyFormat();
    }

    static VatOnlyTaxSchemeIdentifier ParseVatOnlyTaxSchemeIdentifier(ReadOnlySpan<char> value) => value.ToString().ToVatOnlyTaxSchemeIdentifier();

    static DateOnly ParseDateOnly(ReadOnlySpan<char> value)
    {
        if (!DateOnly.TryParseExact(value, "yyyyMMdd", out DateOnly valueDate))
        {
            throw new FormatException($"Expected date to be in format yyyyMMdd, but found {value}.");
        }

        return valueDate;
    }

    static decimal ParseDecimal(ReadOnlySpan<char> value) =>
        decimal.TryParse(value, CultureInfo.InvariantCulture, out decimal d) ? d : throw new FormatException($"Expected value to be a decimal, but found {value}.");
}
