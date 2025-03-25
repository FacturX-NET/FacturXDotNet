using FacturXDotNet;
using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;

namespace Tests.FacturXDotNet.TestTools;

static class FakeData
{
    public static XmpMetadata XmpMetadata =>
        new()
        {
            PdfAIdentification = new XmpPdfAIdentificationMetadata
            {
                Amendment = string.Empty,
                Conformance = XmpPdfAConformanceLevel.A,
                Part = 0
            },
            Basic = new XmpBasicMetadata
            {
                Identifier = [],
                CreateDate = DateTime.MinValue,
                CreatorTool = string.Empty,
                Label = string.Empty,
                MetadataDate = DateTime.MinValue,
                ModifyDate = DateTime.MinValue,
                Rating = 0,
                BaseUrl = string.Empty,
                Nickname = string.Empty,
                Thumbnails = []
            },
            Pdf = new XmpPdfMetadata
            {
                Keywords = string.Empty,
                PdfVersion = string.Empty,
                Producer = string.Empty,
                Trapped = false
            },
            DublinCore = new XmpDublinCoreMetadata
            {
                Contributor = [],
                Coverage = string.Empty,
                Creator = [],
                Date = [],
                Description = [],
                Format = string.Empty,
                Identifier = string.Empty,
                Language = [],
                Publisher = [],
                Relation = [],
                Rights = [],
                Source = string.Empty,
                Subject = [],
                Title = [],
                Type = []
            },
            PdfAExtensions = new XmpPdfAExtensionsMetadata
            {
                Schemas = []
            },
            FacturX = new XmpFacturXMetadata
            {
                DocumentFileName = string.Empty,
                DocumentType = XmpFacturXDocumentType.Invoice,
                Version = string.Empty,
                ConformanceLevel = XmpFacturXConformanceLevel.Basic
            }
        };

    public static CrossIndustryInvoice CrossIndustryInvoice =>
        new()
        {
            ExchangedDocumentContext = new ExchangedDocumentContext
            {
                BusinessProcessSpecifiedDocumentContextParameterId = string.Empty,
                GuidelineSpecifiedDocumentContextParameterId = default

            },
            ExchangedDocument = new ExchangedDocument
            {
                Id = string.Empty,
                TypeCode = default,
                IssueDateTime = default,
                IssueDateTimeFormat = default
            },
            SupplyChainTradeTransaction = new SupplyChainTradeTransaction
            {
                ApplicableHeaderTradeAgreement = new ApplicableHeaderTradeAgreement
                {
                    BuyerReference = string.Empty,
                    SellerTradeParty = new SellerTradeParty
                    {
                        Name = string.Empty,
                        PostalTradeAddress = new SellerTradePartyPostalTradeAddress
                        {
                            CountryId = string.Empty
                        }
                    },
                    BuyerTradeParty = new BuyerTradeParty
                    {
                        Name = string.Empty
                    },
                    BuyerOrderReferencedDocument = new BuyerOrderReferencedDocument
                    {
                        IssuerAssignedId = string.Empty
                    }
                },
                ApplicableHeaderTradeDelivery = new ApplicableHeaderTradeDelivery(),
                ApplicableHeaderTradeSettlement = new ApplicableHeaderTradeSettlement
                {
                    InvoiceCurrencyCode = string.Empty,
                    SpecifiedTradeSettlementHeaderMonetarySummation = new SpecifiedTradeSettlementHeaderMonetarySummation
                    {
                        TaxBasisTotalAmount = 0,
                        TaxTotalAmount = null,
                        TaxTotalAmountCurrencyId = string.Empty,
                        GrandTotalAmount = 0,
                        DuePayableAmount = 0
                    }
                }
            }
        };
}
