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
export interface SpecifiedTradeSettlementHeaderMonetarySummation {
  /**
   * **Invoice total amount without VAT** - The total amount of the Invoice without VAT.
   *
   * @remarks
   * The Invoice total amount without VAT is the Sum of Invoice line net amount minus Sum of allowances on document level plus Sum of charges on document level.
   *
   * @ID BT-109
   * @BusinessRules
   * - **BR-13**: An Invoice shall have the Invoice total amount without VAT.
   * - **BR-CO-13**: `Invoice total amount without VAT (BT-109)` =
   *   ∑ `Invoice line net amount (BT-131)` - `Sum of allowances on document level (BT-107)` + `Sum of charges on document level (BT-108)`
   *
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:TaxBasisTotalAmount
   * @Profile MINIMUM
   */
  taxBasisTotalAmount?: number;

  /**
   * **Invoice total VAT amount** - The total VAT amount for the Invoice.
   *
   * @remarks
   * The Invoice total VAT amount is the sum of all VAT category tax amounts.
   *
   * @ID BT-110
   * @BusinessRules **BR-CO-14**: `Invoice total VAT amount (BT-110)` = ∑ `VAT category tax amount (BT-117)`
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:TaxTotalAmount
   * @Profile MINIMUM
   */
  taxTotalAmount?: number;

  /**
   * **VAT currency**
   *
   * @remarks
   * The currency is mandatory to differentiate between VAT amount and VAT amount in accounting currency.
   *
   * @ID BT-110-1
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:TaxTotalAmount/@currencyID
   * @Profile MINIMUM
   */
  taxTotalAmountCurrencyId?: number;

  /**
   * **Invoice total amount with VAT** - The total amount of the Invoice with VAT.
   *
   * @remarks
   * The Invoice total amount with VAT is the Invoice total amount without VAT plus the Invoice total VAT amount.
   *
   * @ID BT-112
   * @BusinessRules
   * - **BR-14**: An Invoice shall have the Invoice total amount with VAT (BT-112).
   * - **BR-CO-15**: `Invoice total amount with VAT (BT-112)` = `Invoice total amount without VAT (BT-109)` + `Invoice total VAT amount (BT-110)`.
   * - **BR-FXEXT-CO-15**: For EXTENDED profile only, BR-CO-15 is replaced by BR-FXEXT-CO-15, which add a tolerance of 0,01 euro per line, document level charge and allowance in calculation.
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:GrandTotalAmount
   * @Profile MINIMUM
   */
  grandTotalAmount?: number;

  /**
   * **Amount due for payment** - The outstanding amount that is requested to be paid.
   *
   * @remarks
   * This amount is the Invoice total amount with VAT minus the paid amount that has been paid in advance. The amount is zero in case of a fully paid Invoice. The amount may be
   * negative; in that case the Seller owes the amount to the Buyer.
   *
   * @ID BT-115
   * @BR-15 An Invoice shall have the Amount due for payment.
   * @BR-CO-16 `Amount due for payment (BT-115)` = `Invoice total amount with VAT (BT-112)` - `Paid amount (BT-113)` + `Rounding amount (BT-114)`.
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:DuePayableAmount
   * @Profile MINIMUM
   */
  duePayableAmount?: number;
}
