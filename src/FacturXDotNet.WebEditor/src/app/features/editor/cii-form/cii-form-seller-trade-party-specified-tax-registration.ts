import { Component, computed, inject, input } from '@angular/core';
import { CrossIndustryInvoiceFormVerbosity } from './cii-form.component';
import { ControlContainer, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EditorSettings } from '../editor-settings.service';

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
      <div class="row">
        <div class="col">
          <div class="editor__control">
            <label class="form-label text-truncate" for="sellerTaxId"> <span class="fw-semibold">BT-31</span> - Seller VAT identifier </label>
            <input id="sellerTaxId" class="form-control" formControlName="id" placeholder="FR11123456782" />
            <p id="sellerTaxIdHelp" class="form-text">The Seller's VAT identifier (also known as Seller VAT identification number).</p>
          </div>
          @if (settings()?.showBusinessRules === true) {
            <div class="form-text">
              <div class="fw-semibold">Business Rules</div>
              <ul>
                <li>
                  <span class="fw-semibold">BR-CO-9</span>: The Seller VAT identifier, the Seller tax representative VAT identifier (BT-63) and the Buyer VAT identifier (BT-48)
                  shall have a prefix in accordance with ISO code ISO 3166-1 alpha-2 by which the country of issue may be identified. Nevertheless, Greece may use the prefix ‘EL’.
                </li>
              </ul>
              <ul>
                <span class="fw-semibold">BR-CO-26</span
                >: In order for the buyer to automatically identify a supplier, the Seller identifier (BT-29), the Seller legal registration identifier (BT-30) and/or the Seller
                VAT identifier shall be present.
              </ul>
            </div>
          }
          @if (settings()?.showRemarks === true) {
            <div class="alert alert-light small">
              <i class="bi bi-info-circle"></i>
              VAT number prefixed by a country code. A VAT registered Supplier shall include his VAT ID, except when he uses a tax representative.
            </div>
          }
        </div>
        <div class="col">
          <div class="editor__control">
            <label class="form-label text-truncate" for="sellerTaxIdScheme"> <span class="fw-semibold">BT-31-0</span> - Tax Scheme identifier</label>
            <select id="sellerTaxIdScheme" class="form-select" formControlName="idSchemeId">
              <option value="vat" selected>VAT</option>
            </select>
            <p id="sellerTaxIdSchemeHelp" class="form-text">Scheme identifier for supplier VAT identifier.</p>
          </div>
          @if (settings()?.showRemarks === true) {
            <div class="alert alert-light small">
              <i class="bi bi-info-circle"></i>
              Value = VA
            </div>
          }
        </div>
      </div>
    </div>
  `,
  styles: ``,
})
export class CiiFormSellerTradePartySpecifiedTaxRegistration {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();
}
