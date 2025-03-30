import { Component, computed, effect, inject, input } from '@angular/core';
import { CrossIndustryInvoiceFormVerbosity } from './cii-form.component';
import { ControlContainer, FormGroupDirective, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CiiFormApplicableHeaderTradeAgreementComponent } from './cii-form-applicable-header-trade-agreement.component';
import { CollapseComponent } from '../../../core/collapse/collapse.component';
import { CiiFormSellerTradePartySpecifiedLegalOrganizationComponent } from './cii-form-seller-trade-party-specified-legal-organization.component';
import { CiiFormSellerTradePartyPostalTradeAddress } from './cii-form-seller-trade-party-postal-trade-address';
import { CiiFormSellerTradePartySpecifiedTaxRegistration } from './cii-form-seller-trade-party-specified-tax-registration';
import { CiiFormBuyerTradePartySpecifiedLegalOrganizationComponent } from './cii-form-buyer-trade-party-specified-legal-organization.component';

@Component({
  selector: 'app-cii-form-buyer-order-referenced-document',
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
          <label class="form-label text-truncate" for="issuerAssignedId"> <span class="fw-semibold">BT-13</span> - Purchase order reference </label>
          <input id="issuerAssignedId" class="form-control" formControlName="issuerAssignedId" />
          <p id="issuerAssignedIdHelp" class="form-text">An identifier of a referenced purchase order, issued by the Buyer.</p>
        </div>
        @if (showRemarks()) {
          <div class="alert alert-light small">
            <div class="fw-semibold"><i class="bi bi-info-circle"></i> CHORUSPRO</div>
            For the public sector, this is the "Engagement Juridique" (Legal Commitment). It is mandatory for some buyers. You should refer to the ChorusPro Directory to identify
            these public entity buyers that make it mandatory.
          </div>
        }
      </div>
    </div>
  `,
})
export class CiiFormBuyerOrderReferencedDocumentComponent {
  formGroupName = input.required<string>();
  verbosity = input<CrossIndustryInvoiceFormVerbosity>('normal');
  disabled = input<boolean>(false);

  protected showBusinessRules = computed(() => this.verbosity() == 'normal' || this.verbosity() == 'detailed');
  protected showRemarks = computed(() => this.verbosity() == 'detailed');
}
