import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { EditorSettings } from '../editor-settings.service';

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
        <div class="editor__control">
          <label class="form-label text-truncate" for="sellerAddressCountryId"> <span id="BT-40" class="fw-semibold">BT-40</span> - Seller country code </label>
          <input id="sellerAddressCountryId" class="form-control" formControlName="countryId" placeholder="FR" />
          <p id="sellerAddressCountryIdHelp" class="form-text">A code that identifies the country.</p>
        </div>
        @if (settings()?.showBusinessRules === true) {
          <div class="form-text">
            <div class="fw-semibold">Business Rules</div>
            <ul>
              <li><span id="BR-9" class="fw-semibold">BR-9</span>: The Seller postal address (BG-5) shall contain a Seller country code.</li>
            </ul>
          </div>
        }
        @if (settings()?.showRemarks === true) {
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
  settings = input<EditorSettings>();
}
