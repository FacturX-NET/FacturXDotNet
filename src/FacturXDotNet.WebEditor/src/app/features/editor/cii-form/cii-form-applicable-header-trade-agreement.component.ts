import { Component, computed, effect, inject, input } from '@angular/core';
import { CrossIndustryInvoiceFormVerbosity } from './cii-form.component';
import { ControlContainer, FormGroupDirective, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CollapseComponent } from '../../../core/collapse/collapse.component';
import { CiiFormSellerTradePartyComponent } from './cii-form-seller-trade-party.component';
import { CiiFormBuyerTradePartyComponent } from './cii-form-buyer-trade-party.component';
import { CiiFormBuyerOrderReferencedDocumentComponent } from './cii-form-buyer-order-referenced-document.component';
import { EditorSettings } from '../editor-settings.service';

@Component({
  selector: 'app-cii-form-applicable-header-trade-agreement',
  imports: [
    ReactiveFormsModule,
    CollapseComponent,
    CiiFormSellerTradePartyComponent,
    CiiFormBuyerTradePartyComponent,
    CiiFormBuyerTradePartyComponent,
    CiiFormBuyerTradePartyComponent,
    CiiFormBuyerOrderReferencedDocumentComponent,
  ],
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
          <label class="form-label text-truncate" for="buyerReference"> <span id="BT-10" class="fw-semibold">BT-10</span> - Buyer reference </label>
          <input id="buyerReference" class="form-control" formControlName="buyerReference" placeholder="SERVEXEC" />
          <p id="buyerReferenceHelp" class="form-text">An identifier assigned by the Buyer used for internal routing purposes.</p>
        </div>
        @if (settings()?.showRemarks === true) {
          <div class="alert alert-light small">
            <i class="bi bi-info-circle"></i>
            The identifier is defined by the Buyer (e.g. contact ID, department, office id, project code), but provided by the Seller in the Invoice.
          </div>
        }
        @if (settings()?.showChorusProRemarks === true) {
          <div class="alert alert-light small">
            <div class="fw-semibold"><i class="bi bi-info-circle"></i> CHORUSPRO</div>
            The invoice number is limited to 20 characters.
          </div>
        }
      </div>

      <h6 id="BG-4" class="m-0">BG-4 - SELLER</h6>
      <p class="form-text ps-4">A group of business terms providing information about the Seller.</p>
      <div class="ps-4">
        <div class="ps-3 border-start">
          <app-cii-form-seller-trade-party formGroupName="sellerTradeParty" [settings]="settings()"></app-cii-form-seller-trade-party>
        </div>
      </div>

      <h6 id="BG-7" class="m-0">BG-7 - BUYER</h6>
      <p class="form-text ps-4">A group of business terms providing information about the Buyer.</p>
      <div class="ps-4">
        <div class="ps-3 border-start">
          <app-cii-form-buyer-trade-party formGroupName="buyerTradeParty" [settings]="settings()"></app-cii-form-buyer-trade-party>
        </div>
      </div>

      <h6 class="m-0">BT-13-00 - PURCHASE ORDER REFERENCE</h6>
      <p class="form-text ps-4"></p>
      <div class="ps-4">
        <div class="ps-3 border-start">
          <app-cii-form-buyer-order-referenced-document formGroupName="buyerOrderReferencedDocument" [settings]="settings()"></app-cii-form-buyer-order-referenced-document>
        </div>
      </div>
    </div>
  `,
})
export class CiiFormApplicableHeaderTradeAgreementComponent {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();
}
