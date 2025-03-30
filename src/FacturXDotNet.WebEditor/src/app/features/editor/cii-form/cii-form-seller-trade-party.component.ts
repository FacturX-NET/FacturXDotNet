import { Component, computed, effect, inject, input } from '@angular/core';
import { CrossIndustryInvoiceFormVerbosity } from './cii-form.component';
import { ControlContainer, FormGroupDirective, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CiiFormApplicableHeaderTradeAgreementComponent } from './cii-form-applicable-header-trade-agreement.component';
import { CollapseComponent } from '../../../core/collapse/collapse.component';
import { CiiFormSellerTradePartySpecifiedLegalOrganizationComponent } from './cii-form-seller-trade-party-specified-legal-organization.component';
import { CiiFormSellerTradePartyPostalTradeAddress } from './cii-form-seller-trade-party-postal-trade-address';
import { CiiFormSellerTradePartySpecifiedTaxRegistration } from './cii-form-seller-trade-party-specified-tax-registration';

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
        @if (showBusinessRules()) {
          <div class="form-text">
            <div class="fw-semibold">Business Rules</div>
            <ul>
              <li><span class="fw-semibold">BR-6</span>: An Invoice shall contain the Seller name.</li>
            </ul>
          </div>
        }
      </div>

      <app-collapse id="seller-legal-organization" #collapseSellerLegalOrganization>
        <h6 class="m-0" ngProjectAs="trigger">
          @if (collapseSellerLegalOrganization.collapsed()) {
            <i class="bi bi-plus fs-5"></i>
          } @else {
            <i class="bi bi-dash fs-5"></i>
          }

          BT-30-00 - SELLER LEGAL ORGANIZATION
        </h6>
        <p class="form-text ps-4">Details about the organization.</p>
        <div class="ps-4" ngProjectAs="collapsible">
          <div class="ps-3 border-start">
            <app-cii-form-seller-trade-party-specified-legal-organization
              formGroupName="specifiedLegalOrganization"
              [verbosity]="verbosity()"
              [disabled]="disabled()"
            ></app-cii-form-seller-trade-party-specified-legal-organization>
          </div>
        </div>
      </app-collapse>

      <app-collapse id="seller-postal-address" #collapseSellerPostalAddress>
        <h6 class="m-0" ngProjectAs="trigger">
          @if (collapseSellerPostalAddress.collapsed()) {
            <i class="bi bi-plus fs-5"></i>
          } @else {
            <i class="bi bi-dash fs-5"></i>
          }

          BG-5 - SELLER POSTAL ADDRESS
        </h6>
        <p class="form-text ps-4">A group of business terms providing information about the address of the Seller.</p>
        @if (showBusinessRules()) {
          <div class="form-text ps-4">
            <div class="fw-semibold">Business Rules</div>
            <ul>
              <li><span class="fw-semibold">BR-8</span>: An Invoice shall contain the Seller postal address.</li>
            </ul>
          </div>
        }
        @if (showRemarks()) {
          <div class="ps-4">
            <div class="alert alert-light small">
              <i class="bi bi-info-circle"></i>
              Sufficient components of the address are to be filled in order to comply to legal requirements. Like any address, the fields necessary to define the address must
              appear. The country code is mandatory.
            </div>
          </div>
        }
        <div class="ps-4" ngProjectAs="collapsible">
          <div class="ps-3 border-start">
            <app-cii-form-seller-trade-party-postal-trade-address
              formGroupName="postalTradeAddress"
              [verbosity]="verbosity()"
              [disabled]="disabled()"
            ></app-cii-form-seller-trade-party-postal-trade-address>
          </div>
        </div>
      </app-collapse>

      <app-collapse id="seller-vat-identifier" #collapseSellerVatIdentifier>
        <h6 class="m-0" ngProjectAs="trigger">
          @if (collapseSellerVatIdentifier.collapsed()) {
            <i class="bi bi-plus fs-5"></i>
          } @else {
            <i class="bi bi-dash fs-5"></i>
          }

          BT-31-00 - SELLER VAT IDENTIFIER
        </h6>
        <p class="form-text ps-4">Detailed information on tax information of the seller.</p>
        <div class="ps-4" ngProjectAs="collapsible">
          <div class="ps-3 border-start">
            <app-cii-form-seller-trade-party-specified-tax-registration
              formGroupName="specifiedTaxRegistration"
              [verbosity]="verbosity()"
              [disabled]="disabled()"
            ></app-cii-form-seller-trade-party-specified-tax-registration>
          </div>
        </div>
      </app-collapse>
    </div>
  `,
  styles: ``,
})
export class CiiFormSellerTradePartyComponent {
  formGroupName = input.required<string>();
  verbosity = input<CrossIndustryInvoiceFormVerbosity>('normal');
  disabled = input<boolean>(false);

  protected showBusinessRules = computed(() => this.verbosity() == 'normal' || this.verbosity() == 'detailed');
  protected showRemarks = computed(() => this.verbosity() == 'detailed');
}
