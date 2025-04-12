import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { EditorSettings } from '../../../editor-settings.service';
import { CiiFormControlComponent } from './base/cii-form-control.component';
import { ScrollToDirective } from '../../../../../core/scroll-to/scroll-to.directive';
import { ciiTerms, requireTerm } from '../constants/cii-terms';

@Component({
  selector: 'app-cii-form-seller-trade-party-specified-tax-registration',
  imports: [ReactiveFormsModule, CiiFormControlComponent, ScrollToDirective],
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
          <app-cii-form-control [term]="bt31" [settings]="settings()" #bt31Control>
            <input [id]="bt31Control.controlId()" class="form-control" formControlName="id" placeholder="FR11123456782" />
          </app-cii-form-control>
        </div>

        <div class="col">
          <app-cii-form-control [term]="bt310" [settings]="settings()" #bt310Control>
            <select [id]="bt310Control.controlId()" class="form-select" formControlName="idSchemeId">
              <option value="Vat" selected>VAT</option>
            </select>
          </app-cii-form-control>
        </div>
      </div>
    </div>
  `,
})
export class CiiFormSellerTradePartySpecifiedTaxRegistration {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();

  protected bt31 = requireTerm('BT-31');
  protected bt310 = requireTerm('BT-31-0');
}
