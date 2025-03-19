namespace FacturXDotNet;

/// <summary>
///     <b>HEADER TRADE SETTLEMENT DIRECT DEBIT</b> - A group of business terms to specify a direct debit.
/// </summary>
/// <remarks>
///     This group may be used to give prior notice in the invoice that payment will be made through a SEPA or other direct debit initiated by the Seller, in accordance with the
///     rules of the SEPA or other direct debit scheme.
/// </remarks>
/// <ID>BG-19</ID>
/// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement</CiiXPath>
/// <Profile>MINIMUM</Profile>
/// <ChorusPro>Not used.</ChorusPro>
public class ApplicableHeaderTradeSettlement
{
    /// <summary>
    ///     <b>Invoice currency code</b> - The currency in which all Invoice amounts are given, except for the Total VAT amount in accounting currency.
    /// </summary>
    /// <remarks>
    ///     Only one currency shall be used in the Invoice, except for the Total VAT amount in accounting currency (BT-111) in accordance with article 230 of Directive 2006/112/EC on
    ///     VAT. The lists of valid currencies are registered with the ISO 4217 Maintenance Agency "Codes for the representation of currencies and funds".
    /// </remarks>
    /// <ID>BT-5</ID>
    /// <BR-5>: An Invoice shall have an Invoice currency code.</BR-5>
    /// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:InvoiceCurrencyCode</CiiXPath>
    /// <Profile>MINIMUM</Profile>
    /// <ChorusPro>Invoices and credit notes or Chorus Pro are mono-currencies only.</ChorusPro>
    public required string InvoiceCurrencyCode { get; set; }

    /// <inheritdoc cref="FacturXDotNet.SpecifiedTradeSettlementHeaderMonetarySummation" />
    public required SpecifiedTradeSettlementHeaderMonetarySummation SpecifiedTradeSettlementHeaderMonetarySummation { get; set; }
}
