import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { CiiFormSellerTradePartySpecifiedLegalOrganizationComponent } from './cii-form-seller-trade-party-specified-legal-organization.component';
import { CiiFormSellerTradePartyPostalTradeAddress } from './cii-form-seller-trade-party-postal-trade-address';
import { CiiFormSellerTradePartySpecifiedTaxRegistration } from './cii-form-seller-trade-party-specified-tax-registration';
import { EditorSettings } from '../editor-settings.service';
import { CiiFormBuyerTradePartySpecifiedLegalOrganizationComponent } from './cii-form-buyer-trade-party-specified-legal-organization.component';
import { CiiFormParentContainerComponent } from './components/cii-form-parent-container.component';

@Component({
  selector: 'app-cii-form-seller-trade-party',
  imports: [
    ReactiveFormsModule,
    CiiFormSellerTradePartySpecifiedLegalOrganizationComponent,
    CiiFormSellerTradePartyPostalTradeAddress,
    CiiFormSellerTradePartySpecifiedTaxRegistration,

    CiiFormParentContainerComponent,
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
          <label class="form-label text-truncate" for="sellerName"> <span id="BT-27" class="fw-semibold">BT-27</span> - Seller name </label>
          <input id="sellerName" class="form-control" formControlName="name" placeholder="LE FOURNISSEUR" />
          <p id="sellerNameHelp" class="form-text">
            The full formal name by which the Seller is registered in the national registry of legal entities or as a Taxable person or otherwise trades as a person or persons.
          </p>
        </div>
        @if (settings()?.showBusinessRules === true) {
          <div class="form-text">
            <div class="fw-semibold">Business Rules</div>
            <ul>
              <li><span id="BR-6" class="fw-semibold">BR-6</span>: An Invoice shall contain the Seller name.</li>
            </ul>
          </div>
        }
      </div>

      <app-cii-form-parent-container term="BT-30-00" name="SELLER LEGAL ORGANIZATION" [description]="description" [settings]="settings()">
        <ng-template #description>Details about the organization.</ng-template>

        <app-cii-form-seller-trade-party-specified-legal-organization
          formGroupName="specifiedLegalOrganization"
          [settings]="settings()"
        ></app-cii-form-seller-trade-party-specified-legal-organization>
      </app-cii-form-parent-container>

      <app-cii-form-parent-container
        term="BG-5"
        name="SELLER POSTAL ADDRESS"
        [description]="description"
        [remarks]="[remark]"
        [businessRules]="[{ id: 'BR-8', template: br8 }]"
        [settings]="settings()"
      >
        <ng-template #description>A group of business terms providing information about the address of the Seller.</ng-template>
        <ng-template #br8> An Invoice shall contain the Seller postal address. </ng-template>
        <ng-template #remark>
          Sufficient components of the address are to be filled in order to comply to legal requirements. Like any address, the fields necessary to define the address must appear.
          The country code is mandatory.
        </ng-template>

        <app-cii-form-seller-trade-party-postal-trade-address formGroupName="postalTradeAddress" [settings]="settings()"></app-cii-form-seller-trade-party-postal-trade-address>
      </app-cii-form-parent-container>

      <app-cii-form-parent-container term="BT-31-00" name="SELLER VAT IDENTIFIER" [description]="description" [settings]="settings()">
        <ng-template #description>Detailed information on tax information of the seller.</ng-template>

        <app-cii-form-seller-trade-party-specified-tax-registration
          formGroupName="specifiedTaxRegistration"
          [settings]="settings()"
        ></app-cii-form-seller-trade-party-specified-tax-registration>
      </app-cii-form-parent-container>
    </div>
  `,
  styles: ``,
})
export class CiiFormSellerTradePartyComponent {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();
}
