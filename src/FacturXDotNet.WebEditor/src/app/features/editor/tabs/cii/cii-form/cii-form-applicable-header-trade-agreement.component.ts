import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { CiiFormSellerTradePartyComponent } from './cii-form-seller-trade-party.component';
import { CiiFormBuyerTradePartyComponent } from './cii-form-buyer-trade-party.component';
import { CiiFormBuyerOrderReferencedDocumentComponent } from './cii-form-buyer-order-referenced-document.component';
import { EditorSettings } from '../../../editor-settings.service';
import { CiiFormParentContainerComponent } from './base/cii-form-parent-container.component';
import { CiiFormControlComponent } from './base/cii-form-control.component';
import { ciiTerms, requireTerm } from '../constants/cii-terms';

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
      <app-cii-form-control [term]="bt10" [settings]="settings()" #control>
        <input [id]="control.controlId()" class="form-control" formControlName="buyerReference" placeholder="SERVEXEC" />
      </app-cii-form-control>

      <app-cii-form-parent-container [term]="br04" [settings]="settings()" depth="3">
        <app-cii-form-seller-trade-party formGroupName="sellerTradeParty" [settings]="settings()"></app-cii-form-seller-trade-party>
      </app-cii-form-parent-container>

      <app-cii-form-parent-container [term]="br07" [settings]="settings()" depth="3">
        <app-cii-form-buyer-trade-party formGroupName="buyerTradeParty" [settings]="settings()"></app-cii-form-buyer-trade-party>
      </app-cii-form-parent-container>

      <app-cii-form-parent-container [term]="bt1300" [settings]="settings()" depth="3">
        <app-cii-form-buyer-order-referenced-document formGroupName="buyerOrderReferencedDocument" [settings]="settings()"></app-cii-form-buyer-order-referenced-document>
      </app-cii-form-parent-container>
    </div>
  `,
})
export class CiiFormApplicableHeaderTradeAgreementComponent {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();

  protected bt10 = requireTerm('BT-10');
  protected br04 = requireTerm('BR-04');
  protected br07 = requireTerm('BR-07');
  protected bt1300 = requireTerm('BT-13-00');
}
