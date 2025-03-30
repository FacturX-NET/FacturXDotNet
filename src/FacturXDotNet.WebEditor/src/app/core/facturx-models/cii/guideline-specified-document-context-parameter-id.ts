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
export type GuidelineSpecifiedDocumentContextParameterId = 'minimum' | 'basic-wl' | 'basic' | 'en16931' | 'extended';
