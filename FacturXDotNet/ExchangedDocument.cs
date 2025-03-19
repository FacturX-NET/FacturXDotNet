namespace FacturXDotNet;

/// <summary>
///     <b>EXCHANGE DOCUMENT</b>
/// </summary>
/// <ID>BT-1-00</ID>
/// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:ExchangedDocument</CiiXPath>
/// <Profile>MINIMUM</Profile>
public sealed class ExchangedDocument
{
    /// <summary>
    ///     <b>Invoice number</b> - A unique identification of the Invoice.
    /// </summary>
    /// <remarks>
    ///     The sequential number required in Article 226(2) of the directive 2006/112/EC, to uniquely identify the Invoice within the business context, time-frame, operating systems
    ///     and records of the Seller. It may be based on one or more series of numbers, which may include alphanumeric characters. No identification scheme is to be used.
    /// </remarks>
    /// <ID>BT-1</ID>
    /// <BR-2>An Invoice shall have an Invoice number.</BR-2>
    /// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:ExchangedDocument/ram:ID</CiiXPath>
    /// <Profile>MINIMUM</Profile>
    /// <ChorusPro>The invoice number is limited to 20 characters.</ChorusPro>
    public required string Id { get; set; }

    /// <inheritdoc cref="InvoiceTypeCode" />
    public required InvoiceTypeCode TypeCode { get; set; }

    /// <summary>
    ///     <b>Invoice issue date</b> - The date when the Invoice was issued.
    /// </summary>
    /// <ID>BT-2</ID>
    /// <BR-3>An Invoice shall have an Invoice issue date.</BR-3>
    /// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:ExchangedDocument/ram:IssueDateTime/udt:DateTimeString</CiiXPath>
    /// <Profile>MINIMUM</Profile>
    /// <ChorusPro>The issue date must be before or equal to the deposit date.</ChorusPro>
    public required DateOnly IssueDateTime { get; set; }

    /// <summary>
    ///     <b>Date, format</b>
    /// </summary>
    /// <remarks>
    ///     Only value "102"
    /// </remarks>
    /// <ID>BT-2-0</ID>
    /// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:ExchangedDocument/ram:IssueDateTime/udt:DateTimeString/@format</CiiXPath>
    /// <Profile>MINIMUM</Profile>
    public required DateOnlyFormat IssueDateTimeFormat { get; set; }
}
