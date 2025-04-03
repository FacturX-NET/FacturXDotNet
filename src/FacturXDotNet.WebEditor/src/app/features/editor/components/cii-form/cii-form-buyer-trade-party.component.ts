import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { CiiFormBuyerTradePartySpecifiedLegalOrganizationComponent } from './cii-form-buyer-trade-party-specified-legal-organization.component';
import { EditorSettings } from '../../editor-settings.service';
import { CiiFormParentContainerComponent } from './base/cii-form-parent-container.component';
import { CiiFormControlComponent } from './base/cii-form-control.component';

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
      <app-cii-form-control term="BT-44" name="Buyer name" [description]="description" [businessRules]="[{ id: 'BR-7', template: br7 }]" [settings]="settings()" #control>
        <ng-template #description>The full name of the Buyer.</ng-template>
        <ng-template #br7>An Invoice shall contain the Buyer name.</ng-template>

        <input [id]="control.controlId()" class="form-control" formControlName="name" placeholder="LE CLIENT" />
      </app-cii-form-control>

      <app-cii-form-parent-container term="BT-47-00" name="BUYER LEGAL REGISTRATION IDENTIFIER" [description]="description" [settings]="settings()">
        <ng-template #description>Details about the organization.</ng-template>

        <app-cii-form-buyer-trade-party-specified-legal-organization
          formGroupName="specifiedLegalOrganization"
          [settings]="settings()"
        ></app-cii-form-buyer-trade-party-specified-legal-organization>
      </app-cii-form-parent-container>
    </div>
  `,
  styles: ``,
})
export class CiiFormBuyerTradePartyComponent {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();
}
