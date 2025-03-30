import { Component, computed, effect, inject, input } from '@angular/core';
import { CrossIndustryInvoiceFormVerbosity } from './cii-form.component';
import { ControlContainer, FormGroupDirective, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CiiFormApplicableHeaderTradeAgreementComponent } from './cii-form-applicable-header-trade-agreement.component';
import { CollapseComponent } from '../../../core/collapse/collapse.component';
import { CiiFormApplicableHeaderTradeDeliveryComponent } from './cii-form-applicable-header-trade-delivery.component';
import { CiiFormApplicableHeaderTradeSettlementComponent } from './cii-form-applicable-header-trade-settlement.component';

@Component({
  selector: 'app-cii-form-supply-chain-trade-transaction',
  imports: [
    ReactiveFormsModule,
    CiiFormApplicableHeaderTradeAgreementComponent,
    CollapseComponent,
    CiiFormApplicableHeaderTradeDeliveryComponent,
    CiiFormApplicableHeaderTradeSettlementComponent,
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
      <app-collapse id="header-trade-agreement" #collapseHeaderTradeAgreement>
        <h6 class="m-0" ngProjectAs="trigger">
          @if (collapseHeaderTradeAgreement.collapsed()) {
            <i class="bi bi-plus fs-5"></i>
          } @else {
            <i class="bi bi-dash fs-5"></i>
          }

          BT-10-00 - HEADER TRADE AGREEMENT
        </h6>
        <p class="form-text ps-4"></p>
        <div class="ps-4" ngProjectAs="collapsible">
          <div class="ps-3 border-start">
            <app-cii-form-applicable-header-trade-agreement
              formGroupName="applicableHeaderTradeAgreement"
              [verbosity]="verbosity()"
              [disabled]="disabled()"
            ></app-cii-form-applicable-header-trade-agreement>
          </div>
        </div>
      </app-collapse>

      <app-collapse id="header-trade-delivery" #collapseHeaderTradeDelivery>
        <h6 class="m-0" ngProjectAs="trigger">
          @if (collapseHeaderTradeDelivery.collapsed()) {
            <i class="bi bi-plus fs-5"></i>
          } @else {
            <i class="bi bi-dash fs-5"></i>
          }

          BG-13-00 - DELIVERY INFORMATION
        </h6>
        <p class="form-text ps-4">A group of business terms providing information about where and when the goods and services invoiced are delivered.</p>
        <div class="ps-4" ngProjectAs="collapsible">
          <div class="ps-3 border-start">
            <app-cii-form-applicable-header-trade-delivery
              formGroupName="applicableHeaderTradeDelivery"
              [verbosity]="verbosity()"
              [disabled]="disabled()"
            ></app-cii-form-applicable-header-trade-delivery>
          </div>
        </div>
      </app-collapse>

      <app-collapse id="header-trade-settlement" #collapseHeaderTradeSettlement>
        <h6 class="m-0" ngProjectAs="trigger">
          @if (collapseHeaderTradeSettlement.collapsed()) {
            <i class="bi bi-plus fs-5"></i>
          } @else {
            <i class="bi bi-dash fs-5"></i>
          }

          BG-19 - HEADER TRADE SETTLEMENT DIRECT DEBIT
        </h6>
        <p class="form-text ps-4">A group of business terms to specify a direct debit.</p>
        @if (showRemarks()) {
          <div class="alert alert-light small">
            <i class="bi bi-info-circle"></i>
            This group may be used to give prior notice in the invoice that payment will be made through a SEPA or other direct debit initiated by the Seller, in accordance with
            the rules of the SEPA or other direct debit scheme.
          </div>
          <div class="alert alert-light small">
            <div class="fw-semibold"><i class="bi bi-info-circle"></i> CHORUSPRO</div>
            Not used.
          </div>
        }
        <div class="ps-4" ngProjectAs="collapsible">
          <div class="ps-3 border-start">
            <app-cii-form-applicable-header-trade-settlement
              formGroupName="applicableHeaderTradeSettlement"
              [verbosity]="verbosity()"
              [disabled]="disabled()"
            ></app-cii-form-applicable-header-trade-settlement>
          </div>
        </div>
      </app-collapse>
    </div>
  `,
})
export class CiiFormSupplyChainTradeTransactionComponent {
  formGroupName = input.required<string>();
  verbosity = input<CrossIndustryInvoiceFormVerbosity>('normal');
  disabled = input<boolean>(false);

  protected showBusinessRules = computed(() => this.verbosity() == 'normal' || this.verbosity() == 'detailed');
  protected showRemarks = computed(() => this.verbosity() == 'detailed');
}
