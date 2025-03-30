import { Component, computed, input } from '@angular/core';
import { ControlContainer, FormGroupDirective, FormsModule, NgForm, NgModelGroup, ReactiveFormsModule } from '@angular/forms';
import { CrossIndustryInvoiceFormVerbosity } from './cii-form.component';

@Component({
  selector: 'app-cii-form-exchanged-document-context',
  imports: [ReactiveFormsModule],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
  template: `
    <div [formGroupName]="formGroupName()">
      <div class="mb-3">
        <h6>BG-2 - EXCHANGE DOCUMENT CONTEXT</h6>
        @if (showNormal()) {
          <p class="form-text">A group of business terms providing information on the business process and rules applicable to the Invoice document.</p>
        }
      </div>
      <div class="ps-5">
        <div class="mb-3">
          <label class="form-label text-truncate" for="businessProcessSpecifiedDocumentContextParameterId"> <span class="fw-semibold">BT-23</span> - Business process type </label>
          <input id="businessProcessSpecifiedDocumentContextParameterId" class="form-control" formControlName="businessProcessSpecifiedDocumentContextParameterId" />
          @if (showNormal()) {
            <p id="businessProcessSpecifiedDocumentContextParameterIdHelp" class="form-text">
              Identifies the business process context in which the transaction appears, to enable the Buyer to process the Invoice in an appropriate way.
            </p>
          }
        </div>
        <div class="mb-3">
          <label class="form-label text-truncate" for="guidelineSpecifiedDocumentContextParameterId"> <span class="fw-semibold">BT-24</span> - Specification identifier </label>
          <select id="guidelineSpecifiedDocumentContextParameterId" class="form-select" formControlName="guidelineSpecifiedDocumentContextParameterId">
            <option value="" class="text-body-tertiary" selected>Choose a profile</option>
            <option value="minimum">Minimum</option>
            <option value="basic-wl">Basic WL</option>
            <option value="basic">Basic</option>
            <option value="en16931">EN 16931</option>
            <option value="extended">Extended</option>
          </select>
          @if (showNormal()) {
            <p id="guidelineSpecifiedDocumentContextParameterIdHelp" class="form-text">
              An identification of the specification containing the total set of rules regarding semantic content, cardinalities and business rules to which the data contained in
              the instance document conforms.
            </p>
            <div class="form-text">
              <div class="fw-semibold">Business Rules</div>
              <ul>
                <span class="fw-semibold">BR-1</span
                >: An Invoice shall have a Specification identifier.
              </ul>
            </div>
          }
          @if (showDetailed()) {
            <div class="alert alert-light small">
              <i class="bi bi-info-circle"></i>
              This identifies compliance or conformance to the specification. Conformant invoices specify: urn:cen.eu:en16931:2017. Invoices, compliant to a user specification may
              identify that user specification here. No identification scheme is to be used.
            </div>
          }
        </div>
      </div>
    </div>
  `,
  styles: ``,
})
export class CiiFormExchangedDocumentContextComponent {
  formGroupName = input.required<string>();
  verbosity = input<CrossIndustryInvoiceFormVerbosity>('normal');
  disabled = input<boolean>(false);

  protected showMinimal = computed(() => this.verbosity() == 'minimal' || this.verbosity() == 'normal' || this.verbosity() == 'detailed');
  protected showNormal = computed(() => this.verbosity() == 'normal' || this.verbosity() == 'detailed');
  protected showDetailed = computed(() => this.verbosity() == 'detailed');
}
