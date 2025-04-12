import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { CiiFormSellerTradePartySpecifiedLegalOrganizationComponent } from './cii-form-seller-trade-party-specified-legal-organization.component';
import { CiiFormSellerTradePartyPostalTradeAddress } from './cii-form-seller-trade-party-postal-trade-address';
import { CiiFormSellerTradePartySpecifiedTaxRegistration } from './cii-form-seller-trade-party-specified-tax-registration';
import { EditorSettings } from '../../../editor-settings.service';
import { CiiFormParentContainerComponent } from './base/cii-form-parent-container.component';
import { CiiFormControlComponent } from './base/cii-form-control.component';
import { ciiTerms, requireTerm } from '../constants/cii-terms';

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
      <app-cii-form-control [term]="bt27" [description]="bt27Description" [businessRules]="[{ id: 'BR-06', template: br6 }]" [settings]="settings()" #bt27Control>
        <ng-template #bt27Description>
          The full formal name by which the Seller is registered in the national registry of legal entities or as a Taxable person or otherwise trades as a person or persons.
        </ng-template>
        <ng-template #br6>An Invoice shall contain the Seller name.</ng-template>

        <input [id]="bt27Control.controlId()" class="form-control" formControlName="name" placeholder="LE FOURNISSEUR" />
      </app-cii-form-control>

      <app-cii-form-parent-container [term]="bt3000" [description]="description" [settings]="settings()" depth="4">
        <ng-template #description>Details about the organization.</ng-template>

        <app-cii-form-seller-trade-party-specified-legal-organization
          formGroupName="specifiedLegalOrganization"
          [settings]="settings()"
        ></app-cii-form-seller-trade-party-specified-legal-organization>
      </app-cii-form-parent-container>

      <app-cii-form-parent-container
        [term]="br05"
        [description]="description"
        [remarks]="[remark]"
        [businessRules]="[{ id: 'BR-08', template: br8 }]"
        [settings]="settings()"
        depth="4"
      >
        <ng-template #description>A group of business terms providing information about the address of the Seller.</ng-template>
        <ng-template #br8> An Invoice shall contain the Seller postal address.</ng-template>
        <ng-template #remark>
          Sufficient components of the address are to be filled in order to comply to legal requirements. Like any address, the fields necessary to define the address must appear.
          The country code is mandatory.
        </ng-template>

        <app-cii-form-seller-trade-party-postal-trade-address formGroupName="postalTradeAddress" [settings]="settings()"></app-cii-form-seller-trade-party-postal-trade-address>
      </app-cii-form-parent-container>

      <app-cii-form-parent-container [term]="bt3100" [description]="description" [settings]="settings()" depth="4">
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

  protected br05 = requireTerm('BR-05');
  protected bt27 = requireTerm('BT-27');
  protected bt3000 = requireTerm('BT-30-00');
  protected bt3100 = requireTerm('BT-31-00');
}
