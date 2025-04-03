import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { EditorSettings } from '../../editor-settings.service';
import { CiiFormControlComponent } from './base/cii-form-control.component';

@Component({
  selector: 'app-cii-form-seller-trade-party-postal-trade-address',
  imports: [ReactiveFormsModule, CiiFormControlComponent],
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
      <app-cii-form-control
        term="BT-40"
        name="Seller country code"
        [description]="bt40Description"
        [businessRules]="[{ id: 'BR-9', template: br9 }]"
        [remarks]="[bt40Remark]"
        [settings]="settings()"
        #bt40Control
      >
        <ng-template #bt40Description> A code that identifies the country.</ng-template>
        <ng-template #br9>The Seller postal address (BG-5) shall contain a Seller country code.</ng-template>
        <ng-template #bt40Remark>
          If no tax representative is specified, this is the country where VAT is liable. The lists of valid countries are registered with the ISO 3166-1 Maintenance agency, "Codes
          for the representation of names of countries and their subdivisions".
        </ng-template>

        <input [id]="bt40Control.controlId()" class="form-control" formControlName="countryId" placeholder="FR" />
      </app-cii-form-control>
    </div>
  `,
  styles: ``,
})
export class CiiFormSellerTradePartyPostalTradeAddress {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();
}
