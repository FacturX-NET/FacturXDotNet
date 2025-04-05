import { Component, effect, inject, input, model } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ICrossIndustryInvoice } from '../../../../core/api/api.models';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CiiFormExchangedDocumentContextComponent } from './cii-form-exchanged-document-context.component';
import { CiiFormSupplyChainTradeTransactionComponent } from './cii-form-supply-chain-trade-transaction.component';
import { CiiFormExchangedDocumentComponent } from './cii-form-exchanged-document.component';
import { EditorSettings } from '../../editor-settings.service';
import { CiiFormParentContainerComponent } from './base/cii-form-parent-container.component';
import { CiiFormService } from './cii-form.service';

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
  value = model.required<ICrossIndustryInvoice>();
  settings = input<EditorSettings>();
  disabled = input<boolean>();

  private ciiFormService = inject(CiiFormService);

  protected get form(): FormGroup {
    return this.ciiFormService.form;
  }

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

    this.form.valueChanges.pipe(takeUntilDestroyed()).subscribe((_) => {
      this.value.set(this.form.getRawValue());
    });
  }
}
