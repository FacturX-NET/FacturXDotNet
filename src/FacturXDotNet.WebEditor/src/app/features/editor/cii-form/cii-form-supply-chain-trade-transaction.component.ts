import { Component, computed, effect, inject, input } from '@angular/core';
import { CrossIndustryInvoiceFormVerbosity } from './cii-form.component';
import { ControlContainer, FormGroupDirective, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CiiFormApplicableHeaderTradeAgreementComponent } from './cii-form-applicable-header-trade-agreement.component';
import { CollapseComponent } from '../../../core/collapse/collapse.component';

@Component({
  selector: 'app-cii-form-supply-chain-trade-transaction',
  imports: [ReactiveFormsModule, CiiFormApplicableHeaderTradeAgreementComponent, CollapseComponent],
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
