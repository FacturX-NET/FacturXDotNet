import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { EditorSettings } from '../../../editor-settings.service';
import { CiiFormControlComponent } from './base/cii-form-control.component';

@Component({
  selector: 'app-cii-form-buyer-order-referenced-document',
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
      <app-cii-form-control term="BT-13" name="Purchase order reference" [description]="description" [chorusProRemarks]="[chorusProRemark]" [settings]="settings()" #control>
        <ng-template #description>An identifier of a referenced purchase order, issued by the Buyer.</ng-template>
        <ng-template #chorusProRemark>
          For the public sector, this is the "Engagement Juridique" (Legal Commitment). It is mandatory for some buyers. You should refer to the ChorusPro Directory to identify
          these public entity buyers that make it mandatory.
        </ng-template>

        <input [id]="control.controlId()" class="form-control" formControlName="issuerAssignedId" />
      </app-cii-form-control>
    </div>
  `,
})
export class CiiFormBuyerOrderReferencedDocumentComponent {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();
}
