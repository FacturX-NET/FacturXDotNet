interface CiiTerm {
  term: string;
  name: string;
  description?: string;
  kind: 'group' | 'field';
  businessRules?: string[];
  remark?: string;
  chorusProRemark?: string;
}

/**
 * The list of all the controls in the Cross-Industry Invoice form, with their name, term, description, business rules and remarks.
 */
export const ciiTerms: Record<string, CiiTerm> = {
  'BR-02': {
    term: 'BR-02',
    name: 'EXCHANGE DOCUMENT CONTEXT',
    description: 'A group of business terms providing information on the business process and rules applicable to the Invoice document.',
    kind: 'group',
  },
  'BT-23': {
    term: 'BT-23',
    name: 'Business process type',
    description: 'Identifies the business process context in which the transaction appears, to enable the Buyer to process the Invoice in an appropriate way.',
    kind: 'field',
  },
  'BT-24': {
    term: 'BT-24',
    name: 'Specification identifier',
    description: `An identification of the specification containing the total set of rules regarding semantic content, cardinalities and business rules to which the data contained
      in the instance document conforms.`,
    kind: 'field',
    businessRules: ['BR-01'],
    remark: `This identifies compliance or conformance to the specification. Conformant invoices specify: urn:cen.eu:en16931:2017.
      Invoices, compliant to a user specification may identify that user specification here. No identification scheme is to be used.`,
  },
  'BT-1-00': {
    term: 'BT-1-00',
    name: 'EXCHANGED DOCUMENT',
    kind: 'group',
  },
  'BT-1': {
    term: 'BT-1',
    name: 'Invoice number',
    description: 'A unique identification of the Invoice.',
    kind: 'field',
    businessRules: ['BR-02'],
    remark: `The sequential number required in Article 226(2) of the directive 2006/112/EC, to uniquely identify the Invoice within the business context, time-frame,
       operating systems and records of the Seller.
       It may be based on one or more series of numbers, which may include alphanumeric characters. No identification scheme is to be used.`,
    chorusProRemark: 'The invoice number is limited to 20 characters.',
  },
  'BT-3': {
    term: 'BT-3',
    name: 'Invoice type code',
    description: 'A code specifying the functional type of the Invoice.',
    kind: 'field',
    businessRules: ['BR-04'],
    remark: `Commercial invoices and credit notes are defined according the entries in UNTDID 1001.
    Other entries of UNTDID 1001 with specific invoices or credit notes may be used if applicable.`,
    chorusProRemark: `The types of documents used are:
      - 380: Commercial Invoice
      - 381: Credit note
      - 384: Corrected invoice
      - 389: Self-billed invoice (created by the buyer on behalf of the supplier)
      - 261: Self billed credit note (not accepted by CHORUSPRO)
      - 386: Prepayment invoice
      - 751: Invoice information for accounting purposes (not accepted by CHORUSPRO)`,
  },
  'BT-2': {
    term: 'BT-2',
    name: 'Invoice issue date',
    description: 'The date when the Invoice was issued.',
    kind: 'field',
    businessRules: ['BR-03'],
    chorusProRemark: 'The issue date must be before or equal to the deposit date.',
  },
  'BT-2-0': {
    term: 'BT-2-0',
    name: 'Date, format',
    description: 'Only value "102"',
    kind: 'field',
  },
  'BG-25-00': {
    term: 'BG-25-00',
    name: 'SUPPLY CHAIN TRADE TRANSACTION',
    kind: 'group',
  },
  'BT-10-00': {
    term: 'BT-10-00',
    name: 'HEADER TRADE AGREEMENT',
    kind: 'group',
  },
  'BT-10': {
    term: 'BT-10',
    name: 'Buyer reference',
    description: 'An identifier assigned by the Buyer used for internal routing purposes.',
    kind: 'field',
    remark: 'The identifier is defined by the Buyer (e.g. contact ID, department, office id, project code), but provided by the Seller in the Invoice.',
    chorusProRemark: 'The invoice number is limited to 20 characters.',
  },
  'BR-04': {
    term: 'BR-04',
    name: 'SELLER',
    description: 'An identifier assigned by the Buyer used for internal routing purposes.',
    kind: 'group',
  },
  'BT-27': {
    term: 'BT-27',
    name: 'Seller name',
    description:
      'The full formal name by which the Seller is registered in the national registry of legal entities or as a Taxable person or otherwise trades as a person or persons.',
    kind: 'field',
    businessRules: ['BR-06'],
  },
  'BT-30-00': {
    term: 'BT-30-00',
    name: 'SELLER LEGAL ORGANIZATION',
    description: 'Details about the organization.',
    kind: 'group',
  },
  'BT-30': {
    term: 'BT-30',
    name: 'Seller legal registration identifier',
    description: 'An identifier issued by an official registrar that identifies the Seller as a legal entity or person.',
    kind: 'field',
    businessRules: ['BR-CO-26'],
  },
  'BT-30-1': {
    term: 'BT-30-1',
    name: 'Scheme identifier',
    description: 'The identification scheme identifier of the Seller legal registration identifier.',
    kind: 'field',
    remark: `If used, the identification scheme shall be chosen from the entries of the list published by the ISO/IEC 6523 maintenance agency.
    For a SIREN or a SIRET, the value of this field is "0002".`,
  },
  'BR-05': {
    term: 'BR-05',
    name: 'SELLER POSTAL ADDRESS',
    description: 'Details about the organization.',
    kind: 'group',
    businessRules: ['BR-08'],
    remark: `Sufficient components of the address are to be filled in order to comply to legal requirements.
    Like any address, the fields necessary to define the address must appear. The country code is mandatory.`,
  },
  'BT-40': {
    term: 'BT-40',
    name: 'Seller country code',
    description: 'A code that identifies the country.',
    kind: 'field',
    businessRules: ['BR-09'],
    remark: `If no tax representative is specified, this is the country where VAT is liable.
    The lists of valid countries are registered with the ISO 3166-1 Maintenance agency, "Codes for the representation of names of countries and their subdivisions".`,
  },
  'BT-31-00': {
    term: 'BT-31-00',
    name: 'SELLER VAT IDENTIFIER',
    description: 'Details about the organization.',
    kind: 'group',
  },
  'BT-31': {
    term: 'BT-31',
    name: 'Seller VAT identifier',
    description: "The Seller's VAT identifier (also known as Seller VAT identification number).",
    kind: 'field',
    businessRules: ['BR-CO-09', 'BR-CO-26'],
    remark: 'VAT number prefixed by a country code. A VAT registered Supplier shall include his VAT ID, except when he uses a tax representative.',
  },
  'BT-31-0': {
    term: 'BT-31-0',
    name: 'Tax Scheme identifier',
    description: 'Scheme identifier for supplier VAT identifier.',
    kind: 'field',
    remark: 'Value = VAT',
  },
  'BR-07': {
    term: 'BR-07',
    name: 'BUYER',
    description: 'An identifier assigned by the Buyer used for internal routing purposes.',
    kind: 'group',
    businessRules: ['BR-07'],
  },
  'BT-44': {
    term: 'BT-44',
    name: 'Buyer name',
    description: 'The full name of the Buyer.',
    kind: 'field',
  },
  'BT-47-00': {
    term: 'BT-47-00',
    name: 'BUYER LEGAL REGISTRATION IDENTIFIER',
    description: 'Details about the organization.',
    kind: 'group',
  },
  'BT-47': {
    term: 'BT-47',
    name: 'Buyer legal registration identifier',
    description: 'An identifier issued by an official registrar that identifies the Buyer as a legal entity or person.',
    kind: 'field',
    remark: 'If no identification scheme is specified, it should be known by Buyer and Seller, e.g. the identifier that is exclusively used in the applicable legal environment.',
    chorusProRemark: 'The identifier of the buyer (public entity) is mandatory and is always a SIRET number.',
  },
  'BT-47-1': {
    term: 'BT-47-1',
    name: 'Scheme identifier',
    description: 'The identification scheme identifier of the Buyer legal registration identifier.',
    kind: 'field',
    remark: 'The identification scheme identifier of the Buyer legal registration identifier.',
  },
  'BT-13-00': {
    term: 'BT-13-00',
    name: 'PURCHASE ORDER REFERENCE',
    kind: 'group',
  },
  'BT-13': {
    term: 'BT-13',
    name: 'Purchase order reference',
    description: 'An identifier of a referenced purchase order, issued by the Buyer.',
    kind: 'field',
    chorusProRemark: `For the public sector, this is the "Engagement Juridique" (Legal Commitment).
    It is mandatory for some buyers. You should refer to the ChorusPro Directory to identify these public entity buyers that make it mandatory.`,
  },
  'BG-13-00': {
    term: 'BG-13-00',
    name: 'DELIVERY INFORMATION',
    description: 'A group of business terms providing information about where and when the goods and services invoiced are delivered.',
    kind: 'group',
  },
  'BG-19': {
    term: 'BG-19',
    name: 'HEADER TRADE SETTLEMENT DIRECT DEBIT',
    description: 'A group of business terms providing information about where and when the goods and services invoiced are delivered.',
    kind: 'group',
    remark: `This group may be used to give prior notice in the invoice that payment will be made through a SEPA or other direct debit initiated by the Seller,
    in accordance with the rules of the SEPA or other direct debit scheme.`,
    chorusProRemark: 'Not used.',
  },
  'BT-5': {
    term: 'BT-5',
    name: 'Invoice currency code',
    description: 'The currency in which all Invoice amounts are given, except for the Total VAT amount in accounting currency.',
    kind: 'field',
    businessRules: ['BR-05'],
    remark: `Only one currency shall be used in the Invoice, except for the Total VAT amount in accounting currency (BT-111) in accordance with article 230 of Directive 2006/112/EC on VAT.
    The lists of valid currencies are registered with the ISO 4217 Maintenance Agency "Codes for the representation of currencies and funds".`,
    chorusProRemark: 'Invoices and credit notes or Chorus Pro are mono-currencies only.',
  },
  'BG-22': {
    term: 'BG-22',
    name: 'DOCUMENT TOTALS',
    description: 'The currency in which all Invoice amounts are given, except for the Total VAT amount in accounting currency.',
    kind: 'group',
    remark: `Only one currency shall be used in the Invoice, except for the Total VAT amount in accounting currency (BT-111) in accordance with article 230 of Directive 2006/112/EC on VAT.
    The lists of valid currencies are registered with the ISO 4217 Maintenance Agency "Codes for the representation of currencies and funds".`,
    chorusProRemark: 'Invoices and credit notes or Chorus Pro are mono-currencies only.',
  },
  'BT-109': {
    term: 'BT-109',
    name: 'Total amount without VAT',
    description: 'The total amount of the Invoice without VAT.',
    kind: 'field',
    businessRules: ['BR-13', 'BR-CO-13'],
    remark: 'The Invoice total amount without VAT is the Sum of Invoice line net amount minus Sum of allowances on document level plus Sum of charges on document level.',
  },
  'BT-110': {
    term: 'BT-110',
    name: 'Total VAT amount',
    description: 'The total VAT amount for the Invoice.',
    kind: 'field',
    businessRules: ['BR-CO-14'],
    remark: 'The Invoice total VAT amount is the sum of all VAT category tax amounts.',
  },
  'BT-110-1': {
    term: 'BT-110-1',
    name: 'VAT currency',
    kind: 'field',
    remark: 'The currency is mandatory to differentiate between VAT amount and VAT amount in accounting currency.',
  },
  'BT-112': {
    term: 'BT-112',
    name: 'Total amount with VAT',
    description: 'The total amount of the Invoice with VAT.',
    kind: 'field',
    businessRules: ['BR-14', 'BR-CO-15', 'BR-FXEXT-CO-15'],
    remark: 'The Invoice total amount with VAT is the Invoice total amount without VAT plus the Invoice total VAT amount.',
  },
  'BT-115': {
    term: 'BT-115',
    name: 'Amount due for payment',
    description: 'The outstanding amount that is requested to be paid.',
    kind: 'field',
    businessRules: ['BR-15', 'BR-CO-16'],
    remark:
      'This amount is the Invoice total amount with VAT minus the paid amount that has been paid in advance. ' +
      'The amount is zero in case of a fully paid Invoice. The amount may be negative; in that case the Seller owes the amount to the Buyer.',
  },
};
