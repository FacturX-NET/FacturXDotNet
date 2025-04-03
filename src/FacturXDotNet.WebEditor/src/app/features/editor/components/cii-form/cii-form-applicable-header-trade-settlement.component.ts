import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { CiiFormSpecifiedTradeSettlementHeaderMonetarySummation } from './cii-form-specified-trade-settlement-header-monetary-summation.component';
import { EditorSettings } from '../../editor-settings.service';
import { CiiFormParentContainerComponent } from './base/cii-form-parent-container.component';
import { CiiFormControlComponent } from './base/cii-form-control.component';
import { ScrollToDirective } from '../../../../core/scroll-to/scroll-to.directive';

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
      <app-cii-form-control
        term="BT-5"
        name="Invoice currency code"
        [description]="description"
        [businessRules]="[{ id: 'BR-5', template: br5 }]"
        [remarks]="[remark]"
        [chorusProRemarks]="[chorusProRemark]"
        [settings]="settings()"
        #control
      >
        <ng-template #description>The currency in which all Invoice amounts are given, except for the Total VAT amount in accounting currency.</ng-template>
        <ng-template #br5>An Invoice shall have an Invoice currency code.</ng-template>
        <ng-template #remark>
          Only one currency shall be used in the Invoice, except for the <a [scrollTo]="'BT-111'">Total VAT amount in accounting currency (BT-111)</a> in accordance with article
          230 of Directive 2006/112/EC on VAT. The lists of valid currencies are registered with the ISO 4217 Maintenance Agency "Codes for the representation of currencies and
          funds".
        </ng-template>
        <ng-template #chorusProRemark>Invoices and credit notes or Chorus Pro are mono-currencies only.</ng-template>

        <input [id]="control.controlId()" class="form-control" formControlName="invoiceCurrencyCode" placeholder="EUR" />
      </app-cii-form-control>

      <app-cii-form-parent-container
        term="BG-22"
        name="DOCUMENT TOTALS"
        [description]="description"
        [remarks]="[remark]"
        [chorusProRemarks]="[chorusProRemark]"
        [settings]="settings()"
      >
        <ng-template #description> A group of business terms providing the monetary totals for the Invoice.</ng-template>
        <ng-template #remark>
          This group may be used to give prior notice in the invoice that payment will be made through a SEPA or other direct debit initiated by the Seller, in accordance with the
          rules of the SEPA or other direct debit scheme.
        </ng-template>
        <ng-template #chorusProRemark>
          Amounts in an invoice are expressed by a figure on 19 positions. They can not have more than two decimals. The separator is <code>.</code> (dot).
        </ng-template>

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
}
