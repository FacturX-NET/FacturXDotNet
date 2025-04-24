import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { EditorSettings } from '../../../services/editor-settings.service';
import { CiiFormControlComponent } from './base/cii-form-control.component';
import { requireTerm } from '../constants/cii-terms';

@Component({
  selector: 'app-cii-form-seller-trade-party-specified-legal-organization',
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
      <div class="row">
        <div class="col">
          <app-cii-form-control [term]="bt30" [settings]="settings()" #bt30Control>
            <input [id]="bt30Control.controlId()" class="form-control" formControlName="id" placeholder="123456782" />
          </app-cii-form-control>
        </div>
        <div class="col">
          <div class="mb-3">
            <app-cii-form-control [term]="bt301" [settings]="settings()" #bt301Control>
              <input [id]="bt301Control.controlId()" class="form-control" formControlName="idSchemeId" placeholder="0002" />
            </app-cii-form-control>
          </div>
        </div>
      </div>
    </div>
  `,
})
export class CiiFormSellerTradePartySpecifiedLegalOrganizationComponent {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();

  protected bt30 = requireTerm('BT-30');
  protected bt301 = requireTerm('BT-30-1');
}
