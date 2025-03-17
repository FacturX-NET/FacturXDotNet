namespace FacturXDotNet.Models;

/// <summary>
///     <b>DOCUMENT TOTALS</b> - A group of business terms providing the monetary totals for the Invoice.
/// </summary>
/// <remarks>
///     This group may be used to give prior notice in the invoice that payment will be made through a SEPA or other direct debit initiated by the Seller, in accordance with the
///     rules of the SEPA or other direct debit scheme.
/// </remarks>
/// <ID>BG-22</ID>
/// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementHeaderMonetarySummation</CiiXPath>
/// <Profile>MINIMUM</Profile>
/// <ChorusPro>Amounts in an invoice are expressed by a figure on 19 positions. They can not have more than two decimals. The separator is <c>.</c>.</ChorusPro>
public class FacturXSpecifiedTradeSettlementHeaderMonetarySummation
{
    /// <summary>
    ///     <b>Invoice total amount without VAT</b> - The total amount of the Invoice without VAT.
    /// </summary>
    /// <remarks>
    ///     The Invoice total amount without VAT is the Sum of Invoice line net amount minus Sum of allowances on document level plus Sum of charges on document level.
    /// </remarks>
    /// <ID>BT-109</ID>
    /// <BR-13>An Invoice shall have the Invoice total amount without VAT.</BR-13>
    /// <BR-CO-13>
    ///     <c>Invoice total amount without VAT (BT-109)</c> =
    ///     ∑ <c>Invoice line net amount (BT-131)</c> - <c>Sum of allowances on document level (BT-107)</c> +
    ///     <c>Sum of charges on document level (BT-108)</c>
    /// </BR-CO-13>
    /// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:TaxBasisTotalAmount</CiiXPath>
    /// <Profile>MINIMUM</Profile>
    public required decimal TaxBasisTotalAmount { get; set; }

    /// <summary>
    ///     <b>Invoice total VAT amount</b> - The total VAT amount for the Invoice.
    /// </summary>
    /// <remarks>
    ///     The Invoice total VAT amount is the sum of all VAT category tax amounts.
    /// </remarks>
    /// <ID>BT-110</ID>
    /// <BR-CO-14><c>Invoice total VAT amount (BT-110)</c> = ∑ <c>VAT category tax amount (BT-117)</c></BR-CO-14>
    /// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:TaxTotalAmount</CiiXPath>
    /// <Profile>MINIMUM</Profile>
    public decimal? TaxTotalAmount { get; set; }

    /// <summary>
    ///     <b>VAT currency</b>
    /// </summary>
    /// <remarks>
    ///     The currency is mandatory to differentiate between VAT amount and VAT amount in accounting currency.
    /// </remarks>
    /// <ID>BT-110-1</ID>
    /// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:TaxTotalAmount/@currencyID</CiiXPath>
    /// <Profile>MINIMUM</Profile>
    public required string TaxTotalAmountCurrencyId { get; set; }

    /// <summary>
    ///     <b>Invoice total amount with VAT</b> - The total amount of the Invoice with VAT.
    /// </summary>
    /// <remarks>
    ///     The Invoice total amount with VAT is the Invoice total amount without VAT plus the Invoice total VAT amount.
    /// </remarks>
    /// <ID>BT-112</ID>
    /// <BR-14>An Invoice shall have the Invoice total amount with VAT (BT-112).</BR-14>
    /// <BR-CO-15><c>Invoice total amount with VAT (BT-112)</c> = <c>Invoice total amount without VAT (BT-109)</c> + <c>Invoice total VAT amount (BT-110)</c>.</BR-CO-15>
    /// <BR-FXEXT-CO-15>For EXTENDED profile only, BR-CO-15 is replaced by BR-FXEXT-CO-15, which add a tolerance of 0,01 euro per line, document level charge and allowance in calculation.</BR-FXEXT-CO-15>
    /// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:GrandTotalAmount</CiiXPath>
    /// <Profile>MINIMUM</Profile>
    public required decimal GrandTotalAmount { get; set; }

    /// <summary>
    ///     <b>Amount due for payment</b> - The outstanding amount that is requested to be paid.
    /// </summary>
    /// <remarks>
    ///     This amount is the Invoice total amount with VAT minus the paid amount that has been paid in advance. The amount is zero in case of a fully paid Invoice. The amount may be
    ///     negative; in that case the Seller owes the amount to the Buyer.
    /// </remarks>
    /// <ID>BT-115</ID>
    /// <BR-15>An Invoice shall have the Amount due for payment.</BR-15>
    /// <BR-CO-16><c>Amount due for payment (BT-115)</c> = <c>Invoice total amount with VAT (BT-112)</c> - <c>Paid amount (BT-113)</c> + <c>Rounding amount (BT-114)</c>.</BR-CO-16>
    /// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:DuePayableAmount</CiiXPath>
    /// <Profile>MINIMUM</Profile>
    public required decimal DuePayableAmount { get; set; }
}
