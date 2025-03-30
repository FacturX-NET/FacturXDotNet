import { Component, computed, effect, input, model } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { GuidelineSpecifiedDocumentContextParameterId } from '../../../core/facturx-models/cii/guideline-specified-document-context-parameter-id';
import { InvoiceTypeCode } from '../../../core/facturx-models/cii/invoice-type-code';
import { DateOnlyFormat } from '../../../core/facturx-models/cii/date-only-format';
import { CrossIndustryInvoice } from '../../../core/facturx-models/cii/cross-industry-invoice';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { DateOnly } from '../../../core/facturx-models/cii/date-only';
import { VatOnlyTaxSchemeIdentifier } from '../../../core/facturx-models/cii/vat-only-tax-scheme-identifier';
import { CollapseComponent } from '../../../core/collapse/collapse.component';
import { CiiFormExchangedDocumentContextComponent } from './cii-form-exchanged-document-context.component';
import { CiiFormSupplyChainTradeTransactionComponent } from './cii-form-supply-chain-trade-transaction.component';
import { CiiFormExchangedDocumentComponent } from './cii-form-exchanged-document.component';

@Component({
  selector: 'app-cii-form',
  imports: [ReactiveFormsModule, CollapseComponent, CiiFormExchangedDocumentContextComponent, CiiFormSupplyChainTradeTransactionComponent, CiiFormExchangedDocumentComponent],
  template: `
    <form [formGroup]="form">
      <app-collapse id="exchanged-document-context" #collapseExchangedDocumentContext>
        <h6 class="m-0" ngProjectAs="trigger">
          @if (collapseExchangedDocumentContext.collapsed()) {
            <i class="bi bi-plus fs-5"></i>
          } @else {
            <i class="bi bi-dash fs-5"></i>
          }

          BG-2 - EXCHANGE DOCUMENT CONTEXT
        </h6>
        <p class="form-text ps-4">A group of business terms providing information on the business process and rules applicable to the Invoice document.</p>
        <div class="ps-4" ngProjectAs="collapsible">
          <div class="ps-3 border-start">
            <app-cii-form-exchanged-document-context
              formGroupName="exchangedDocumentContext"
              [verbosity]="verbosity()"
              [disabled]="disabled()"
            ></app-cii-form-exchanged-document-context>
          </div>
        </div>
      </app-collapse>

      <app-collapse id="exchanged-document" #collapsedExchangeDocument>
        <h6 class="m-0" ngProjectAs="trigger">
          @if (collapsedExchangeDocument.collapsed()) {
            <i class="bi bi-plus fs-5"></i>
          } @else {
            <i class="bi bi-dash fs-5"></i>
          }

          BT-1-00 - EXCHANGE DOCUMENT
        </h6>
        <p class="form-text ps-4"></p>
        <div class="ps-4" ngProjectAs="collapsible">
          <div class="ps-3 border-start">
            <app-cii-form-exchanged-document formGroupName="exchangedDocument" [verbosity]="verbosity()" [disabled]="disabled()"></app-cii-form-exchanged-document>
          </div>
        </div>
      </app-collapse>

      <app-collapse id="supply-chain-trade-transaction" #collapseSupplyChainTradeTransaction>
        <h6 class="m-0" ngProjectAs="trigger">
          @if (collapseSupplyChainTradeTransaction.collapsed()) {
            <i class="bi bi-plus fs-5"></i>
          } @else {
            <i class="bi bi-dash fs-5"></i>
          }

          BG-25-00 - SUPPLY CHAIN TRADE TRANSACTION
        </h6>
        <p class="form-text ps-4"></p>
        <div class="ps-4" ngProjectAs="collapsible">
          <div class="ps-3 border-start">
            <app-cii-form-supply-chain-trade-transaction
              formGroupName="supplyChainTradeTransaction"
              [verbosity]="verbosity()"
              [disabled]="disabled()"
            ></app-cii-form-supply-chain-trade-transaction>
          </div>
        </div>
      </app-collapse>
    </form>
  `,
})
export class CiiFormComponent {
  value = model.required<CrossIndustryInvoice>();
  verbosity = input<CrossIndustryInvoiceFormVerbosity>('normal');
  disabled = input<boolean>(false);

  protected showBusinessRules = computed(() => this.verbosity() == 'normal' || this.verbosity() == 'detailed');
  protected showRemarks = computed(() => this.verbosity() == 'detailed');

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
