import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { CiiFormApplicableHeaderTradeAgreementComponent } from './cii-form-applicable-header-trade-agreement.component';
import { CiiFormApplicableHeaderTradeDeliveryComponent } from './cii-form-applicable-header-trade-delivery.component';
import { CiiFormApplicableHeaderTradeSettlementComponent } from './cii-form-applicable-header-trade-settlement.component';
import { EditorSettings } from '../../../editor-settings.service';
import { CiiFormParentContainerComponent } from './base/cii-form-parent-container.component';
import { requireTerm } from '../constants/cii-terms';

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

      <app-cii-form-parent-container [term]="bg1300" [settings]="settings()" depth="2">
        <app-cii-form-applicable-header-trade-delivery formGroupName="applicableHeaderTradeDelivery" [settings]="settings()"></app-cii-form-applicable-header-trade-delivery>
      </app-cii-form-parent-container>

      <app-cii-form-parent-container [term]="bg19" [settings]="settings()" depth="2">
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
