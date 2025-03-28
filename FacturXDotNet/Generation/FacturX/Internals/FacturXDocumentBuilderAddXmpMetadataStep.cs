using FacturXDotNet.Generation.PDF.Internals;
using FacturXDotNet.Generation.XMP;
using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;
using FacturXDotNet.Parsing.XMP;
using Microsoft.Extensions.Logging;
using PdfSharp.Pdf;

namespace FacturXDotNet.Generation.FacturX.Internals;

static class FacturXDocumentBuilderAddXmpMetadataStep
{
    static readonly string? Version = typeof(FacturXDocumentBuilderAddXmpMetadataStep).Assembly.GetName().Version?.ToString();

    public static async Task<XmpMetadata> RunAsync(PdfDocument pdfDocument, CrossIndustryInvoice cii, FacturXDocumentBuildArgs args)
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

        if (!args.XmpLeaveOpen)
        {
            await xmpStream.DisposeAsync();
        }

        DateTimeOffset now = DateTimeOffset.Now;

        if (!args.DisableXmpMetadataAutoGeneration)
        {
            // Note: some values are overwritten on purpose, they correspond to values that are the responsibility of the writer.
            // For example
            // - the PDF/A version must be 3 or higher
            // - the FacturX DocumentFileName must be set to the name of the CII attachment
            // - the FacturX ConformanceLevel must be set to the value of the GuidelineSpecifiedDocumentContextParameterId
            // - ... 
            //
            // They can still be post processed by the user: it is a choice to give them more control. Changing these values is dangerous: it might make the document invalid
            // with respect to the PDF/A standard.
            //
            // Finally, some values cannot be post-processed: for example the name of the creator tool (this lib)

            xmpMetadata.PdfAIdentification ??= new XmpPdfAIdentificationMetadata();
            xmpMetadata.PdfAIdentification.Conformance ??= XmpPdfAConformanceLevel.B;
            xmpMetadata.PdfAIdentification.Part = 3;
            xmpMetadata.Basic ??= new XmpBasicMetadata();
            xmpMetadata.Basic.CreateDate = now;
            xmpMetadata.Basic.ModifyDate = now;
            xmpMetadata.Pdf ??= new XmpPdfMetadata();
            xmpMetadata.Pdf.Keywords ??= JoinStrings(
                "Factur-X",
                GetInvoiceType(cii),
                cii.ExchangedDocument?.Id,
                cii.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.Name,
                cii.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.Name
            );
            xmpMetadata.DublinCore ??= new XmpDublinCoreMetadata();
            // Note: these values must be set by the user, they are not automatically generated
            // xmpMetadata.DublinCore.Title = [ComputeTitle(cii)];
            // xmpMetadata.DublinCore.Description = [ComputeDescription(cii)];
            // xmpMetadata.DublinCore.Date = [now];
            // xmpMetadata.DublinCore.Creator = GetIssuer(cii) is { } issuer ? [issuer] : xmpMetadata.DublinCore.Creator;
            xmpMetadata.PdfAExtensions ??= new XmpPdfAExtensionsMetadata();
            AddFacturXPdfAExtensionIfNecessary(xmpMetadata.PdfAExtensions);
            xmpMetadata.FacturX ??= new XmpFacturXMetadata();
            xmpMetadata.FacturX.DocumentFileName = args.CiiAttachmentName;
            xmpMetadata.FacturX.DocumentType = XmpFacturXDocumentType.Invoice;
            xmpMetadata.FacturX.Version = "1.0";
            xmpMetadata.FacturX.ConformanceLevel =
                cii.ExchangedDocumentContext?.GuidelineSpecifiedDocumentContextParameterId?.ToFacturXProfileOrNull()?.ToXmpFacturXConformanceLevel()
                ?? throw new InvalidOperationException("The CII document does not contain a valid GuidelineSpecifiedDocumentContextParameterId.");
        }

        args.PostProcess.ConfigureXmpMetadata(xmpMetadata);

        string toolName = string.IsNullOrWhiteSpace(Version) ? "FacturX.NET ~dev" : $"FacturX.NET v{Version}";
        xmpMetadata.Basic ??= new XmpBasicMetadata();
        xmpMetadata.Basic.CreatorTool = toolName;
        xmpMetadata.Pdf ??= new XmpPdfMetadata();
        xmpMetadata.Pdf.Producer = toolName;

        await using MemoryStream finalXmpStream = new();
        XmpMetadataWriter xmpWriter = new();
        await xmpWriter.WriteAsync(finalXmpStream, xmpMetadata);

        pdfDocument.ReplaceXmpMetadata(finalXmpStream.GetBuffer().AsSpan(0, (int)finalXmpStream.Length));
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

    static string ComputeTitle(CrossIndustryInvoice cii) => $"{cii.ExchangedDocument?.Id} | {cii.ExchangedDocument?.IssueDateTime?.ToString("d")}";

    static string ComputeDescription(CrossIndustryInvoice cii)
    {
        string documentType = GetInvoiceType(cii);

        string? issuer = GetIssuer(cii);
        string? recipient = GetRecipient(cii);

        return $"{documentType} number {cii.ExchangedDocument?.Id} dated {cii.ExchangedDocument?.IssueDateTime?.ToString("d") ?? "???"} issued by {issuer} to {recipient}";
    }

    static string GetInvoiceType(CrossIndustryInvoice cii) =>
        cii.ExchangedDocument?.TypeCode switch
        {
            InvoiceTypeCode.RequestForPayment => "Request for payment",
            InvoiceTypeCode.DebitNoteRelatedToGoodsOrServices => "Debit note related to goods or services",
            InvoiceTypeCode.CreditNoteRelatedToGoodsOrServices => "Credit note related to goods or services",
            InvoiceTypeCode.MeteredServicesInvoice => "Metered services invoice",
            InvoiceTypeCode.CreditNoteRelatedToFinancialAdjustments => "Credit note related to financial adjustments",
            InvoiceTypeCode.DebitNoteRelatedToFinancialAdjustments => "Debit note related to financial adjustments",
            InvoiceTypeCode.TaxNotification => "Tax notification",
            InvoiceTypeCode.InvoicingDataSheet => "Invoicing data sheet",
            InvoiceTypeCode.DirectPaymentValuation => "Direct payment valuation",
            InvoiceTypeCode.ProvisionalPaymentValuation => "Provisional payment valuation",
            InvoiceTypeCode.PaymentValuation => "Payment valuation",
            InvoiceTypeCode.InterimApplicationForPayment => "Interim application for payment",
            InvoiceTypeCode.FinalPaymentRequestBasedOnCompletionOfWork => "Final payment request based on completion of work",
            InvoiceTypeCode.PaymentRequestForCompletedUnits => "Payment request for completed units",
            InvoiceTypeCode.SelfBilledCreditNote => "Self billed credit note",
            InvoiceTypeCode.ConsolidatedCreditNoteGoodsAndServices => "Consolidated credit note - goods and services",
            InvoiceTypeCode.PriceVariationInvoice => "Price variation invoice",
            InvoiceTypeCode.CreditNoteForPriceVariation => "Credit note for price variation",
            InvoiceTypeCode.DelcredereCreditNote => "Delcredere credit note",
            InvoiceTypeCode.ProformaInvoice => "Proforma invoice",
            InvoiceTypeCode.PartialInvoice => "Partial invoice",
            InvoiceTypeCode.CommercialInvoiceWhichIncludesPackingList => "Commercial invoice which includes a packing list",
            InvoiceTypeCode.CommercialInvoice => "Commercial invoice",
            InvoiceTypeCode.CreditNote => "Credit note",
            InvoiceTypeCode.CommissionNote => "Commission note",
            InvoiceTypeCode.DebitNote => "Debit note",
            InvoiceTypeCode.CorrectedInvoice => "Corrected invoice",
            InvoiceTypeCode.ConsolidatedInvoice => "Consolidated invoice",
            InvoiceTypeCode.PrepaymentInvoice => "Prepayment invoice",
            InvoiceTypeCode.HireInvoice => "Hire invoice",
            InvoiceTypeCode.TaxInvoice => "Tax invoice",
            InvoiceTypeCode.SelfBilledInvoice => "Self-billed invoice",
            InvoiceTypeCode.DelcredereInvoice => "Delcredere invoice",
            InvoiceTypeCode.FactoredInvoice => "Factored invoice",
            InvoiceTypeCode.LeaseInvoice => "Lease invoice",
            InvoiceTypeCode.ConsignmentInvoice => "Consignment invoice",
            InvoiceTypeCode.FactoredCreditNote => "Factored credit note",
            InvoiceTypeCode.OcrPaymentCreditNote => "Optical Character Reading payment credit note",
            InvoiceTypeCode.DebitAdvice => "Debit advice",
            InvoiceTypeCode.ReversalOfDebit => "Reversal of debit",
            InvoiceTypeCode.ReversalOfCredit => "Reversal of credit",
            InvoiceTypeCode.SelfBilledDebitNote => "Self billed debit note",
            InvoiceTypeCode.ForwardersCreditNote => "Forwarder's credit note",
            InvoiceTypeCode.ForwardersInvoiceDiscrepancyReport => "Forwarder's invoice discrepancy report",
            InvoiceTypeCode.InsurersInvoice => "Insurer's invoice",
            InvoiceTypeCode.ForwardersInvoice => "Forwarder's invoice",
            InvoiceTypeCode.PortChargesDocuments => "Port charges documents",
            InvoiceTypeCode.InvoiceInformationForAccountingPurposes => "Invoice information for accounting purposes",
            InvoiceTypeCode.FreightInvoice => "Freight invoice",
            InvoiceTypeCode.ClaimNotification => "Claim notification",
            InvoiceTypeCode.ConsularInvoice => "Consular invoice",
            InvoiceTypeCode.PartialConstructionInvoice => "Partial construction invoice",
            InvoiceTypeCode.PartialFinalConstructionInvoice => "Partial final construction invoice",
            InvoiceTypeCode.FinalConstructionInvoice => "Final construction invoice",
            InvoiceTypeCode.CustomsInvoice => "Customs invoice",
            null or _ => "Document"
        };

    static string? GetIssuer(CrossIndustryInvoice cii) =>
        cii.ExchangedDocument?.TypeCode switch
        {
            InvoiceTypeCode.SelfBilledInvoice or InvoiceTypeCode.SelfBilledCreditNote or InvoiceTypeCode.SelfBilledDebitNote => cii.SupplyChainTradeTransaction
                ?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.Name,
            _ => cii.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.Name
        };

    static string? GetRecipient(CrossIndustryInvoice cii) =>
        cii.ExchangedDocument?.TypeCode switch
        {
            InvoiceTypeCode.SelfBilledInvoice or InvoiceTypeCode.SelfBilledCreditNote or InvoiceTypeCode.SelfBilledDebitNote => cii.SupplyChainTradeTransaction
                ?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.Name,
            _ => cii.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.Name
        };

    static string JoinStrings(params IEnumerable<string?> parts) => string.Join(", ", parts.Where(s => !string.IsNullOrWhiteSpace(s)));
}
