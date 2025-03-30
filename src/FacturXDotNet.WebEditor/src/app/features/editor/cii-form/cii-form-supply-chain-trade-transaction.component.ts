import { Component, computed, input } from '@angular/core';
import { CrossIndustryInvoiceFormVerbosity } from './cii-form.component';
import { ControlContainer, FormGroupDirective, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CiiFormApplicableHeaderTradeAgreementComponent } from './cii-form-applicable-header-trade-agreement.component';

@Component({
  selector: 'app-cii-form-supply-chain-trade-transaction',
  imports: [ReactiveFormsModule, CiiFormApplicableHeaderTradeAgreementComponent],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
  template: ` <div [formGroupName]="formGroupName()">
    <div class="pb-3">
      <div class="mb-3">
        <h6>BT-10-00 - HEADER TRADE AGREEMENT</h6>
      </div>
      <div class="ps-3 border-start">
        <app-cii-form-applicable-header-trade-agreement
          formGroupName="applicableHeaderTradeAgreement"
          [verbosity]="verbosity()"
          [disabled]="disabled()"
        ></app-cii-form-applicable-header-trade-agreement>
      </div>
    </div>
  </div>`,
})
export class CiiFormSupplyChainTradeTransactionComponent {
  formGroupName = input.required<string>();
  verbosity = input<CrossIndustryInvoiceFormVerbosity>('normal');
  disabled = input<boolean>(false);

  protected showMinimal = computed(() => this.verbosity() == 'minimal' || this.verbosity() == 'normal' || this.verbosity() == 'detailed');
  protected showNormal = computed(() => this.verbosity() == 'normal' || this.verbosity() == 'detailed');
  protected showDetailed = computed(() => this.verbosity() == 'detailed');
}
