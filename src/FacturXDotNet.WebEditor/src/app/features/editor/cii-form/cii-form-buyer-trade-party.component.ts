import { Component, computed, effect, inject, input } from '@angular/core';
import { CrossIndustryInvoiceFormVerbosity } from './cii-form.component';
import { ControlContainer, FormGroupDirective, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CiiFormApplicableHeaderTradeAgreementComponent } from './cii-form-applicable-header-trade-agreement.component';
import { CollapseComponent } from '../../../core/collapse/collapse.component';
import { CiiFormSellerTradePartySpecifiedLegalOrganizationComponent } from './cii-form-seller-trade-party-specified-legal-organization.component';
import { CiiFormSellerTradePartyPostalTradeAddress } from './cii-form-seller-trade-party-postal-trade-address';
import { CiiFormSellerTradePartySpecifiedTaxRegistration } from './cii-form-seller-trade-party-specified-tax-registration';
import { CiiFormBuyerTradePartySpecifiedLegalOrganizationComponent } from './cii-form-buyer-trade-party-specified-legal-organization.component';
import { EditorSettings } from '../editor-settings.service';

@Component({
  selector: 'app-cii-form-buyer-trade-party',
  imports: [ReactiveFormsModule, CollapseComponent, CiiFormBuyerTradePartySpecifiedLegalOrganizationComponent],
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
          <label class="form-label text-truncate" for="buyerName"> <span class="fw-semibold">BT-44</span> - Buyer name </label>
          <input id="buyerName" class="form-control" formControlName="name" placeholder="LE CLIENT" />
          <p id="buyerNameHelp" class="form-text">The full name of the Buyer.</p>
        </div>
        @if (settings()?.showBusinessRules === true) {
          <div class="form-text">
            <div class="fw-semibold">Business Rules</div>
            <ul>
              <li><span class="fw-semibold">BR-7</span>: An Invoice shall contain the Buyer name.</li>
            </ul>
          </div>
        }
      </div>

      <h6 class="m-0">BT-47-00 - BUYER LEGAL REGISTRATION IDENTIFIER</h6>
      <p class="form-text ps-4">Details about the organization.</p>
      <div class="ps-4">
        <div class="ps-3 border-start">
          <app-cii-form-buyer-trade-party-specified-legal-organization
            formGroupName="specifiedLegalOrganization"
            [settings]="settings()"
          ></app-cii-form-buyer-trade-party-specified-legal-organization>
        </div>
      </div>
    </div>
  `,
  styles: ``,
})
export class CiiFormBuyerTradePartyComponent {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();
}
