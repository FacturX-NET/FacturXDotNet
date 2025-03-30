import { BuyerTradePartySpecifiedLegalOrganization } from './buyer-trade-party-specified-legal-organization';

/**
 * **BUYER** - A group of business terms providing information about the Buyer.
 *
 * @ID BG-7
 * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerTradeParty
 * @Profile MINIMUM
 */
export interface BuyerTradeParty {
  /**
   * **Buyer name** - The full name of the Buyer.
   *
   * @ID BT-44
   * @BusinessRules **BR-7**: An Invoice shall contain the Buyer name.
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerTradeParty/ram:Name
   * @Profile MINIMUM
   * @ChorusPro This field is limited to 99 characters.
   */
  name?: string;

  /**
   * **BUYER LEGAL REGISTRATION IDENTIFIER** - Details about the organization
   *
   * @ID BT-47-00
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:BuyerTradeParty/ram:SpecifiedLegalOrganization
   * @Profile MINIMUM
   */
  specifiedLegalOrganization?: BuyerTradePartySpecifiedLegalOrganization;
}
