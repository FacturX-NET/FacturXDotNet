import { ExchangedDocumentContext } from './exchanged-document-context';
import { ExchangedDocument } from './exchanged-document';
import { SupplyChainTradeTransaction } from './supply-chain-trade-transaction';

/**
 * An invoice in the Factur-X format.
 * This class represents invoices in any profile of the Factur-X format. To that end, the nullability of the properties is determined by the MINIMUM profile.
 */
export interface CrossIndustryInvoice {
  /**
   * **EXCHANGE DOCUMENT CONTEXT** - A group of business terms providing information on the business process and rules applicable to the Invoice document.
   *
   * @ID BG-2
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:ExchangedDocumentContext
   * @Profile MINIMUM
   */
  exchangedDocumentContext?: ExchangedDocumentContext;

  /**
   * **EXCHANGE DOCUMENT**
   *
   * @ID BT-1-00
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:ExchangedDocument
   * @Profile MINIMUM
   */
  exchangedDocument?: ExchangedDocument;

  /**
   * **SUPPLY CHAIN TRADE TRANSACTION**
   *
   * @ID BG-25-00
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction
   * @Profile MINIMUM
   */
  supplyChainTradeTransaction?: SupplyChainTradeTransaction;
}
