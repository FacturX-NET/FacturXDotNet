/**
 * **Invoice type code** - A code specifying the functional type of the Invoice.
 *
 * @remarks
 * Commercial invoices and credit notes are defined according the entries in UNTDID 1001. Other  entries of UNTDID 1001 with specific invoices or credit notes may be used if
 * applicable.
 *
 * @ID BT-3
 * @BusinessRules **BR-4**: An Invoice shall have an Invoice type code.
 * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:ExchangedDocument/ram:TypeCode
 * @Profile MINIMUM
 * @ChorusPro The types of documents used are:
 *     - 380: Commercial Invoice
 *     - 381: Credit note
 *     - 384: Corrected invoice
 *     - 389: Self-billed invoice (created by the buyer on behalf of the supplier)
 *     - 261: Self billed credit note (not accepted by CHORUSPRO)
 *     - 386: Prepayment invoice
 *     - 751: Invoice information for accounting purposes (not accepted by CHORUSPRO)
 */
export type InvoiceTypeCode =
  | '71-request-for-payment'
  | '80-debit-note-related-to-goods-or-services'
  | '81-credit-note-related-to-goods-or-services'
  | '82-metered-services-invoice'
  | '83-credit-note-related-to-financial-adjustments'
  | '84-debit-note-related-to-financial-adjustments'
  | '102-tax-notification'
  | '130-invoicing-data-sheet'
  | '202-direct-payment-valuation'
  | '203-provisional-payment-valuation'
  | '204-payment-valuation'
  | '211-interim-application-for-payment'
  | '218-final-payment-request-based-on-completion-of-work'
  | '219-payment-request-for-completed-units'
  | '261-self-billed-credit-note'
  | '262-consolidated-credit-note---goods-and-services'
  | '295-price-variation-invoice'
  | '296-credit-note-for-price-variation'
  | '308-delcredere-credit-note'
  | '325-proforma-invoice'
  | '326-partial-invoice'
  | '331-commercial-invoice-which-includes-a-packing-list'
  | '380-commercial-invoice'
  | '381-credit-note'
  | '382-commission-note'
  | '383-debit-note'
  | '384-corrected-invoice'
  | '385-consolidated-invoice'
  | '386-prepayment-invoice'
  | '387-hire-invoice'
  | '388-tax-invoice'
  | '389-self-billed-invoice'
  | '390-delcredere-invoice'
  | '393-factored-invoice'
  | '394-lease-invoice'
  | '395-consignment-invoice'
  | '396-factored-credit-note'
  | '420-optical-character-reading-(ocr)-payment-credit-note'
  | '456-debit-advice'
  | '457-reversal-of-debit'
  | '458-reversal-of-credit'
  | '527-self-billed-debit-note'
  | '532-forwarders-credit-note'
  | '553-forwarders-invoice-discrepancy-report'
  | '575-insurers-invoice'
  | '623-forwarders-invoice'
  | '633-port-charges-documents'
  | '751-invoice-information-for-accounting-purposes'
  | '780-freight-invoice'
  | '817-claim-notification'
  | '870-consular-invoice'
  | '875-partial-construction-invoice'
  | '876-partial-final-construction-invoice'
  | '877-final-construction-invoice'
  | '935-customs-invoice';
