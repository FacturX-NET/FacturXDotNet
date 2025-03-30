import { ApplicableHeaderTradeAgreement } from './applicable-header-trade-agreement';
import { ApplicableHeaderTradeDelivery } from './applicable-header-trade-delivery';
import { ApplicableHeaderTradeSettlement } from './applicable-header-trade-settlement';

/**
 * **SUPPLY CHAIN TRADE TRANSACTION**
 *
 * @ID BG-25-00
 * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction
 * @Profile MINIMUM
 */
export interface SupplyChainTradeTransaction {
  /**
   * **HEADER TRADE AGREEMENT**
   *
   * @ID BT-10-00
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement
   * @Profile MINIMUM
   */
  applicableHeaderTradeAgreement?: ApplicableHeaderTradeAgreement;

  /**
   * **DELIVERY INFORMATION** - A group of business terms providing information about where and when the goods and services invoiced are delivered.
   *
   * @ID BG-13-00
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeDelivery
   * @Profile MINIMUM
   */
  applicableHeaderTradeDelivery?: ApplicableHeaderTradeDelivery;

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
  applicableHeaderTradeSettlement?: ApplicableHeaderTradeSettlement;
}
