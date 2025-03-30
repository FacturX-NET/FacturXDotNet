import { Component, computed, inject, input } from '@angular/core';
import { CrossIndustryInvoiceFormVerbosity } from './cii-form.component';
import { ControlContainer, FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-cii-form-seller-trade-party-postal-trade-address',
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
        <label class="form-label text-truncate" for="sellerAddressCountryId"> <span class="fw-semibold">BT-40</span> - Seller country code </label>
        <div class="editor__control">
          <input id="sellerAddressCountryId" class="form-control" formControlName="countryId" />
        </div>
        <p id="sellerAddressCountryIdHelp" class="form-text">A code that identifies the country.</p>
        @if (showBusinessRules()) {
          <div class="form-text">
            <div class="fw-semibold">Business Rules</div>
            <ul>
              <li><span class="fw-semibold">BR-9</span>: The Seller postal address (BG-5) shall contain a Seller country code.</li>
            </ul>
          </div>
        }
        @if (showRemarks()) {
          <div class="alert alert-light small">
            <i class="bi bi-info-circle"></i>
            If no tax representative is specified, this is the country where VAT is liable. The lists of valid countries are registered with the ISO 3166-1 Maintenance agency,
            "Codes for the representation of names of countries and their subdivisions".
          </div>
        }
      </div>
    </div>
  `,
  styles: ``,
})
export class CiiFormSellerTradePartyPostalTradeAddress {
  formGroupName = input.required<string>();
  verbosity = input<CrossIndustryInvoiceFormVerbosity>('normal');
  disabled = input<boolean>(false);

  protected showBusinessRules = computed(() => this.verbosity() == 'normal' || this.verbosity() == 'detailed');
  protected showRemarks = computed(() => this.verbosity() == 'detailed');
}
