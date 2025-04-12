import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { CiiFormSellerTradePartySpecifiedLegalOrganizationComponent } from './cii-form-seller-trade-party-specified-legal-organization.component';
import { CiiFormSellerTradePartyPostalTradeAddress } from './cii-form-seller-trade-party-postal-trade-address';
import { CiiFormSellerTradePartySpecifiedTaxRegistration } from './cii-form-seller-trade-party-specified-tax-registration';
import { EditorSettings } from '../../../editor-settings.service';
import { CiiFormParentContainerComponent } from './base/cii-form-parent-container.component';
import { CiiFormControlComponent } from './base/cii-form-control.component';
import { requireTerm } from '../constants/cii-terms';

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
      <app-cii-form-control [term]="bt27" [settings]="settings()" #bt27Control>
        <input [id]="bt27Control.controlId()" class="form-control" formControlName="name" placeholder="LE FOURNISSEUR" />
      </app-cii-form-control>

      <app-cii-form-parent-container [term]="bt3000" [settings]="settings()" depth="4">
        <app-cii-form-seller-trade-party-specified-legal-organization
          formGroupName="specifiedLegalOrganization"
          [settings]="settings()"
        ></app-cii-form-seller-trade-party-specified-legal-organization>
      </app-cii-form-parent-container>

      <app-cii-form-parent-container [term]="br05" [settings]="settings()" depth="4">
        <app-cii-form-seller-trade-party-postal-trade-address formGroupName="postalTradeAddress" [settings]="settings()"></app-cii-form-seller-trade-party-postal-trade-address>
      </app-cii-form-parent-container>

      <app-cii-form-parent-container [term]="bt3100" [settings]="settings()" depth="4">
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
