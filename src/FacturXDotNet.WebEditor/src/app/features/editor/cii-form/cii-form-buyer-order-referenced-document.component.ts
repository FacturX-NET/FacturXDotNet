import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { EditorSettings } from '../editor-settings.service';

@Component({
  selector: 'app-cii-form-buyer-order-referenced-document',
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
          <label class="form-label text-truncate" for="issuerAssignedId"> <span id="BT-13" class="fw-semibold">BT-13</span> - Purchase order reference </label>
          <input id="issuerAssignedId" class="form-control" formControlName="issuerAssignedId" />
          <p id="issuerAssignedIdHelp" class="form-text">An identifier of a referenced purchase order, issued by the Buyer.</p>
        </div>
        @if (settings()?.showChorusProRemarks === true) {
          <div class="alert alert-light small">
            <div class="fw-semibold"><i class="bi bi-info-circle"></i> CHORUSPRO</div>
            For the public sector, this is the "Engagement Juridique" (Legal Commitment). It is mandatory for some buyers. You should refer to the ChorusPro Directory to identify
            these public entity buyers that make it mandatory.
          </div>
        }
      </div>
    </div>
  `,
})
export class CiiFormBuyerOrderReferencedDocumentComponent {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();
}
