import {VatOnlyTaxSchemeIdentifier} from "./vat-only-tax-scheme-identifier";

/**
 * **SELLER VAT IDENTIFIER** - Detailed information on tax information of the seller
 *
 * @ID BT-31-00
 * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:SpecifiedTaxRegistration
 * @Profile MINIMUM
 */
export interface SellerTradePartySpecifiedTaxRegistration {
  /**
   * **Seller VAT identifier** - The Seller's VAT identifier (also known as Seller VAT identification number).
   *
   * @remarks VAT number prefixed by a country code. A VAT registered Supplier shall include his VAT ID, except when he uses a tax representative.
   *
   * @ID BT-31
   * @BusinessRules
   * - **BR-CO-9**:
   *     The Seller VAT identifier, the Seller tax representative VAT identifier (BT-63) and the Buyer VAT identifier (BT-48) shall have a prefix in accordance with ISO
   *     code ISO 3166-1 alpha-2 by which the country of issue may be identified. Nevertheless, Greece may use the prefix ‘EL’.
   * - **BR-CO-26**:
   *     In order for the buyer to automatically identify a supplier, the Seller identifier (BT-29), the Seller legal registration identifier (BT-30) and/or the Seller VAT
   *     identifier shall be present.
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:SpecifiedTaxRegistration/ram:ID
   * @Profile MINIMUM
   */
  id?: string;

  /**
   * **Tax Scheme identifier** - Scheme identifier for supplier VAT identifier.
   *
   * @remarks Value = VA
   *
   * @ID BT-31-0
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:SpecifiedTaxRegistration/ram:ID/@schemeID
   * @Profile MINIMUM
   */
  idSchemeId?: VatOnlyTaxSchemeIdentifier;
}
