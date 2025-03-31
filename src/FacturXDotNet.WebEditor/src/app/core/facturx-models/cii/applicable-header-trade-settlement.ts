import { SpecifiedTradeSettlementHeaderMonetarySummation } from './specified-trade-settlement-header-monetary-summation';

/**
 * **HEADER TRADE SETTLEMENT DIRECT DEBIT** - A group of business terms to specify a direct debit.
 *
 * @remarks
 * This group may be used to give prior notice in the invoice that payment will be made through a SEPA or other direct debit initiated by the Seller, in accordance with the
 * rules of the SEPA or other direct debit scheme.
 *
 * @ID BG-19
 * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement
 * @Profile MINIMUM
 * @ChorusPro Not used.
 */
export interface ApplicableHeaderTradeSettlement {
  /**
   * **Invoice currency code** - The currency in which all Invoice amounts are given, except for the Total VAT amount in accounting currency.
   *
   * @remarks
   * Only one currency shall be used in the Invoice, except for the Total VAT amount in accounting currency (BT-111) in accordance with article 230 of Directive 2006/112/EC on
   * VAT. The lists of valid currencies are registered with the ISO 4217 Maintenance Agency "Codes for the representation of currencies and funds".
   *
   * @ID BT-5
   * @BusinessRules **BR-5**: An Invoice shall have an Invoice currency code.
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:InvoiceCurrencyCode
   * @Profile MINIMUM
   * @ChorusPro Invoices and credit notes or Chorus Pro are mono-currencies only.
   */
  invoiceCurrencyCode?: string;

  /**
   * **DOCUMENT TOTALS** - A group of business terms providing the monetary totals for the Invoice.
   *
   * @remarks
   * This group may be used to give prior notice in the invoice that payment will be made through a SEPA or other direct debit initiated by the Seller, in accordance with the
   * rules of the SEPA or other direct debit scheme.
   *
   * @ID BG-22
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementHeaderMonetarySummation
   * @Profile MINIMUM
   * @ChorusPro Amounts in an invoice are expressed by a figure on 19 positions. They can not have more than two decimals. The separator is `.`.
   */
  specifiedTradeSettlementHeaderMonetarySummation?: SpecifiedTradeSettlementHeaderMonetarySummation;
}
