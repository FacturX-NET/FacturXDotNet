import { Component, computed, effect, inject, input } from '@angular/core';
import { CrossIndustryInvoiceFormVerbosity } from './cii-form.component';
import { ControlContainer, FormGroupDirective, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CollapseComponent } from '../../../core/collapse/collapse.component';
import { CiiFormSellerTradePartyComponent } from './cii-form-seller-trade-party.component';

@Component({
  selector: 'app-cii-form-applicable-header-trade-agreement',
  imports: [ReactiveFormsModule, CollapseComponent, CiiFormSellerTradePartyComponent],
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
      <div class="mb-3">
        <label class="form-label text-truncate" for="buyerReference"> <span class="fw-semibold">BT-10</span> - Buyer reference </label>
        <input id="buyerReference" class="form-control" formControlName="buyerReference" />
        <p id="buyerReferenceHelp" class="form-text">An identifier assigned by the Buyer used for internal routing purposes.</p>
        @if (showRemarks()) {
          <div class="alert alert-light small">
            <i class="bi bi-info-circle"></i>
            The identifier is defined by the Buyer (e.g. contact ID, department, office id, project code), but provided by the Seller in the Invoice.
          </div>
          <div class="alert alert-light small">
            <div class="fw-semibold"><i class="bi bi-info-circle"></i> CHORUSPRO</div>
            The invoice number is limited to 20 characters.
          </div>
        }
      </div>

      <app-collapse id="seller" #collapse>
        <h6 class="m-0" ngProjectAs="trigger">
          @if (collapse.collapsed()) {
            <i class="bi bi-plus fs-5"></i>
          } @else {
            <i class="bi bi-dash fs-5"></i>
          }

          BG-4 - SELLER
        </h6>
        <p class="form-text ps-4">A group of business terms providing information about the Seller.</p>
        <div class="ps-4" ngProjectAs="collapsible">
          <div class="ps-3 border-start">
            <app-cii-form-seller-trade-party formGroupName="sellerTradeParty" [verbosity]="verbosity()" [disabled]="disabled()"></app-cii-form-seller-trade-party>
          </div>
        </div>
      </app-collapse>
    </div>
  `,
})
export class CiiFormApplicableHeaderTradeAgreementComponent {
  formGroupName = input.required<string>();
  verbosity = input<CrossIndustryInvoiceFormVerbosity>('normal');
  disabled = input<boolean>(false);

  protected showBusinessRules = computed(() => this.verbosity() == 'normal' || this.verbosity() == 'detailed');
  protected showRemarks = computed(() => this.verbosity() == 'detailed');
}
