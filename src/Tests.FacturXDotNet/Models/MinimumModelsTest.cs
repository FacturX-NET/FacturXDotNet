using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.CII.Minimum;
using FluentAssertions;
using Tests.FacturXDotNet.TestTools;

namespace Tests.FacturXDotNet.Models;

[TestClass]
public class MinimumModelsTest
{
    [TestMethod]
    public void ValuesOfMinimumInvoice_ShouldBeSameAsValuesOfSourceInvoice()
    {
        CrossIndustryInvoice invoice = FakeData.CrossIndustryInvoice;
        MinimumCrossIndustryInvoice minimumInvoice = invoice.AsMinimumInvoice();

        minimumInvoice.Should().BeEquivalentTo(invoice);
    }

    [TestMethod]
    public void ValuesSetInSourceInvoice_ShouldBeSetInMinimumInvoice()
    {
        CrossIndustryInvoice invoice = FakeData.CrossIndustryInvoice;
        MinimumCrossIndustryInvoice minimumInvoice = invoice.AsMinimumInvoice();

        //@formatter:off
        invoice.ExchangedDocumentContext!.BusinessProcessSpecifiedDocumentContextParameterId = "BusinessProcessSpecifiedDocumentContextParameterId";
        invoice.ExchangedDocumentContext!.GuidelineSpecifiedDocumentContextParameterId = GuidelineSpecifiedDocumentContextParameterId.Extended;
        invoice.ExchangedDocument!.Id = "ExhangedDocumentId";
        invoice.ExchangedDocument!.TypeCode = InvoiceTypeCode.CustomsInvoice;
        invoice.ExchangedDocument!.IssueDateTime = new DateOnly(1, 2,3);
        invoice.ExchangedDocument!.IssueDateTimeFormat = DateOnlyFormat.DateOnly;
        invoice.SupplyChainTradeTransaction!.ApplicableHeaderTradeAgreement!.BuyerReference = "BUYER_REFERENCE";
        invoice.SupplyChainTradeTransaction!.ApplicableHeaderTradeAgreement!.SellerTradeParty!.Name = "SELLER_NAME";
        invoice.SupplyChainTradeTransaction!.ApplicableHeaderTradeAgreement!.SellerTradeParty!.SpecifiedLegalOrganization!.Id = "SELLER_LEGAL_ID";
        invoice.SupplyChainTradeTransaction!.ApplicableHeaderTradeAgreement!.SellerTradeParty!.SpecifiedLegalOrganization!.IdSchemeId = "SELLER_LEGAL_ID_SCHEME";
        invoice.SupplyChainTradeTransaction!.ApplicableHeaderTradeAgreement!.SellerTradeParty!.PostalTradeAddress!.CountryId = "SELLER_COUNTRY";
        invoice.SupplyChainTradeTransaction!.ApplicableHeaderTradeAgreement!.SellerTradeParty!.SpecifiedTaxRegistration!.Id = "SELLER_TAX_ID";
        invoice.SupplyChainTradeTransaction!.ApplicableHeaderTradeAgreement!.SellerTradeParty!.SpecifiedTaxRegistration.IdSchemeId = VatOnlyTaxSchemeIdentifier.Vat;
        invoice.SupplyChainTradeTransaction!.ApplicableHeaderTradeAgreement!.BuyerTradeParty!.Name = "BUYER_NAME";
        invoice.SupplyChainTradeTransaction!.ApplicableHeaderTradeAgreement!.BuyerTradeParty!.SpecifiedLegalOrganization!.Id = "BUYER_LEGAL_ID";
        invoice.SupplyChainTradeTransaction!.ApplicableHeaderTradeAgreement!.BuyerTradeParty!.SpecifiedLegalOrganization!.IdSchemeId = "BUYER_LEGAL_ID_SCHEME";
        invoice.SupplyChainTradeTransaction!.ApplicableHeaderTradeAgreement!.BuyerOrderReferencedDocument!.IssuerAssignedId = "ISSUER_ASSIGNED_ID";
        invoice.SupplyChainTradeTransaction!.ApplicableHeaderTradeSettlement!.InvoiceCurrencyCode = "INVOICE_CURRENCY_CODE";
        invoice.SupplyChainTradeTransaction!.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation!.TaxBasisTotalAmount = 123;
        invoice.SupplyChainTradeTransaction!.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation!.TaxTotalAmount = 234;
        invoice.SupplyChainTradeTransaction!.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation!.TaxTotalAmountCurrencyId = "TAX_TOTAL_AMOUNT_CURRENCY_ID";
        invoice.SupplyChainTradeTransaction!.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation!.GrandTotalAmount = 345;
        invoice.SupplyChainTradeTransaction!.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation!.DuePayableAmount = 456;
        //@formatter:on

        minimumInvoice.Should()
            .BeEquivalentTo(
                new CrossIndustryInvoice
                {
                    ExchangedDocumentContext = new ExchangedDocumentContext
                    {
                        BusinessProcessSpecifiedDocumentContextParameterId = "BusinessProcessSpecifiedDocumentContextParameterId",
                        GuidelineSpecifiedDocumentContextParameterId = GuidelineSpecifiedDocumentContextParameterId.Extended
                    },
                    ExchangedDocument = new ExchangedDocument
                    {
                        Id = "ExhangedDocumentId",
                        TypeCode = InvoiceTypeCode.CustomsInvoice,
                        IssueDateTime = new DateOnly(1, 2, 3),
                        IssueDateTimeFormat = DateOnlyFormat.DateOnly
                    },
                    SupplyChainTradeTransaction = new SupplyChainTradeTransaction
                    {
                        ApplicableHeaderTradeAgreement = new ApplicableHeaderTradeAgreement
                        {
                            BuyerReference = "BUYER_REFERENCE",
                            SellerTradeParty = new SellerTradeParty
                            {
                                Name = "SELLER_NAME",
                                SpecifiedLegalOrganization = new SellerTradePartySpecifiedLegalOrganization
                                {
                                    Id = "SELLER_LEGAL_ID",
                                    IdSchemeId = "SELLER_LEGAL_ID_SCHEME"
                                },
                                PostalTradeAddress = new SellerTradePartyPostalTradeAddress
                                {
                                    CountryId = "SELLER_COUNTRY"
                                },
                                SpecifiedTaxRegistration = new SellerTradePartySpecifiedTaxRegistration
                                {
                                    Id = "SELLER_TAX_ID",
                                    IdSchemeId = VatOnlyTaxSchemeIdentifier.Vat
                                }
                            },
                            BuyerTradeParty = new BuyerTradeParty
                            {
                                Name = "BUYER_NAME",
                                SpecifiedLegalOrganization = new BuyerTradePartySpecifiedLegalOrganization
                                {
                                    Id = "BUYER_LEGAL_ID",
                                    IdSchemeId = "BUYER_LEGAL_ID_SCHEME"
                                }

                            },
                            BuyerOrderReferencedDocument = new BuyerOrderReferencedDocument
                            {
                                IssuerAssignedId = "ISSUER_ASSIGNED_ID"
                            }
                        },
                        ApplicableHeaderTradeDelivery = new ApplicableHeaderTradeDelivery(),
                        ApplicableHeaderTradeSettlement = new ApplicableHeaderTradeSettlement
                        {
                            InvoiceCurrencyCode = "INVOICE_CURRENCY_CODE",
                            SpecifiedTradeSettlementHeaderMonetarySummation = new SpecifiedTradeSettlementHeaderMonetarySummation
                            {
                                TaxBasisTotalAmount = 123,
                                TaxTotalAmount = 234,
                                TaxTotalAmountCurrencyId = "TAX_TOTAL_AMOUNT_CURRENCY_ID",
                                GrandTotalAmount = 345,
                                DuePayableAmount = 456
                            }
                        }
                    }
                }
            );
    }

    [TestMethod]
    public void ValuesSetInMinimumInvoice_ShouldBeSetInSourceInvoice()
    {
        CrossIndustryInvoice invoice = FakeData.CrossIndustryInvoice;
        MinimumCrossIndustryInvoice minimumInvoice = invoice.AsMinimumInvoice();
        
        //@formatter:off
        minimumInvoice.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameterId = "BusinessProcessSpecifiedDocumentContextParameterId";
        minimumInvoice.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameterId = GuidelineSpecifiedDocumentContextParameterId.Extended;
        minimumInvoice.ExchangedDocument.Id = "ExhangedDocumentId";
        minimumInvoice.ExchangedDocument.TypeCode = InvoiceTypeCode.CustomsInvoice;
        minimumInvoice.ExchangedDocument.IssueDateTime = new DateOnly(1, 2,3);
        minimumInvoice.ExchangedDocument.IssueDateTimeFormat = DateOnlyFormat.DateOnly;
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference = "BUYER_REFERENCE";
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.Name = "SELLER_NAME";
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization!.Id = "SELLER_LEGAL_ID";
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.IdSchemeId = "SELLER_LEGAL_ID_SCHEME";
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryId = "SELLER_COUNTRY";
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration!.Id = "SELLER_TAX_ID";
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.IdSchemeId = VatOnlyTaxSchemeIdentifier.Vat;
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name = "BUYER_NAME";
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization!.Id = "BUYER_LEGAL_ID";
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization!.IdSchemeId = "BUYER_LEGAL_ID_SCHEME";
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument!.IssuerAssignedId = "ISSUER_ASSIGNED_ID";
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement!.InvoiceCurrencyCode = "INVOICE_CURRENCY_CODE";
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount = 123;
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount = 234;
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmountCurrencyId = "TAX_TOTAL_AMOUNT_CURRENCY_ID";
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount = 345;
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount = 456;
        //@formatter:on

        minimumInvoice.Should()
            .BeEquivalentTo(
                new CrossIndustryInvoice
                {
                    ExchangedDocumentContext = new ExchangedDocumentContext
                    {
                        BusinessProcessSpecifiedDocumentContextParameterId = "BusinessProcessSpecifiedDocumentContextParameterId",
                        GuidelineSpecifiedDocumentContextParameterId = GuidelineSpecifiedDocumentContextParameterId.Extended
                    },
                    ExchangedDocument = new ExchangedDocument
                    {
                        Id = "ExhangedDocumentId",
                        TypeCode = InvoiceTypeCode.CustomsInvoice,
                        IssueDateTime = new DateOnly(1, 2, 3),
                        IssueDateTimeFormat = DateOnlyFormat.DateOnly
                    },
                    SupplyChainTradeTransaction = new SupplyChainTradeTransaction
                    {
                        ApplicableHeaderTradeAgreement = new ApplicableHeaderTradeAgreement
                        {
                            BuyerReference = "BUYER_REFERENCE",
                            SellerTradeParty = new SellerTradeParty
                            {
                                Name = "SELLER_NAME",
                                SpecifiedLegalOrganization = new SellerTradePartySpecifiedLegalOrganization
                                {
                                    Id = "SELLER_LEGAL_ID",
                                    IdSchemeId = "SELLER_LEGAL_ID_SCHEME"
                                },
                                PostalTradeAddress = new SellerTradePartyPostalTradeAddress
                                {
                                    CountryId = "SELLER_COUNTRY"
                                },
                                SpecifiedTaxRegistration = new SellerTradePartySpecifiedTaxRegistration
                                {
                                    Id = "SELLER_TAX_ID",
                                    IdSchemeId = VatOnlyTaxSchemeIdentifier.Vat
                                }
                            },
                            BuyerTradeParty = new BuyerTradeParty
                            {
                                Name = "BUYER_NAME",
                                SpecifiedLegalOrganization = new BuyerTradePartySpecifiedLegalOrganization
                                {
                                    Id = "BUYER_LEGAL_ID",
                                    IdSchemeId = "BUYER_LEGAL_ID_SCHEME"
                                }
                            },
                            BuyerOrderReferencedDocument = new BuyerOrderReferencedDocument
                            {
                                IssuerAssignedId = "ISSUER_ASSIGNED_ID"
                            }
                        },
                        ApplicableHeaderTradeDelivery = new ApplicableHeaderTradeDelivery(),
                        ApplicableHeaderTradeSettlement = new ApplicableHeaderTradeSettlement
                        {
                            InvoiceCurrencyCode = "INVOICE_CURRENCY_CODE",
                            SpecifiedTradeSettlementHeaderMonetarySummation = new SpecifiedTradeSettlementHeaderMonetarySummation
                            {
                                TaxBasisTotalAmount = 123,
                                TaxTotalAmount = 234,
                                TaxTotalAmountCurrencyId = "TAX_TOTAL_AMOUNT_CURRENCY_ID",
                                GrandTotalAmount = 345,
                                DuePayableAmount = 456
                            }
                        }
                    }
                }
            );
    }

    [TestMethod]
    public void SettingNullableValuesToNullInMinimumInvoice_ShouldSetValuesToNullInSourceInvoice()
    {
        CrossIndustryInvoice invoice = FakeData.CrossIndustryInvoice;
        MinimumCrossIndustryInvoice minimumInvoice = invoice.AsMinimumInvoice();
        
        //@formatter:off
        minimumInvoice.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameterId = null;
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference = null;
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization = null;
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration = null;
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization = null;
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument = null;
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement = null;
        //@formatter:on

        minimumInvoice.Should()
            .BeEquivalentTo(
                new CrossIndustryInvoice
                {
                    ExchangedDocumentContext = new ExchangedDocumentContext
                    {
                        BusinessProcessSpecifiedDocumentContextParameterId = null,
                        GuidelineSpecifiedDocumentContextParameterId = invoice.ExchangedDocumentContext!.GuidelineSpecifiedDocumentContextParameterId
                    },
                    ExchangedDocument = new ExchangedDocument
                    {
                        Id = string.Empty,
                        TypeCode = invoice.ExchangedDocument!.TypeCode,
                        IssueDateTime = invoice.ExchangedDocument!.IssueDateTime,
                        IssueDateTimeFormat = invoice.ExchangedDocument!.IssueDateTimeFormat
                    },
                    SupplyChainTradeTransaction = new SupplyChainTradeTransaction
                    {
                        ApplicableHeaderTradeAgreement = new ApplicableHeaderTradeAgreement
                        {
                            BuyerReference = null,
                            SellerTradeParty = new SellerTradeParty
                            {
                                Name = string.Empty,
                                SpecifiedLegalOrganization = null,
                                PostalTradeAddress = new SellerTradePartyPostalTradeAddress
                                {
                                    CountryId = string.Empty
                                },
                                SpecifiedTaxRegistration = null
                            },
                            BuyerTradeParty = new BuyerTradeParty
                            {
                                Name = string.Empty,
                                SpecifiedLegalOrganization = null
                            },
                            BuyerOrderReferencedDocument = null
                        },
                        ApplicableHeaderTradeDelivery = new ApplicableHeaderTradeDelivery(),
                        ApplicableHeaderTradeSettlement = null
                    }
                }
            );
    }

    [TestMethod]
    public void ShouldGetTheSourceObjectsBack()
    {
        CrossIndustryInvoice invoice = FakeData.CrossIndustryInvoice;
        MinimumCrossIndustryInvoice minimumInvoice = invoice.AsMinimumInvoice();

        minimumInvoice.ToCrossIndustryInvoice().Should().Be(invoice);
        minimumInvoice.ExchangedDocument.ToExchangedDocument().Should().Be(invoice.ExchangedDocument);
        minimumInvoice.ExchangedDocumentContext.ToExchangedDocumentContext().Should().Be(invoice.ExchangedDocumentContext);
        minimumInvoice.SupplyChainTradeTransaction.ToSupplyChainTradeTransaction().Should().Be(invoice.SupplyChainTradeTransaction);
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.ToApplicableHeaderTradeAgreement()
            .Should()
            .Be(invoice.SupplyChainTradeTransaction!.ApplicableHeaderTradeAgreement);
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.ToBuyerTradeParty()
            .Should()
            .Be(invoice.SupplyChainTradeTransaction!.ApplicableHeaderTradeAgreement!.BuyerTradeParty);
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization!.ToBuyerTradePartySpecifiedLegalOrganization()
            .Should()
            .Be(invoice.SupplyChainTradeTransaction!.ApplicableHeaderTradeAgreement!.BuyerTradeParty!.SpecifiedLegalOrganization);
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.ToSellerTradeParty()
            .Should()
            .Be(invoice.SupplyChainTradeTransaction!.ApplicableHeaderTradeAgreement!.SellerTradeParty);
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.ToSellerTradePartyPostalTradeAddress()
            .Should()
            .Be(invoice.SupplyChainTradeTransaction!.ApplicableHeaderTradeAgreement!.SellerTradeParty!.PostalTradeAddress);
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization!.ToSellerTradePartySpecifiedLegalOrganization()
            .Should()
            .Be(invoice.SupplyChainTradeTransaction!.ApplicableHeaderTradeAgreement!.SellerTradeParty!.SpecifiedLegalOrganization);
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration!.ToSellerTradePartySpecifiedTaxRegistration()
            .Should()
            .Be(invoice.SupplyChainTradeTransaction!.ApplicableHeaderTradeAgreement!.SellerTradeParty!.SpecifiedTaxRegistration);
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument!.ToBuyerOrderReferencedDocument()
            .Should()
            .Be(invoice.SupplyChainTradeTransaction!.ApplicableHeaderTradeAgreement!.BuyerOrderReferencedDocument);
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeDelivery.ToApplicableHeaderTradeDelivery()
            .Should()
            .Be(invoice.SupplyChainTradeTransaction!.ApplicableHeaderTradeDelivery);
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement!.ToApplicableHeaderTradeSettlement()
            .Should()
            .Be(invoice.SupplyChainTradeTransaction!.ApplicableHeaderTradeSettlement);
        minimumInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement!.SpecifiedTradeSettlementHeaderMonetarySummation
            .ToSpecifiedTradeSettlementHeaderMonetarySummation()
            .Should()
            .Be(invoice.SupplyChainTradeTransaction!.ApplicableHeaderTradeSettlement!.SpecifiedTradeSettlementHeaderMonetarySummation);
    }
}
