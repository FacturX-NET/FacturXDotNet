import { BusinessTermIdentifier } from './cii-terms';

export type BusinessRuleIdentifier = keyof typeof ciiRules;

export interface CiiRule {
  id: BusinessRuleIdentifier;
  description: string;
  termsInvolved: readonly BusinessTermIdentifier[];
}

export function getBusinessRuleIdentifiers(): BusinessRuleIdentifier[] {
  return Object.keys(ciiRules) as BusinessRuleIdentifier[];
}

export function isBusinessRuleIdentifier(id: string): id is BusinessRuleIdentifier {
  return Object.keys(ciiRules).includes(id);
}

export function getBusinessRule(id: string): CiiRule | undefined {
  if (!isBusinessRuleIdentifier(id)) {
    return undefined;
  }

  return ciiRules[id];
}

export function requireBusinessRule(rule: BusinessRuleIdentifier): CiiRule {
  const result = ciiRules[rule];
  if (result === undefined) {
    throw new Error(`Could not find rule ${rule}.`);
  }

  return result;
}

export const ciiRules = {
  'BR-1': { id: 'BR-1', description: 'An Invoice shall have a Specification identifier (BT-24).', termsInvolved: ['BT-24'] },
  'BR-2': { id: 'BR-2', description: 'An Invoice shall have an Invoice number (BT-1).', termsInvolved: ['BT-1'] },
  'BR-3': { id: 'BR-3', description: 'An Invoice shall have an Invoice issue date (BT-2).', termsInvolved: ['BT-2'] },
  'BR-4': { id: 'BR-4', description: 'An Invoice shall have an Invoice type code (BT-3).', termsInvolved: ['BT-3'] },
  'BR-5': { id: 'BR-5', description: 'An Invoice shall have an Invoice currency code (BT-5).', termsInvolved: ['BT-5'] },
  'BR-6': { id: 'BR-6', description: 'An Invoice shall contain the Seller name (BT-27).', termsInvolved: ['BT-27'] },
  'BR-7': { id: 'BR-7', description: 'An Invoice shall contain the Buyer name (BT-44).', termsInvolved: ['BT-44'] },
  'BR-8': { id: 'BR-8', description: 'An Invoice shall contain the Seller postal address (BG-5).', termsInvolved: ['BG-5'] },
  'BR-9': { id: 'BR-9', description: 'The Seller postal address (BG-5) shall contain a Seller country code (BT-40).', termsInvolved: ['BT-40'] },
  'BR-13': { id: 'BR-13', description: 'An Invoice shall have the Invoice total amount without VAT (BT-109).', termsInvolved: ['BT-109'] },
  'BR-14': { id: 'BR-14', description: 'An Invoice shall have the Invoice total amount with VAT (BT-112).', termsInvolved: ['BT-112'] },
  'BR-15': { id: 'BR-15', description: 'An Invoice shall have the Amount due for payment (BT- 115).', termsInvolved: ['BT-115'] },
  'BR-CO-9': {
    id: 'BR-CO-9',
    description:
      'The Seller VAT identifier (BT-31), the Seller tax representative VAT identifier (BT-63) and the Buyer VAT identifier (BT-48) shall have a prefix in accordance \nwith ISO code ISO 3166-1 alpha-2 by which the country of issue may be identified. Nevertheless, Greece may use the prefix ‘EL’.',
    termsInvolved: ['BT-31'],
  },
  'BR-CO-13': {
    id: 'BR-CO-13',
    description:
      'Invoice total amount without VAT (BT-109) = ∑ Invoice line net amount (BT-131) - Sum of allowances on document level (BT-107) + Sum of charges on document level (BT-108)',
    termsInvolved: [],
  },
  'BR-CO-14': {
    id: 'BR-CO-14',
    description: 'Invoice total VAT amount (BT-110) = ∑ VAT category tax amount (BT-117)',
    termsInvolved: [],
  },
  'BR-CO-15': {
    id: 'BR-CO-15',
    description: 'Invoice total amount with VAT (BT-112) = Invoice total amount without VAT (BT-109) + Invoice total VAT amount (BT-110)',
    termsInvolved: [],
  },
  'BR-FXEXT-CO-15': {
    id: 'BR-FXEXT-CO-15',
    description:
      'For EXTENDED profile only, BR-CO-15 is replaced by BR-FXEXT-CO-15, which add a tolerance of 0,01 euro per line, document level charge and allowance in calculation.',
    termsInvolved: [],
  },
  'BR-CO-16': {
    id: 'BR-CO-16',
    description: 'Amount due for payment (BT-115) = Invoice total amount with VAT (BT-112) - Paid amount (BT-113) + Rounding amount (BT-114)',
    termsInvolved: [],
  },
  'BR-CO-26': {
    id: 'BR-CO-26',
    description:
      'In order for the buyer to automatically identify a supplier, the Seller identifier (BT-29), the Seller legal registration identifier (BT-30) \nand/or the Seller VAT identifier (BT-31) shall be present.',
    termsInvolved: ['BT-30', 'BT-31'],
  },
  'BR-DEC-12': { id: 'BR-DEC-12', description: 'The allowed maximum number of decimals for the Invoice total amount without VAT (BT-109) is 2.', termsInvolved: ['BT-109'] },
  'BR-DEC-13': { id: 'BR-DEC-13', description: 'The allowed maximum number of decimals for the Invoice total VAT amount (BT-110) is 2.', termsInvolved: ['BT-110'] },
  'BR-DEC-14': { id: 'BR-DEC-14', description: 'The allowed maximum number of decimals for the Invoice total amount with VAT (BT-112) is 2.', termsInvolved: ['BT-112'] },
  'BR-DEC-18': { id: 'BR-DEC-18', description: 'The allowed maximum number of decimals for the Amount due for payment (BT-115) is 2.', termsInvolved: ['BT-115'] },
  'BR-S-01': {
    id: 'BR-S-01',
    description:
      'An Invoice that contains an Invoice line (BG-25), a Document level allowance (BG-20) or a Document level charge (BG-21) where the VAT category code (BT-151, BT-95 or BT-102) \nis “Standard rated” shall contain in the VAT breakdown (BG-23) at least one VAT category code (BT-118) equal with "Standard rated".',
    termsInvolved: [],
  },
} as const;
