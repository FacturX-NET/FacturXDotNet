import { Component, computed, inject, input } from '@angular/core';
import { CrossIndustryInvoiceFormVerbosity } from './cii-form.component';
import { ControlContainer, FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-cii-form-seller-trade-party-specified-tax-registration',
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
        <div class="row">
          <div class="col">
            <label class="form-label text-truncate" for="sellerTaxId"> <span class="fw-semibold">BT-31</span> - Seller VAT identifier </label>
            <input id="sellerTaxId" class="form-control" formControlName="id" />
          </div>
          <div class="col-4">
            <label class="form-label text-truncate" for="sellerTaxIdScheme"> Scheme </label>
            <select id="sellerTaxIdScheme" class="form-select" formControlName="idSchemeId">
              <option value="vat" selected>VAT</option>
            </select>
          </div>
        </div>
        <p id="sellerTaxIdHelp" class="form-text">The Seller's VAT identifier (also known as Seller VAT identification number).</p>
        @if (showBusinessRules()) {
          <div class="form-text">
            <div class="fw-semibold">Business Rules</div>
            <ul>
              <li>
                <span class="fw-semibold">BR-CO-9</span>: The Seller VAT identifier, the Seller tax representative VAT identifier (BT-63) and the Buyer VAT identifier (BT-48) shall
                have a prefix in accordance with ISO code ISO 3166-1 alpha-2 by which the country of issue may be identified. Nevertheless, Greece may use the prefix ‘EL’.
              </li>
            </ul>
            <ul>
              <span class="fw-semibold">BR-CO-26</span
              >: In order for the buyer to automatically identify a supplier, the Seller identifier (BT-29), the Seller legal registration identifier (BT-30) and/or the Seller VAT
              identifier shall be present.
            </ul>
          </div>
        }
        @if (showRemarks()) {
          <div class="alert alert-light small">
            <i class="bi bi-info-circle"></i>
            VAT number prefixed by a country code. A VAT registered Supplier shall include his VAT ID, except when he uses a tax representative.
          </div>
        }
      </div>
    </div>
  `,
  styles: ``,
})
export class CiiFormSellerTradePartySpecifiedTaxRegistration {
  formGroupName = input.required<string>();
  verbosity = input<CrossIndustryInvoiceFormVerbosity>('normal');
  disabled = input<boolean>(false);

  protected showBusinessRules = computed(() => this.verbosity() == 'normal' || this.verbosity() == 'detailed');
  protected showRemarks = computed(() => this.verbosity() == 'detailed');
}
