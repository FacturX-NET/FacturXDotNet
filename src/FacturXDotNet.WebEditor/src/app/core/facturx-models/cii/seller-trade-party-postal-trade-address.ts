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
export interface SellerTradePartyPostalTradeAddress {
  /**
   * **Seller country code** - A code that identifies the country.
   *
   * @remarks
   * If no tax representative is specified, this is the country where VAT is liable. The lists of valid countries are registered with the ISO 3166-1 Maintenance agency, "Codes for
   * the representation of names of countries and their subdivisions".
   *
   * @ID BT-40
   * @BusinessRules **BR-9**: The Seller postal address (BG-5) shall contain a Seller country code.
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:PostalTradeAddress/ram:CountryID
   * @Profile MINIMUM
   */
  countryId?: string;
}
