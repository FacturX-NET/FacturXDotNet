import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { EditorSettings } from '../../../editor-settings.service';
import { CiiFormControlComponent } from './base/cii-form-control.component';
import { requireTerm } from '../constants/cii-terms';

@Component({
  selector: 'app-cii-form-exchanged-document-context',
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
      <app-cii-form-control [term]="bt23" [settings]="settings()" #bt23Control>
        <input [id]="bt23Control.controlId()" class="form-control" formControlName="businessProcessSpecifiedDocumentContextParameterId" placeholder="A1" />
      </app-cii-form-control>

      <app-cii-form-control [term]="bt24" [settings]="settings()" #bt24Control>
        <select [id]="bt24Control.controlId()" class="form-select" formControlName="guidelineSpecifiedDocumentContextParameterId">
          <option [ngValue]="undefined" class="text-body-tertiary" selected>Choose a profile</option>
          <option value="Minimum">Minimum</option>
          <option value="BasicWl">Basic WL</option>
          <option value="Basic">Basic</option>
          <option value="En16931">EN 16931</option>
          <option value="Extended">Extended</option>
        </select>
      </app-cii-form-control>
    </div>
  `,
})
export class CiiFormExchangedDocumentContextComponent {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();

  protected bt23 = requireTerm('BT-23');
  protected bt24 = requireTerm('BT-24');
}
