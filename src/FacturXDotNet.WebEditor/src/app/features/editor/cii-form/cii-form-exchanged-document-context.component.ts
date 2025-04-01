import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { EditorSettings } from '../editor-settings.service';
import { CiiFormControlComponent } from './components/cii-form-control.component';

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
      <app-cii-form-control term="BT-23" name="Business process type" [description]="bt23Description" [settings]="settings()" #bt23Control>
        <ng-template #bt23Description
          >Identifies the business process context in which the transaction appears, to enable the Buyer to process the Invoice in an appropriate way.
        </ng-template>

        <input [id]="bt23Control.controlId()" class="form-control" formControlName="businessProcessSpecifiedDocumentContextParameterId" placeholder="A1" />
      </app-cii-form-control>

      <app-cii-form-control
        term="BT-24"
        name="Specification identifier"
        [description]="bt24Description"
        [businessRules]="[{ id: 'BR-1', template: br1 }]"
        [remarks]="[bt24Remark]"
        [settings]="settings()"
        #bt24Control
      >
        <ng-template #bt24Description>
          An identification of the specification containing the total set of rules regarding semantic content, cardinalities and business rules to which the data contained in the
          instance document conforms.
        </ng-template>
        <ng-template #br1>An Invoice shall have a Specification identifier.</ng-template>
        <ng-template #bt24Remark>
          This identifies compliance or conformance to the specification. Conformant invoices specify: urn:cen.eu:en16931:2017. Invoices, compliant to a user specification may
          identify that user specification here. No identification scheme is to be used.
        </ng-template>

        <select [id]="bt24Control.controlId()" class="form-select" formControlName="guidelineSpecifiedDocumentContextParameterId">
          <option value="" class="text-body-tertiary" selected>Choose a profile</option>
          <option value="minimum">Minimum</option>
          <option value="basic-wl">Basic WL</option>
          <option value="basic">Basic</option>
          <option value="en16931">EN 16931</option>
          <option value="extended">Extended</option>
        </select>
      </app-cii-form-control>
    </div>
  `,
})
export class CiiFormExchangedDocumentContextComponent {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();
}
