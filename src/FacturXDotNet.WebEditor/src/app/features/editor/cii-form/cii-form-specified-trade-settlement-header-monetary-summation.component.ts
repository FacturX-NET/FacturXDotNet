import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { EditorSettings } from '../editor-settings.service';
import { CiiFormControlComponent } from './components/cii-form-control.component';
import { ScrollToDirective } from '../../../core/scroll-to/scroll-to.directive';

@Component({
  selector: 'app-cii-form-specified-trade-settlement-header-monetary-summation',
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
    <div [formGroupName]="formGroupName()" xmlns="http://www.w3.org/1999/html">
      <app-cii-form-control
        term="BT-109"
        name="Total amount without VAT"
        [description]="bt109Description"
        [businessRules]="[
          { id: 'BR-13', template: br13 },
          { id: 'BR-CO-13', template: brCo13 },
        ]"
        [remarks]="[bt109Remark]"
        [settings]="settings()"
        #bt109Control
      >
        <ng-template #bt109Description>The total amount of the Invoice without VAT.</ng-template>
        <ng-template #br13> An Invoice shall have the Invoice total amount without VAT.</ng-template>
        <ng-template #brCo13>
          <math>
            <a [scrollTo]="'BT-109'">
              <mi>Invoice total amount without VAT (BT-109)</mi>
            </a>
            <mo>=</mo>
            <mo>∑</mo>
            <a [scrollTo]="'BT-131'">
              <mi>Invoice line net amount (BT-131)</mi>
            </a>
            <mo>-</mo>
            <a [scrollTo]="'BT-107'">
              <mi>Sum of allowances on document level (BT-107)</mi>
            </a>
            <mo>+</mo>
            <a [scrollTo]="'BT-108'">
              <mi>Sum of charges on document level (BT-108)</mi>
            </a>
          </math>
        </ng-template>
        <ng-template #bt109Remark>
          The Invoice total amount without VAT is the Sum of Invoice line net amount minus Sum of allowances on document level plus Sum of charges on document level.
        </ng-template>

        <input [id]="bt109Control.controlId()" class="form-control" formControlName="taxBasisTotalAmount" placeholder="100.00" />
      </app-cii-form-control>

      <div class="row">
        <div class="col">
          <app-cii-form-control
            term="BT-110"
            name="Total VAT amount"
            [description]="bt110Description"
            [businessRules]="[{ id: 'BR-CO-14', template: brCo14 }]"
            [remarks]="[bt110Remark]"
            [settings]="settings()"
            #bt110Control
          >
            <ng-template #bt110Description>The total VAT amount for the Invoice.</ng-template>
            <ng-template #brCo14>
              <math>
                <a [scrollTo]="'BT-110'">
                  <mi>Invoice total VAT amount (BT-110)</mi>
                </a>
                <mo>=</mo>
                <mo>∑</mo>
                <a [scrollTo]="'BT-117'">
                  <mi>VAT category tax amount (BT-117)</mi>
                </a>
              </math>
            </ng-template>
            <ng-template #bt110Remark> The Invoice total VAT amount is the sum of all VAT category tax amounts.</ng-template>

            <input [id]="bt110Control.controlId()" class="form-control" formControlName="taxTotalAmount" placeholder="4.90" />
          </app-cii-form-control>
        </div>
        <div class="col">
          <app-cii-form-control term="BT-110-1" name="VAT currency" [remarks]="[bt1101Remark]" [settings]="settings()" #bt1101Control>
            <ng-template #bt1101Remark> The currency is mandatory to differentiate between VAT amount and VAT amount in accounting currency.</ng-template>

            <input [id]="bt1101Control.controlId()" class="form-control" formControlName="taxTotalAmountCurrencyId" placeholder="EUR" />
          </app-cii-form-control>
        </div>
      </div>

      <app-cii-form-control
        term="BT-112"
        name="Total amount with VAT"
        [description]="bt112Description"
        [businessRules]="[
          { id: 'BR-14', template: br14 },
          { id: 'BR-CO-15', template: brCo15 },
          { id: 'BR-FXEXT-CO-15', template: brFxextCo15 },
        ]"
        [remarks]="[bt112Remark]"
        [settings]="settings()"
        #bt112Control
      >
        <ng-template #bt112Description>The total amount of the Invoice with VAT.</ng-template>
        <ng-template #br14> An Invoice shall have the <a [scrollTo]="'BT-112'">Invoice total amount with VAT (BT-112)</a>.</ng-template>
        <ng-template #brCo15>
          <math>
            <mrow>
              <a [scrollTo]="'BT-112'">
                <mi>Invoice total amount with VAT (BT-112)</mi>
              </a>
              <mo>=</mo>
              <a [scrollTo]="'BT-109'">
                <mi>Invoice total amount without VAT (BT-109)</mi>
              </a>
              <mo>+</mo>
              <a [scrollTo]="'BT-110'">
                <mi>Invoice total VAT amount (BT-110)</mi>
              </a>
            </mrow>
          </math>
        </ng-template>
        <ng-template #brFxextCo15>
          For EXTENDED profile only, <a [scrollTo]="'BR-CO-15'">BR-CO-15</a> is replaced by <a [scrollTo]="'BR-FXEXT-CO-15'">BR-FXEXT-CO-15</a>, which add a tolerance of 0,01 euro
          per line, document level charge and allowance in calculation.
        </ng-template>
        <ng-template #bt112Remark> The Invoice total amount with VAT is the Invoice total amount without VAT plus the Invoice total VAT amount.</ng-template>

        <input [id]="bt112Control.controlId()" class="form-control" formControlName="grandTotalAmount" placeholder="104.90" />
      </app-cii-form-control>

      <app-cii-form-control
        term="BT-115"
        name="Amount due for payment"
        [description]="bt115Description"
        [businessRules]="[
          { id: 'BR-15', template: br15 },
          { id: 'BR-CO-16', template: brCo16 },
        ]"
        [remarks]="[bt115Remark]"
        [settings]="settings()"
        #bt115Control
      >
        <ng-template #bt115Description>The outstanding amount that is requested to be paid.</ng-template>
        <ng-template #br15>An Invoice shall have the Amount due for payment.</ng-template>
        <ng-template #brCo16>
          <math>
            <mtable>
              <mrow>
                <a [scrollTo]="'BT-115'">
                  <mi>Amount due for payment (BT-115)</mi>
                </a>
                <mo>=</mo>
                <a [scrollTo]="'BT-112'">
                  <mi>Invoice total amount with VAT (BT-112)</mi>
                </a>
                <mo>-</mo>
                <a [scrollTo]="'BT-113'">
                  <mi>Paid amount (BT-113)</mi>
                </a>
                <mo>+</mo>
                <a [scrollTo]="'BT-114'">
                  <mi>Rounding amount (BT-114)</mi>
                </a>
              </mrow>
            </mtable>
          </math>
        </ng-template>
        <ng-template #bt115Remark>
          This amount is the Invoice total amount with VAT minus the paid amount that has been paid in advance. The amount is zero in case of a fully paid Invoice. The amount may
          be negative; in that case the Seller owes the amount to the Buyer.
        </ng-template>

        <input [id]="bt115Control.controlId()" class="form-control" formControlName="duePayableAmount" placeholder="104.90" />
      </app-cii-form-control>
    </div>
  `,
})
export class CiiFormSpecifiedTradeSettlementHeaderMonetarySummation {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();
}
