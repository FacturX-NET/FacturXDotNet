import { Component, computed, effect, inject, input } from '@angular/core';
import { CrossIndustryInvoiceFormVerbosity } from './cii-form.component';
import { ControlContainer, FormGroupDirective, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CiiFormSellerTradePartyComponent } from './cii-form-seller-trade-party.component';
import { CiiFormBuyerTradePartyComponent } from './cii-form-buyer-trade-party.component';
import { CiiFormBuyerOrderReferencedDocumentComponent } from './cii-form-buyer-order-referenced-document.component';
import { CiiFormBuyerTradePartySpecifiedLegalOrganizationComponent } from './cii-form-buyer-trade-party-specified-legal-organization.component';
import { CiiFormSpecifiedTradeSettlementHeaderMonetarySummation } from './cii-form-specified-trade-settlement-header-monetary-summation.component';
import { EditorSettings } from '../editor-settings.service';

@Component({
  selector: 'app-cii-form-applicable-header-trade-settlement',
  imports: [ReactiveFormsModule, CiiFormSpecifiedTradeSettlementHeaderMonetarySummation],
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
          <label class="form-label text-truncate" for="invoiceCurrencyCode"> <span id="BT-5" class="fw-semibold">BT-5</span> - Invoice currency code </label>
          <input id="invoiceCurrencyCode" class="form-control" formControlName="invoiceCurrencyCode" placeholder="EUR" />
          <p id="invoiceCurrencyCodeHelp" class="form-text">The currency in which all Invoice amounts are given, except for the Total VAT amount in accounting currency.</p>
        </div>
        @if (settings()?.showBusinessRules === true) {
          <div class="form-text">
            <div class="fw-semibold">Business Rules</div>
            <ul>
              <li><span id="BR-5" class="fw-semibold">BR-5</span>: An Invoice shall have an Invoice currency code.</li>
            </ul>
          </div>
        }
        @if (settings()?.showRemarks === true) {
          <div class="alert alert-light small">
            <i class="bi bi-info-circle"></i>
            Only one currency shall be used in the Invoice, except for the Total VAT amount in accounting currency (BT-111) in accordance with article 230 of Directive 2006/112/EC
            on VAT. The lists of valid currencies are registered with the ISO 4217 Maintenance Agency "Codes for the representation of currencies and funds".
          </div>
        }
        @if (settings()?.showChorusProRemarks === true) {
          <div class="alert alert-light small">
            <div class="fw-semibold"><i class="bi bi-info-circle"></i> CHORUSPRO</div>
            Invoices and credit notes or Chorus Pro are mono-currencies only.
          </div>
        }
      </div>

      <h6 id="BG-22" class="m-0" tabindex="-1">BG-22 - DOCUMENT TOTALS</h6>
      <p class="form-text ps-4">A group of business terms providing the monetary totals for the Invoice.</p>
      @if (settings()?.showRemarks === true) {
        <div class="ps-4">
          <div class="alert alert-light small">
            <i class="bi bi-info-circle"></i>
            This group may be used to give prior notice in the invoice that payment will be made through a SEPA or other direct debit initiated by the Seller, in accordance with
            the rules of the SEPA or other direct debit scheme.
          </div>
        </div>
      }
      @if (settings()?.showChorusProRemarks === true) {
        <div class="ps-4">
          <div class="alert alert-light small">
            <div class="fw-semibold"><i class="bi bi-info-circle"></i> CHORUSPRO</div>
            Amounts in an invoice are expressed by a figure on 19 positions. They can not have more than two decimals. The separator is
            <code>.</code> (dot).
          </div>
        </div>
      }
      <div class="ps-4">
        <div class="ps-3 border-start">
          <app-cii-form-specified-trade-settlement-header-monetary-summation
            formGroupName="specifiedTradeSettlementHeaderMonetarySummation"
            [settings]="settings()"
          ></app-cii-form-specified-trade-settlement-header-monetary-summation>
        </div>
      </div>
    </div>
  `,
})
export class CiiFormApplicableHeaderTradeSettlementComponent {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();
}
