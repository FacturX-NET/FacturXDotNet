import { Injectable } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DateOnlyFormat, GuidelineSpecifiedDocumentContextParameterId, InvoiceTypeCode, VatOnlyTaxSchemeIdentifier } from '../../../../core/api/api.models';

@Injectable({
  providedIn: 'root',
})
export class CiiFormService {
  validate() {
    this.form.markAllAsTouched();
    return this.form.valid;
  }

  form = new FormGroup({
    exchangedDocumentContext: new FormGroup({
      businessProcessSpecifiedDocumentContextParameterId: new FormControl('', { nonNullable: true }),
      guidelineSpecifiedDocumentContextParameterId: new FormControl<GuidelineSpecifiedDocumentContextParameterId | undefined>(undefined, {
        nonNullable: true,
        validators: [Validators.required],
      }),
    }),
    exchangedDocument: new FormGroup({
      id: new FormControl('', { nonNullable: true }),
      typeCode: new FormControl<InvoiceTypeCode | undefined>(undefined, { nonNullable: true, validators: [Validators.required] }),
      issueDateTime: new FormControl<Date | undefined>(undefined, { nonNullable: true }),
      issueDateTimeFormat: new FormControl<DateOnlyFormat | undefined>(undefined, { nonNullable: true, validators: [Validators.required] }),
    }),
    supplyChainTradeTransaction: new FormGroup({
      applicableHeaderTradeAgreement: new FormGroup({
        buyerReference: new FormControl('', { nonNullable: true }),
        sellerTradeParty: new FormGroup({
          name: new FormControl('', { nonNullable: true }),
          specifiedLegalOrganization: new FormGroup({
            id: new FormControl<string>('', { nonNullable: true }),
            idSchemeId: new FormControl<string>('', { nonNullable: true }),
          }),
          postalTradeAddress: new FormGroup({
            countryId: new FormControl<string>('', { nonNullable: true }),
          }),
          specifiedTaxRegistration: new FormGroup({
            id: new FormControl<string>('', { nonNullable: true }),
            idSchemeId: new FormControl<VatOnlyTaxSchemeIdentifier | undefined>(undefined, { nonNullable: true, validators: [Validators.required] }),
          }),
        }),
        buyerTradeParty: new FormGroup({
          name: new FormControl<string>('', { nonNullable: true }),
          specifiedLegalOrganization: new FormGroup({
            id: new FormControl<string>('', { nonNullable: true }),
            idSchemeId: new FormControl<string>('', { nonNullable: true }),
          }),
        }),
        buyerOrderReferencedDocument: new FormGroup({
          issuerAssignedId: new FormControl<string>('', { nonNullable: true }),
        }),
      }),
      applicableHeaderTradeDelivery: new FormGroup({}),
      applicableHeaderTradeSettlement: new FormGroup({
        invoiceCurrencyCode: new FormControl<string>('', { nonNullable: true }),
        specifiedTradeSettlementHeaderMonetarySummation: new FormGroup({
          taxBasisTotalAmount: new FormControl<number | undefined>(undefined, { nonNullable: true }),
          taxTotalAmount: new FormControl<number | undefined>(undefined, { nonNullable: true }),
          taxTotalAmountCurrencyId: new FormControl<string>('', { nonNullable: true }),
          grandTotalAmount: new FormControl<number | undefined>(undefined, { nonNullable: true }),
          duePayableAmount: new FormControl<number | undefined>(undefined, { nonNullable: true }),
        }),
      }),
    }),
  });

  /**
   * The list of all the controls in the Cross-Industry Invoice form, with their name, term, description, business rules and remarks.
   */
  doc: CiiFormControl[] = [
    {
      term: 'BG-2',
      name: 'EXCHANGE DOCUMENT CONTEXT',
      kind: 'group',
      control: this.form.controls.exchangedDocumentContext,
      children: [
        {
          term: 'BT-23',
          name: 'Business process type',
          kind: 'field',
          control: this.form.controls.exchangedDocumentContext.controls.businessProcessSpecifiedDocumentContextParameterId,
        },
        {
          term: 'BT-24',
          name: 'Specification identifier',
          kind: 'field',
          control: this.form.controls.exchangedDocumentContext.controls.guidelineSpecifiedDocumentContextParameterId,
          businessRules: ['BR-1'],
          hasRemarks: true,
        },
      ],
    },
    {
      term: 'BT-1-00',
      name: 'EXCHANGED DOCUMENT',
      kind: 'group',
      control: this.form.controls.exchangedDocument,
      children: [
        {
          term: 'BT-1',
          name: 'Invoice number',
          kind: 'field',
          control: this.form.controls.exchangedDocument.controls.id,
          businessRules: ['BR-2'],
          hasRemarks: true,
          hasChorusProRemarks: true,
        },
        {
          term: 'BT-3',
          name: 'Invoice type code',
          kind: 'field',
          control: this.form.controls.exchangedDocument.controls.typeCode,
          businessRules: ['BR-4'],
          hasRemarks: true,
          hasChorusProRemarks: true,
        },
        {
          term: 'BT-2',
          name: 'Invoice issue date',
          kind: 'field',
          control: this.form.controls.exchangedDocument.controls.issueDateTime,
          businessRules: ['BR-3'],
          hasChorusProRemarks: true,
          children: [
            {
              term: 'BT-2-0',
              name: 'Date, format',
              kind: 'field',
              control: this.form.controls.exchangedDocument.controls.issueDateTimeFormat,
            },
          ],
        },
      ],
    },
    {
      term: 'BG-25-00',
      name: 'SUPPLY CHAIN TRADE TRANSACTION',
      kind: 'group',
      control: this.form.controls.supplyChainTradeTransaction,
      children: [
        {
          term: 'BT-10-00',
          name: 'HEADER TRADE AGREEMENT',
          kind: 'group',
          control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement,
          children: [
            {
              term: 'BT-10',
              name: 'Buyer reference',
              kind: 'field',
              control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerReference,
              hasRemarks: true,
              hasChorusProRemarks: true,
            },
            {
              term: 'BG-4',
              name: 'SELLER',
              kind: 'group',
              control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty,
              children: [
                {
                  term: 'BT-27',
                  name: 'Seller name',
                  kind: 'field',
                  control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.name,
                  businessRules: ['BR-6'],
                },
                {
                  term: 'BT-30-00',
                  name: 'SELLER LEGAL ORGANIZATION',
                  kind: 'group',
                  control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.specifiedLegalOrganization,
                  children: [
                    {
                      term: 'BT-30',
                      name: 'Seller legal registration identifier',
                      kind: 'field',
                      control:
                        this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.specifiedLegalOrganization
                          .controls.id,
                      businessRules: ['BR-CO-26'],
                      children: [
                        {
                          term: 'BT-30-1',
                          name: 'Scheme identifier',
                          kind: 'field',
                          control:
                            this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.specifiedLegalOrganization
                              .controls.idSchemeId,
                          hasRemarks: true,
                        },
                      ],
                    },
                  ],
                },
                {
                  term: 'BG-5',
                  name: 'SELLER POSTAL ADDRESS',
                  kind: 'group',
                  control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.postalTradeAddress,
                  businessRules: ['BR-8'],
                  hasRemarks: true,
                  children: [
                    {
                      term: 'BT-40',
                      name: 'Seller country code',
                      kind: 'field',
                      control:
                        this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.postalTradeAddress.controls
                          .countryId,
                      businessRules: ['BR-9'],
                      hasRemarks: true,
                    },
                  ],
                },
                {
                  term: 'BT-31-00',
                  name: 'SELLER VAT IDENTIFIER',
                  kind: 'group',
                  control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.specifiedTaxRegistration,
                  children: [
                    {
                      term: 'BT-31',
                      name: 'Seller VAT identifier',
                      kind: 'field',
                      control:
                        this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.specifiedTaxRegistration.controls
                          .id,
                      businessRules: ['BR-CO-9', 'BR-CO-26'],
                      hasRemarks: true,
                      children: [
                        {
                          term: 'BT-31-0',
                          name: 'Tax Scheme identifier',
                          kind: 'field',
                          control:
                            this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.specifiedTaxRegistration
                              .controls.idSchemeId,
                          hasRemarks: true,
                        },
                      ],
                    },
                  ],
                },
              ],
            },
            {
              term: 'BG-7',
              name: 'BUYER',
              kind: 'group',
              control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerTradeParty,
              children: [
                {
                  term: 'BT-44',
                  name: 'Buyer name',
                  kind: 'field',
                  control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerTradeParty.controls.name,
                  businessRules: ['BR-7'],
                },
                {
                  term: 'BT-47-00',
                  name: 'BUYER LEGAL REGISTRATION IDENTIFIER',
                  kind: 'group',
                  control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerTradeParty.controls.specifiedLegalOrganization,
                  children: [
                    {
                      term: 'BT-47',
                      name: 'Buyer legal registration identifier',
                      kind: 'field',
                      control:
                        this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerTradeParty.controls.specifiedLegalOrganization.controls
                          .id,
                      hasRemarks: true,
                      hasChorusProRemarks: true,
                      children: [
                        {
                          term: 'BT-47-1',
                          name: 'Scheme identifier',
                          kind: 'field',
                          control:
                            this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerTradeParty.controls.specifiedLegalOrganization
                              .controls.idSchemeId,
                          hasRemarks: true,
                        },
                      ],
                    },
                  ],
                },
              ],
            },
            {
              term: 'BT-13-00',
              name: 'PURCHASE ORDER REFERENCE',
              kind: 'group',
              control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerOrderReferencedDocument,
              children: [
                {
                  term: 'BT-13',
                  name: 'Purchase order reference',
                  kind: 'field',
                  control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerOrderReferencedDocument.controls.issuerAssignedId,
                  hasChorusProRemarks: true,
                },
              ],
            },
          ],
        },
        {
          term: 'BG-13-00',
          name: 'DELIVERY INFORMATION',
          kind: 'group',
          control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeDelivery,
        },
        {
          term: 'BG-19',
          name: 'HEADER TRADE SETTLEMENT DIRECT DEBIT',
          kind: 'group',
          control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement,
          hasRemarks: true,
          hasChorusProRemarks: true,
          children: [
            {
              term: 'BT-5',
              name: 'Invoice currency code',
              kind: 'field',
              control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement.controls.invoiceCurrencyCode,
              businessRules: ['BR-5'],
              hasRemarks: true,
              hasChorusProRemarks: true,
            },
            {
              term: 'BG-22',
              name: 'DOCUMENT TOTALS',
              kind: 'group',
              control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement.controls.specifiedTradeSettlementHeaderMonetarySummation,
              hasRemarks: true,
              hasChorusProRemarks: true,
              children: [
                {
                  term: 'BT-109',
                  name: 'Total amount without VAT',
                  kind: 'field',
                  control:
                    this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement.controls.specifiedTradeSettlementHeaderMonetarySummation.controls
                      .taxBasisTotalAmount,
                  businessRules: ['BR-13', 'BR-CO-13'],
                  hasRemarks: true,
                },
                {
                  term: 'BT-110',
                  name: 'Total VAT amount',
                  kind: 'field',
                  control:
                    this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement.controls.specifiedTradeSettlementHeaderMonetarySummation.controls
                      .taxTotalAmount,
                  businessRules: ['BR-CO-14'],
                  hasRemarks: true,
                  children: [
                    {
                      term: 'BT-110-1',
                      name: 'VAT currency',
                      kind: 'field',
                      control:
                        this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement.controls.specifiedTradeSettlementHeaderMonetarySummation.controls
                          .taxTotalAmountCurrencyId,
                      hasRemarks: true,
                    },
                  ],
                },
                {
                  term: 'BT-112',
                  name: 'Total amount with VAT',
                  kind: 'field',
                  control:
                    this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement.controls.specifiedTradeSettlementHeaderMonetarySummation.controls
                      .grandTotalAmount,
                  businessRules: ['BR-14', 'BR-CO-15', 'BR-FXEXT-CO-15'],
                  hasRemarks: true,
                },
                {
                  term: 'BT-115',
                  name: 'Amount due for payment',
                  kind: 'field',
                  control:
                    this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement.controls.specifiedTradeSettlementHeaderMonetarySummation.controls
                      .duePayableAmount,
                  businessRules: ['BR-15', 'BR-CO-16'],
                  hasRemarks: true,
                },
              ],
            },
          ],
        },
      ],
    },
  ];
}

export type CiiFormControl = CiiFormGroup | CiiFormField;

/**
 * An element of a Cross Industry Invoice form.
 */
interface CiiFormControlBase {
  /**
   * The term of the element.
   */
  term: string;

  /**
   * The name of the element.
   */
  name: string;

  /**
   * The business rules associated with this element.
   */
  businessRules?: string[];

  /**
   * Whether the element has remarks.
   */
  hasRemarks?: boolean;

  /**
   * Whether the element has Chorus Pro remarks.
   */
  hasChorusProRemarks?: boolean;

  /**
   * The children of this element.
   */
  children?: CiiFormControl[];
}

interface CiiFormGroup extends CiiFormControlBase {
  /**
   * Whether the control is a field or a group.
   */
  kind: 'group';

  /**
   * The form control for this element.
   */
  control: FormGroup;
}

interface CiiFormField extends CiiFormControlBase {
  /**
   * Whether the control is a field or a group.
   */
  kind: 'field';

  /**
   * The form control for this element.
   */
  control: FormControl;
}
