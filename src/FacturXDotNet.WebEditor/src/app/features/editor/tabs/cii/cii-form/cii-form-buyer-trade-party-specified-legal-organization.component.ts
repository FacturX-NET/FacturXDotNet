import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { EditorSettings } from '../../../services/editor-settings.service';
import { CiiFormControlComponent } from './base/cii-form-control.component';
import { requireTerm } from '../constants/cii-terms';

@Component({
  selector: 'app-cii-form-buyer-trade-party-specified-legal-organization',
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
          <app-cii-form-control [term]="bt47" [settings]="settings()" #bt47Control>
            <input [id]="bt47Control.controlId()" class="form-control" formControlName="id" placeholder="987654321" />
          </app-cii-form-control>
        </div>

        <div class="col">
          <app-cii-form-control [term]="bt471" [settings]="settings()" #bt470Control>
            <input [id]="bt470Control.controlId()" class="form-control" formControlName="idSchemeId" placeholder="0002" />
          </app-cii-form-control>
        </div>
      </div>
    </div>
  `,
})
export class CiiFormBuyerTradePartySpecifiedLegalOrganizationComponent {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();

  protected bt47 = requireTerm('BT-47');
  protected bt471 = requireTerm('BT-47-1');
}
