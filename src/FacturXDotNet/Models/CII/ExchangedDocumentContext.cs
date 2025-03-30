namespace FacturXDotNet.Models.CII;

/// <summary>
///     <b>EXCHANGE DOCUMENT CONTEXT</b> - A group of business terms providing information on the business process and rules applicable to the Invoice document.
/// </summary>
/// <ID>BG-2</ID>
/// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:ExchangedDocumentContext</CiiXPath>
/// <Profile>MINIMUM</Profile>
public class ExchangedDocumentContext
{
    /// <summary>
    ///     <b>Business process type</b> - Identifies the business process context in which the transaction appears, to enable the Buyer to process the Invoice in an appropriate way.
    /// </summary>
    /// <ID>BT-23</ID>
    /// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:ExchangedDocumentContext/ram:BusinessProcessSpecifiedDocumentContextParameter/ram:ID</CiiXPath>
    /// <Profile>MINIMUM</Profile>
    public string? BusinessProcessSpecifiedDocumentContextParameterId { get; set; }

    /// <inheritdoc cref="CII.GuidelineSpecifiedDocumentContextParameterId" />
    public GuidelineSpecifiedDocumentContextParameterId? GuidelineSpecifiedDocumentContextParameterId { get; set; }
}
