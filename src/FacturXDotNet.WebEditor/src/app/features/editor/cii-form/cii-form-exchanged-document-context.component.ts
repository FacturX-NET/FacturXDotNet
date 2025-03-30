import { Component, computed, effect, inject, input } from '@angular/core';
import { ControlContainer, FormGroupDirective, ReactiveFormsModule } from '@angular/forms';
import { CrossIndustryInvoiceFormVerbosity } from './cii-form.component';

@Component({
  selector: 'app-cii-form-exchanged-document-context',
  imports: [ReactiveFormsModule],
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
      <div>
        <label class="form-label text-truncate" for="businessProcessSpecifiedDocumentContextParameterId"> <span class="fw-semibold">BT-23</span> - Business process type </label>
        <div class="editor__control">
          <input id="businessProcessSpecifiedDocumentContextParameterId" class="form-control" formControlName="businessProcessSpecifiedDocumentContextParameterId" />
        </div>
        <p id="businessProcessSpecifiedDocumentContextParameterIdHelp" class="form-text">
          Identifies the business process context in which the transaction appears, to enable the Buyer to process the Invoice in an appropriate way.
        </p>
      </div>

      <div>
        <label class="form-label text-truncate" for="guidelineSpecifiedDocumentContextParameterId"> <span class="fw-semibold">BT-24</span> - Specification identifier </label>
        <div class="editor__control">
          <select id="guidelineSpecifiedDocumentContextParameterId" class="form-select" formControlName="guidelineSpecifiedDocumentContextParameterId">
            <option value="" class="text-body-tertiary" selected>Choose a profile</option>
            <option value="minimum">Minimum</option>
            <option value="basic-wl">Basic WL</option>
            <option value="basic">Basic</option>
            <option value="en16931">EN 16931</option>
            <option value="extended">Extended</option>
          </select>
        </div>
        <p id="guidelineSpecifiedDocumentContextParameterIdHelp" class="form-text">
          An identification of the specification containing the total set of rules regarding semantic content, cardinalities and business rules to which the data contained in the
          instance document conforms.
        </p>
        @if (showBusinessRules()) {
          <div class="form-text">
            <div class="fw-semibold">Business Rules</div>
            <ul>
              <li><span class="fw-semibold">BR-1</span>: An Invoice shall have a Specification identifier.</li>
            </ul>
          </div>
        }
        @if (showRemarks()) {
          <div class="alert alert-light small">
            <i class="bi bi-info-circle"></i>
            This identifies compliance or conformance to the specification. Conformant invoices specify: urn:cen.eu:en16931:2017. Invoices, compliant to a user specification may
            identify that user specification here. No identification scheme is to be used.
          </div>
        }
      </div>
    </div>
  `,
})
export class CiiFormExchangedDocumentContextComponent {
  formGroupName = input.required<string>();
  verbosity = input<CrossIndustryInvoiceFormVerbosity>('normal');
  disabled = input<boolean>(false);

  protected showBusinessRules = computed(() => this.verbosity() == 'normal' || this.verbosity() == 'detailed');
  protected showRemarks = computed(() => this.verbosity() == 'detailed');
}
