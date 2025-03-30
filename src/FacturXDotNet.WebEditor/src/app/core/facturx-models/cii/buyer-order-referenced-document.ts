/**
 * **PURCHASE ORDER REFERENCE**
 *
 * @ID BT-13-00
 * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerOrderReferencedDocument
 * @Profile MINIMUM
 */
export interface BuyerOrderReferencedDocument {
  /**
   * **Purchase order reference** - An identifier of a referenced purchase order, issued by the Buyer.
   *
   * @ID BT-13
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerOrderReferencedDocument/ram:IssuerAssignedID
   * @Profile MINIMUM
   * @ChorusPro
   * For the public sector, this is the "Engagement Juridique" (Legal Commitment). It is mandatory for some buyers. You should refer to the ChorusPro Directory
   * to identify these public entity buyers that make it mandatory.
   */
  issuerAssignedId?: string;
}
