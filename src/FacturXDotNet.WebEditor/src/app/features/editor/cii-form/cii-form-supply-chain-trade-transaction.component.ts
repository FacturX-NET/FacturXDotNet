import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { CiiFormApplicableHeaderTradeAgreementComponent } from './cii-form-applicable-header-trade-agreement.component';
import { CiiFormApplicableHeaderTradeDeliveryComponent } from './cii-form-applicable-header-trade-delivery.component';
import { CiiFormApplicableHeaderTradeSettlementComponent } from './cii-form-applicable-header-trade-settlement.component';
import { EditorSettings } from '../editor-settings.service';
import { CiiFormParentContainerComponent } from './components/cii-form-parent-container.component';

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
      <app-cii-form-parent-container term="BT-10-00" name="HEADER TARDE AGREEMENT" [settings]="settings()">
        <app-cii-form-applicable-header-trade-agreement formGroupName="applicableHeaderTradeAgreement" [settings]="settings()"></app-cii-form-applicable-header-trade-agreement>
      </app-cii-form-parent-container>

      <app-cii-form-parent-container term="BG-13-00" name="DELIVERY INFORMATION" [description]="description" [settings]="settings()">
        <ng-template #description>A group of business terms providing information about where and when the goods and services invoiced are delivered.</ng-template>

        <app-cii-form-applicable-header-trade-delivery formGroupName="applicableHeaderTradeDelivery" [settings]="settings()"></app-cii-form-applicable-header-trade-delivery>
      </app-cii-form-parent-container>

      <app-cii-form-parent-container
        term="BG-19"
        name="HEADER TRADE SETTLEMENT DIRECT DEBIT"
        [description]="description"
        [remarks]="[remark]"
        [chorusProRemarks]="[chorusProRemark]"
        [settings]="settings()"
      >
        <ng-template #description>A group of business terms to specify a direct debit.</ng-template>
        <ng-template #remark>
          This group may be used to give prior notice in the invoice that payment will be made through a SEPA or other direct debit initiated by the Seller, in accordance with the
          rules of the SEPA or other direct debit scheme.
        </ng-template>
        <ng-template #chorusProRemark> Not used.</ng-template>

        <app-cii-form-applicable-header-trade-settlement formGroupName="applicableHeaderTradeSettlement" [settings]="settings()"></app-cii-form-applicable-header-trade-settlement>
      </app-cii-form-parent-container>
    </div>
  `,
})
export class CiiFormSupplyChainTradeTransactionComponent {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();
}
