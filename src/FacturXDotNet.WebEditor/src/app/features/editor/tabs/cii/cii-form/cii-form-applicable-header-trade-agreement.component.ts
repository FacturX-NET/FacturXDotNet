import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { CiiFormSellerTradePartyComponent } from './cii-form-seller-trade-party.component';
import { CiiFormBuyerTradePartyComponent } from './cii-form-buyer-trade-party.component';
import { CiiFormBuyerOrderReferencedDocumentComponent } from './cii-form-buyer-order-referenced-document.component';
import { EditorSettings } from '../../../editor-settings.service';
import { CiiFormParentContainerComponent } from './base/cii-form-parent-container.component';
import { CiiFormControlComponent } from './base/cii-form-control.component';

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
    CiiFormControlComponent,
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
      <app-cii-form-control
        term="BT-10"
        name="Buyer reference"
        [description]="description"
        [remarks]="[remark]"
        [chorusProRemarks]="[chorusProRemark]"
        [settings]="settings()"
        #control
      >
        <ng-template #description>An identifier assigned by the Buyer used for internal routing purposes.</ng-template>
        <ng-template #remark>The identifier is defined by the Buyer (e.g. contact ID, department, office id, project code), but provided by the Seller in the Invoice.</ng-template>
        <ng-template #chorusProRemark>The invoice number is limited to 20 characters.</ng-template>

        <input [id]="control.controlId()" class="form-control" formControlName="buyerReference" placeholder="SERVEXEC" />
      </app-cii-form-control>

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
