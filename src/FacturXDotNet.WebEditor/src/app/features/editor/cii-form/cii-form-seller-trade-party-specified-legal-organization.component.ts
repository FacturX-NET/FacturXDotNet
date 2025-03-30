import { Component, computed, inject, input } from '@angular/core';
import { CrossIndustryInvoiceFormVerbosity } from './cii-form.component';
import { ControlContainer, FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-cii-form-seller-trade-party-specified-legal-organization',
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
      <div class="mb-3">
        <label class="form-label text-truncate" for="sellerLegalId"> <span class="fw-semibold">BT-30</span> - Seller legal registration identifier </label>
        <input id="sellerLegalId" class="form-control" formControlName="id" />
        <p id="sellerLegalIdHelp" class="form-text">An identifier issued by an official registrar that identifies the Seller as a legal entity or person.</p>
        @if (showBusinessRules()) {
          <div class="form-text">
            <div class="fw-semibold">Business Rules</div>
            <ul>
              <span class="fw-semibold">BR-CO-26</span
              >: In order for the buyer to automatically identify a supplier, the Seller identifier (BT-29), the Seller legal registration identifier (BT-30) and/or the Seller VAT
              identifier (BT-31) shall be present.
            </ul>
          </div>
        }
      </div>

      <div class="mb-3">
        <label class="form-label text-truncate" for="sellerLegalIdScheme"> <span class="fw-semibold">BT-30-1</span> - Scheme identifier </label>
        <input id="sellerLegalIdScheme" class="form-control" formControlName="idSchemeId" />
        <p id="sellerLegalIdSchemeHelp" class="form-text">The identification scheme identifier of the Seller legal registration identifier.</p>
        @if (showRemarks()) {
          <div class="alert alert-light small">
            <i class="bi bi-info-circle"></i>
            If used, the identification scheme shall be chosen from the entries of the list published by the ISO/IEC 6523 maintenance agency. For a SIREN or a SIRET, the value of
            this field is "0002".
          </div>
        }
      </div>
    </div>
  `,
  styles: ``,
})
export class CiiFormSellerTradePartySpecifiedLegalOrganizationComponent {
  formGroupName = input.required<string>();
  verbosity = input<CrossIndustryInvoiceFormVerbosity>('normal');
  disabled = input<boolean>(false);

  protected showBusinessRules = computed(() => this.verbosity() == 'normal' || this.verbosity() == 'detailed');
  protected showRemarks = computed(() => this.verbosity() == 'detailed');
}
