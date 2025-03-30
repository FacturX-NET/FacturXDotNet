/**
 * **SELLER LEGAL ORGANIZATION** - Details about the organization.
 *
 * @ID BT-30-00
 * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:SpecifiedLegalOrganization
 * @Profile MINIMUM
 */
export interface SellerTradePartySpecifiedLegalOrganization {
  /**
   * **Seller legal registration identifier** - An identifier issued by an official registrar that identifies the Seller as a legal entity or person.
   *
   * @remarks If no identification scheme is specified, it must be known by Buyer and Seller.
   *
   * @ID BT-30
   * @BusinessRules **BR-CO-26
   **:   * In order for the buyer to automatically identify a supplier, the Seller identifier (BT-29), the Seller legal registration identifier (BT-30)
   * and/or the Seller VAT identifier (BT-31) shall be present.
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:SpecifiedLegalOrganization/ram:ID
   * @Profile MINIMUM
   */
  id?: string;

  /**
   * **Scheme identifier** - The identification scheme identifier of the Seller legal registration identifier.
   *
   * @remarks
   * If used, the identification scheme shall be chosen from the entries of the list published by the ISO/IEC 6523 maintenance agency. For a SIREN or a SIRET, the value of this
   * field is "0002".
   *
   * @ID BT-30-1
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:SpecifiedLegalOrganization/ram:ID@schemeID
   * @Profile MINIMUM
   */
  idSchemeId?: string;
}
