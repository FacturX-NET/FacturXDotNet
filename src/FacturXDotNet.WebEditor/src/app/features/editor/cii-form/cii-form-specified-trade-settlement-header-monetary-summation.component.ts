import { Component, computed, effect, inject, input } from '@angular/core';
import { CrossIndustryInvoiceFormVerbosity } from './cii-form.component';
import { ControlContainer, FormGroupDirective, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CiiFormSellerTradePartyComponent } from './cii-form-seller-trade-party.component';
import { CiiFormBuyerTradePartyComponent } from './cii-form-buyer-trade-party.component';
import { CiiFormBuyerOrderReferencedDocumentComponent } from './cii-form-buyer-order-referenced-document.component';
import { EditorSettings } from '../editor-settings.service';

@Component({
  selector: 'app-cii-form-specified-trade-settlement-header-monetary-summation',
  imports: [ReactiveFormsModule],
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
      <div>
        <div class="editor__control">
          <label class="form-label text-truncate" for="taxBasisTotalAmount"> <span id="BT-109" class="fw-semibold">BT-109</span> - Total amount without VAT </label>
          <input id="taxBasisTotalAmount" class="form-control" formControlName="taxBasisTotalAmount" placeholder="100.00" />
          <p id="taxBasisTotalAmountHelp" class="form-text">The total amount of the Invoice without VAT.</p>
        </div>
        @if (settings()?.showBusinessRules === true) {
          <div class="form-text">
            <div class="fw-semibold">Business Rules</div>
            <ul>
              <li><span id="BR-13" class="fw-semibold">BR-13</span>: An Invoice shall have the Invoice total amount without VAT.</li>
              <li>
                <span id="BR-CO-13" class="fw-semibold">BR-CO-13</span>:
                <div class="d-flex column-gap-2 flex-wrap">
                  <div class="text-nowrap"><code>Invoice total amount without VAT (BT-109)</code></div>
                  <div class="d-flex column-gap-2">
                    <div class="text-nowrap"><code>=</code></div>
                    <div class="d-flex column-gap-1 flex-wrap">
                      <div class="text-nowrap"><code>∑ Invoice line net amount (BT-131)</code></div>
                      <div class="text-nowrap"><code>- Sum of allowances on document level (BT-107)</code></div>
                      <div class="text-nowrap"><code>+ Sum of charges on document level (BT-108)</code></div>
                    </div>
                  </div>
                </div>
              </li>
            </ul>
          </div>
        }
        @if (settings()?.showRemarks === true) {
          <div class="alert alert-light small">
            <i class="bi bi-info-circle"></i>
            The Invoice total amount without VAT is the Sum of Invoice line net amount minus Sum of allowances on document level plus Sum of charges on document level.
          </div>
        }
      </div>

      <div class="row">
        <div class="col">
          <div class="editor__control">
            <label class="form-label text-truncate" for="taxTotalAmount"> <span id="BT-110" class="fw-semibold">BT-110</span> - Total VAT amount </label>
            <input id="taxTotalAmount" class="form-control" formControlName="taxTotalAmount" placeholder="4.90" />
            <p id="taxTotalAmountHelp" class="form-text">The total VAT amount for the Invoice.</p>
          </div>
          @if (settings()?.showBusinessRules === true) {
            <div class="form-text">
              <div class="fw-semibold">Business Rules</div>
              <ul>
                <li>
                  <span id="BR-CO-14" class="fw-semibold">BR-CO-14</span>:
                  <div class="d-flex flex-wrap">
                    <div class="text-nowrap"><code>Invoice total VAT amount (BT-110)</code></div>
                    <div class="text-nowrap"><code>= ∑ VAT category tax amount (BT-117)</code></div>
                  </div>
                </li>
              </ul>
            </div>
          }
          @if (settings()?.showRemarks === true) {
            <div class="alert alert-light small">
              <i class="bi bi-info-circle"></i>
              The Invoice total VAT amount is the sum of all VAT category tax amounts.
            </div>
          }
        </div>
        <div class="col">
          <div class="editor__control">
            <label class="form-label text-truncate" for="taxTotalAmountCurrencyId"> <span id="BT-110-1" class="fw-semibold">BT-110-1</span> - VAT currency </label>
            <input id="taxTotalAmountCurrencyId" class="form-control" formControlName="taxTotalAmountCurrencyId" placeholder="EUR" />
            <p id="taxTotalAmountCurrencyIdHelp" class="form-text"></p>
            @if (settings()?.showRemarks === true) {
              <div class="alert alert-light small">
                <i class="bi bi-info-circle"></i>
                The currency is mandatory to differentiate between VAT amount and VAT amount in accounting currency.
              </div>
            }
          </div>
        </div>
      </div>

      <div>
        <div class="editor__control">
          <label class="form-label text-truncate" for="grandTotalAmount"> <span id="BT-112" class="fw-semibold">BT-112</span> - Total amount with VAT </label>
          <input id="grandTotalAmount" class="form-control" formControlName="grandTotalAmount" placeholder="104.90" />
          <p id="grandTotalAmountHelp" class="form-text">The total amount of the Invoice with VAT.</p>
        </div>
        @if (settings()?.showBusinessRules === true) {
          <div class="form-text">
            <div class="fw-semibold">Business Rules</div>
            <ul>
              <li><span id="BR-14" class="fw-semibold">BR-14</span>: An Invoice shall have the Invoice total amount with VAT (BT-112).</li>
              <li>
                <span id="BR-CO-15" class="fw-semibold">BR-CO-15</span>:
                <div class="d-flex column-gap-2 flex-wrap">
                  <div class="text-nowrap"><code>Invoice total amount with VAT (BT-112)</code></div>
                  <div class="d-flex column-gap-2">
                    <div class="text-nowrap"><code>=</code></div>
                    <div class="d-flex column-gap-2 flex-wrap">
                      <div class="text-nowrap"><code>Invoice total amount without VAT (BT-109)</code></div>
                      <div class="text-nowrap"><code>+ Invoice total VAT amount (BT-110)</code></div>
                    </div>
                  </div>
                </div>
              </li>
              <li>
                <span id="BR-FXEXT-CO-15" class="fw-semibold">BR-FXEXT-CO-15</span>: For EXTENDED profile only, BR-CO-15 is replaced by BR-FXEXT-CO-15, which add a tolerance of
                0,01 euro per line, document level charge and allowance in calculation.
              </li>
            </ul>
          </div>
        }
        @if (settings()?.showRemarks === true) {
          <div class="alert alert-light small">
            <i class="bi bi-info-circle"></i>
            The Invoice total amount with VAT is the Invoice total amount without VAT plus the Invoice total VAT amount.
          </div>
        }
      </div>

      <div>
        <div class="editor__control">
          <label class="form-label text-truncate" for="duePayableAmount"> <span id="BT-115" class="fw-semibold">BT-115</span> - Amount due for payment </label>
          <input id="duePayableAmount" class="form-control" formControlName="duePayableAmount" placeholder="104.90" />
          <p id="duePayableAmountHelp" class="form-text">The outstanding amount that is requested to be paid.</p>
        </div>
        @if (settings()?.showBusinessRules === true) {
          <div class="form-text">
            <div class="fw-semibold">Business Rules</div>
            <ul>
              <li><span id="BR-15" class="fw-semibold">BR-15</span>: An Invoice shall have the Amount due for payment.</li>
              <li>
                <span id="BR-CO-16" class="fw-semibold">BR-CO-16</span>:
                <div class="d-flex column-gap-2 flex-wrap">
                  <div><code>Amount due for payment (BT-115)</code></div>
                  <div class="d-flex column-gap-2">
                    <div><code>=</code></div>
                    <div class="d-flex column-gap-2 flex-wrap">
                      <div><code>Invoice total amount with VAT (BT-112)</code></div>
                      <div><code>- Paid amount (BT-113)</code></div>
                      <div><code>+ Rounding amount (BT-114)</code></div>
                    </div>
                  </div>
                </div>
              </li>
            </ul>
          </div>
        }
        @if (settings()?.showRemarks === true) {
          <div class="alert alert-light small">
            <i class="bi bi-info-circle"></i>
            This amount is the Invoice total amount with VAT minus the paid amount that has been paid in advance. The amount is zero in case of a fully paid Invoice. The amount may
            be negative; in that case the Seller owes the amount to the Buyer.
          </div>
        }
      </div>
    </div>
  `,
})
export class CiiFormSpecifiedTradeSettlementHeaderMonetarySummation {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();
}
