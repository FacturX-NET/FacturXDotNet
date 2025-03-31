import { Component, computed, inject, input } from '@angular/core';
import { CrossIndustryInvoiceFormVerbosity } from './cii-form.component';
import { ControlContainer, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EditorSettings } from '../editor-settings.service';

@Component({
  selector: 'app-cii-form-buyer-trade-party-specified-legal-organization',
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
      <div class="row">
        <div class="col">
          <div class="editor__control">
            <label class="form-label text-truncate" for="buyerLegalId"> <span id="BT-47" class="fw-semibold">BT-47</span> - Buyer legal registration identifier </label>
            <input id="buyerLegalId" class="form-control" formControlName="id" placeholder="987654321" />
            <p id="buyerLegalIdHelp" class="form-text">An identifier issued by an official registrar that identifies the Buyer as a legal entity or person.</p>
          </div>
          @if (settings()?.showRemarks === true) {
            <div class="alert alert-light small">
              <i class="bi bi-info-circle"></i>
              If no identification scheme is specified, it should be known by Buyer and Seller, e.g. the identifier that is exclusively used in the applicable legal environment.
            </div>
          }
          @if (settings()?.showChorusProRemarks === true) {
            <div class="alert alert-light small">
              <div class="fw-semibold"><i class="bi bi-info-circle"></i> CHORUSPRO</div>
              The identifier of the buyer (public entity) is mandatory and is always a SIRET number.
            </div>
          }
        </div>

        <div class="col">
          <div class="editor__control">
            <label class="form-label text-truncate" for="buyerLegalIdScheme"> <span id="BT-47-1" class="fw-semibold">BT-47-1</span> - Scheme identifier </label>
            <input id="buyerLegalIdScheme" class="form-control" formControlName="idSchemeId" placeholder="0002" />
            <p id="buyerLegalIdSchemeHelp" class="form-text">The identification scheme identifier of the Buyer legal registration identifier.</p>
          </div>
          @if (settings()?.showRemarks === true) {
            <div class="alert alert-light small">
              <i class="bi bi-info-circle"></i>
              If used, the identification scheme shall be chosen from the entries of the list published by the ISO 6523 maintenance agency. For a SIREN or a SIRET, the value of
              this field is "0002".
            </div>
          }
        </div>
      </div>
    </div>
  `,
  styles: ``,
})
export class CiiFormBuyerTradePartySpecifiedLegalOrganizationComponent {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();
}
