import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { CiiFormSpecifiedTradeSettlementHeaderMonetarySummation } from './cii-form-specified-trade-settlement-header-monetary-summation.component';
import { EditorSettings } from '../../../editor-settings.service';
import { CiiFormParentContainerComponent } from './base/cii-form-parent-container.component';
import { CiiFormControlComponent } from './base/cii-form-control.component';
import { ScrollToDirective } from '../../../../../core/scroll-to/scroll-to.directive';
import { ciiTerms, requireTerm } from '../constants/cii-terms';

@Component({
  selector: 'app-cii-form-applicable-header-trade-settlement',
  imports: [ReactiveFormsModule, CiiFormSpecifiedTradeSettlementHeaderMonetarySummation, CiiFormParentContainerComponent, CiiFormControlComponent, ScrollToDirective],
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
      <app-cii-form-control [term]="bt5" [settings]="settings()" #control>
        <input [id]="control.controlId()" class="form-control" formControlName="invoiceCurrencyCode" placeholder="EUR" />
      </app-cii-form-control>

      <app-cii-form-parent-container [term]="bg22" [settings]="settings()" depth="3">
        <app-cii-form-specified-trade-settlement-header-monetary-summation
          formGroupName="specifiedTradeSettlementHeaderMonetarySummation"
          [settings]="settings()"
        ></app-cii-form-specified-trade-settlement-header-monetary-summation>
      </app-cii-form-parent-container>
    </div>
  `,
})
export class CiiFormApplicableHeaderTradeSettlementComponent {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();

  protected bt5 = requireTerm('BT-5');
  protected bg22 = requireTerm('BG-22');
}
