using System.Text;
using System.Text.RegularExpressions;
using FacturXDotNet.Generation.CII;
using FacturXDotNet.Models.CII;
using FluentAssertions;

namespace Tests.FacturXDotNet.Generation;

[TestClass]
public class CrossIndustryInvoiceWriterTest
{
    [TestMethod]
    public async Task ShouldWriteCrossIndustryInvoiceXml_Minimum()
    {
        CrossIndustryInvoice cii = new()
        {
            ExchangedDocumentContext = new ExchangedDocumentContext
            {
                BusinessProcessSpecifiedDocumentContextParameterId = "BUSINESS_PROCESS",
                GuidelineSpecifiedDocumentContextParameterId = GuidelineSpecifiedDocumentContextParameterId.Minimum
            },
            ExchangedDocument = new ExchangedDocument
            {
                Id = "DOC_ID",
                TypeCode = InvoiceTypeCode.CommercialInvoice,
                IssueDateTime = new DateOnly(1, 2, 3),
                IssueDateTimeFormat = DateOnlyFormat.DateOnly
            },
            SupplyChainTradeTransaction = new SupplyChainTradeTransaction
            {
                ApplicableHeaderTradeAgreement = new ApplicableHeaderTradeAgreement
                {
                    BuyerReference = "BUYER_REF",
                    SellerTradeParty = new SellerTradeParty
                    {
                        Name = "SELLER_NAME",
                        SpecifiedLegalOrganization = new SellerTradePartySpecifiedLegalOrganization
                        {
                            Id = "SELLER_LEGAL_ID", IdSchemeId = "1234"
                        },
                        PostalTradeAddress = new SellerTradePartyPostalTradeAddress
                        {
                            CountryId = "SELLER_COUNTRY"
                        },
                        SpecifiedTaxRegistration = new SellerTradePartySpecifiedTaxRegistration
                        {
                            Id = "SELLER_TAX_ID", IdSchemeId = VatOnlyTaxSchemeIdentifier.Vat
                        }
                    },
                    BuyerTradeParty = new BuyerTradeParty
                    {
                        Name = "BUYER_NAME", SpecifiedLegalOrganization = new BuyerTradePartySpecifiedLegalOrganization
                        {
                            Id = "BUYER_LEGAL_ID", IdSchemeId = "4321"
                        }
                    },
                    BuyerOrderReferencedDocument = new BuyerOrderReferencedDocument
                    {
                        IssuerAssignedId = "ORDER_ID"
                    }
                },
                ApplicableHeaderTradeDelivery = new ApplicableHeaderTradeDelivery(),
                ApplicableHeaderTradeSettlement = new ApplicableHeaderTradeSettlement
                {
                    InvoiceCurrencyCode = "CURRENCY_CODE", SpecifiedTradeSettlementHeaderMonetarySummation = new SpecifiedTradeSettlementHeaderMonetarySummation
                    {
                        TaxBasisTotalAmount = 123,
                        TaxTotalAmount = 234,
                        TaxTotalAmountCurrencyId = "TAX_CURRENCY_ID",
                        GrandTotalAmount = 345,
                        DuePayableAmount = 456
                    }
                }
            }
        };

        CrossIndustryInvoiceWriter writer = new();

        await using MemoryStream resultStream = new();
        await writer.WriteAsync(resultStream, cii);
        resultStream.Seek(0, SeekOrigin.Begin);

        const string expectedFile = """
                                    <?xml version='1.0' encoding='UTF-8'?>
                                    <rsm:CrossIndustryInvoice 
                                    xmlns:qdt="urn:un:unece:uncefact:data:standard:QualifiedDataType:100"
                                    xmlns:ram="urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100"
                                    xmlns:rsm="urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100"
                                    xmlns:udt="urn:un:unece:uncefact:data:standard:UnqualifiedDataType:100"
                                    xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
                                         <rsm:ExchangedDocumentContext>
                                              <ram:BusinessProcessSpecifiedDocumentContextParameter>
                                                   <ram:ID>BUSINESS_PROCESS</ram:ID>
                                              </ram:BusinessProcessSpecifiedDocumentContextParameter>
                                              <ram:GuidelineSpecifiedDocumentContextParameter>
                                                   <ram:ID>urn:factur-x.eu:1p0:minimum</ram:ID>
                                              </ram:GuidelineSpecifiedDocumentContextParameter>
                                         </rsm:ExchangedDocumentContext>
                                         <rsm:ExchangedDocument>
                                              <ram:ID>DOC_ID</ram:ID>
                                              <ram:TypeCode>380</ram:TypeCode>
                                              <ram:IssueDateTime>
                                                   <udt:DateTimeString format="102">00010203</udt:DateTimeString>
                                              </ram:IssueDateTime>
                                         </rsm:ExchangedDocument>
                                         <rsm:SupplyChainTradeTransaction>
                                              <ram:ApplicableHeaderTradeAgreement>
                                                   <ram:BuyerReference>BUYER_REF</ram:BuyerReference>
                                                   <ram:SellerTradeParty>
                                                        <ram:Name>SELLER_NAME</ram:Name>
                                                        <ram:SpecifiedLegalOrganization>
                                                             <ram:ID schemeID="1234">SELLER_LEGAL_ID</ram:ID>
                                                        </ram:SpecifiedLegalOrganization>
                                                        <ram:PostalTradeAddress>
                                                             <ram:CountryID>SELLER_COUNTRY</ram:CountryID>
                                                        </ram:PostalTradeAddress>
                                                        <ram:SpecifiedTaxRegistration>
                                                             <ram:ID schemeID="VA">SELLER_TAX_ID</ram:ID>
                                                        </ram:SpecifiedTaxRegistration>
                                                   </ram:SellerTradeParty>
                                                   <ram:BuyerTradeParty>
                                                        <ram:Name>BUYER_NAME</ram:Name>
                                                        <ram:SpecifiedLegalOrganization>
                                                             <ram:ID schemeID="4321">BUYER_LEGAL_ID</ram:ID>
                                                        </ram:SpecifiedLegalOrganization>
                                                   </ram:BuyerTradeParty>
                                                   <ram:BuyerOrderReferencedDocument>
                                                        <ram:IssuerAssignedID>ORDER_ID</ram:IssuerAssignedID>
                                                   </ram:BuyerOrderReferencedDocument>
                                              </ram:ApplicableHeaderTradeAgreement>
                                    <ram:ApplicableHeaderTradeDelivery/>
                                              <ram:ApplicableHeaderTradeSettlement>
                                                   <ram:InvoiceCurrencyCode>CURRENCY_CODE</ram:InvoiceCurrencyCode>
                                                   <ram:SpecifiedTradeSettlementHeaderMonetarySummation>
                                                        <ram:TaxBasisTotalAmount>123</ram:TaxBasisTotalAmount>
                                                        <ram:TaxTotalAmount currencyID="TAX_CURRENCY_ID">234</ram:TaxTotalAmount>
                                                        <ram:GrandTotalAmount>345</ram:GrandTotalAmount>
                                                        <ram:DuePayableAmount>456</ram:DuePayableAmount>
                                                   </ram:SpecifiedTradeSettlementHeaderMonetarySummation>
                                              </ram:ApplicableHeaderTradeSettlement>
                                         </rsm:SupplyChainTradeTransaction>
                                    </rsm:CrossIndustryInvoice>
                                    """;
        await using MemoryStream expectedFileStream = new(Encoding.UTF8.GetBytes(expectedFile));

        CompareXmlFiles(resultStream, expectedFile);
    }

    static void CompareXmlFiles(Stream fileStream, string expectedFile)
    {
        using StreamReader fileStreamReader = new(fileStream);
        string file = fileStreamReader.ReadToEnd();

        string fileSanitized = Sanitize(file);
        string expectedFileSanitized = Sanitize(expectedFile);

        fileSanitized.Should().Be(expectedFileSanitized);
    }

    static string Sanitize(string file)
    {
        Regex regex = new("\\s");
        return regex.Replace(file, string.Empty).Replace("'", "\"").ToLowerInvariant();
    }
}
