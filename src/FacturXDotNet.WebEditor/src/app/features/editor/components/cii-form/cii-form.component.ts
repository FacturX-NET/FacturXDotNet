import { Component, effect, input, model } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { GuidelineSpecifiedDocumentContextParameterId } from '../../../../core/facturx-models/cii/guideline-specified-document-context-parameter-id';
import { InvoiceTypeCode } from '../../../../core/facturx-models/cii/invoice-type-code';
import { DateOnlyFormat } from '../../../../core/facturx-models/cii/date-only-format';
import { CrossIndustryInvoice } from '../../../../core/facturx-models/cii/cross-industry-invoice';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { DateOnly } from '../../../../core/facturx-models/cii/date-only';
import { VatOnlyTaxSchemeIdentifier } from '../../../../core/facturx-models/cii/vat-only-tax-scheme-identifier';
import { CiiFormExchangedDocumentContextComponent } from './cii-form-exchanged-document-context.component';
import { CiiFormSupplyChainTradeTransactionComponent } from './cii-form-supply-chain-trade-transaction.component';
import { CiiFormExchangedDocumentComponent } from './cii-form-exchanged-document.component';
import { EditorSettings } from '../../editor-settings.service';
import { CiiFormParentContainerComponent } from './base/cii-form-parent-container.component';

@Component({
  selector: 'app-cii-form',
  imports: [
    ReactiveFormsModule,
    CiiFormExchangedDocumentContextComponent,
    CiiFormSupplyChainTradeTransactionComponent,
    CiiFormExchangedDocumentComponent,
    CiiFormParentContainerComponent,
  ],
  template: `
    <form [formGroup]="form">
      <app-cii-form-parent-container term="BG-2" name="EXCHANGED DOCUMENT CONTEXT" [description]="description" [settings]="settings()">
        <ng-template #description>A group of business terms providing information on the business process and rules applicable to the Invoice document.</ng-template>

        <app-cii-form-exchanged-document-context formGroupName="exchangedDocumentContext" [settings]="settings()"></app-cii-form-exchanged-document-context>
      </app-cii-form-parent-container>

      <app-cii-form-parent-container term="BT-1-00" name="EXCHANGED DOCUMENT" [settings]="settings()">
        <app-cii-form-exchanged-document formGroupName="exchangedDocument" [settings]="settings()"></app-cii-form-exchanged-document>
      </app-cii-form-parent-container>

      <app-cii-form-parent-container term="BG-25-00" name="SUPPLY CHAIN TRADE TRANSACTION" [settings]="settings()">
        <app-cii-form-supply-chain-trade-transaction formGroupName="supplyChainTradeTransaction" [settings]="settings()"></app-cii-form-supply-chain-trade-transaction>
      </app-cii-form-parent-container>
    </form>
  `,
})
export class CiiFormComponent {
  value = model.required<CrossIndustryInvoice>();
  settings = input<EditorSettings>();
  disabled = input<boolean>();

  constructor() {
    effect(() => {
      if (this.disabled()) {
        this.form.disable({ emitEvent: false });
      } else {
        this.form.enable({ emitEvent: false });
      }
    });

    effect(() => {
      const value = this.value();
      this.form.patchValue(value, { emitEvent: false });
    });

    this.form.valueChanges.pipe(takeUntilDestroyed()).subscribe((v) => {
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
          taxBasisTotalAmount: new FormControl<number | undefined>(undefined, { nonNullable: true }),
          taxTotalAmount: new FormControl<number | undefined>(undefined, { nonNullable: true }),
          taxTotalAmountCurrencyId: new FormControl<string>('', { nonNullable: true }),
          grandTotalAmount: new FormControl<number | undefined>(undefined, { nonNullable: true }),
          duePayableAmount: new FormControl<number | undefined>(undefined, { nonNullable: true }),
        }),
      }),
    }),
  });
}

export type CrossIndustryInvoiceFormVerbosity = 'minimal' | 'normal' | 'detailed';
