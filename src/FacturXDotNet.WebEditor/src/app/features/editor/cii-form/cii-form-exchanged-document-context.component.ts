import { Component, computed, effect, inject, input } from '@angular/core';
import { ControlContainer, FormGroupDirective, ReactiveFormsModule } from '@angular/forms';
import { CrossIndustryInvoiceFormVerbosity } from './cii-form.component';
import { EditorSettings } from '../editor-settings.service';

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
        <div class="editor__control">
          <label class="form-label text-truncate" for="businessProcessSpecifiedDocumentContextParameterId">
            <span id="BT-23" class="fw-semibold">BT-23</span> - Business process type
          </label>
          <input
            id="businessProcessSpecifiedDocumentContextParameterId"
            class="form-control"
            formControlName="businessProcessSpecifiedDocumentContextParameterId"
            placeholder="A1"
          />
          <p id="businessProcessSpecifiedDocumentContextParameterIdHelp" class="form-text">
            Identifies the business process context in which the transaction appears, to enable the Buyer to process the Invoice in an appropriate way.
          </p>
        </div>
      </div>

      <div>
        <div class="editor__control">
          <label class="form-label text-truncate" for="guidelineSpecifiedDocumentContextParameterId">
            <span id="BT-24" class="fw-semibold">BT-24</span> - Specification identifier
          </label>
          <select id="guidelineSpecifiedDocumentContextParameterId" class="form-select" formControlName="guidelineSpecifiedDocumentContextParameterId">
            <option value="" class="text-body-tertiary" selected>Choose a profile</option>
            <option value="minimum">Minimum</option>
            <option value="basic-wl">Basic WL</option>
            <option value="basic">Basic</option>
            <option value="en16931">EN 16931</option>
            <option value="extended">Extended</option>
          </select>
          <p id="guidelineSpecifiedDocumentContextParameterIdHelp" class="form-text">
            An identification of the specification containing the total set of rules regarding semantic content, cardinalities and business rules to which the data contained in the
            instance document conforms.
          </p>
        </div>
        @if (settings()?.showBusinessRules === true) {
          <div class="form-text">
            <div class="fw-semibold">Business Rules</div>
            <ul>
              <li><span class="fw-semibold">BR-1</span>: An Invoice shall have a Specification identifier.</li>
            </ul>
          </div>
        }
        @if (settings()?.showRemarks === true) {
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
  settings = input<EditorSettings>();
}
