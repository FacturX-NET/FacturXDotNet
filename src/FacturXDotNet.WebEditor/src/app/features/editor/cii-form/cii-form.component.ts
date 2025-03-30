import { Component, computed, effect, input, model, Signal } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { GuidelineSpecifiedDocumentContextParameterId } from '../../../core/facturx-models/cii/guideline-specified-document-context-parameter-id';
import { InvoiceTypeCode } from '../../../core/facturx-models/cii/invoice-type-code';
import { DateOnlyFormat } from '../../../core/facturx-models/cii/date-only-format';
import { CrossIndustryInvoice } from '../../../core/facturx-models/cii/cross-industry-invoice';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { DateOnly } from '../../../core/facturx-models/cii/date-only';
import { VatOnlyTaxSchemeIdentifier } from '../../../core/facturx-models/cii/vat-only-tax-scheme-identifier';
import { CiiFormExchangedDocumentContextComponent } from './cii-form-exchanged-document-context.component';
import { CiiFormExchangedDocumentComponent } from './cii-form-exchanged-document.component';

@Component({
  selector: 'app-cii-form',
  imports: [ReactiveFormsModule, CiiFormExchangedDocumentContextComponent, CiiFormExchangedDocumentComponent],
  template: `
    <form [formGroup]="form">
      <div class="pb-3">
        <app-cii-form-exchanged-document-context
          formGroupName="exchangedDocumentContext"
          [verbosity]="verbosity()"
          [disabled]="disabled()"
        ></app-cii-form-exchanged-document-context>
      </div>
      <div class="pb-3">
        <app-cii-form-exchanged-document formGroupName="exchangedDocument" [verbosity]="verbosity()" [disabled]="disabled()"></app-cii-form-exchanged-document>
      </div>
    </form>
  `,
  styles: ``,
})
export class CiiFormComponent {
  value = model.required<CrossIndustryInvoice>();
  verbosity = input<CrossIndustryInvoiceFormVerbosity>('normal');
  disabled = input<boolean>(false);

  protected showMinimal = computed(() => this.verbosity() == 'minimal' || this.verbosity() == 'normal' || this.verbosity() == 'detailed');
  protected showNormal = computed(() => this.verbosity() == 'normal' || this.verbosity() == 'detailed');
  protected showDetailed = computed(() => this.verbosity() == 'detailed');

  constructor() {
    effect(() => {
      if (this.disabled()) {
        this.form.disable({ emitEvent: false });
      } else {
        this.form.enable({ emitEvent: false });
      }
    });

    this.form.valueChanges.pipe(takeUntilDestroyed()).subscribe((v) => {
      console.log('value changed', this.form.getRawValue());
      this.value.set(this.form.getRawValue());
    });
  }

  protected form = new FormGroup({
    exchangedDocumentContext: new FormGroup({
      businessProcessSpecifiedDocumentContextParameterId: new FormControl('', { nonNullable: true }),
      guidelineSpecifiedDocumentContextParameterId: new FormControl<GuidelineSpecifiedDocumentContextParameterId>('minimum', { nonNullable: true }),
    }),
    exchangedDocument: new FormGroup({
      id: new FormControl('', { nonNullable: true }),
      typeCode: new FormControl<InvoiceTypeCode | undefined>(undefined, { nonNullable: true }),
      issueDateTime: new FormControl<DateOnly | undefined>(undefined, { nonNullable: true }),
      issueDateTimeFormat: new FormControl<DateOnlyFormat>('102-date-only', { nonNullable: true }),
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
            idSchemeId: new FormControl<VatOnlyTaxSchemeIdentifier>('vat', { nonNullable: true }),
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
          taxBasisTotalAmount: new FormControl<number>(0, { nonNullable: true }),
          taxTotalAmount: new FormControl<number>(0, { nonNullable: true }),
          taxTotalAmountCurrencyId: new FormControl<string>('', { nonNullable: true }),
          grandTotalAmount: new FormControl<number>(0, { nonNullable: true }),
          duePayableAmount: new FormControl<number>(0, { nonNullable: true }),
        }),
      }),
    }),
  });
}

export type CrossIndustryInvoiceFormVerbosity = 'minimal' | 'normal' | 'detailed';
