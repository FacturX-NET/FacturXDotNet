import { Component, computed, effect, inject, input } from '@angular/core';
import { CrossIndustryInvoiceFormVerbosity } from './cii-form.component';
import { ControlContainer, FormGroupDirective, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CiiFormApplicableHeaderTradeAgreementComponent } from './cii-form-applicable-header-trade-agreement.component';
import { CiiFormApplicableHeaderTradeDeliveryComponent } from './cii-form-applicable-header-trade-delivery.component';
import { CiiFormApplicableHeaderTradeSettlementComponent } from './cii-form-applicable-header-trade-settlement.component';
import { EditorSettings } from '../editor-settings.service';

@Component({
  selector: 'app-cii-form-supply-chain-trade-transaction',
  imports: [ReactiveFormsModule, CiiFormApplicableHeaderTradeAgreementComponent, CiiFormApplicableHeaderTradeDeliveryComponent, CiiFormApplicableHeaderTradeSettlementComponent],
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
      <h6 id="BT-10-00" class="m-0">BT-10-00 - HEADER TRADE AGREEMENT</h6>
      <p class="form-text ps-4"></p>
      <div class="ps-4">
        <div class="ps-3 border-start">
          <app-cii-form-applicable-header-trade-agreement formGroupName="applicableHeaderTradeAgreement" [settings]="settings()"></app-cii-form-applicable-header-trade-agreement>
        </div>
      </div>

      <h6 id="BG-13-00" class="m-0">BG-13-00 - DELIVERY INFORMATION</h6>
      <p class="form-text ps-4">A group of business terms providing information about where and when the goods and services invoiced are delivered.</p>
      <div class="ps-4">
        <div class="ps-3 border-start">
          <app-cii-form-applicable-header-trade-delivery formGroupName="applicableHeaderTradeDelivery" [settings]="settings()"></app-cii-form-applicable-header-trade-delivery>
        </div>
      </div>

      <h6 id="BG-19" class="m-0">BG-19 - HEADER TRADE SETTLEMENT DIRECT DEBIT</h6>
      <p class="form-text ps-4">A group of business terms to specify a direct debit.</p>
      @if (settings()?.showRemarks === true) {
        <div class="ps-4">
          <div class="alert alert-light small">
            <i class="bi bi-info-circle"></i>
            This group may be used to give prior notice in the invoice that payment will be made through a SEPA or other direct debit initiated by the Seller, in accordance with
            the rules of the SEPA or other direct debit scheme.
          </div>
        </div>
      }
      @if (settings()?.showChorusProRemarks === true) {
        <div class="ps-4">
          <div class="alert alert-light small">
            <div class="fw-semibold"><i class="bi bi-info-circle"></i> CHORUSPRO</div>
            Not used.
          </div>
        </div>
      }
      <div class="ps-4">
        <div class="ps-3 border-start">
          <app-cii-form-applicable-header-trade-settlement
            formGroupName="applicableHeaderTradeSettlement"
            [settings]="settings()"
          ></app-cii-form-applicable-header-trade-settlement>
        </div>
      </div>
    </div>
  `,
})
export class CiiFormSupplyChainTradeTransactionComponent {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();
}
