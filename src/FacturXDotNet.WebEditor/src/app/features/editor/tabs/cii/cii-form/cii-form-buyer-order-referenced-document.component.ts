import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { EditorSettings } from '../../../editor-settings.service';
import { CiiFormControlComponent } from './base/cii-form-control.component';
import { ciiTerms, requireTerm } from '../constants/cii-terms';

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
      <app-cii-form-control [term]="bt13" [settings]="settings()" #control>
        <input [id]="control.controlId()" class="form-control" formControlName="issuerAssignedId" />
      </app-cii-form-control>
    </div>
  `,
})
export class CiiFormBuyerOrderReferencedDocumentComponent {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();

  protected bt13 = requireTerm('BT-13');
}
