import { Component, computed, effect, inject, input } from '@angular/core';
import { ControlContainer, FormGroupDirective, ReactiveFormsModule } from '@angular/forms';
import { CrossIndustryInvoiceFormVerbosity } from './cii-form.component';

@Component({
  selector: 'app-cii-form-exchanged-document',
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
          <label class="form-label text-truncate" for="exchangeDocumentId"> <span class="fw-semibold">BT-1</span> - Invoice number </label>
          <input id="exchangeDocumentId" class="form-control" formControlName="id" placeholder="F20250023" />
          <p id="exchangeDocumentIdHelp" class="form-text">A unique identification of the Invoice.</p>
        </div>
        @if (showBusinessRules()) {
          <div class="form-text">
            <div class="fw-semibold">Business Rules</div>
            <ul>
              <li><span class="fw-semibold">BR-2</span>: An Invoice shall have an Invoice number.</li>
            </ul>
          </div>
        }
        @if (showRemarks()) {
          <div class="alert alert-light small">
            <i class="bi bi-info-circle"></i>
            The sequential number required in Article 226(2) of the directive 2006/112/EC, to uniquely identify the Invoice within the business context, time-frame, operating
            systems and records of the Seller. It may be based on one or more series of numbers, which may include alphanumeric characters. No identification scheme is to be used.
          </div>
          <div class="alert alert-light small">
            <div class="fw-semibold"><i class="bi bi-info-circle"></i> CHORUSPRO</div>
            The invoice number is limited to 20 characters.
          </div>
        }
      </div>

      <div>
        <div class="editor__control">
          <label class="form-label text-truncate" for="typeCode"> <span class="fw-semibold">BT-3</span> - Invoice type code </label>
          <select id="typeCode" class="form-select" formControlName="typeCode">
            <option value="" class="text-body-tertiary" selected>Choose a type</option>
            <option value="71-request-for-payment">71 - Request for payment</option>
            <option value="80-debit-note-related-to-goods-or-services">80 - Debit note related to goods or services</option>
            <option value="81-credit-note-related-to-goods-or-services">81 - Credit note related to goods or services</option>
            <option value="82-metered-services-invoice">82 - Metered services invoice</option>
            <option value="83-credit-note-related-to-financial-adjustments">83 - Credit note related to financial adjustments</option>
            <option value="84-debit-note-related-to-financial-adjustments">84 - Debit note related to financial adjustments</option>
            <option value="102-tax-notification">102 - Tax notification</option>
            <option value="130-invoicing-data-sheet">130 - Invoicing data sheet</option>
            <option value="202-direct-payment-valuation">202 - Direct payment valuation</option>
            <option value="203-provisional-payment-valuation">203 - Provisional payment valuation</option>
            <option value="204-payment-valuation">204 - Payment valuation</option>
            <option value="211-interim-application-for-payment">211 - Interim application for payment</option>
            <option value="218-final-payment-request-based-on-completion-of-work">218 - Final payment request based on completion of work</option>
            <option value="219-payment-request-for-completed-units">219 - Payment request for completed units</option>
            <option value="261-self-billed-credit-note">261 - Self billed credit note</option>
            <option value="262-consolidated-credit-note---goods-and-services">262 - Consolidated credit note - goods and services</option>
            <option value="295-price-variation-invoice">295 - Price variation invoice</option>
            <option value="296-credit-note-for-price-variation">296 - Credit note for price variation</option>
            <option value="308-delcredere-credit-note">308 - Delcredere credit note</option>
            <option value="325-proforma-invoice">325 - Proforma invoice</option>
            <option value="326-partial-invoice">326 - Partial invoice</option>
            <option value="331-commercial-invoice-which-includes-a-packing-list">331 - Commercial invoice which includes a packing list</option>
            <option value="380-commercial-invoice">380 - Commercial invoice</option>
            <option value="381-credit-note">381 - Credit note</option>
            <option value="382-commission-note">382 - Commission note</option>
            <option value="383-debit-note">383 - Debit note</option>
            <option value="384-corrected-invoice">384 - Corrected invoice</option>
            <option value="385-consolidated-invoice">385 - Consolidated invoice</option>
            <option value="386-prepayment-invoice">386 - Prepayment invoice</option>
            <option value="387-hire-invoice">387 - Hire invoice</option>
            <option value="388-tax-invoice">388 - Tax invoice</option>
            <option value="389-self-billed-invoice">389 - Self-billed invoice</option>
            <option value="390-delcredere-invoice">390 - Delcredere invoice</option>
            <option value="393-factored-invoice">393 - Factored invoice</option>
            <option value="394-lease-invoice">394 - Lease invoice</option>
            <option value="395-consignment-invoice">395 - Consignment invoice</option>
            <option value="396-factored-credit-note">396 - Factored credit note</option>
            <option value="420-optical-character-reading-(ocr)-payment-credit-note">420 - Optical Character Reading (OCR) payment credit note</option>
            <option value="456-debit-advice">456 - Debit advice</option>
            <option value="457-reversal-of-debit">457 - Reversal of debit</option>
            <option value="458-reversal-of-credit">458 - Reversal of credit</option>
            <option value="527-self-billed-debit-note">527 - Self billed debit note</option>
            <option value="532-forwarders-credit-note">532 - Forwarder's credit note</option>
            <option value="553-forwarders-invoice-discrepancy-report">553 - Forwarder's invoice discrepancy report</option>
            <option value="575-insurers-invoice">575 - Insurer's invoice</option>
            <option value="623-forwarders-invoice">623 - Forwarder's invoice</option>
            <option value="633-port-charges-documents">633 - Port charges documents</option>
            <option value="751-invoice-information-for-accounting-purposes">751 - Invoice information for accounting purposes</option>
            <option value="780-freight-invoice">780 - Freight invoice</option>
            <option value="817-claim-notification">817 - Claim notification</option>
            <option value="870-consular-invoice">870 - Consular invoice</option>
            <option value="875-partial-construction-invoice">875 - Partial construction invoice</option>
            <option value="876-partial-final-construction-invoice">876 - Partial final construction invoice</option>
            <option value="877-final-construction-invoice">877 - Final construction invoice</option>
            <option value="935-customs-invoice">935 - Customs invoice</option>
          </select>
          <p id="typeCodeHelp" class="form-text">A code specifying the functional type of the Invoice.</p>
        </div>
        @if (showBusinessRules()) {
          <div class="form-text">
            <div class="fw-semibold">Business Rules</div>
            <ul>
              <li><span class="fw-semibold">BR-4</span>: An Invoice shall have an Invoice type code.</li>
            </ul>
          </div>
        }
        @if (showRemarks()) {
          <div class="alert alert-light small">
            <i class="bi bi-info-circle"></i>
            Commercial invoices and credit notes are defined according the entries in UNTDID 1001. Other entries of UNTDID 1001 with specific invoices or credit notes may be used
            if applicable.
          </div>
          <div class="alert alert-light small">
            <div class="fw-semibold"><i class="bi bi-info-circle"></i> CHORUSPRO</div>
            The types of documents used are:
            <ul>
              <li>380: Commercial Invoice</li>
              <li>381: Credit note</li>
              <li>384: Corrected invoice</li>
              <li>389: Self-billed invoice (created by the buyer on behalf of the supplier)</li>
              <li>261: Self billed credit note (not accepted by CHORUSPRO)</li>
              <li>386: Prepayment invoice</li>
              <li>751: Invoice information for accounting purposes (not accepted by CHORUSPRO)</li>
            </ul>
          </div>
        }
      </div>

      <div class="d-flex flex-wrap column-gap-4">
        <div>
          <div class="editor__control">
            <label class="form-label text-truncate" for="issueDateTime"> <span class="fw-semibold">BT-2</span> - Invoice issue date </label>
            <input id="issueDateTime" class="form-control" formControlName="issueDateTime" type="date" />
            <p id="issueDateTimeHelp" class="form-text">The date when the Invoice was issued.</p>
          </div>
          @if (showBusinessRules()) {
            <div class="form-text">
              <div class="fw-semibold">Business Rules</div>
              <ul>
                <li><span class="fw-semibold">BR-3</span>: An Invoice shall have an Invoice issue date.</li>
              </ul>
            </div>
          }
          @if (showRemarks()) {
            <div class="alert alert-light small">
              <div class="fw-semibold"><i class="bi bi-info-circle"></i> CHORUSPRO</div>
              The issue date must be before or equal to the deposit date.
            </div>
          }
        </div>

        <div>
          <div class="editor__control">
            <label class="form-label text-truncate" for="issueDateTimeFormat"> <span class="fw-semibold">BT-2-0</span> - Date, format </label>
            <select id="issueDateTimeFormat" class="form-select" formControlName="issueDateTimeFormat">
              <option value="102-date-only" selected>102 - Date only</option>
            </select>
            <p id="issueDateTimeHelp" class="form-text">Only value "102"</p>
          </div>
        </div>
      </div>
    </div>
  `,
})
export class CiiFormExchangedDocumentComponent {
  formGroupName = input.required<string>();
  verbosity = input<CrossIndustryInvoiceFormVerbosity>('normal');
  disabled = input<boolean>(false);

  protected showBusinessRules = computed(() => this.verbosity() == 'normal' || this.verbosity() == 'detailed');
  protected showRemarks = computed(() => this.verbosity() == 'detailed');
}
