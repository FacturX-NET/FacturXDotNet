import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { EditorSettings } from '../editor-settings.service';

@Component({
  selector: 'app-cii-form-applicable-header-trade-delivery',
  imports: [ReactiveFormsModule],
  viewProviders: [
    {
      provide: ControlContainer,
      useFactory: () => {
        return inject(ControlContainer, { skipSelf: true });
      },
    },
  ],
  template: ` <div [formGroupName]="formGroupName()"></div> `,
})
export class CiiFormApplicableHeaderTradeDeliveryComponent {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();
}
