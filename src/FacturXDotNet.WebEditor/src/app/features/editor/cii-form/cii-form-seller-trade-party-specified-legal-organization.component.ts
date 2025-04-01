import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { EditorSettings } from '../editor-settings.service';
import { CiiFormControlComponent } from './components/cii-form-control.component';
import { ScrollToDirective } from '../../../core/scroll-to/scroll-to.directive';

@Component({
  selector: 'app-cii-form-seller-trade-party-specified-legal-organization',
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
          <app-cii-form-control
            term="BT-30"
            name="Seller legal registration identifier"
            [description]="bt30Description"
            [businessRules]="[{ id: 'BR-CO-26', template: brCo26 }]"
            [settings]="settings()"
            #bt30Control
          >
            <ng-template #bt30Description> An identifier issued by an official registrar that identifies the Seller as a legal entity or person. </ng-template>
            <ng-template #brCo26>
              In order for the buyer to automatically identify a supplier, the <a [scrollTo]="'BT-29'">Seller identifier (BT-29)</a>, the
              <a [scrollTo]="'BT-30'">Seller legal registration identifier (BT-30)</a> and/or the <a [scrollTo]="'BT-31'">Seller VAT identifier (BT-31)</a> shall be present.
            </ng-template>

            <input [id]="bt30Control.controlId()" class="form-control" formControlName="id" placeholder="123456782" />
          </app-cii-form-control>
        </div>
        <div class="col">
          <div class="mb-3">
            <app-cii-form-control term="BT-30-1" name="Scheme identifier" [description]="bt301Description" [remarks]="[bt301remark]" [settings]="settings()" #bt301Control>
              <ng-template #bt301Description> The identification scheme identifier of the Seller legal registration identifier. </ng-template>
              <ng-template #bt301remark>
                If used, the identification scheme shall be chosen from the entries of the list published by the ISO/IEC 6523 maintenance agency. For a SIREN or a SIRET, the value
                of this field is "0002".
              </ng-template>

              <input [id]="bt301Control.controlId()" class="form-control" formControlName="idSchemeId" placeholder="0002" />
            </app-cii-form-control>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: ``,
})
export class CiiFormSellerTradePartySpecifiedLegalOrganizationComponent {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();
}
