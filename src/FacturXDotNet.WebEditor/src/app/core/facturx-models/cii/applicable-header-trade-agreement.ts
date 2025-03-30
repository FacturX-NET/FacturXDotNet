import { SellerTradeParty } from './seller-trade-party';
import { BuyerTradeParty } from './buyer-trade-party';
import { BuyerOrderReferencedDocument } from './buyer-order-referenced-document';

/**
 * **HEADER TRADE AGREEMENT**
 *
 * @ID BT-10-00
 * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement
 * @Profile MINIMUM
 */
export interface ApplicableHeaderTradeAgreement {
  /**
   * **Buyer reference** - An identifier assigned by the Buyer used for internal routing purposes.
   *
   * @remarks The identifier is defined by the Buyer (e.g. contact ID, department, office id, project code), but provided by the Seller in the Invoice.
   *
   * @ID BT-10
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerReference
   * @Profile MINIMUM
   * @ChorusPro For the public sector, it is the "Service Exécutant". It is mandatory for some buyers. It must belong to the Chorus Pro repository. It is limited to 100 characters.
   */
  buyerReference?: string;

  /**
   * **SELLER** - A group of business terms providing information about the Seller.
   *
   * @ID BG-4
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty
   * @Profile MINIMUM
   */
  sellerTradeParty?: SellerTradeParty;

  /**
   * **BUYER** - A group of business terms providing information about the Buyer.
   *
   * @ID BG-7
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerTradeParty
   * @Profile MINIMUM
   */
  buyerTradeParty?: BuyerTradeParty;

  /**
   * **PURCHASE ORDER REFERENCE**
   *
   * @ID BT-13-00
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerOrderReferencedDocument
   * @Profile MINIMUM
   */
  buyerOrderReferencedDocument?: BuyerOrderReferencedDocument;
}
