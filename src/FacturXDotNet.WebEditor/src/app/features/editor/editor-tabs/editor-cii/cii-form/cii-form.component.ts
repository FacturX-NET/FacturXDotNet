import { Component, inject, input, model } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CiiFormExchangedDocumentContextComponent } from './cii-form-exchanged-document-context.component';
import { CiiFormSupplyChainTradeTransactionComponent } from './cii-form-supply-chain-trade-transaction.component';
import { CiiFormExchangedDocumentComponent } from './cii-form-exchanged-document.component';
import { EditorSettings } from '../../../services/editor-settings.service';
import { CiiFormParentContainerComponent } from './base/cii-form-parent-container.component';
import { CiiFormService } from './cii-form.service';
import { ICrossIndustryInvoice } from '../../../../../core/api/api.models';
import { requireTerm } from '../constants/cii-terms';

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
      <app-cii-form-parent-container [term]="br2" [settings]="settings()" depth="1">
        <app-cii-form-exchanged-document-context formGroupName="exchangedDocumentContext" [settings]="settings()"></app-cii-form-exchanged-document-context>
      </app-cii-form-parent-container>

      <app-cii-form-parent-container [term]="bt100" [settings]="settings()" depth="1">
        <app-cii-form-exchanged-document formGroupName="exchangedDocument" [settings]="settings()"></app-cii-form-exchanged-document>
      </app-cii-form-parent-container>

      <app-cii-form-parent-container [term]="bg2500" [settings]="settings()" depth="1">
        <app-cii-form-supply-chain-trade-transaction formGroupName="supplyChainTradeTransaction" [settings]="settings()"></app-cii-form-supply-chain-trade-transaction>
      </app-cii-form-parent-container>
    </form>
  `,
})
export class CiiFormComponent {
  value = model.required<ICrossIndustryInvoice>();
  settings = input<EditorSettings>();

  protected br2 = requireTerm('BG-2');
  protected bt100 = requireTerm('BT-1-00');
  protected bg2500 = requireTerm('BG-25-00');

  private ciiFormService = inject(CiiFormService);

  protected get form(): FormGroup {
    return this.ciiFormService.form;
  }
}
