import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { EditorSettings } from '../../../editor-settings.service';
import { CiiFormControlComponent } from './base/cii-form-control.component';
import { ciiTerms, requireTerm } from '../constants/cii-terms';

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
      <app-cii-form-control [term]="bt40" [settings]="settings()" #bt40Control>
        <input [id]="bt40Control.controlId()" class="form-control" formControlName="countryId" placeholder="FR" />
      </app-cii-form-control>
    </div>
  `,
})
export class CiiFormSellerTradePartyPostalTradeAddress {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();

  protected bt40 = requireTerm('BT-40');
}
