interface CiiTerm {
  term: string;
  name: string;
  kind: 'group' | 'field';
  businessRules?: string[];
}

/**
 * The list of all the controls in the Cross-Industry Invoice form, with their name, term, description, business rules and remarks.
 */
export const ciiTerms: Record<string, CiiTerm> = {
  'BR-02': {
    term: 'BR-02',
    name: 'EXCHANGE DOCUMENT CONTEXT',
    kind: 'group',
  },
  'BT-23': {
    term: 'BT-23',
    name: 'Business process type',
    kind: 'field',
  },
  'BT-24': {
    term: 'BT-24',
    name: 'Specification identifier',
    kind: 'field',
    businessRules: ['BR-01'],
  },
  'BT-1-00': {
    term: 'BT-1-00',
    name: 'EXCHANGED DOCUMENT',
    kind: 'group',
  },
  'BT-1': {
    term: 'BT-1',
    name: 'Invoice number',
    kind: 'field',
    businessRules: ['BR-02'],
  },
  'BT-3': {
    term: 'BT-3',
    name: 'Invoice type code',
    kind: 'field',
    businessRules: ['BR-04'],
  },
  'BT-2': {
    term: 'BT-2',
    name: 'Invoice issue date',
    kind: 'field',
    businessRules: ['BR-03'],
  },
  'BT-2-0': {
    term: 'BT-2-0',
    name: 'Date, format',
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
    kind: 'field',
  },
  'BR-04': {
    term: 'BR-04',
    name: 'SELLER',
    kind: 'group',
  },
  'BT-27': {
    term: 'BT-27',
    name: 'Seller name',
    kind: 'field',
    businessRules: ['BR-06'],
  },
  'BT-30-00': {
    term: 'BT-30-00',
    name: 'SELLER LEGAL ORGANIZATION',
    kind: 'group',
  },
  'BT-30': {
    term: 'BT-30',
    name: 'Seller legal registration identifier',
    kind: 'field',
    businessRules: ['BR-CO-26'],
  },
  'BT-30-1': {
    term: 'BT-30-1',
    name: 'Scheme identifier',
    kind: 'field',
  },
  'BR-05': {
    term: 'BR-05',
    name: 'SELLER POSTAL ADDRESS',
    kind: 'group',
    businessRules: ['BR-08'],
  },
  'BT-40': {
    term: 'BT-40',
    name: 'Seller country code',
    kind: 'field',
    businessRules: ['BR-09'],
  },
  'BT-31-00': {
    term: 'BT-31-00',
    name: 'SELLER VAT IDENTIFIER',
    kind: 'group',
  },
  'BT-31': {
    term: 'BT-31',
    name: 'Seller VAT identifier',
    kind: 'field',
    businessRules: ['BR-CO-09', 'BR-CO-26'],
  },
  'BT-31-0': {
    term: 'BT-31-0',
    name: 'Tax Scheme identifier',
    kind: 'field',
  },
  'BR-07': {
    term: 'BR-07',
    name: 'BUYER',
    kind: 'group',
    businessRules: ['BR-07'],
  },
  'BT-44': {
    term: 'BT-44',
    name: 'Buyer name',
    kind: 'field',
  },
  'BT-47-00': {
    term: 'BT-47-00',
    name: 'BUYER LEGAL REGISTRATION IDENTIFIER',
    kind: 'group',
  },
  'BT-47': {
    term: 'BT-47',
    name: 'Buyer legal registration identifier',
    kind: 'field',
  },
  'BT-47-1': {
    term: 'BT-47-1',
    name: 'Scheme identifier',
    kind: 'field',
  },
  'BT-13-00': {
    term: 'BT-13-00',
    name: 'PURCHASE ORDER REFERENCE',
    kind: 'group',
  },
  'BT-13': {
    term: 'BT-13',
    name: 'Purchase order reference',
    kind: 'field',
  },
  'BG-13-00': {
    term: 'BG-13-00',
    name: 'DELIVERY INFORMATION',
    kind: 'group',
  },
  'BG-19': {
    term: 'BG-19',
    name: 'HEADER TRADE SETTLEMENT DIRECT DEBIT',
    kind: 'group',
  },
  'BT-5': {
    term: 'BT-5',
    name: 'Invoice currency code',
    kind: 'field',
    businessRules: ['BR-05'],
  },
  'BG-22': {
    term: 'BG-22',
    name: 'DOCUMENT TOTALS',
    kind: 'group',
  },
  'BT-109': {
    term: 'BT-109',
    name: 'Total amount without VAT',
    kind: 'field',
    businessRules: ['BR-13', 'BR-CO-13'],
  },
  'BT-110': {
    term: 'BT-110',
    name: 'Total VAT amount',
    kind: 'field',
    businessRules: ['BR-CO-14'],
  },
  'BT-110-1': {
    term: 'BT-110-1',
    name: 'VAT currency',
    kind: 'field',
  },
  'BT-112': {
    term: 'BT-112',
    name: 'Total amount with VAT',
    kind: 'field',
    businessRules: ['BR-14', 'BR-CO-15', 'BR-FXEXT-CO-15'],
  },
  'BT-115': {
    term: 'BT-115',
    name: 'Amount due for payment',
    kind: 'field',
    businessRules: ['BR-15', 'BR-CO-16'],
  },
};
