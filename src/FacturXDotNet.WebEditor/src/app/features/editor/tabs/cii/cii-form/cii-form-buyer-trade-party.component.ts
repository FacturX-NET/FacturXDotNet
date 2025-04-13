import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { CiiFormBuyerTradePartySpecifiedLegalOrganizationComponent } from './cii-form-buyer-trade-party-specified-legal-organization.component';
import { EditorSettings } from '../../../editor-settings.service';
import { CiiFormParentContainerComponent } from './base/cii-form-parent-container.component';
import { CiiFormControlComponent } from './base/cii-form-control.component';
import { requireTerm } from '../constants/cii-terms';

@Component({
  selector: 'app-cii-form-buyer-trade-party',
  imports: [ReactiveFormsModule, CiiFormBuyerTradePartySpecifiedLegalOrganizationComponent, CiiFormParentContainerComponent, CiiFormControlComponent],
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
      <app-cii-form-control [term]="bt44" [settings]="settings()" #control>
        <input [id]="control.controlId()" class="form-control" formControlName="name" placeholder="LE CLIENT" />
      </app-cii-form-control>

      <app-cii-form-parent-container [term]="bt4700" [settings]="settings()" depth="4">
        <app-cii-form-buyer-trade-party-specified-legal-organization
          formGroupName="specifiedLegalOrganization"
          [settings]="settings()"
        ></app-cii-form-buyer-trade-party-specified-legal-organization>
      </app-cii-form-parent-container>
    </div>
  `,
})
export class CiiFormBuyerTradePartyComponent {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();

  protected bt44 = requireTerm('BT-44');
  protected bt4700 = requireTerm('BT-47-00');
}
