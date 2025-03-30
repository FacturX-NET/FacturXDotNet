import { SellerTradePartySpecifiedLegalOrganization } from './seller-trade-party-specified-legal-organization';
import { SellerTradePartyPostalTradeAddress } from './seller-trade-party-postal-trade-address';
import { SellerTradePartySpecifiedTaxRegistration } from './seller-trade-party-specified-tax-registration';

/**
 * **SELLER** - A group of business terms providing information about the Seller.
 *
 * @ID BG-4
 * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty
 * @Profile MINIMUM
 */
export interface SellerTradeParty {
  /**
   * **Seller name** - The full formal name by which the Seller is registered in the national registry of legal entities or as a Taxable person or otherwise trades as a person
   * or persons.
   *
   * @ID BT-27
   * @BusinessRules **BR-6**: An Invoice shall contain the Seller name.
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:Name
   * @Profile MINIMUM
   */
  name?: string;

  /**
   * **SELLER LEGAL ORGANIZATION** - Details about the organization.
   *
   * @ID BT-30-00
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:SpecifiedLegalOrganization
   * @Profile MINIMUM
   */
  specifiedLegalOrganization?: SellerTradePartySpecifiedLegalOrganization;

  /**
   * **SELLER POSTAL ADDRESS** - A group of business terms providing information about the address of the Seller.
   *
   * @remarks
   * Sufficient components of the address are to be filled in order to comply to legal requirements.
   * Like any address, the fields necessary to define the address must appear. The country code is mandatory.
   *
   * @ID BG-5
   * @BusinessRules **BR-8**: An Invoice shall contain the Seller postal address.
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:PostalTradeAddress
   * @Profile MINIMUM
   */
  postalTradeAddress?: SellerTradePartyPostalTradeAddress;

  /**
   * **SELLER VAT IDENTIFIER** - Detailed information on tax information of the seller
   *
   * @ID BT-31-00
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:SpecifiedTaxRegistration
   * @Profile MINIMUM
   */
  specifiedTaxRegistration?: SellerTradePartySpecifiedTaxRegistration;
}
