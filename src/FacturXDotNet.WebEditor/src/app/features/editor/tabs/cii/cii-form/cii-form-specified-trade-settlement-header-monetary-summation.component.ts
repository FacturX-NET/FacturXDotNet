import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { EditorSettings } from '../../../editor-settings.service';
import { CiiFormControlComponent } from './base/cii-form-control.component';
import { ciiTerms, requireTerm } from '../constants/cii-terms';

@Component({
  selector: 'app-cii-form-specified-trade-settlement-header-monetary-summation',
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
    <div [formGroupName]="formGroupName()" xmlns="http://www.w3.org/1999/html">
      <app-cii-form-control [term]="bt109" [settings]="settings()" #bt109Control>
        <input [id]="bt109Control.controlId()" class="form-control" formControlName="taxBasisTotalAmount" placeholder="100.00" />
      </app-cii-form-control>

      <div class="row">
        <div class="col">
          <app-cii-form-control [term]="bt110" [settings]="settings()" #bt110Control>
            <input [id]="bt110Control.controlId()" class="form-control" formControlName="taxTotalAmount" placeholder="4.90" />
          </app-cii-form-control>
        </div>
        <div class="col">
          <app-cii-form-control [term]="bt1101" [settings]="settings()" #bt1101Control>
            <input [id]="bt1101Control.controlId()" class="form-control" formControlName="taxTotalAmountCurrencyId" placeholder="EUR" />
          </app-cii-form-control>
        </div>
      </div>

      <app-cii-form-control [term]="bt112" [settings]="settings()" #bt112Control>
        <input [id]="bt112Control.controlId()" class="form-control" formControlName="grandTotalAmount" placeholder="104.90" />
      </app-cii-form-control>

      <app-cii-form-control [term]="bt115" [settings]="settings()" #bt115Control>
        <input [id]="bt115Control.controlId()" class="form-control" formControlName="duePayableAmount" placeholder="104.90" />
      </app-cii-form-control>
    </div>
  `,
})
export class CiiFormSpecifiedTradeSettlementHeaderMonetarySummation {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();

  protected bt109 = requireTerm('BT-109');
  protected bt110 = requireTerm('BT-110');
  protected bt1101 = requireTerm('BT-110-1');
  protected bt112 = requireTerm('BT-112');
  protected bt115 = requireTerm('BT-115');
}
