using FacturXDotNet.Generation.PDF;
using FacturXDotNet.Generation.XMP;
using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;
using FacturXDotNet.Parsing.XMP;
using Microsoft.Extensions.Logging;
using PdfSharp.Pdf;

namespace FacturXDotNet.Generation.Internals;

static class FacturXBuilderXmpMetadata
{
    static readonly string? Version = typeof(FacturXBuilderXmpMetadata).Assembly.GetName().Version?.ToString();

    public static async Task<XmpMetadata> AddXmpMetadataAsync(PdfDocument pdfDocument, CrossIndustryInvoice cii, FacturXDocumentBuildArgs args)
    {
        Stream xmpStream;
        if (args.Xmp is null)
        {
            ExtractXmpFromPdf extractor = new();
            xmpStream = extractor.ExtractXmpMetadata(pdfDocument);

            // ensure xmp stream is disposed
            args.XmpLeaveOpen = false;
        }
        else
        {
            xmpStream = args.Xmp;
        }

        XmpMetadataReader xmpReader = new();
        XmpMetadata xmpMetadata = xmpReader.Read(xmpStream);

        DateTimeOffset now = DateTimeOffset.Now;

        xmpMetadata.PdfAIdentification ??= new XmpPdfAIdentificationMetadata();
        xmpMetadata.PdfAIdentification.Conformance ??= XmpPdfAConformanceLevel.B;
        xmpMetadata.PdfAIdentification.Part ??= 3;
        xmpMetadata.Basic ??= new XmpBasicMetadata();
        xmpMetadata.Basic.CreateDate = now;
        xmpMetadata.Basic.ModifyDate = now;
        xmpMetadata.Basic.MetadataDate = now;
        xmpMetadata.Pdf ??= new XmpPdfMetadata();
        xmpMetadata.DublinCore ??= new XmpDublinCoreMetadata();
        xmpMetadata.DublinCore.Title = [ComputeTitle(cii)];
        xmpMetadata.DublinCore.Description = [ComputeDescription(cii)];
        xmpMetadata.DublinCore.Date = [now];
        xmpMetadata.PdfAExtensions ??= new XmpPdfAExtensionsMetadata();
        AddFacturXPdfAExtensionIfNecessary(xmpMetadata.PdfAExtensions);
        xmpMetadata.FacturX ??= new XmpFacturXMetadata();
        xmpMetadata.FacturX.DocumentFileName = args.CiiAttachmentName;
        xmpMetadata.FacturX.DocumentType = XmpFacturXDocumentType.Invoice;
        xmpMetadata.FacturX.Version = "1.0";
        xmpMetadata.FacturX.ConformanceLevel = cii.ExchangedDocumentContext?.GuidelineSpecifiedDocumentContextParameterId?.ToFacturXProfileOrNull()?.ToXmpFacturXConformanceLevel()
                                               ?? throw new InvalidOperationException("The CII document does not contain a valid GuidelineSpecifiedDocumentContextParameterId.");

        args.PostProcessXmpMetadata?.Invoke(xmpMetadata);

        string toolName = string.IsNullOrWhiteSpace(Version) ? "FacturX.NET ~dev" : $"FacturX.NET v{Version}";
        xmpMetadata.Basic.CreatorTool = toolName;
        xmpMetadata.Pdf.Producer = toolName;

        if (!args.XmpLeaveOpen)
        {
            await xmpStream.DisposeAsync();
        }

        await using MemoryStream finalXmpStream = new();
        XmpMetadataWriter xmpWriter = new();
        await xmpWriter.WriteAsync(finalXmpStream, xmpMetadata);

        ReplaceXmpMetadataOfPdfDocument.ReplaceXmpMetadata(pdfDocument, finalXmpStream.GetBuffer().AsSpan(0, (int)finalXmpStream.Length));
        args.Logger?.LogInformation("Added XMP metadata to the PDF document.");

        return xmpMetadata;
    }

    static void AddFacturXPdfAExtensionIfNecessary(XmpPdfAExtensionsMetadata extensions)
    {
        const string namespaceUri = "urn:factur-x:pdfa:CrossIndustryDocument:invoice:1p0#";
        const string prefix = "fx";
        const string name = "Factur-X PDFA Extension Schema";

        if (extensions.Schemas.Any(s => s.NamespaceUri == namespaceUri))
        {
            return;
        }

        extensions.Schemas.Add(
            new XmpPdfASchemaMetadata
            {
                NamespaceUri = namespaceUri,
                Prefix = prefix,
                Property =
                [
                    new XmpPdfAPropertyMetadata
                    {
                        Category = XmpPdfAPropertyCategory.External,
                        Description = "The name of the embedded XML document",
                        Name = "DocumentFileName",
                        ValueType = "Text"
                    },
                    new XmpPdfAPropertyMetadata
                    {
                        Category = XmpPdfAPropertyCategory.External,
                        Description = "The type of the hybrid document in capital letters, e.g. INVOICE or ORDER",
                        Name = "DocumentType",
                        ValueType = "Text"
                    },
                    new XmpPdfAPropertyMetadata
                    {
                        Category = XmpPdfAPropertyCategory.External,
                        Description = "The actual version of the standard applying to the embedded XML document",
                        Name = "Version",
                        ValueType = "Text"
                    },
                    new XmpPdfAPropertyMetadata
                    {
                        Category = XmpPdfAPropertyCategory.External,
                        Description = "The conformance level of the embedded XML document",
                        Name = "ConformanceLevel",
                        ValueType = "Text"
                    }
                ],
                Schema = name,
                ValueType = []
            }
        );
    }

    static string ComputeTitle(CrossIndustryInvoice cii)
    {
        string documentType = GetInvoiceType(cii);
        return $"{documentType} {cii.ExchangedDocument?.Id} dated {cii.ExchangedDocument?.IssueDateTime?.ToString("d") ?? "???"}";
    }

    static string ComputeDescription(CrossIndustryInvoice cii)
    {
        string documentType = GetInvoiceType(cii);

        string? issuer = cii.ExchangedDocument?.TypeCode switch
        {
            InvoiceTypeCode.SelfBilledInvoice or InvoiceTypeCode.SelfBilledCreditNote or InvoiceTypeCode.SelfBilledDebitNote => cii.SupplyChainTradeTransaction
                ?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.Name,
            _ => cii.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.Name
        };

        string? recipient = cii.ExchangedDocument?.TypeCode switch
        {
            InvoiceTypeCode.SelfBilledInvoice or InvoiceTypeCode.SelfBilledCreditNote or InvoiceTypeCode.SelfBilledDebitNote => cii.SupplyChainTradeTransaction
                ?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.Name,
            _ => cii.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.Name
        };

        return $"{documentType} {cii.ExchangedDocument?.Id} dated {cii.ExchangedDocument?.IssueDateTime?.ToString("d") ?? "???"} issued by {issuer} to {recipient}";
    }

    static string GetInvoiceType(CrossIndustryInvoice cii) =>
        cii.ExchangedDocument?.TypeCode switch
        {
            InvoiceTypeCode.RequestForPayment => "Request for payment (71)",
            InvoiceTypeCode.DebitNoteRelatedToGoodsOrServices => "Debit note related to goods or services (80)",
            InvoiceTypeCode.CreditNoteRelatedToGoodsOrServices => "Credit note related to goods or services (81)",
            InvoiceTypeCode.MeteredServicesInvoice => "Metered services invoice (82)",
            InvoiceTypeCode.CreditNoteRelatedToFinancialAdjustments => "Credit note related to financial adjustments (83)",
            InvoiceTypeCode.DebitNoteRelatedToFinancialAdjustments => "Debit note related to financial adjustments (84)",
            InvoiceTypeCode.TaxNotification => "Tax notification (102)",
            InvoiceTypeCode.InvoicingDataSheet => "Invoicing data sheet (130)",
            InvoiceTypeCode.DirectPaymentValuation => "Direct payment valuation (202)",
            InvoiceTypeCode.ProvisionalPaymentValuation => "Provisional payment valuation (203)",
            InvoiceTypeCode.PaymentValuation => "Payment valuation (204)",
            InvoiceTypeCode.InterimApplicationForPayment => "Interim application for payment (211)",
            InvoiceTypeCode.FinalPaymentRequestBasedOnCompletionOfWork => "Final payment request based on completion of work (218)",
            InvoiceTypeCode.PaymentRequestForCompletedUnits => "Payment request for completed units (219)",
            InvoiceTypeCode.SelfBilledCreditNote => "Self billed credit note (261)",
            InvoiceTypeCode.ConsolidatedCreditNoteGoodsAndServices => "Consolidated credit note - goods and services (262)",
            InvoiceTypeCode.PriceVariationInvoice => "Price variation invoice (295)",
            InvoiceTypeCode.CreditNoteForPriceVariation => "Credit note for price variation (296)",
            InvoiceTypeCode.DelcredereCreditNote => "Delcredere credit note (308)",
            InvoiceTypeCode.ProformaInvoice => "Proforma invoice (325)",
            InvoiceTypeCode.PartialInvoice => "Partial invoice (326)",
            InvoiceTypeCode.CommercialInvoiceWhichIncludesPackingList => "Commercial invoice which includes a packing list (331)",
            InvoiceTypeCode.CommercialInvoice => "Commercial invoice (380)",
            InvoiceTypeCode.CreditNote => "Credit note (381)",
            InvoiceTypeCode.CommissionNote => "Commission note (382)",
            InvoiceTypeCode.DebitNote => "Debit note (383)",
            InvoiceTypeCode.CorrectedInvoice => "Corrected invoice (384)",
            InvoiceTypeCode.ConsolidatedInvoice => "Consolidated invoice (385)",
            InvoiceTypeCode.PrepaymentInvoice => "Prepayment invoice (386)",
            InvoiceTypeCode.HireInvoice => "Hire invoice (387)",
            InvoiceTypeCode.TaxInvoice => "Tax invoice (388)",
            InvoiceTypeCode.SelfBilledInvoice => "Self-billed invoice (389)",
            InvoiceTypeCode.DelcredereInvoice => "Delcredere invoice (390)",
            InvoiceTypeCode.FactoredInvoice => "Factored invoice (393)",
            InvoiceTypeCode.LeaseInvoice => "Lease invoice (394)",
            InvoiceTypeCode.ConsignmentInvoice => "Consignment invoice (395)",
            InvoiceTypeCode.FactoredCreditNote => "Factored credit note (396)",
            InvoiceTypeCode.OcrPaymentCreditNote => "Optical Character Reading (OCR) payment credit note (420)",
            InvoiceTypeCode.DebitAdvice => "Debit advice (456)",
            InvoiceTypeCode.ReversalOfDebit => "Reversal of debit (457)",
            InvoiceTypeCode.ReversalOfCredit => "Reversal of credit (458)",
            InvoiceTypeCode.SelfBilledDebitNote => "Self billed debit note (527)",
            InvoiceTypeCode.ForwardersCreditNote => "Forwarder's credit note (532)",
            InvoiceTypeCode.ForwardersInvoiceDiscrepancyReport => "Forwarder's invoice discrepancy report (553)",
            InvoiceTypeCode.InsurersInvoice => "Insurer's invoice (575)",
            InvoiceTypeCode.ForwardersInvoice => "Forwarder's invoice (623)",
            InvoiceTypeCode.PortChargesDocuments => "Port charges documents (633)",
            InvoiceTypeCode.InvoiceInformationForAccountingPurposes => "Invoice information for accounting purposes (751)",
            InvoiceTypeCode.FreightInvoice => "Freight invoice (780)",
            InvoiceTypeCode.ClaimNotification => "Claim notification (817)",
            InvoiceTypeCode.ConsularInvoice => "Consular invoice (870)",
            InvoiceTypeCode.PartialConstructionInvoice => "Partial construction invoice (875)",
            InvoiceTypeCode.PartialFinalConstructionInvoice => "Partial final construction invoice (876)",
            InvoiceTypeCode.FinalConstructionInvoice => "Final construction invoice (877)",
            InvoiceTypeCode.CustomsInvoice => "Customs invoice (935)",
            null or _ => "Document"
        };
}
