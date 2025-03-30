import { Component, computed, inject, input } from '@angular/core';
import { CrossIndustryInvoiceFormVerbosity } from './cii-form.component';
import { ControlContainer, FormsModule, ReactiveFormsModule } from '@angular/forms';

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
      <div class="mb-3">
        <label class="form-label text-truncate" for="buyerLegalId"> <span class="fw-semibold">BT-47</span> - Buyer legal registration identifier </label>
        <input id="buyerLegalId" class="form-control" formControlName="id" />
        <p id="buyerLegalIdHelp" class="form-text">An identifier issued by an official registrar that identifies the Buyer as a legal entity or person.</p>
        @if (showRemarks()) {
          <div class="alert alert-light small">
            <i class="bi bi-info-circle"></i>
            If no identification scheme is specified, it should be known by Buyer and Seller, e.g. the identifier that is exclusively used in the applicable legal environment.
          </div>
          <div class="alert alert-light small">
            <div class="fw-semibold"><i class="bi bi-info-circle"></i> CHORUSPRO</div>
            The identifier of the buyer (public entity) is mandatory and is always a SIRET number.
          </div>
        }
      </div>

      <div class="mb-3">
        <label class="form-label text-truncate" for="buyerLegalIdScheme"> <span class="fw-semibold">BT-47-1</span> - Scheme identifier </label>
        <input id="buyerLegalIdScheme" class="form-control" formControlName="idSchemeId" />
        <p id="buyerLegalIdSchemeHelp" class="form-text">The identification scheme identifier of the Buyer legal registration identifier.</p>
        @if (showRemarks()) {
          <div class="alert alert-light small">
            <i class="bi bi-info-circle"></i>
            If used, the identification scheme shall be chosen from the entries of the list published by the ISO 6523 maintenance agency. For a SIREN or a SIRET, the value of this
            field is "0002".
          </div>
        }
      </div>
    </div>
  `,
  styles: ``,
})
export class CiiFormBuyerTradePartySpecifiedLegalOrganizationComponent {
  formGroupName = input.required<string>();
  verbosity = input<CrossIndustryInvoiceFormVerbosity>('normal');
  disabled = input<boolean>(false);

  protected showBusinessRules = computed(() => this.verbosity() == 'normal' || this.verbosity() == 'detailed');
  protected showRemarks = computed(() => this.verbosity() == 'detailed');
}
