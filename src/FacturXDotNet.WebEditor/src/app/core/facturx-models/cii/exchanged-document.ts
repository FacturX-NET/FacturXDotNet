import { InvoiceTypeCode } from './invoice-type-code';
import { DateOnly } from './date-only';
import { DateOnlyFormat } from './date-only-format';

/**
 * **EXCHANGE DOCUMENT**
 *
 * @ID BT-1-00
 * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:ExchangedDocument
 * @Profile MINIMUM
 */
export interface ExchangedDocument {
  /**
   * **Invoice number** - A unique identification of the Invoice.
   *
   * @remarks
   * The sequential number required in Article 226(2) of the directive 2006/112/EC, to uniquely identify the Invoice within the business context, time-frame, operating systems
   * and records of the Seller. It may be based on one or more series of numbers, which may include alphanumeric characters. No identification scheme is to be used.
   *
   * @ID BT-1
   * @BusinessRules **BR-2**: An Invoice shall have an Invoice number.
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:ExchangedDocument/ram:ID
   * @Profile MINIMUM
   * @ChorusPro The invoice number is limited to 20 characters.
   */
  id?: string;

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
  typeCode?: InvoiceTypeCode;

  /**
   * **Invoice issue date** - The date when the Invoice was issued.
   *
   * @ID BT-2
   * @BusinessRules **BR-3**: An Invoice shall have an Invoice issue date.
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:ExchangedDocument/ram:IssueDateTime/udt:DateTimeString
   * @Profile MINIMUM
   * @ChorusPro The issue date must be before or equal to the deposit date.
   */
  issueDateTime?: DateOnly;

  /**
   * **Date, format**
   *
   * @remarks Only value "102"
   *
   * @ID BT-2-0
   * @CiiXmlPath /rsm:CrossIndustryInvoice/rsm:ExchangedDocument/ram:IssueDateTime/udt:DateTimeString/@format
   * @Profile MINIMUM
   */
  issueDateTimeFormat?: DateOnlyFormat;
}
