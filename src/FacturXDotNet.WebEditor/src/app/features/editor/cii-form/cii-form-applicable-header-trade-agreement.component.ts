import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { CiiFormSellerTradePartyComponent } from './cii-form-seller-trade-party.component';
import { CiiFormBuyerTradePartyComponent } from './cii-form-buyer-trade-party.component';
import { CiiFormBuyerOrderReferencedDocumentComponent } from './cii-form-buyer-order-referenced-document.component';
import { EditorSettings } from '../editor-settings.service';
import { CiiFormExchangedDocumentContextComponent } from './cii-form-exchanged-document-context.component';
import { CiiFormParentContainerComponent } from './components/cii-form-parent-container.component';

@Component({
  selector: 'app-cii-form-applicable-header-trade-agreement',
  imports: [
    ReactiveFormsModule,
    CiiFormSellerTradePartyComponent,
    CiiFormBuyerTradePartyComponent,
    CiiFormBuyerTradePartyComponent,
    CiiFormBuyerTradePartyComponent,
    CiiFormBuyerOrderReferencedDocumentComponent,
    CiiFormParentContainerComponent,
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

      <app-cii-form-parent-container term="BG-4" name="SELLER" [description]="description" [settings]="settings()">
        <ng-template #description>A group of business terms providing information about the Seller.</ng-template>
        <app-cii-form-seller-trade-party formGroupName="sellerTradeParty" [settings]="settings()"></app-cii-form-seller-trade-party>
      </app-cii-form-parent-container>

      <app-cii-form-parent-container term="BG-7" name="BUYER" [description]="description" [settings]="settings()">
        <ng-template #description>A group of business terms providing information about the Buyer.</ng-template>
        <app-cii-form-buyer-trade-party formGroupName="buyerTradeParty" [settings]="settings()"></app-cii-form-buyer-trade-party>
      </app-cii-form-parent-container>

      <app-cii-form-parent-container term="BT-13-00" name="PURCHASE ORDER REFERENCE" [settings]="settings()">
        <app-cii-form-buyer-order-referenced-document formGroupName="buyerOrderReferencedDocument" [settings]="settings()"></app-cii-form-buyer-order-referenced-document>
      </app-cii-form-parent-container>
    </div>
  `,
})
export class CiiFormApplicableHeaderTradeAgreementComponent {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();
}
