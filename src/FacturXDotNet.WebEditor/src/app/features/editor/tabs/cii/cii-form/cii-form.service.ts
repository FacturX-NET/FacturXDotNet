import { DestroyRef, effect, inject, Injectable, signal } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import {
  DateOnlyFormat,
  GuidelineSpecifiedDocumentContextParameterId,
  ICrossIndustryInvoice,
  InvoiceTypeCode,
  VatOnlyTaxSchemeIdentifier,
} from '../../../../../core/api/api.models';
import { debounceTime, firstValueFrom, from, Subject, switchMap, tap } from 'rxjs';
import { EditorSavedState, EditorStateService } from '../../../editor-state.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ValidateApi } from '../../../../../core/api/validate.api';
import { BusinessRuleIdentifier, ciiBusinessRules } from '../constants/cii-business-rules';

@Injectable({
  providedIn: 'root',
})
export class CiiFormService {
  get state() {
    return this.stateInternal.asReadonly();
  }

  get businessRules() {
    return this.businessRulesInternal.asReadonly();
  }

  private saveSubject = new Subject<EditorSavedState>();
  private stateInternal = signal<CiiFormState>('pristine');
  private businessRulesInternal = signal<Partial<Record<BusinessRuleIdentifier, 'valid' | 'invalid'>>>({});

  private validateApi = inject(ValidateApi);
  private editorStateService = inject(EditorStateService);
  private destroyRef = inject(DestroyRef);

  constructor() {
    this.saveSubject
      .pipe(
        tap(() => this.stateInternal.set('dirty')),
        debounceTime(1000),
        tap(() => this.stateInternal.set('saving')),
        switchMap((state) => from(this.editorStateService.updateCii(state.cii))),
        tap(() => this.stateInternal.set('pristine')),
        takeUntilDestroyed(),
      )
      .subscribe();

    effect(() => {
      const value = this.editorStateService.savedState.value()?.cii;
      if (value === undefined) {
        return;
      }

      this.form.patchValue(this.toFormValue(value), { emitEvent: false });
    });

    this.form.valueChanges.pipe(takeUntilDestroyed()).subscribe(() => {
      const value = this.editorStateService.savedState.value();
      if (value === null) {
        return;
      }

      this.saveSubject.next({ ...value, cii: this.form.getRawValue() });
    });
  }

  async validate(): Promise<boolean> {
    this.form.markAllAsTouched();
    if (!this.form.valid) {
      return false;
    }

    const cii: ICrossIndustryInvoice = this.fromFormValue(this.form.getRawValue());
    const validationResult = await firstValueFrom(this.validateApi.validateCrossIndustryInvoice(cii).pipe(takeUntilDestroyed(this.destroyRef)));

    const rulesStatuses: Record<string, 'invalid' | 'valid'> = Object.fromEntries(Object.keys(ciiBusinessRules).map((r) => [r, 'valid']));
    if (validationResult.errors !== undefined) {
      for (const [_, errors] of Object.entries(validationResult.errors)) {
        for (const rule of errors) {
          rulesStatuses[rule] = 'invalid';
        }
      }
    }
    this.businessRulesInternal.set(rulesStatuses);

    return validationResult.valid;
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
      issueDateTime: new FormControl<string | undefined>(undefined, { nonNullable: true }),
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
      term: 'BR-02',
      control: this.form.controls.exchangedDocumentContext,
      children: [
        {
          term: 'BT-23',
          control: this.form.controls.exchangedDocumentContext.controls.businessProcessSpecifiedDocumentContextParameterId,
        },
        {
          term: 'BT-24',
          control: this.form.controls.exchangedDocumentContext.controls.guidelineSpecifiedDocumentContextParameterId,
        },
      ],
    },
    {
      term: 'BT-1-00',
      control: this.form.controls.exchangedDocument,
      children: [
        {
          term: 'BT-1',
          control: this.form.controls.exchangedDocument.controls.id,
        },
        {
          term: 'BT-3',
          control: this.form.controls.exchangedDocument.controls.typeCode,
        },
        {
          term: 'BT-2',
          control: this.form.controls.exchangedDocument.controls.issueDateTime,
          children: [
            {
              term: 'BT-2-0',
              control: this.form.controls.exchangedDocument.controls.issueDateTimeFormat,
            },
          ],
        },
      ],
    },
    {
      term: 'BG-25-00',
      control: this.form.controls.supplyChainTradeTransaction,
      children: [
        {
          term: 'BT-10-00',
          control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement,
          children: [
            {
              term: 'BT-10',
              control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerReference,
            },
            {
              term: 'BR-04',
              control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty,
              children: [
                {
                  term: 'BT-27',
                  control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.name,
                },
                {
                  term: 'BT-30-00',
                  control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.specifiedLegalOrganization,
                  children: [
                    {
                      term: 'BT-30',
                      control:
                        this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.specifiedLegalOrganization
                          .controls.id,
                      children: [
                        {
                          term: 'BT-30-1',
                          control:
                            this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.specifiedLegalOrganization
                              .controls.idSchemeId,
                        },
                      ],
                    },
                  ],
                },
                {
                  term: 'BR-05',
                  control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.postalTradeAddress,
                  children: [
                    {
                      term: 'BT-40',
                      control:
                        this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.postalTradeAddress.controls
                          .countryId,
                    },
                  ],
                },
                {
                  term: 'BT-31-00',
                  control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.specifiedTaxRegistration,
                  children: [
                    {
                      term: 'BT-31',
                      control:
                        this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.specifiedTaxRegistration.controls
                          .id,
                      children: [
                        {
                          term: 'BT-31-0',
                          control:
                            this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.specifiedTaxRegistration
                              .controls.idSchemeId,
                        },
                      ],
                    },
                  ],
                },
              ],
            },
            {
              term: 'BR-07',
              control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerTradeParty,
              children: [
                {
                  term: 'BT-44',
                  control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerTradeParty.controls.name,
                },
                {
                  term: 'BT-47-00',
                  control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerTradeParty.controls.specifiedLegalOrganization,
                  children: [
                    {
                      term: 'BT-47',
                      control:
                        this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerTradeParty.controls.specifiedLegalOrganization.controls
                          .id,
                      children: [
                        {
                          term: 'BT-47-1',
                          control:
                            this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerTradeParty.controls.specifiedLegalOrganization
                              .controls.idSchemeId,
                        },
                      ],
                    },
                  ],
                },
              ],
            },
            {
              term: 'BT-13-00',
              control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerOrderReferencedDocument,
              children: [
                {
                  term: 'BT-13',
                  control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerOrderReferencedDocument.controls.issuerAssignedId,
                },
              ],
            },
          ],
        },
        {
          term: 'BG-13-00',
          control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeDelivery,
        },
        {
          term: 'BG-19',
          control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement,
          children: [
            {
              term: 'BT-5',
              control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement.controls.invoiceCurrencyCode,
            },
            {
              term: 'BG-22',
              control: this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement.controls.specifiedTradeSettlementHeaderMonetarySummation,
              children: [
                {
                  term: 'BT-109',
                  control:
                    this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement.controls.specifiedTradeSettlementHeaderMonetarySummation.controls
                      .taxBasisTotalAmount,
                },
                {
                  term: 'BT-110',
                  control:
                    this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement.controls.specifiedTradeSettlementHeaderMonetarySummation.controls
                      .taxTotalAmount,
                  children: [
                    {
                      term: 'BT-110-1',
                      control:
                        this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement.controls.specifiedTradeSettlementHeaderMonetarySummation.controls
                          .taxTotalAmountCurrencyId,
                    },
                  ],
                },
                {
                  term: 'BT-112',
                  control:
                    this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement.controls.specifiedTradeSettlementHeaderMonetarySummation.controls
                      .grandTotalAmount,
                },
                {
                  term: 'BT-115',
                  control:
                    this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement.controls.specifiedTradeSettlementHeaderMonetarySummation.controls
                      .duePayableAmount,
                },
              ],
            },
          ],
        },
      ],
    },
  ];

  private fromFormValue(value: Required<typeof this.form.value>): ICrossIndustryInvoice {
    return {
      exchangedDocumentContext: {
        businessProcessSpecifiedDocumentContextParameterId: value.exchangedDocumentContext?.businessProcessSpecifiedDocumentContextParameterId,
        guidelineSpecifiedDocumentContextParameterId: value.exchangedDocumentContext?.guidelineSpecifiedDocumentContextParameterId,
      },
      exchangedDocument: {
        id: value.exchangedDocument?.id,
        typeCode: value.exchangedDocument?.typeCode,
        issueDateTime: value.exchangedDocument?.issueDateTime,
        issueDateTimeFormat: value.exchangedDocument?.issueDateTimeFormat,
      },
      supplyChainTradeTransaction: {
        applicableHeaderTradeAgreement: {
          buyerReference: value.supplyChainTradeTransaction?.applicableHeaderTradeAgreement?.buyerReference,
          sellerTradeParty: {
            name: value.supplyChainTradeTransaction?.applicableHeaderTradeAgreement?.sellerTradeParty?.name,
            specifiedLegalOrganization: {
              id: value.supplyChainTradeTransaction?.applicableHeaderTradeAgreement?.sellerTradeParty?.specifiedLegalOrganization?.id,
              idSchemeId: value.supplyChainTradeTransaction?.applicableHeaderTradeAgreement?.sellerTradeParty?.specifiedLegalOrganization?.idSchemeId,
            },
            postalTradeAddress: {
              countryId: value.supplyChainTradeTransaction?.applicableHeaderTradeAgreement?.sellerTradeParty?.postalTradeAddress?.countryId,
            },
            specifiedTaxRegistration: {
              id: value.supplyChainTradeTransaction?.applicableHeaderTradeAgreement?.sellerTradeParty?.specifiedTaxRegistration?.id,
              idSchemeId: value.supplyChainTradeTransaction?.applicableHeaderTradeAgreement?.sellerTradeParty?.specifiedTaxRegistration?.idSchemeId,
            },
          },
          buyerTradeParty: {
            name: value.supplyChainTradeTransaction?.applicableHeaderTradeAgreement?.buyerTradeParty?.name,
            specifiedLegalOrganization: {
              id: value.supplyChainTradeTransaction?.applicableHeaderTradeAgreement?.buyerTradeParty?.specifiedLegalOrganization?.id,
              idSchemeId: value.supplyChainTradeTransaction?.applicableHeaderTradeAgreement?.buyerTradeParty?.specifiedLegalOrganization?.idSchemeId,
            },
          },
          buyerOrderReferencedDocument: {
            issuerAssignedId: value.supplyChainTradeTransaction?.applicableHeaderTradeAgreement?.buyerOrderReferencedDocument?.issuerAssignedId,
          },
        },
        applicableHeaderTradeDelivery: {},
        applicableHeaderTradeSettlement: {
          invoiceCurrencyCode: value.supplyChainTradeTransaction?.applicableHeaderTradeSettlement?.invoiceCurrencyCode,
          specifiedTradeSettlementHeaderMonetarySummation: {
            taxBasisTotalAmount: value.supplyChainTradeTransaction?.applicableHeaderTradeSettlement?.specifiedTradeSettlementHeaderMonetarySummation?.taxBasisTotalAmount,
            taxTotalAmount: value.supplyChainTradeTransaction?.applicableHeaderTradeSettlement?.specifiedTradeSettlementHeaderMonetarySummation?.taxTotalAmount,
            taxTotalAmountCurrencyId: value.supplyChainTradeTransaction?.applicableHeaderTradeSettlement?.specifiedTradeSettlementHeaderMonetarySummation?.taxTotalAmountCurrencyId,
            grandTotalAmount: value.supplyChainTradeTransaction?.applicableHeaderTradeSettlement?.specifiedTradeSettlementHeaderMonetarySummation?.grandTotalAmount,
            duePayableAmount: value.supplyChainTradeTransaction?.applicableHeaderTradeSettlement?.specifiedTradeSettlementHeaderMonetarySummation?.duePayableAmount,
          },
        },
      },
    };
  }

  private toFormValue(value: ICrossIndustryInvoice): typeof this.form.value {
    return {
      exchangedDocumentContext: {
        businessProcessSpecifiedDocumentContextParameterId: value.exchangedDocumentContext?.businessProcessSpecifiedDocumentContextParameterId,
        guidelineSpecifiedDocumentContextParameterId: value.exchangedDocumentContext?.guidelineSpecifiedDocumentContextParameterId,
      },
      exchangedDocument: {
        id: value.exchangedDocument?.id,
        typeCode: value.exchangedDocument?.typeCode,
        issueDateTime: value.exchangedDocument?.issueDateTime,
        issueDateTimeFormat: value.exchangedDocument?.issueDateTimeFormat,
      },
      supplyChainTradeTransaction: {
        applicableHeaderTradeAgreement: {
          buyerReference: value.supplyChainTradeTransaction?.applicableHeaderTradeAgreement?.buyerReference,
          sellerTradeParty: {
            name: value.supplyChainTradeTransaction?.applicableHeaderTradeAgreement?.sellerTradeParty?.name,
            specifiedLegalOrganization: {
              id: value.supplyChainTradeTransaction?.applicableHeaderTradeAgreement?.sellerTradeParty?.specifiedLegalOrganization?.id,
              idSchemeId: value.supplyChainTradeTransaction?.applicableHeaderTradeAgreement?.sellerTradeParty?.specifiedLegalOrganization?.idSchemeId,
            },
            postalTradeAddress: {
              countryId: value.supplyChainTradeTransaction?.applicableHeaderTradeAgreement?.sellerTradeParty?.postalTradeAddress?.countryId,
            },
            specifiedTaxRegistration: {
              id: value.supplyChainTradeTransaction?.applicableHeaderTradeAgreement?.sellerTradeParty?.specifiedTaxRegistration?.id,
              idSchemeId: value.supplyChainTradeTransaction?.applicableHeaderTradeAgreement?.sellerTradeParty?.specifiedTaxRegistration?.idSchemeId,
            },
          },
          buyerTradeParty: {
            name: value.supplyChainTradeTransaction?.applicableHeaderTradeAgreement?.buyerTradeParty?.name,
            specifiedLegalOrganization: {
              id: value.supplyChainTradeTransaction?.applicableHeaderTradeAgreement?.buyerTradeParty?.specifiedLegalOrganization?.id,
              idSchemeId: value.supplyChainTradeTransaction?.applicableHeaderTradeAgreement?.buyerTradeParty?.specifiedLegalOrganization?.idSchemeId,
            },
          },
          buyerOrderReferencedDocument: {
            issuerAssignedId: value.supplyChainTradeTransaction?.applicableHeaderTradeAgreement?.buyerOrderReferencedDocument?.issuerAssignedId,
          },
        },
        applicableHeaderTradeDelivery: {},
        applicableHeaderTradeSettlement: {
          invoiceCurrencyCode: value.supplyChainTradeTransaction?.applicableHeaderTradeSettlement?.invoiceCurrencyCode,
          specifiedTradeSettlementHeaderMonetarySummation: {
            taxBasisTotalAmount: value.supplyChainTradeTransaction?.applicableHeaderTradeSettlement?.specifiedTradeSettlementHeaderMonetarySummation?.taxBasisTotalAmount,
            taxTotalAmount: value.supplyChainTradeTransaction?.applicableHeaderTradeSettlement?.specifiedTradeSettlementHeaderMonetarySummation?.taxTotalAmount,
            taxTotalAmountCurrencyId: value.supplyChainTradeTransaction?.applicableHeaderTradeSettlement?.specifiedTradeSettlementHeaderMonetarySummation?.taxTotalAmountCurrencyId,
            grandTotalAmount: value.supplyChainTradeTransaction?.applicableHeaderTradeSettlement?.specifiedTradeSettlementHeaderMonetarySummation?.grandTotalAmount,
            duePayableAmount: value.supplyChainTradeTransaction?.applicableHeaderTradeSettlement?.specifiedTradeSettlementHeaderMonetarySummation?.duePayableAmount,
          },
        },
      },
    };
  }
}

export type CiiFormState = 'pristine' | 'dirty' | 'saving';

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
   * The children of this element.
   */
  children?: CiiFormControl[];
}

interface CiiFormGroup extends CiiFormControlBase {
  /**
   * The form control for this element.
   */
  control: FormGroup;
}

interface CiiFormField extends CiiFormControlBase {
  /**
   * The form control for this element.
   */
  control: FormControl;
}

type ExtractFormControl<T> = {
  [K in keyof T]: T[K] extends FormControl<infer U> ? U : T[K];
};
