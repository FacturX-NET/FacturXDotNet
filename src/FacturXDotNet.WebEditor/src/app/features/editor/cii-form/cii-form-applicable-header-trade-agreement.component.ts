import { Component, computed, input } from '@angular/core';
import { CrossIndustryInvoiceFormVerbosity } from './cii-form.component';
import { ControlContainer, FormGroupDirective } from '@angular/forms';

@Component({
  selector: 'app-cii-form-applicable-header-trade-agreement',
  imports: [],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
  template: ` <p>cii-form-applicable-header-trade-agreement works!</p> `,
  styles: ``,
})
export class CiiFormApplicableHeaderTradeAgreementComponent {
  formGroupName = input.required<string>();
  verbosity = input<CrossIndustryInvoiceFormVerbosity>('normal');
  disabled = input<boolean>(false);

  protected showMinimal = computed(() => this.verbosity() == 'minimal' || this.verbosity() == 'normal' || this.verbosity() == 'detailed');
  protected showNormal = computed(() => this.verbosity() == 'normal' || this.verbosity() == 'detailed');
  protected showDetailed = computed(() => this.verbosity() == 'detailed');
}
