export type BusinessRuleIdentifier = keyof typeof ciiBusinessRules;

export interface BusinessRule {
  name: BusinessRuleIdentifier;
  description: string;
}

export function getRule(rule: string): BusinessRule | undefined {
  const key = rule as BusinessRuleIdentifier;
  return ciiBusinessRules[key];
}

export function requireRule(rule: BusinessRuleIdentifier): BusinessRule {
  const result = ciiBusinessRules[rule];
  if (result === undefined) {
    throw new Error(`Could not find rule ${rule}.`);
  }

  return result;
}

export const ciiBusinessRules = {
  'BR-01': { name: 'BR-01', description: 'An Invoice shall have a Specification identifier (BT-24).' },
  'BR-02': { name: 'BR-02', description: 'An Invoice shall have an Invoice number (BT-1).' },
  'BR-03': { name: 'BR-03', description: 'An Invoice shall have an Invoice issue date (BT-2).' },
  'BR-04': { name: 'BR-04', description: 'An Invoice shall have an Invoice type code (BT-3).' },
  'BR-05': { name: 'BR-05', description: 'An Invoice shall have an Invoice currency code (BT-5).' },
  'BR-06': { name: 'BR-06', description: 'An Invoice shall contain the Seller name (BT-27).' },
  'BR-07': { name: 'BR-07', description: 'An Invoice shall contain the Buyer name (BT-44).' },
  'BR-08': { name: 'BR-08', description: 'An Invoice shall contain the Seller postal address (BR-05).' },
  'BR-09': { name: 'BR-09', description: 'The Seller postal address (BR-05) shall contain a Seller country code (BT-40).' },
  'BR-13': { name: 'BR-13', description: 'An Invoice shall have the Invoice total amount without VAT (BT-109).' },
  'BR-14': { name: 'BR-14', description: 'An Invoice shall have the Invoice total amount with VAT (BT-112).' },
  'BR-15': { name: 'BR-15', description: 'An Invoice shall have the Amount due for payment (BT- 115).' },
  'BR-CO-09': {
    name: 'BR-CO-09',
    description:
      'The Seller VAT identifier (BT-31), the Seller tax representative VAT identifier (BT-63) and the Buyer VAT identifier (BT-48) shall have a prefix in accordance with ISO code ISO 3166-1 alpha-2 by which the country of issue may be identified. Nevertheless, Greece may use the prefix ‘EL’.',
  },
  'BR-CO-13': {
    name: 'BR-CO-13',
    description:
      'Invoice total amount without VAT (BT-109) = ∑ Invoice line net amount (BT-131) - Sum of allowances on document level (BT-107) + Sum of charges on document level (BT-108)',
  },
  'BR-CO-14': {
    name: 'BR-CO-14',
    description: ' Invoice total VAT amount (BT-110) = ∑ VAT category tax amount (BT-117)',
  },
  'BR-CO-15': {
    name: 'BR-CO-15',
    description: 'Invoice total amount with VAT (BT-112) = Invoice total amount without VAT (BT-109) + Invoice total VAT amount (BT-110)',
  },
  'BR-CO-16': {
    name: 'BR-CO-16',
    description: 'Amount due for payment (BT-115) = Invoice total amount with VAT (BT-112) - Paid amount (BT-113) + Rounding amount (BT-114)',
  },
  'BR-CO-26': {
    name: 'BR-CO-26',
    description:
      'In order for the buyer to automatically identify a supplier, the Seller identifier (BT-29), the Seller legal registration identifier (BT-30) and/or the Seller VAT identifier (BT-31) shall be present.',
  },
  'BR-DEC-12': { name: 'BR-DEC-12', description: 'The allowed maximum number of decimals for the Invoice total amount without VAT (BT-109) is 2.' },
  'BR-DEC-13': { name: 'BR-DEC-13', description: 'The allowed maximum number of decimals for the Invoice total VAT amount (BT-110) is 2.' },
  'BR-DEC-14': { name: 'BR-DEC-14', description: 'The allowed maximum number of decimals for the Invoice total amount with VAT (BT-112) is 2.' },
  'BR-DEC-18': { name: 'BR-DEC-18', description: 'The allowed maximum number of decimals for the Amount due for payment (BT-115) is 2.' },
  'BR-S-01': {
    name: 'BR-S-01',
    description:
      'An Invoice that contains an Invoice line (BG-25), a Document level allowance (BG-20) or a Document level charge (BG-21) where the VAT category code (BT-151, BT-95 or BT-102) is “Standard rated” shall contain in the VAT breakdown (BG-23) at least one VAT category code (BT-118) equal with "Standard rated".',
  },
  'BR-FXEXT-CO-15': {
    name: 'BR-FXEXT-CO-15',
    description:
      'For EXTENDED profile only, BR-CO-15 is replaced by BR-FXEXT-CO-15, which add a tolerance of 0,01 euro per line, document level charge and allowance in calculation.',
  },
} as const;
