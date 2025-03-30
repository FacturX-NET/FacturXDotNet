/**
 * **BUYER LEGAL REGISTRATION IDENTIFIER** - Details about the organization
 *
 * @ID BT-47-00
 * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerTradeParty/ram:SpecifiedLegalOrganization
 * @Profile MINIMUM
 */
export interface BuyerTradePartySpecifiedLegalOrganization {
  /**
   * **Buyer legal registration identifier** - An identifier issued by an official registrar that identifies the Buyer as a legal entity or person.
   *
   * @remarks
   * If no identification scheme is specified, it should be known by Buyer and Seller, e.g. the identifier that is exclusively used in the applicable legal environment.
   *
   * @ID BT-47
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerTradeParty/ram:SpecifiedLegalOrganization/ram:ID
   * @Profile MINIMUM
   * @ChorusPro The identifier of the buyer (public entity) is mandatory and is always a SIRET number.
   */
  id?: string;

  /**
   * **Scheme identifier** - The identification scheme identifier of the Buyer legal registration identifier.
   *
   * @remarks
   * If used, the identification scheme shall be chosen from the entries of the list published by the ISO 6523 maintenance agency. For a SIREN or a SIRET, the value of this field
   * is "0002".
   *
   * @ID BT-47-1
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerTradeParty/ram:SpecifiedLegalOrganization/ram:ID/@schemeID
   * @Profile MINIMUM
   */
  idSchemeId?: string;
}
