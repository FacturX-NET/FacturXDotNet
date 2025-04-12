import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { EditorSettings } from '../../../editor-settings.service';
import { CiiFormControlComponent } from './base/cii-form-control.component';
import { ciiTerms, requireTerm } from '../constants/cii-terms';

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
          <app-cii-form-control
            [term]="bt47"
            [description]="bt47Description"
            [remarks]="[bt47Remark]"
            [chorusProRemarks]="[bt47ChorusProRemark]"
            [settings]="settings()"
            #bt47Control
          >
            <ng-template #bt47Description>An identifier issued by an official registrar that identifies the Buyer as a legal entity or person.</ng-template>
            <ng-template #bt47Remark>
              If no identification scheme is specified, it should be known by Buyer and Seller, e.g. the identifier that is exclusively used in the applicable legal environment.
            </ng-template>
            <ng-template #bt47ChorusProRemark>The identifier of the buyer (public entity) is mandatory and is always a SIRET number.</ng-template>

            <input [id]="bt47Control.controlId()" class="form-control" formControlName="id" placeholder="987654321" />
          </app-cii-form-control>
        </div>

        <div class="col">
          <app-cii-form-control [term]="bt471" [description]="bt471Description" [remarks]="[bt471Description]" [settings]="settings()" #bt470Control>
            <ng-template #bt471Description>The identification scheme identifier of the Buyer legal registration identifier.</ng-template>
            <ng-template #bt471Description>
              If used, the identification scheme shall be chosen from the entries of the list published by the ISO 6523 maintenance agency. For a SIREN or a SIRET, the value of
              this field is "0002".
            </ng-template>

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
