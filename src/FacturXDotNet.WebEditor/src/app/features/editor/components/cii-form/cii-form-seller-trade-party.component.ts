import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { CiiFormSellerTradePartySpecifiedLegalOrganizationComponent } from './cii-form-seller-trade-party-specified-legal-organization.component';
import { CiiFormSellerTradePartyPostalTradeAddress } from './cii-form-seller-trade-party-postal-trade-address';
import { CiiFormSellerTradePartySpecifiedTaxRegistration } from './cii-form-seller-trade-party-specified-tax-registration';
import { EditorSettings } from '../../editor-settings.service';
import { CiiFormParentContainerComponent } from './base/cii-form-parent-container.component';
import { CiiFormControlComponent } from './base/cii-form-control.component';

@Component({
  selector: 'app-cii-form-seller-trade-party',
  imports: [
    ReactiveFormsModule,
    CiiFormSellerTradePartySpecifiedLegalOrganizationComponent,
    CiiFormSellerTradePartyPostalTradeAddress,
    CiiFormSellerTradePartySpecifiedTaxRegistration,

    CiiFormParentContainerComponent,
    CiiFormControlComponent,
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
      <app-cii-form-control term="BT-27" name="Seller name" [description]="bt27Description" [businessRules]="[{ id: 'BR-6', template: br6 }]" [settings]="settings()" #bt27Control>
        <ng-template #bt27Description>
          The full formal name by which the Seller is registered in the national registry of legal entities or as a Taxable person or otherwise trades as a person or persons.
        </ng-template>
        <ng-template #br6>An Invoice shall contain the Seller name.</ng-template>

        <input [id]="bt27Control.controlId()" class="form-control" formControlName="name" placeholder="LE FOURNISSEUR" />
      </app-cii-form-control>

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
        <ng-template #br8> An Invoice shall contain the Seller postal address.</ng-template>
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
})
export class CiiFormSellerTradePartyComponent {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();
}
