import { DestroyRef, effect, inject, Injectable, signal } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators, ValueChangeEvent } from '@angular/forms';
import {
  DateOnlyFormat,
  GuidelineSpecifiedDocumentContextParameterId,
  ICrossIndustryInvoice,
  InvoiceTypeCode,
  VatOnlyTaxSchemeIdentifier,
} from '../../../../../core/api/api.models';
import { debounceTime, firstValueFrom, from, Subject, switchMap, tap } from 'rxjs';
import { EditorSavedState, EditorStateService } from '../../../editor-state.service';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { ValidateApi } from '../../../../../core/api/validate.api';
import { BusinessRuleIdentifier, getBusinessRuleIdentifiers, isBusinessRuleIdentifier, requireBusinessRule } from '../constants/cii-rules';
import { BusinessTermIdentifier, CiiTerm, getBusinessTermIdentifiers, requireTerm } from '../constants/cii-terms';
import { fromPromise } from 'rxjs/internal/observable/innerFrom';

@Injectable({
  providedIn: 'root',
})
export class CiiFormService {
  get state() {
    return this.stateInternal.asReadonly();
  }

  get validating() {
    return this.validatingInternal.asReadonly();
  }

  get businessTermsValidation() {
    return this.businessTermsValidationInternal.asReadonly();
  }

  get businessRulesValidation() {
    return this.businessRulesValidationInternal.asReadonly();
  }

  private saveSubject = new Subject<EditorSavedState>();
  private stateInternal = signal<CiiFormState>('pristine');
  private validatingInternal = signal<boolean>(false);
  private businessTermsValidationInternal = signal<Partial<Record<BusinessTermIdentifier, 'valid' | 'invalid'>>>({});
  private businessRulesValidationInternal = signal<Partial<Record<BusinessRuleIdentifier, 'valid' | 'invalid'>>>({});

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

    this.form.events.pipe(takeUntilDestroyed()).subscribe((evt) => {
      if (evt instanceof ValueChangeEvent) {
        const bt = this.findBusinessTerm(evt.source);
        if (bt !== undefined) {
          this.businessTermsValidationInternal.update((current) => {
            const newValue = { ...current };
            newValue[bt.id] = undefined;
            return newValue;
          });

          const rules = bt.businessRules;
          if (rules !== undefined) {
            this.businessRulesValidationInternal.update((current) => {
              const newValue = { ...current };
              for (const rule of rules) {
                newValue[rule] = undefined;
              }
              return newValue;
            });
          }
        }

        const value = this.editorStateService.savedState.value();
        if (value === null) {
          return;
        }

        this.saveSubject.next({ ...value, cii: this.form.getRawValue() });
      }
    });
  }

  /**
   * Retrieves a form control by its business term identifier.
   *
   * This method returns the associated `AbstractControl` for a given business term identifier.
   * If the control does not exist, it returns `undefined`.
   *
   * Example Usage:
   * ```typescript
   * const control = ciiFormService.getControl('BT-23');
   * if (control) {
   *   console.log(control.value); // Access the value of the control.
   * }
   * ```
   *
   * @param {BusinessTermIdentifier} term - The business term identifier of the control.
   * @returns {AbstractControl | undefined} The form control if found, otherwise `undefined`.
   */
  getControl(term: string): AbstractControl | undefined {
    const key = term as BusinessTermIdentifier;
    return this.controls[key];
  }

  /**
   * Retrieves a form control by its business term identifier, or throws an error if not found.
   *
   * Use this method when a control is expected to exist and you want to enforce strict validation.
   *
   * Example Usage:
   * ```typescript
   * const control = ciiFormService.requireControl('BT-23');
   * control.setValue('New Value'); // Set a value directly.
   * ```
   *
   * @param {BusinessTermIdentifier} term - The business term identifier of the control.
   * @returns {AbstractControl} The form control if found.
   */
  requireControl(term: BusinessTermIdentifier): AbstractControl {
    return this.controls[term];
  }

  /**
   * Validates the Cross-Industry Invoice (CII) form and its business rules.
   *
   * This method performs the following actions:
   * 1. Marks all fields in the form as touched to trigger validation feedback.
   * 2. Checks if the form is valid based on built-in, synchronous validators. If invalid, returns `false`.
   * 3. Transforms the form data into a CII model and sends it to the validation API.
   * 4. Updates the statuses of business rules based on validation results.
   *
   * Returns a `Promise` indicating whether the form and its business rules are valid.
   *
   * Example Usage:
   * ```typescript
   * const isValid = await ciiFormService.validate();
   * if (isValid) {
   *   console.log('Form is valid and business rules are satisfied.');
   * } else {
   *   console.log('Form validation failed. Please review the errors.');
   * }
   * ```
   *
   * @returns {Promise<boolean>} `true` if the form is valid and business rules are satisfied, otherwise `false`.
   */
  async validate(): Promise<boolean> {
    if (this.validating()) {
      return firstValueFrom(toObservable(this.validatingInternal).pipe(takeUntilDestroyed(this.destroyRef)));
    }

    this.validatingInternal.set(true);

    const businessTermIdentifiers = getBusinessTermIdentifiers();
    const businessRuleIdentifiers = getBusinessRuleIdentifiers();

    this.form.markAllAsTouched();
    if (!this.form.valid) {
      const termsStatuses: Partial<Record<BusinessTermIdentifier, 'invalid' | 'valid'>> = {};
      for (const termId of businessTermIdentifiers) {
        const termControl = this.getControl(termId);
        if (termControl?.invalid) {
          termsStatuses[termId] = 'invalid';
        }
      }

      this.businessTermsValidationInternal.set(termsStatuses);
      this.businessRulesValidationInternal.set({});

      this.validatingInternal.set(false);

      return false;
    }

    const cii: ICrossIndustryInvoice = this.fromFormValue(this.form.getRawValue());
    const validationResult = await firstValueFrom(this.validateApi.validateCrossIndustryInvoice(cii).pipe(takeUntilDestroyed(this.destroyRef)));

    const termsStatuses: Partial<Record<BusinessTermIdentifier, 'invalid' | 'valid'>> = {};
    for (const termId of businessTermIdentifiers) {
      termsStatuses[termId] = 'valid';
    }

    const rulesStatuses: Partial<Record<BusinessRuleIdentifier, 'invalid' | 'valid'>> = {};
    for (const ruleId of businessRuleIdentifiers) {
      rulesStatuses[ruleId] = 'valid';
    }

    if (validationResult.errors !== undefined) {
      for (const [_, errors] of Object.entries(validationResult.errors)) {
        for (const ruleName of errors) {
          if (!isBusinessRuleIdentifier(ruleName)) {
            continue;
          }

          rulesStatuses[ruleName] = 'invalid';

          const rule = requireBusinessRule(ruleName);
          for (const term of rule.termsInvolved) {
            termsStatuses[term] = 'invalid';
          }
        }
      }
    }

    this.businessTermsValidationInternal.set(termsStatuses);
    this.businessRulesValidationInternal.set(rulesStatuses);

    this.validatingInternal.set(false);

    return validationResult.valid;
  }

  /**
   * The main `FormGroup` representing the Cross-Industry Invoice (CII) form.
   *
   * This form models the structure of the CII document, including nested groups and fields
   * for document context, exchanged document details, and supply chain trade transactions.
   * It incorporates validation and change detection for seamless integration with Angular's
   * reactive forms module.
   *
   * Key Sections:
   * - **`exchangedDocumentContext`**: Document context parameters.
   * - **`exchangedDocument`**: Primary document details (e.g., ID and date).
   * - **`supplyChainTradeTransaction`**: Trade agreements, settlements, and delivery details.
   */
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
   * A mapping of business terms to their corresponding Angular form controls within the Cross-Industry Invoice (CII) form.
   *
   * Each business term identifier is associated with an instance of `AbstractControl`, which may represent
   * a `FormGroup` or a `FormControl`. This provides direct access to manage and manipulate form data
   * as per business requirements while preserving validation and input tracking functionality.
   *
   * The mapping facilitates:
   * - Simplified retrieval of controls using business term identifiers.
   * - Validation and interaction with specific parts of the CII form hierarchy.
   */
  controls: Record<BusinessTermIdentifier, AbstractControl> = {
    'BG-2': this.form.controls.exchangedDocumentContext,
    'BT-23': this.form.controls.exchangedDocumentContext.controls.businessProcessSpecifiedDocumentContextParameterId,
    'BT-24': this.form.controls.exchangedDocumentContext.controls.guidelineSpecifiedDocumentContextParameterId,
    'BT-1-00': this.form.controls.exchangedDocument,
    'BT-1': this.form.controls.exchangedDocument.controls.id,
    'BT-3': this.form.controls.exchangedDocument.controls.typeCode,
    'BT-2': this.form.controls.exchangedDocument.controls.issueDateTime,
    'BT-2-0': this.form.controls.exchangedDocument.controls.issueDateTimeFormat,
    'BG-25-00': this.form.controls.supplyChainTradeTransaction,
    'BT-10-00': this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement,
    'BT-10': this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerReference,
    'BG-4': this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty,
    'BT-27': this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.name,
    'BT-30-00': this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.specifiedLegalOrganization,
    'BT-30': this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.specifiedLegalOrganization.controls.id,
    'BT-30-1':
      this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.specifiedLegalOrganization.controls.idSchemeId,
    'BG-5': this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.postalTradeAddress,
    'BT-40': this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.postalTradeAddress.controls.countryId,
    'BT-31-00': this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.specifiedTaxRegistration,
    'BT-31': this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.specifiedTaxRegistration.controls.id,
    'BT-31-0':
      this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.specifiedTaxRegistration.controls.idSchemeId,
    'BG-7': this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerTradeParty,
    'BT-44': this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerTradeParty.controls.name,
    'BT-47-00': this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerTradeParty.controls.specifiedLegalOrganization,
    'BT-47': this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerTradeParty.controls.specifiedLegalOrganization.controls.id,
    'BT-47-1':
      this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerTradeParty.controls.specifiedLegalOrganization.controls.idSchemeId,
    'BT-13-00': this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerOrderReferencedDocument,
    'BT-13': this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerOrderReferencedDocument.controls.issuerAssignedId,
    'BG-13-00': this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeDelivery,
    'BG-19': this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement,
    'BT-5': this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement.controls.invoiceCurrencyCode,
    'BG-22': this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement.controls.specifiedTradeSettlementHeaderMonetarySummation,
    'BT-109':
      this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement.controls.specifiedTradeSettlementHeaderMonetarySummation.controls.taxBasisTotalAmount,
    'BT-110':
      this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement.controls.specifiedTradeSettlementHeaderMonetarySummation.controls.taxTotalAmount,
    'BT-110-1':
      this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement.controls.specifiedTradeSettlementHeaderMonetarySummation.controls
        .taxTotalAmountCurrencyId,
    'BT-112':
      this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement.controls.specifiedTradeSettlementHeaderMonetarySummation.controls.grandTotalAmount,
    'BT-115':
      this.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement.controls.specifiedTradeSettlementHeaderMonetarySummation.controls.duePayableAmount,
  };

  /**
   * An ordered, hierarchical representation of all form elements within the Cross-Industry Invoice (CII) form,
   * structured as a tree with nodes representing groups, fields, or nested elements.
   *
   * Each node in the hierarchy contains:
   * - A `term` representing the business term identifier.
   * - A `control` referencing the associated `AbstractControl` (either `FormGroup` or `FormControl`).
   * - An optional list of `children`, which captures the nested structure of form groups and fields.
   *
   * The hierarchy provides a structured organization of form controls, reflecting the underlying
   * business logic and semantics of the invoice. This allows for streamlined access to related controls
   * and simplifies recursive operations, such as rendering or validation across multiple levels.
   *
   * Example Usage:
   * ```typescript
   * const topLevelNode = ciiFormService.hierarchy[0];
   * console.log(topLevelNode.term);  // Access business term of the top-level node.
   *
   * const childNode = topLevelNode.children?.[0];
   * console.log(childNode?.control.value);  // Get the value of a nested control.
   * ```
   *
   * Key Features:
   * - Reflects the business structure of the CII document.
   * - Serves as a navigable representation of the form's controls.
   *
   * Use this property to traverse the form logically or to align form operations with the
   * business rules defined for the Cross-Industry Invoice.
   */
  hierarchy: CiiFormNode[] = [
    {
      term: requireTerm('BG-2'),
      control: this.requireControl('BG-2'),
      children: [
        { term: requireTerm('BT-23'), control: this.requireControl('BT-23') },
        { term: requireTerm('BT-24'), control: this.requireControl('BT-24') },
      ],
    },
    {
      term: requireTerm('BT-1-00'),
      control: this.requireControl('BT-1-00'),
      children: [
        { term: requireTerm('BT-1'), control: this.requireControl('BT-1') },
        { term: requireTerm('BT-3'), control: this.requireControl('BT-3') },
        {
          term: requireTerm('BT-2'),
          control: this.requireControl('BT-2'),
          children: [{ term: requireTerm('BT-2-0'), control: this.requireControl('BT-2-0') }],
        },
      ],
    },
    {
      term: requireTerm('BG-25-00'),
      control: this.requireControl('BG-25-00'),
      children: [
        {
          term: requireTerm('BT-10-00'),
          control: this.requireControl('BT-10-00'),
          children: [
            { term: requireTerm('BT-10'), control: this.requireControl('BT-10') },
            {
              term: requireTerm('BG-4'),
              control: this.requireControl('BG-4'),
              children: [
                { term: requireTerm('BT-27'), control: this.requireControl('BT-27') },
                {
                  term: requireTerm('BT-30-00'),
                  control: this.requireControl('BT-30-00'),
                  children: [
                    {
                      term: requireTerm('BT-30'),
                      control: this.requireControl('BT-30'),
                      children: [{ term: requireTerm('BT-30-1'), control: this.requireControl('BT-30-1') }],
                    },
                  ],
                },
                {
                  term: requireTerm('BG-5'),
                  control: this.requireControl('BG-5'),
                  children: [{ term: requireTerm('BT-40'), control: this.requireControl('BT-40') }],
                },
                {
                  term: requireTerm('BT-31-00'),
                  control: this.requireControl('BT-31-00'),
                  children: [
                    {
                      term: requireTerm('BT-31'),
                      control: this.requireControl('BT-31'),
                      children: [{ term: requireTerm('BT-31-0'), control: this.requireControl('BT-31-0') }],
                    },
                  ],
                },
              ],
            },
            {
              term: requireTerm('BG-7'),
              control: this.requireControl('BG-7'),
              children: [
                { term: requireTerm('BT-44'), control: this.requireControl('BT-44') },
                {
                  term: requireTerm('BT-47-00'),
                  control: this.requireControl('BT-47-00'),
                  children: [
                    {
                      term: requireTerm('BT-47'),
                      control: this.requireControl('BT-47'),
                      children: [{ term: requireTerm('BT-47-1'), control: this.requireControl('BT-47-1') }],
                    },
                  ],
                },
              ],
            },
            {
              term: requireTerm('BT-13-00'),
              control: this.requireControl('BT-13-00'),
              children: [{ term: requireTerm('BT-13'), control: this.requireControl('BT-13') }],
            },
          ],
        },
        { term: requireTerm('BG-13-00'), control: this.requireControl('BG-13-00') },
        {
          term: requireTerm('BG-19'),
          control: this.requireControl('BG-19'),
          children: [
            { term: requireTerm('BT-5'), control: this.requireControl('BT-5') },
            {
              term: requireTerm('BG-22'),
              control: this.requireControl('BG-22'),
              children: [
                { term: requireTerm('BT-109'), control: this.requireControl('BT-109') },
                {
                  term: requireTerm('BT-110'),
                  control: this.requireControl('BT-110'),
                  children: [{ term: requireTerm('BT-110-1'), control: this.requireControl('BT-110-1') }],
                },
                { term: requireTerm('BT-112'), control: this.requireControl('BT-112') },
                { term: requireTerm('BT-115'), control: this.requireControl('BT-115') },
              ],
            },
          ],
        },
      ],
    },
  ];

  private findBusinessTerm(control: AbstractControl): CiiTerm | undefined {
    const businessTermIdentifier = getBusinessTermIdentifiers().find((key) => this.controls[key] === control);
    if (businessTermIdentifier === undefined) {
      return undefined;
    }

    return requireTerm(businessTermIdentifier);
  }

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

export interface CiiFormNode {
  /**
   * The term of the element.
   */
  term: CiiTerm;

  /**
   * The form control for this element.
   */
  control: AbstractControl;

  /**
   * The children of this element.
   */
  children?: CiiFormNode[];
}

export type CiiFormControl = CiiFormGroup | CiiFormField;

/**
 * An element of a Cross Industry Invoice form.
 */
interface CiiFormControlBase {
  /**
   * The term of the element.
   */
  term: BusinessTermIdentifier;

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
