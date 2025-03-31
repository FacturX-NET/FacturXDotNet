import { Component, computed, effect, inject, input } from '@angular/core';
import { CrossIndustryInvoiceFormVerbosity } from './cii-form.component';
import { ControlContainer, FormGroupDirective, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CiiFormApplicableHeaderTradeAgreementComponent } from './cii-form-applicable-header-trade-agreement.component';
import { CollapseComponent } from '../../../core/collapse/collapse.component';
import { CiiFormSellerTradePartySpecifiedLegalOrganizationComponent } from './cii-form-seller-trade-party-specified-legal-organization.component';
import { CiiFormSellerTradePartyPostalTradeAddress } from './cii-form-seller-trade-party-postal-trade-address';
import { CiiFormSellerTradePartySpecifiedTaxRegistration } from './cii-form-seller-trade-party-specified-tax-registration';
import { EditorSettings } from '../editor-settings.service';

@Component({
  selector: 'app-cii-form-seller-trade-party',
  imports: [
    ReactiveFormsModule,
    CollapseComponent,
    CiiFormSellerTradePartySpecifiedLegalOrganizationComponent,
    CiiFormSellerTradePartyPostalTradeAddress,
    CiiFormSellerTradePartySpecifiedTaxRegistration,
  ],
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
          <label class="form-label text-truncate" for="sellerName"> <span class="fw-semibold">BT-27</span> - Seller name </label>
          <input id="sellerName" class="form-control" formControlName="name" placeholder="LE FOURNISSEUR" />
          <p id="sellerNameHelp" class="form-text">
            The full formal name by which the Seller is registered in the national registry of legal entities or as a Taxable person or otherwise trades as a person or persons.
          </p>
        </div>
        @if (settings()?.showBusinessRules === true) {
          <div class="form-text">
            <div class="fw-semibold">Business Rules</div>
            <ul>
              <li><span class="fw-semibold">BR-6</span>: An Invoice shall contain the Seller name.</li>
            </ul>
          </div>
        }
      </div>

      <h6 class="m-0">BT-30-00 - SELLER LEGAL ORGANIZATION</h6>
      <p class="form-text ps-4">Details about the organization.</p>
      <div class="ps-4">
        <div class="ps-3 border-start">
          <app-cii-form-seller-trade-party-specified-legal-organization
            formGroupName="specifiedLegalOrganization"
            [settings]="settings()"
          ></app-cii-form-seller-trade-party-specified-legal-organization>
        </div>
      </div>

      <h6 class="m-0">BG-5 - SELLER POSTAL ADDRESS</h6>
      <p class="form-text ps-4">A group of business terms providing information about the address of the Seller.</p>
      @if (settings()?.showBusinessRules === true) {
        <div class="form-text ps-4">
          <div class="fw-semibold">Business Rules</div>
          <ul>
            <li><span class="fw-semibold">BR-8</span>: An Invoice shall contain the Seller postal address.</li>
          </ul>
        </div>
      }
      @if (settings()?.showRemarks === true) {
        <div class="ps-4">
          <div class="alert alert-light small">
            <i class="bi bi-info-circle"></i>
            Sufficient components of the address are to be filled in order to comply to legal requirements. Like any address, the fields necessary to define the address must
            appear. The country code is mandatory.
          </div>
        </div>
      }
      <div class="ps-4">
        <div class="ps-3 border-start">
          <app-cii-form-seller-trade-party-postal-trade-address formGroupName="postalTradeAddress" [settings]="settings()"></app-cii-form-seller-trade-party-postal-trade-address>
        </div>
      </div>

      <h6 class="m-0">BT-31-00 - SELLER VAT IDENTIFIER</h6>
      <p class="form-text ps-4">Detailed information on tax information of the seller.</p>
      <div class="ps-4">
        <div class="ps-3 border-start">
          <app-cii-form-seller-trade-party-specified-tax-registration
            formGroupName="specifiedTaxRegistration"
            [settings]="settings()"
          ></app-cii-form-seller-trade-party-specified-tax-registration>
        </div>
      </div>
    </div>
  `,
  styles: ``,
})
export class CiiFormSellerTradePartyComponent {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();
}
