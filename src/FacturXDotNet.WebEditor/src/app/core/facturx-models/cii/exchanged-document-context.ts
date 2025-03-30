import { GuidelineSpecifiedDocumentContextParameterId } from './guideline-specified-document-context-parameter-id';

/**
 * **EXCHANGE DOCUMENT CONTEXT** - A group of business terms providing information on the business process and rules applicable to the Invoice document.
 *
 * @ID BG-2
 * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:ExchangedDocumentContext
 * @Profile MINIMUM
 */
export interface ExchangedDocumentContext {
  /**
   * **Business process type** - Identifies the business process context in which the transaction appears, to enable the Buyer to process the Invoice in an appropriate way.
   *
   * @ID BT-23
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:ExchangedDocumentContext/ram:BusinessProcessSpecifiedDocumentContextParameter/ram:ID
   * @Profile MINIMUM
   */
  businessProcessSpecifiedDocumentContextParameterId?: string;

  /**
   * **Specification identifier** - An identification of the specification containing the total set of rules regarding semantic content, cardinalities and business rules to
   * which the data contained in the instance document conforms.
   *
   * @remarks
   * This identifies compliance or conformance to the specification. Conformant invoices specify: urn:cen.eu:en16931:2017. Invoices, compliant to a user specification may identify
   * that user specification here. No identification scheme is to be used.
   *
   * @ID BT-24
   * @BusinessRules **BR-1**: An Invoice shall have a Specification identifier.
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:ExchangedDocumentContext/ram:GuidelineSpecifiedDocumentContextParameter/ram:ID
   * @Profile MINIMUM
   */
  guidelineSpecifiedDocumentContextParameterId?: GuidelineSpecifiedDocumentContextParameterId;
}
