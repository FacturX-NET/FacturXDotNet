using System.Text;
using FacturXDotNet.Models.CII;
using FacturXDotNet.Parsing.CII;
using FacturXDotNet.Parsing.CII.Exceptions;
using FluentAssertions;

namespace Tests.FacturXDotNet.Parsing;

[TestClass]
public class CrossIndustryInvoiceReaderTest
{
    [TestMethod]
    public async Task ShouldReadCrossIndustryInvoiceXml_Minimum()
    {
        const string file = """
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
        await using MemoryStream fileStream = new(Encoding.UTF8.GetBytes(file));

        CrossIndustryInvoiceReader reader = new();
        CrossIndustryInvoice result = reader.Read(fileStream);

        result.Should()
            .BeEquivalentTo(
                new CrossIndustryInvoice
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
                }
            );
    }

    [TestMethod]
    public async Task ShouldFailToReadInvalidCrossIndustryInvoiceXml()
    {
        const string invalidContent = "not a CII XML";
        await using MemoryStream fileStream = new(Encoding.UTF8.GetBytes(invalidContent));

        CrossIndustryInvoiceReader reader = new();
        Action action = () => reader.Read(fileStream);

        action.Should().Throw<CrossIndustryInvoiceReaderException>();
    }
}
