using System.Globalization;
using System.Text;
using System.Xml;
using FacturXDotNet.Generation.Internals;
using FacturXDotNet.Models.CII;

namespace FacturXDotNet.Generation.CII;

/// <summary>
///     Write a <see cref="CrossIndustryInvoice" /> to a stream.
/// </summary>
public class CrossIndustryInvoiceWriter(CrossIndustryInvoiceWriterOptions? options = null)
{
    const string NsXmlns = "http://www.w3.org/2000/xmlns/";
    const string NsRsm = "urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100";
    const string PrefixRsm = "rsm";
    const string NsQdt = "urn:un:unece:uncefact:data:standard:QualifiedDataType:100";
    const string PrefixQdt = "qdt";
    const string NsRam = "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100";
    const string PrefixRam = "ram";
    const string NsUdt = "urn:un:unece:uncefact:data:standard:UnqualifiedDataType:100";
    const string PrefixUdt = "udt";
    const string NsXsi = "http://www.w3.org/2001/XMLSchema-instance";
    const string PrefixXsi = "xsi";

    CrossIndustryInvoiceWriterOptions _options = options ?? new CrossIndustryInvoiceWriterOptions();

    /// <summary>
    ///     Write the given <see cref="CrossIndustryInvoice" /> to the given stream.
    /// </summary>
    /// <param name="outputStream">The stream to write the invoice to.</param>
    /// <param name="cii">The invoice to write.</param>
    public async Task WriteAsync(Stream outputStream, CrossIndustryInvoice cii)
    {
        await using XmlWriter writer = XmlWriter.Create(outputStream, new XmlWriterSettings { Encoding = Encoding.UTF8, Async = true, Indent = true, CloseOutput = false });

        await writer.WriteStartDocumentAsync();

        await StartRsmAsync(writer, "CrossIndustryInvoice");
        await writer.WriteAttributeStringAsync("xmlns", PrefixQdt, NsXmlns, NsQdt);
        await writer.WriteAttributeStringAsync("xmlns", PrefixRam, NsXmlns, NsRam);
        await writer.WriteAttributeStringAsync("xmlns", PrefixRsm, NsXmlns, NsRsm);
        await writer.WriteAttributeStringAsync("xmlns", PrefixUdt, NsXmlns, NsUdt);
        await writer.WriteAttributeStringAsync("xmlns", PrefixXsi, NsXmlns, NsXsi);

        if (cii.ExchangedDocumentContext is not null)
        {
            await WriteExchangeDocumentContextAsync(writer, cii.ExchangedDocumentContext);
        }

        if (cii.ExchangedDocument is not null)
        {
            await WriteExchangeDocumentAsync(writer, cii.ExchangedDocument);
        }

        if (cii.SupplyChainTradeTransaction is not null)
        {
            await WriteSupplyChainTradeTransactionAsync(writer, cii.SupplyChainTradeTransaction);
        }

        await writer.WriteEndElementAsync();

        await writer.WriteEndDocumentAsync();
        await writer.FlushAsync();
    }

    static async Task WriteExchangeDocumentContextAsync(XmlWriter writer, ExchangedDocumentContext exchangedDocumentContext)
    {
        //@formatter:off
        
        await StartRsmAsync(writer,  "ExchangedDocumentContext");

        await StartRamAsync(writer,  "BusinessProcessSpecifiedDocumentContextParameter");
        await TryWriteRamAsync(writer, "ID", exchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameterId);
        await writer.WriteEndElementAsync();
        
        await StartRamAsync(writer,  "GuidelineSpecifiedDocumentContextParameter");
        await TryWriteRamAsync(writer, "ID", exchangedDocumentContext.GuidelineSpecifiedDocumentContextParameterId?.ToGuidelineSpecifiedDocumentContextParameterId().ToString());
        await writer.WriteEndElementAsync();

        await writer.WriteEndElementAsync();
        
        //@formatter:on
    }

    static async Task WriteExchangeDocumentAsync(XmlWriter writer, ExchangedDocument exchangedDocument)
    {
        await StartRsmAsync(writer, "ExchangedDocument");

        await TryWriteRamAsync(writer, "ID", exchangedDocument.Id);
        await TryWriteRamAsync(writer, "TypeCode", exchangedDocument.TypeCode?.ToSpecificationIdentifier().ToString());

        if (exchangedDocument.IssueDateTime.HasValue)
        {
            await StartRamAsync(writer, "IssueDateTime");
            await writer.WriteDateOnlyAsync(exchangedDocument.IssueDateTime.Value);
            await writer.WriteEndElementAsync();
        }

        await writer.WriteEndElementAsync();
    }

    static async Task WriteSupplyChainTradeTransactionAsync(XmlWriter writer, SupplyChainTradeTransaction supplyChainTradeTransaction)
    {
        await StartRsmAsync(writer, "SupplyChainTradeTransaction");

        if (supplyChainTradeTransaction.ApplicableHeaderTradeAgreement is not null)
        {
            await WriteApplicableHeaderTradeAgreementAsync(writer, supplyChainTradeTransaction.ApplicableHeaderTradeAgreement);
        }

        if (supplyChainTradeTransaction.ApplicableHeaderTradeDelivery is not null)
        {
            await WriteApplicableHeaderTradeDeliveryAsync(writer, supplyChainTradeTransaction.ApplicableHeaderTradeDelivery);
        }

        if (supplyChainTradeTransaction.ApplicableHeaderTradeSettlement is not null)
        {
            await WriteApplicableHeaderTradeSettlementAsync(writer, supplyChainTradeTransaction.ApplicableHeaderTradeSettlement);
        }

        await writer.WriteEndElementAsync();
    }

    static async Task WriteApplicableHeaderTradeAgreementAsync(XmlWriter writer, ApplicableHeaderTradeAgreement applicableHeaderTradeAgreement)
    {
        await StartRamAsync(writer, "ApplicableHeaderTradeAgreement");

        await TryWriteRamAsync(writer, "BuyerReference", applicableHeaderTradeAgreement.BuyerReference);

        if (applicableHeaderTradeAgreement.SellerTradeParty is not null)
        {
            await WriteSellerTradePartyAsync(writer, applicableHeaderTradeAgreement.SellerTradeParty);
        }

        if (applicableHeaderTradeAgreement.BuyerTradeParty is not null)
        {
            await WriteBuyerTradePartyAsync(writer, applicableHeaderTradeAgreement.BuyerTradeParty);
        }

        if (applicableHeaderTradeAgreement.BuyerOrderReferencedDocument is not null)
        {
            await WriteBuyerOrderReferencedDocument(writer, applicableHeaderTradeAgreement.BuyerOrderReferencedDocument);
        }

        await writer.WriteEndElementAsync();
    }

    static async Task WriteApplicableHeaderTradeDeliveryAsync(XmlWriter writer, ApplicableHeaderTradeDelivery _)
    {
        await StartRamAsync(writer, "ApplicableHeaderTradeDelivery");
        await writer.WriteEndElementAsync();
    }

    static async Task WriteApplicableHeaderTradeSettlementAsync(XmlWriter writer, ApplicableHeaderTradeSettlement applicableHeaderTradeSettlement)
    {
        await StartRamAsync(writer, "ApplicableHeaderTradeSettlement");

        await TryWriteRamAsync(writer, "InvoiceCurrencyCode", applicableHeaderTradeSettlement.InvoiceCurrencyCode);

        if (applicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation is not null)
        {
            await WriteSpecifiedTradeSettlementHeaderMonetarySummationAsync(writer, applicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation);
        }

        await writer.WriteEndElementAsync();
    }

    static async Task WriteSellerTradePartyAsync(XmlWriter writer, SellerTradeParty sellerTradeParty)
    {
        await StartRamAsync(writer, "SellerTradeParty");

        await TryWriteRamAsync(writer, "Name", sellerTradeParty.Name);

        if (sellerTradeParty.SpecifiedLegalOrganization is not null)
        {
            await WriteSellerTradePartySpecifiedLegalOrganizationAsync(writer, sellerTradeParty.SpecifiedLegalOrganization);
        }

        if (sellerTradeParty.PostalTradeAddress is not null)
        {
            await WriteSellerTradePartyPostalTradeAddressAsync(writer, sellerTradeParty.PostalTradeAddress);
        }

        if (sellerTradeParty.SpecifiedTaxRegistration is not null)
        {
            await WriteSellerTradePartySpecifiedTaxRegistrationAsync(writer, sellerTradeParty.SpecifiedTaxRegistration);
        }

        await writer.WriteEndElementAsync();
    }

    static async Task WriteSellerTradePartySpecifiedLegalOrganizationAsync(XmlWriter writer, SellerTradePartySpecifiedLegalOrganization specifiedLegalOrganization)
    {
        await StartRamAsync(writer, "SpecifiedLegalOrganization");

        await TryWriteIdAndSchemeId(writer, specifiedLegalOrganization.Id, specifiedLegalOrganization.IdSchemeId);

        await writer.WriteEndElementAsync();
    }

    static async Task WriteSellerTradePartyPostalTradeAddressAsync(XmlWriter writer, SellerTradePartyPostalTradeAddress postalTradeAddress)
    {
        await StartRamAsync(writer, "PostalTradeAddress");

        await TryWriteRamAsync(writer, "CountryID", postalTradeAddress.CountryId);

        await writer.WriteEndElementAsync();
    }

    static async Task WriteSellerTradePartySpecifiedTaxRegistrationAsync(XmlWriter writer, SellerTradePartySpecifiedTaxRegistration specifiedTaxRegistration)
    {
        await StartRamAsync(writer, "SpecifiedTaxRegistration");

        await TryWriteIdAndSchemeId(writer, specifiedTaxRegistration.Id, specifiedTaxRegistration.IdSchemeId?.ToVatOnlyTaxSchemeIdentifier());

        await writer.WriteEndElementAsync();
    }

    static async Task WriteBuyerTradePartyAsync(XmlWriter writer, BuyerTradeParty buyerTradeParty)
    {
        await StartRamAsync(writer, "BuyerTradeParty");

        await TryWriteRamAsync(writer, "Name", buyerTradeParty.Name);

        if (buyerTradeParty.SpecifiedLegalOrganization is not null)
        {
            await WriteBuyerTradePartySpecifiedLegalOrganizationAsync(writer, buyerTradeParty.SpecifiedLegalOrganization);
        }

        await writer.WriteEndElementAsync();
    }

    static async Task WriteBuyerTradePartySpecifiedLegalOrganizationAsync(XmlWriter writer, BuyerTradePartySpecifiedLegalOrganization specifiedLegalOrganization)
    {
        await StartRamAsync(writer, "SpecifiedLegalOrganization");

        await TryWriteIdAndSchemeId(writer, specifiedLegalOrganization.Id, specifiedLegalOrganization.IdSchemeId);

        await writer.WriteEndElementAsync();
    }

    static async Task WriteBuyerOrderReferencedDocument(XmlWriter writer, BuyerOrderReferencedDocument buyerOrderReferencedDocument)
    {
        await StartRamAsync(writer, "BuyerOrderReferencedDocument");

        await TryWriteRamAsync(writer, "IssuerAssignedID", buyerOrderReferencedDocument.IssuerAssignedId);

        await writer.WriteEndElementAsync();
    }

    static async Task WriteSpecifiedTradeSettlementHeaderMonetarySummationAsync(XmlWriter writer, SpecifiedTradeSettlementHeaderMonetarySummation headerMonetarySummation)
    {
        await StartRamAsync(writer, "SpecifiedTradeSettlementHeaderMonetarySummation");

        await TryWriteRamAsync(writer, "TaxBasisTotalAmount", TryFormatAmount(headerMonetarySummation.TaxBasisTotalAmount));

        if (headerMonetarySummation.TaxTotalAmount.HasValue)
        {
            await StartRamAsync(writer, "TaxTotalAmount");
            await writer.WriteAttributeStringAsync(null, "currencyID", null, headerMonetarySummation.TaxTotalAmountCurrencyId);
            await writer.WriteStringAsync(FormatAmount(headerMonetarySummation.TaxTotalAmount.Value));
            await writer.WriteEndElementAsync();
        }

        await TryWriteRamAsync(writer, "GrandTotalAmount", TryFormatAmount(headerMonetarySummation.GrandTotalAmount));
        await TryWriteRamAsync(writer, "DuePayableAmount", TryFormatAmount(headerMonetarySummation.DuePayableAmount));

        await writer.WriteEndElementAsync();
    }


    static async Task TryWriteIdAndSchemeId(XmlWriter writer, string? id, string? schemeId)
    {
        if (id is null)
        {
            return;
        }

        await StartRamAsync(writer, "ID");

        if (schemeId is not null)
        {
            await writer.WriteAttributeStringAsync(null, "schemeID", null, schemeId);
        }

        await writer.WriteStringAsync(id);
        await writer.WriteEndElementAsync();
    }

    static string? TryFormatAmount(decimal? amount) => amount?.ToString("0.##", CultureInfo.InvariantCulture);
    static string FormatAmount(decimal amount) => amount.ToString("0.##", CultureInfo.InvariantCulture);

    static async Task StartRsmAsync(XmlWriter writer, string localname) => await writer.WriteStartElementAsync(PrefixRsm, localname, NsRsm);
    static async Task StartRamAsync(XmlWriter writer, string localname) => await writer.WriteStartElementAsync(PrefixRam, localname, NsRam);

    static async Task WriteRamAsync(XmlWriter writer, string localname, string value) => await writer.WriteElementStringAsync(PrefixRam, localname, NsRam, value);

    static async Task TryWriteRamAsync(XmlWriter writer, string localname, string? value)
    {
        if (value is null)
        {
            return;
        }

        await WriteRamAsync(writer, localname, value);
    }
}
