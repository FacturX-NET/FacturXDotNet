import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { CiiFormApplicableHeaderTradeAgreementComponent } from './cii-form-applicable-header-trade-agreement.component';
import { CiiFormApplicableHeaderTradeDeliveryComponent } from './cii-form-applicable-header-trade-delivery.component';
import { CiiFormApplicableHeaderTradeSettlementComponent } from './cii-form-applicable-header-trade-settlement.component';
import { EditorSettings } from '../../../editor-settings.service';
import { CiiFormParentContainerComponent } from './base/cii-form-parent-container.component';
import { ciiTerms, requireTerm } from '../constants/cii-terms';

@Component({
  selector: 'app-cii-form-supply-chain-trade-transaction',
  imports: [
    ReactiveFormsModule,
    CiiFormApplicableHeaderTradeAgreementComponent,
    CiiFormApplicableHeaderTradeDeliveryComponent,
    CiiFormApplicableHeaderTradeSettlementComponent,
    CiiFormParentContainerComponent,
  ],
  viewProviders: [
    {
      provide: ControlContainer,
      useFactory: () => {
        return inject(ControlContainer, { skipSelf: true });
      },
    },
  ],
  template: `
    <div [formGroupName]="formGroupName()">
      <app-cii-form-parent-container [term]="bt1000" [settings]="settings()" depth="2">
        <app-cii-form-applicable-header-trade-agreement formGroupName="applicableHeaderTradeAgreement" [settings]="settings()"></app-cii-form-applicable-header-trade-agreement>
      </app-cii-form-parent-container>

      <app-cii-form-parent-container [term]="bg1300" [description]="description" [settings]="settings()" depth="2">
        <ng-template #description>A group of business terms providing information about where and when the goods and services invoiced are delivered.</ng-template>

        <app-cii-form-applicable-header-trade-delivery formGroupName="applicableHeaderTradeDelivery" [settings]="settings()"></app-cii-form-applicable-header-trade-delivery>
      </app-cii-form-parent-container>

      <app-cii-form-parent-container [term]="bg19" [description]="description" [remarks]="[remark]" [chorusProRemarks]="[chorusProRemark]" [settings]="settings()" depth="2">
        <ng-template #description>A group of business terms to specify a direct debit.</ng-template>
        <ng-template #remark>
          This group may be used to give prior notice in the invoice that payment will be made through a SEPA or other direct debit initiated by the Seller, in accordance with the
          rules of the SEPA or other direct debit scheme.
        </ng-template>
        <ng-template #chorusProRemark> Not used.</ng-template>

        <app-cii-form-applicable-header-trade-settlement formGroupName="applicableHeaderTradeSettlement" [settings]="settings()"></app-cii-form-applicable-header-trade-settlement>
        <app-cii-form-applicable-header-trade-settlement formGroupName="applicableHeaderTradeSettlement" [settings]="settings()"></app-cii-form-applicable-header-trade-settlement>
      </app-cii-form-parent-container>
    </div>
  `,
})
export class CiiFormSupplyChainTradeTransactionComponent {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();

  protected bt1000 = requireTerm('BT-10-00');
  protected bg1300 = requireTerm('BG-13-00');
  protected bg19 = requireTerm('BG-19');
}
