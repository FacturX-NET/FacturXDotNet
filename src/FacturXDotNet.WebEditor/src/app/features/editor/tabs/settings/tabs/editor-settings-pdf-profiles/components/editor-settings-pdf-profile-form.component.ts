import { Component, computed, inject, Signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ImportFileService } from '../../../../../../../core/import-file/import-file.service';
import { rxResource, toSignal } from '@angular/core/rxjs-interop';
import { GenerateApi } from '../../../../../../../core/api/generate.api';
import { distinctUntilChanged, map } from 'rxjs';
import { IStandardPdfGeneratorLanguagePackDto } from '../../../../../../../core/api/api.models';
import { EditorPdfGenerationProfileData } from '../../../../../editor-pdf-generation-profiles.service';

@Component({
  selector: 'app-editor-settings-pdf-profile-form',
  imports: [ReactiveFormsModule],
  template: `
    <form [formGroup]="formGroup">
      <div class="editor__control mb-3">
        <label class="form-label fw-semibold" for="editor-settings-profile-name">Name</label>
        <input class="form-control" id="editor-settings-profile-name" formControlName="name" />
        <p class="form-text">The profile name is for your reference only and won’t appear in the generated PDF.</p>
      </div>

      <div class="editor__control mb-3">
        <label class="form-label fw-semibold" for="editor-settings-profile-logo">Logo</label>
        <div>
          @if (formGroup.controls.logoBase64.value) {
            <div class="mb-2">
              <img class="img-thumbnail" [src]="formGroup.controls.logoBase64.value" alt="Logo" />
            </div>
            <div class="d-flex flex-wrap gap-2">
              <button role="button" class="btn btn-outline-secondary" (click)="chooseLogo()">Change logo</button>
              <button role="button" class="btn btn-outline-secondary" (click)="removeLogo()">Remove logo</button>
            </div>
          } @else {
            <button role="button" class="btn btn-outline-secondary" (click)="chooseLogo()">Upload logo</button>
          }
        </div>
        <p class="form-text">Upload your logo to display it in the top-left corner of the generated invoice.</p>
      </div>

      <div class="mb-3">
        <label class="form-label fw-semibold" for="editor-settings-profile-footer">Footer</label>
        <textarea class="form-control" id="editor-settings-profile-footer" formControlName="footer"></textarea>
        <p class="form-text">
          Enter the text you want to appear at the bottom of the invoice. This is useful for adding legal notices, contact information, or any other custom message.
        </p>
      </div>

      <div class="pt-3" formGroupName="languagePack">
        <h5>
          Language pack

          @if (languagePacks.isLoading() || baseLanguagePack() === undefined) {
            <div class="spinner-border spinner-border-sm" role="status">
              <span class="visually-hidden">Loading...</span>
            </div>
          }
        </h5>
        <div class="border-top mb-3"></div>

        @if (!languagePacks.isLoading()) {
          @if (baseLanguagePack(); as baseLanguagePack) {
            <div class="editor__control mb-3">
              <label class="form-label fw-semibold" for="editor-settings-profile-base-pack">Base pack</label>
              <select class="form-select" id="editor-settings-profile-base-pack" formControlName="baseLanguagePack">
                <option [ngValue]="undefined"></option>
                @for (languagePack of languagePacks.value(); track languagePack.culture) {
                  <option [ngValue]="languagePack.culture">{{ languagePack.culture }}</option>
                }
              </select>
              <p class="form-text">
                Select a predefined language pack to use as a starting point for the labels on your invoice. You can override any of the default terms to better fit your needs.
              </p>
            </div>

            <div class="editor__control mb-3">
              <label class="form-label fw-semibold" for="editor-settings-profile-culture">Culture</label>
              <input class="form-control" id="editor-settings-profile-culture" formControlName="culture" [placeholder]="baseLanguagePack?.culture ?? ''" />
              <p class="form-text">
                Specifies the language and regional settings used for the invoice. This affect region-specific formatting rules such as date formats, and number separators.
              </p>
            </div>

            <div class="editor__control mb-3">
              <label class="form-label fw-semibold" for="editor-settings-profile-date-label">Date</label>
              <input class="form-control" id="editor-settings-profile-date-label" formControlName="dateLabel" [placeholder]="baseLanguagePack?.dateLabel ?? ''" />
              <p class="form-text">The label used for the invoice issue date.</p>
            </div>

            <div class="editor__control mb-3">
              <label class="form-label fw-semibold" for="editor-settings-profile-default-legal-id-type">Default Legal ID name</label>
              <input
                class="form-control"
                id="editor-settings-profile-default-legal-id-type"
                formControlName="defaultLegalIdType"
                [placeholder]="baseLanguagePack?.defaultLegalIdType ?? ''"
              />
              <p class="form-text">
                The label used when the legal identification scheme in the invoice is not recognized. While known schemes like SIREN or SIRET have specific labels, this default
                value is used as a fallback for unrecognized or unmapped schemes.
              </p>
            </div>

            <div class="d-flex flex-wrap gap-3 mb-3">
              <div class="editor__control flex-grow-1">
                <label class="form-label fw-semibold" for="editor-settings-profile-vat-number-label">VAT Number</label>
                <input class="form-control" id="editor-settings-profile-vat-number-label" formControlName="vatNumberLabel" [placeholder]="baseLanguagePack?.vatNumberLabel ?? ''" />
                <p class="form-text">The label used to display your VAT (Value Added Tax) number on the invoice.</p>
              </div>
              <div class="editor__control flex-grow-1">
                <label class="form-label fw-semibold" for="editor-settings-profile-customer-address-label">Customer Address</label>
                <input
                  class="form-control"
                  id="editor-settings-profile-customer-address-label"
                  formControlName="customerAddressLabel"
                  [placeholder]="baseLanguagePack?.customerAddressLabel ?? ''"
                />
                <p class="form-text">The label for the section showing the client’s billing address.</p>
              </div>
            </div>

            <div class="d-flex flex-wrap gap-3 mb-3">
              <div class="editor__control flex-grow-1">
                <label class="form-label fw-semibold" for="editor-settings-profile-supplier-references-label">Supplier references</label>
                <input
                  class="form-control"
                  id="editor-settings-profile-supplier-references-label"
                  formControlName="supplierReferencesLabel"
                  [placeholder]="baseLanguagePack?.supplierReferencesLabel ?? ''"
                />
                <p class="form-text">The label for your internal reference or tracking number related to the invoice.</p>
              </div>

              <div class="editor__control flex-grow-1">
                <label class="form-label fw-semibold" for="editor-settings-profile-customer-identifiers-label">Customer Identifiers</label>
                <input
                  class="form-control"
                  id="editor-settings-profile-customer-identifiers-label"
                  formControlName="customerIdentifiersLabel"
                  [placeholder]="baseLanguagePack?.customerIdentifiersLabel ?? ''"
                />
                <p class="form-text">The label for the client's identifiers, such as customer number or account ID.</p>
              </div>
            </div>

            <div class="d-flex flex-wrap gap-3 mb-3">
              <div class="editor__control flex-grow-1">
                <label class="form-label fw-semibold" for="editor-settings-profile-customer-references-label">Customer references</label>
                <input
                  class="form-control"
                  id="editor-settings-profile-customer-references-label"
                  formControlName="customerReferencesLabel"
                  [placeholder]="baseLanguagePack?.customerReferencesLabel ?? ''"
                />
                <p class="form-text">The label for the client’s reference number or identifier for this transaction.</p>
              </div>

              <div class="editor__control flex-grow-1">
                <label class="form-label fw-semibold" for="editor-settings-profile-delivery-information-label">Delivery Information</label>
                <input
                  class="form-control"
                  id="editor-settings-profile-delivery-information-label"
                  formControlName="deliveryInformationLabel"
                  [placeholder]="baseLanguagePack?.deliveryInformationLabel ?? ''"
                />
                <p class="form-text">The label for the section containing shipping or delivery details.</p>
              </div>
            </div>

            <div class="editor__control mb-3">
              <label class="form-label fw-semibold" for="editor-settings-profile-order-label">Order</label>
              <input class="form-control" id="editor-settings-profile-order-label" formControlName="orderLabel" [placeholder]="baseLanguagePack?.orderLabel ?? ''" />
              <p class="form-text">The label used for the purchase order or sales order associated with the invoice.</p>
            </div>

            <div class="d-flex flex-wrap gap-3 mb-3">
              <div class="editor__control flex-grow-1">
                <label class="form-label fw-semibold" for="editor-settings-profile-invoice-references-label">Invoice References</label>
                <input
                  class="form-control"
                  id="editor-settings-profile-invoice-references-label"
                  formControlName="invoiceReferencesLabel"
                  [placeholder]="baseLanguagePack?.invoiceReferencesLabel ?? ''"
                />
                <p class="form-text">The label for any references specific to the invoice document itself.</p>
              </div>

              <div class="editor__control flex-grow-1">
                <label class="form-label fw-semibold" for="editor-settings-profile-currency-label">Currency</label>
                <input class="form-control" id="editor-settings-profile-currency-label" formControlName="currencyLabel" [placeholder]="baseLanguagePack?.currencyLabel ?? ''" />
                <p class="form-text">The label used to indicate the currency used on the invoice.</p>
              </div>
            </div>

            <div class="editor__control mb-3">
              <label class="form-label fw-semibold" for="editor-settings-profile-business-process-label">Business Process</label>
              <input
                class="form-control"
                id="editor-settings-profile-business-process-label"
                formControlName="businessProcessLabel"
                [placeholder]="baseLanguagePack?.businessProcessLabel ?? ''"
              />
              <p class="form-text">The label that describes the business process or transaction context (e.g., sales, service).</p>
            </div>

            <div class="d-flex flex-wrap gap-3 mb-3">
              <div class="editor__control flex-grow-1">
                <label class="form-label fw-semibold" for="editor-settings-profile-total-without-vat-label">Total (Net)</label>
                <input
                  class="form-control"
                  id="editor-settings-profile-total-without-vat-label"
                  formControlName="totalWithoutVatLabel"
                  [placeholder]="baseLanguagePack?.totalWithoutVatLabel ?? ''"
                />
                <p class="form-text">The label for the total amount before VAT is applied.</p>
              </div>

              <div class="editor__control flex-grow-1">
                <label class="form-label fw-semibold" for="editor-settings-profile-total-vat-label">Total VAT</label>
                <input class="form-control" id="editor-settings-profile-total-vat-label" formControlName="totalVatLabel" [placeholder]="baseLanguagePack?.totalVatLabel ?? ''" />
                <p class="form-text">The label for the total VAT amount calculated on the invoice.</p>
              </div>

              <div class="editor__control flex-grow-1">
                <label class="form-label fw-semibold" for="editor-settings-profile-total-gross-label">Total (Gross)</label>
                <input
                  class="form-control"
                  id="editor-settings-profile-total-gross-label"
                  formControlName="totalWithVatLabel"
                  [placeholder]="baseLanguagePack?.totalWithVatLabel ?? ''"
                />
                <p class="form-text">The label for the total amount including VAT.</p>
              </div>
            </div>

            <div class="editor__control mb-3">
              <label class="form-label fw-semibold" for="editor-settings-profile-prepaid-amount-label">Prepaid</label>
              <input
                class="form-control"
                id="editor-settings-profile-prepaid-amount-label"
                formControlName="prepaidAmountLabel"
                [placeholder]="baseLanguagePack?.prepaidAmountLabel ?? ''"
              />
              <p class="form-text">The label for any amount already paid in advance.</p>
            </div>

            <div class="d-flex flex-wrap gap-3 mb-3">
              <div class="editor__control flex-grow-1">
                <label class="form-label fw-semibold" for="editor-settings-profile-due-date-label">Due Date</label>
                <input class="form-control" id="editor-settings-profile-due-date-label" formControlName="dueDateLabel" [placeholder]="baseLanguagePack?.dueDateLabel ?? ''" />
                <p class="form-text">The label for the payment due date.</p>
              </div>

              <div class="editor__control flex-grow-1">
                <label class="form-label fw-semibold" for="editor-settings-profile-due-amount-label">Due Amount</label>
                <input class="form-control" id="editor-settings-profile-due-amount-label" formControlName="dueAmountLabel" [placeholder]="baseLanguagePack?.dueAmountLabel ?? ''" />
                <p class="form-text">The label for the remaining amount that needs to be paid.</p>
              </div>
            </div>

            <div class="editor__control mb-3">
              <label class="form-label fw-semibold" for="editor-settings-profile-page-label">Page</label>
              <input class="form-control" id="editor-settings-profile-page-label" formControlName="pageLabel" [placeholder]="baseLanguagePack?.pageLabel ?? ''" />
              <p class="form-text">The label used to indicate page numbers in multi-page invoices.</p>
            </div>

            <div formGroupName="documentTypeNames">
              <h6>Document Types</h6>
              <div class="border-top mb-3"></div>

              <div class="editor__control mb-3">
                <label class="form-label fw-semibold" for="editor-settings-profile-default-document-type-name">Default Document Type</label>
                <input
                  class="form-control"
                  id="editor-settings-profile-default-document-type-name"
                  formControlName="defaultDocumentTypeName"
                  [placeholder]="baseLanguagePack?.defaultDocumentTypeName ?? ''"
                />
                <p class="form-text">
                  The label used when the actual invoice type cannot be matched to a more specific name. Typically set to "Invoice", it serves as a fallback when no precise
                  document type label is available.
                </p>
              </div>

              <div class="d-flex flex-wrap gap-3 mb-3">
                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-RequestForPayment">71 - Request for payment</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-RequestForPayment"
                    formControlName="requestForPayment"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['RequestForPayment'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-DebitNoteRelatedToGoodsOrServices">80 - Debit note related to goods or services</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-DebitNoteRelatedToGoodsOrServices"
                    formControlName="debitNoteRelatedToGoodsOrServices"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['DebitNoteRelatedToGoodsOrServices'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-CreditNoteRelatedToGoodsOrServices">81 - Credit note related to goods or services</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-CreditNoteRelatedToGoodsOrServices"
                    formControlName="creditNoteRelatedToGoodsOrServices"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['CreditNoteRelatedToGoodsOrServices'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-MeteredServicesInvoice">82 - Metered services invoice</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-MeteredServicesInvoice"
                    formControlName="meteredServicesInvoice"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['MeteredServicesInvoice'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-CreditNoteRelatedToFinancialAdjustments"
                    >83 - Credit note related to financial adjustments</label
                  >
                  <input
                    class="form-control"
                    id="editor-settings-profile-CreditNoteRelatedToFinancialAdjustments"
                    formControlName="creditNoteRelatedToFinancialAdjustments"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['CreditNoteRelatedToFinancialAdjustments'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-DebitNoteRelatedToFinancialAdjustments"
                    >84 - Debit note related to financial adjustments</label
                  >
                  <input
                    class="form-control"
                    id="editor-settings-profile-DebitNoteRelatedToFinancialAdjustments"
                    formControlName="debitNoteRelatedToFinancialAdjustments"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['DebitNoteRelatedToFinancialAdjustments'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-TaxNotification">102 - Tax notification</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-TaxNotification"
                    formControlName="taxNotification"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['TaxNotification'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-InvoicingDataSheet">130 - Invoicing data sheet</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-InvoicingDataSheet"
                    formControlName="invoicingDataSheet"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['InvoicingDataSheet'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-DirectPaymentValuation">202 - Direct payment valuation</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-DirectPaymentValuation"
                    formControlName="directPaymentValuation"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['DirectPaymentValuation'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-ProvisionalPaymentValuation">203 - Provisional payment valuation</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-ProvisionalPaymentValuation"
                    formControlName="provisionalPaymentValuation"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['ProvisionalPaymentValuation'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-PaymentValuation">204 - Payment valuation</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-PaymentValuation"
                    formControlName="paymentValuation"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['PaymentValuation'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-InterimApplicationForPayment">211 - Interim application for payment</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-InterimApplicationForPayment"
                    formControlName="interimApplicationForPayment"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['InterimApplicationForPayment'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-FinalPaymentRequestBasedOnCompletionOfWork">
                    218 - Final payment request based on completion of work
                  </label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-FinalPaymentRequestBasedOnCompletionOfWork"
                    formControlName="finalPaymentRequestBasedOnCompletionOfWork"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['FinalPaymentRequestBasedOnCompletionOfWork'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-PaymentRequestForCompletedUnits">219 - Payment request for completed units</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-PaymentRequestForCompletedUnits"
                    formControlName="paymentRequestForCompletedUnits"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['PaymentRequestForCompletedUnits'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-SelfBilledCreditNote">261 - Self billed credit note</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-SelfBilledCreditNote"
                    formControlName="selfBilledCreditNote"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['SelfBilledCreditNote'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-ConsolidatedCreditNoteGoodsAndServices"
                    >262 - Consolidated credit note - goods and services</label
                  >
                  <input
                    class="form-control"
                    id="editor-settings-profile-ConsolidatedCreditNoteGoodsAndServices"
                    formControlName="consolidatedCreditNoteGoodsAndServices"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['ConsolidatedCreditNoteGoodsAndServices'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-PriceVariationInvoice">295 - Price variation invoice</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-PriceVariationInvoice"
                    formControlName="priceVariationInvoice"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['PriceVariationInvoice'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-CreditNoteForPriceVariation">296 - Credit note for price variation</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-CreditNoteForPriceVariation"
                    formControlName="creditNoteForPriceVariation"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['CreditNoteForPriceVariation'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-DelcredereCreditNote">308 - Delcredere credit note</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-DelcredereCreditNote"
                    formControlName="delcredereCreditNote"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['DelcredereCreditNote'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-ProformaInvoice">325 - Proforma invoice</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-ProformaInvoice"
                    formControlName="proformaInvoice"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['ProformaInvoice'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-PartialInvoice">326 - Partial invoice</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-PartialInvoice"
                    formControlName="partialInvoice"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['PartialInvoice'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-CommercialInvoiceWhichIncludesPackingList">
                    331 - Commercial invoice which includes a packing list
                  </label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-CommercialInvoiceWhichIncludesPackingList"
                    formControlName="commercialInvoiceWhichIncludesPackingList"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['CommercialInvoiceWhichIncludesPackingList'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-CommercialInvoice">380 - Commercial invoice</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-CommercialInvoice"
                    formControlName="commercialInvoice"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['CommercialInvoice'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-CreditNote">381 - Credit note</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-CreditNote"
                    formControlName="creditNote"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['CreditNote'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-CommissionNote">382 - Commission note</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-CommissionNote"
                    formControlName="commissionNote"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['CommissionNote'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-DebitNote">383 - Debit note</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-DebitNote"
                    formControlName="debitNote"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['DebitNote'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-CorrectedInvoice">384 - Corrected invoice</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-CorrectedInvoice"
                    formControlName="correctedInvoice"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['CorrectedInvoice'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-ConsolidatedInvoice">385 - Consolidated invoice</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-ConsolidatedInvoice"
                    formControlName="consolidatedInvoice"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['ConsolidatedInvoice'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-PrepaymentInvoice">386 - Prepayment invoice</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-PrepaymentInvoice"
                    formControlName="prepaymentInvoice"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['PrepaymentInvoice'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-HireInvoice">387 - Hire invoice</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-HireInvoice"
                    formControlName="hireInvoice"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['HireInvoice'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-TaxInvoice">388 - Tax invoice</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-TaxInvoice"
                    formControlName="taxInvoice"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['TaxInvoice'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-SelfBilledInvoice">389 - Self-billed invoice</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-SelfBilledInvoice"
                    formControlName="selfBilledInvoice"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['SelfBilledInvoice'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-DelcredereInvoice">390 - Delcredere invoice</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-DelcredereInvoice"
                    formControlName="delcredereInvoice"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['DelcredereInvoice'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-FactoredInvoice">393 - Factored invoice</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-FactoredInvoice"
                    formControlName="factoredInvoice"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['FactoredInvoice'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-LeaseInvoice">394 - Lease invoice</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-LeaseInvoice"
                    formControlName="leaseInvoice"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['LeaseInvoice'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-ConsignmentInvoice">395 - Consignment invoice</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-ConsignmentInvoice"
                    formControlName="consignmentInvoice"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['ConsignmentInvoice'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-FactoredCreditNote">396 - Factored credit note</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-FactoredCreditNote"
                    formControlName="factoredCreditNote"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['FactoredCreditNote'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-OcrPaymentCreditNote">420 - Optical Character Reading (OCR) payment credit note</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-OcrPaymentCreditNote"
                    formControlName="ocrPaymentCreditNote"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['OcrPaymentCreditNote'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-DebitAdvice">456 - Debit advice</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-DebitAdvice"
                    formControlName="debitAdvice"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['DebitAdvice'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-ReversalOfDebit">457 - Reversal of debit</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-ReversalOfDebit"
                    formControlName="reversalOfDebit"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['ReversalOfDebit'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-ReversalOfCredit">458 - Reversal of credit</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-ReversalOfCredit"
                    formControlName="reversalOfCredit"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['ReversalOfCredit'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-SelfBilledDebitNote">527 - Self billed debit note</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-SelfBilledDebitNote"
                    formControlName="selfBilledDebitNote"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['SelfBilledDebitNote'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-ForwardersCreditNote">532 - Forwarder's credit note</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-ForwardersCreditNote"
                    formControlName="forwardersCreditNote"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['ForwardersCreditNote'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-ForwardersInvoiceDiscrepancyReport">553 - Forwarder's invoice discrepancy report</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-ForwardersInvoiceDiscrepancyReport"
                    formControlName="forwardersInvoiceDiscrepancyReport"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['ForwardersInvoiceDiscrepancyReport'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-InsurersInvoice">575 - Insurer's invoice</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-InsurersInvoice"
                    formControlName="insurersInvoice"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['InsurersInvoice'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-ForwardersInvoice">623 - Forwarder's invoice</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-ForwardersInvoice"
                    formControlName="forwardersInvoice"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['ForwardersInvoice'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-PortChargesDocuments">633 - Port charges documents</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-PortChargesDocuments"
                    formControlName="portChargesDocuments"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['PortChargesDocuments'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-InvoiceInformationForAccountingPurposes"
                    >751 - Invoice information for accounting purposes</label
                  >
                  <input
                    class="form-control"
                    id="editor-settings-profile-InvoiceInformationForAccountingPurposes"
                    formControlName="invoiceInformationForAccountingPurposes"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['InvoiceInformationForAccountingPurposes'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-FreightInvoice">780 - Freight invoice</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-FreightInvoice"
                    formControlName="freightInvoice"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['FreightInvoice'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-ClaimNotification">817 - Claim notification</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-ClaimNotification"
                    formControlName="claimNotification"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['ClaimNotification'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-ConsularInvoice">870 - Consular invoice</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-ConsularInvoice"
                    formControlName="consularInvoice"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['ConsularInvoice'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-PartialConstructionInvoice">875 - Partial construction invoice</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-PartialConstructionInvoice"
                    formControlName="partialConstructionInvoice"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['PartialConstructionInvoice'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-PartialFinalConstructionInvoice">876 - Partial final construction invoice</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-PartialFinalConstructionInvoice"
                    formControlName="partialFinalConstructionInvoice"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['PartialFinalConstructionInvoice'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-FinalConstructionInvoice">877 - Final construction invoice</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-FinalConstructionInvoice"
                    formControlName="finalConstructionInvoice"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['FinalConstructionInvoice'] ?? ''"
                  />
                </div>

                <div class="editor__control col-6">
                  <label class="form-label fw-semibold" for="editor-settings-profile-CustomsInvoice">935 - Customs invoice</label>
                  <input
                    class="form-control"
                    id="editor-settings-profile-CustomsInvoice"
                    formControlName="customsInvoice"
                    [placeholder]="baseLanguagePack?.documentTypeNames?.['CustomsInvoice'] ?? ''"
                  />
                </div>
              </div>
            </div>
          }
        }
      </div>
    </form>
  `,
  styles: ``,
})
export class EditorSettingsPdfProfileFormComponent {
  private importFileService = inject(ImportFileService);
  private generateApi = inject(GenerateApi);

  protected formGroup = new FormGroup({
    name: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    logoBase64: new FormControl<string | undefined>(undefined, { nonNullable: true }),
    footer: new FormControl<string | undefined>(undefined, { nonNullable: true }),
    languagePack: new FormGroup({
      baseLanguagePack: new FormControl<string | null>(null),
      culture: new FormControl<string | null>(null),
      vatNumberLabel: new FormControl<string | null>(null),
      supplierReferencesLabel: new FormControl<string | null>(null),
      customerReferencesLabel: new FormControl<string | null>(null),
      orderLabel: new FormControl<string | null>(null),
      invoiceReferencesLabel: new FormControl<string | null>(null),
      businessProcessLabel: new FormControl<string | null>(null),
      dateLabel: new FormControl<string | null>(null),
      customerAddressLabel: new FormControl<string | null>(null),
      customerIdentifiersLabel: new FormControl<string | null>(null),
      deliveryInformationLabel: new FormControl<string | null>(null),
      currencyLabel: new FormControl<string | null>(null),
      totalWithoutVatLabel: new FormControl<string | null>(null),
      totalVatLabel: new FormControl<string | null>(null),
      totalWithVatLabel: new FormControl<string | null>(null),
      prepaidAmountLabel: new FormControl<string | null>(null),
      dueDateLabel: new FormControl<string | null>(null),
      dueAmountLabel: new FormControl<string | null>(null),
      defaultLegalIdType: new FormControl<string | null>(null),
      pageLabel: new FormControl<string | null>(null),
      documentTypeNames: new FormGroup({
        defaultDocumentTypeName: new FormControl<string | null>(null),
        requestForPayment: new FormControl<string | null>(null),
        debitNoteRelatedToGoodsOrServices: new FormControl<string | null>(null),
        creditNoteRelatedToGoodsOrServices: new FormControl<string | null>(null),
        meteredServicesInvoice: new FormControl<string | null>(null),
        creditNoteRelatedToFinancialAdjustments: new FormControl<string | null>(null),
        debitNoteRelatedToFinancialAdjustments: new FormControl<string | null>(null),
        taxNotification: new FormControl<string | null>(null),
        invoicingDataSheet: new FormControl<string | null>(null),
        directPaymentValuation: new FormControl<string | null>(null),
        provisionalPaymentValuation: new FormControl<string | null>(null),
        paymentValuation: new FormControl<string | null>(null),
        interimApplicationForPayment: new FormControl<string | null>(null),
        finalPaymentRequestBasedOnCompletionOfWork: new FormControl<string | null>(null),
        paymentRequestForCompletedUnits: new FormControl<string | null>(null),
        selfBilledCreditNote: new FormControl<string | null>(null),
        consolidatedCreditNoteGoodsAndServices: new FormControl<string | null>(null),
        priceVariationInvoice: new FormControl<string | null>(null),
        creditNoteForPriceVariation: new FormControl<string | null>(null),
        delcredereCreditNote: new FormControl<string | null>(null),
        proformaInvoice: new FormControl<string | null>(null),
        partialInvoice: new FormControl<string | null>(null),
        commercialInvoiceWhichIncludesPackingList: new FormControl<string | null>(null),
        commercialInvoice: new FormControl<string | null>(null),
        creditNote: new FormControl<string | null>(null),
        commissionNote: new FormControl<string | null>(null),
        debitNote: new FormControl<string | null>(null),
        correctedInvoice: new FormControl<string | null>(null),
        consolidatedInvoice: new FormControl<string | null>(null),
        prepaymentInvoice: new FormControl<string | null>(null),
        hireInvoice: new FormControl<string | null>(null),
        taxInvoice: new FormControl<string | null>(null),
        selfBilledInvoice: new FormControl<string | null>(null),
        delcredereInvoice: new FormControl<string | null>(null),
        factoredInvoice: new FormControl<string | null>(null),
        leaseInvoice: new FormControl<string | null>(null),
        consignmentInvoice: new FormControl<string | null>(null),
        factoredCreditNote: new FormControl<string | null>(null),
        ocrPaymentCreditNote: new FormControl<string | null>(null),
        debitAdvice: new FormControl<string | null>(null),
        reversalOfDebit: new FormControl<string | null>(null),
        reversalOfCredit: new FormControl<string | null>(null),
        selfBilledDebitNote: new FormControl<string | null>(null),
        forwardersCreditNote: new FormControl<string | null>(null),
        forwardersInvoiceDiscrepancyReport: new FormControl<string | null>(null),
        insurersInvoice: new FormControl<string | null>(null),
        forwardersInvoice: new FormControl<string | null>(null),
        portChargesDocuments: new FormControl<string | null>(null),
        invoiceInformationForAccountingPurposes: new FormControl<string | null>(null),
        freightInvoice: new FormControl<string | null>(null),
        claimNotification: new FormControl<string | null>(null),
        consularInvoice: new FormControl<string | null>(null),
        partialConstructionInvoice: new FormControl<string | null>(null),
        partialFinalConstructionInvoice: new FormControl<string | null>(null),
        finalConstructionInvoice: new FormControl<string | null>(null),
        customsInvoice: new FormControl<string | null>(null),
      }),
    }),
  });

  protected languagePacks = rxResource({
    loader: () => this.generateApi.getStandardPdfLanguagePacks(),
  });
  private selectedBaseLanguagePack = toSignal(
    this.formGroup.valueChanges.pipe(
      map((_) => this.formGroup.controls.languagePack.controls.baseLanguagePack.value),
      distinctUntilChanged(),
    ),
  );
  protected baseLanguagePack: Signal<IStandardPdfGeneratorLanguagePackDto | undefined> = computed(() => {
    if (this.languagePacks.isLoading()) {
      return undefined;
    }

    const languagePacks = this.languagePacks.value();
    if (languagePacks === undefined || languagePacks.length === 0) {
      return {};
    }

    const selectedPack = this.selectedBaseLanguagePack();
    if (selectedPack === undefined) {
      return {};
    }

    const result = languagePacks.find((x) => x.culture === selectedPack) ?? {};
    console.log(languagePacks, result);
    return result;
  });

  getValue(): EditorPdfGenerationProfileData {
    this.formGroup.markAllAsTouched();
    if (this.formGroup.invalid) {
      throw new Error('Please fill in all required fields.');
    }

    return {
      name: this.formGroup.controls.name.value,
      logoBase64: this.formGroup.controls.logoBase64.value,
      footer: this.formGroup.controls.footer.value,
      languagePack: {
        baseLanguagePack: this.formGroup.controls.languagePack.controls.baseLanguagePack.value ?? undefined,
        culture: this.formGroup.controls.languagePack.controls.culture.value ?? undefined,
        vatNumberLabel: this.formGroup.controls.languagePack.controls.vatNumberLabel.value ?? undefined,
        supplierReferencesLabel: this.formGroup.controls.languagePack.controls.supplierReferencesLabel.value ?? undefined,
        customerReferencesLabel: this.formGroup.controls.languagePack.controls.customerReferencesLabel.value ?? undefined,
        orderLabel: this.formGroup.controls.languagePack.controls.orderLabel.value ?? undefined,
        invoiceReferencesLabel: this.formGroup.controls.languagePack.controls.invoiceReferencesLabel.value ?? undefined,
        businessProcessLabel: this.formGroup.controls.languagePack.controls.businessProcessLabel.value ?? undefined,
        dateLabel: this.formGroup.controls.languagePack.controls.dateLabel.value ?? undefined,
        customerAddressLabel: this.formGroup.controls.languagePack.controls.customerAddressLabel.value ?? undefined,
        customerIdentifiersLabel: this.formGroup.controls.languagePack.controls.customerIdentifiersLabel.value ?? undefined,
        deliveryInformationLabel: this.formGroup.controls.languagePack.controls.deliveryInformationLabel.value ?? undefined,
        currencyLabel: this.formGroup.controls.languagePack.controls.currencyLabel.value ?? undefined,
        totalWithoutVatLabel: this.formGroup.controls.languagePack.controls.totalWithoutVatLabel.value ?? undefined,
        totalVatLabel: this.formGroup.controls.languagePack.controls.totalVatLabel.value ?? undefined,
        totalWithVatLabel: this.formGroup.controls.languagePack.controls.totalWithVatLabel.value ?? undefined,
        prepaidAmountLabel: this.formGroup.controls.languagePack.controls.prepaidAmountLabel.value ?? undefined,
        dueDateLabel: this.formGroup.controls.languagePack.controls.dueDateLabel.value ?? undefined,
        dueAmountLabel: this.formGroup.controls.languagePack.controls.dueAmountLabel.value ?? undefined,
        defaultLegalIdType: this.formGroup.controls.languagePack.controls.defaultLegalIdType.value ?? undefined,
        pageLabel: this.formGroup.controls.languagePack.controls.pageLabel.value ?? undefined,
        defaultDocumentTypeName: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.defaultDocumentTypeName.value ?? undefined,
        documentTypeNames: {
          requestForPayment: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.requestForPayment.value ?? '',
          debitNoteRelatedToGoodsOrServices: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.debitNoteRelatedToGoodsOrServices.value ?? '',
          creditNoteRelatedToGoodsOrServices: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.creditNoteRelatedToGoodsOrServices.value ?? '',
          meteredServicesInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.meteredServicesInvoice.value ?? '',
          creditNoteRelatedToFinancialAdjustments: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.creditNoteRelatedToFinancialAdjustments.value ?? '',
          debitNoteRelatedToFinancialAdjustments: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.debitNoteRelatedToFinancialAdjustments.value ?? '',
          taxNotification: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.taxNotification.value ?? '',
          invoicingDataSheet: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.invoicingDataSheet.value ?? '',
          directPaymentValuation: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.directPaymentValuation.value ?? '',
          provisionalPaymentValuation: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.provisionalPaymentValuation.value ?? '',
          paymentValuation: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.paymentValuation.value ?? '',
          interimApplicationForPayment: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.interimApplicationForPayment.value ?? '',
          finalPaymentRequestBasedOnCompletionOfWork:
            this.formGroup.controls.languagePack.controls.documentTypeNames.controls.finalPaymentRequestBasedOnCompletionOfWork.value ?? '',
          paymentRequestForCompletedUnits: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.paymentRequestForCompletedUnits.value ?? '',
          selfBilledCreditNote: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.selfBilledCreditNote.value ?? '',
          consolidatedCreditNoteGoodsAndServices: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.consolidatedCreditNoteGoodsAndServices.value ?? '',
          priceVariationInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.priceVariationInvoice.value ?? '',
          creditNoteForPriceVariation: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.creditNoteForPriceVariation.value ?? '',
          delcredereCreditNote: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.delcredereCreditNote.value ?? '',
          proformaInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.proformaInvoice.value ?? '',
          partialInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.partialInvoice.value ?? '',
          commercialInvoiceWhichIncludesPackingList: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.commercialInvoiceWhichIncludesPackingList.value ?? '',
          commercialInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.commercialInvoice.value ?? '',
          creditNote: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.creditNote.value ?? '',
          commissionNote: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.commissionNote.value ?? '',
          debitNote: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.debitNote.value ?? '',
          correctedInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.correctedInvoice.value ?? '',
          consolidatedInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.consolidatedInvoice.value ?? '',
          prepaymentInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.prepaymentInvoice.value ?? '',
          hireInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.hireInvoice.value ?? '',
          taxInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.taxInvoice.value ?? '',
          selfBilledInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.selfBilledInvoice.value ?? '',
          delcredereInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.delcredereInvoice.value ?? '',
          factoredInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.factoredInvoice.value ?? '',
          leaseInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.leaseInvoice.value ?? '',
          consignmentInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.consignmentInvoice.value ?? '',
          factoredCreditNote: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.factoredCreditNote.value ?? '',
          ocrPaymentCreditNote: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.ocrPaymentCreditNote.value ?? '',
          debitAdvice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.debitAdvice.value ?? '',
          reversalOfDebit: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.reversalOfDebit.value ?? '',
          reversalOfCredit: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.reversalOfCredit.value ?? '',
          selfBilledDebitNote: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.selfBilledDebitNote.value ?? '',
          forwardersCreditNote: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.forwardersCreditNote.value ?? '',
          forwardersInvoiceDiscrepancyReport: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.forwardersInvoiceDiscrepancyReport.value ?? '',
          insurersInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.insurersInvoice.value ?? '',
          forwardersInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.forwardersInvoice.value ?? '',
          portChargesDocuments: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.portChargesDocuments.value ?? '',
          invoiceInformationForAccountingPurposes: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.invoiceInformationForAccountingPurposes.value ?? '',
          freightInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.freightInvoice.value ?? '',
          claimNotification: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.claimNotification.value ?? '',
          consularInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.consularInvoice.value ?? '',
          partialConstructionInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.partialConstructionInvoice.value ?? '',
          partialFinalConstructionInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.partialFinalConstructionInvoice.value ?? '',
          finalConstructionInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.finalConstructionInvoice.value ?? '',
          customsInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.customsInvoice.value ?? '',
        } satisfies { [key: string]: string },
      },
    };
  }

  setValue(profile: EditorPdfGenerationProfileData) {
    this.formGroup.setValue({
      name: profile.name,
      logoBase64: profile.logoBase64,
      footer: profile.footer,
      languagePack: {
        baseLanguagePack: profile.languagePack?.baseLanguagePack ?? null,
        culture: profile.languagePack?.culture ?? null,
        vatNumberLabel: profile.languagePack?.vatNumberLabel ?? null,
        supplierReferencesLabel: profile.languagePack?.supplierReferencesLabel ?? null,
        customerReferencesLabel: profile.languagePack?.customerReferencesLabel ?? null,
        orderLabel: profile.languagePack?.orderLabel ?? null,
        invoiceReferencesLabel: profile.languagePack?.invoiceReferencesLabel ?? null,
        businessProcessLabel: profile.languagePack?.businessProcessLabel ?? null,
        dateLabel: profile.languagePack?.dateLabel ?? null,
        customerAddressLabel: profile.languagePack?.customerAddressLabel ?? null,
        customerIdentifiersLabel: profile.languagePack?.customerIdentifiersLabel ?? null,
        deliveryInformationLabel: profile.languagePack?.deliveryInformationLabel ?? null,
        currencyLabel: profile.languagePack?.currencyLabel ?? null,
        totalWithoutVatLabel: profile.languagePack?.totalWithoutVatLabel ?? null,
        totalVatLabel: profile.languagePack?.totalVatLabel ?? null,
        totalWithVatLabel: profile.languagePack?.totalWithVatLabel ?? null,
        prepaidAmountLabel: profile.languagePack?.prepaidAmountLabel ?? null,
        dueDateLabel: profile.languagePack?.dueDateLabel ?? null,
        dueAmountLabel: profile.languagePack?.dueAmountLabel ?? null,
        defaultLegalIdType: profile.languagePack?.defaultLegalIdType ?? null,
        pageLabel: profile.languagePack?.pageLabel ?? null,
        documentTypeNames: {
          defaultDocumentTypeName: profile.languagePack?.defaultDocumentTypeName ?? null,
          requestForPayment: profile.languagePack?.documentTypeNames?.['RequestForPayment'] ?? null,
          debitNoteRelatedToGoodsOrServices: profile.languagePack?.documentTypeNames?.['DebitNoteRelatedToGoodsOrServices'] ?? null,
          creditNoteRelatedToGoodsOrServices: profile.languagePack?.documentTypeNames?.['CreditNoteRelatedToGoodsOrServices'] ?? null,
          meteredServicesInvoice: profile.languagePack?.documentTypeNames?.['MeteredServicesInvoice'] ?? null,
          creditNoteRelatedToFinancialAdjustments: profile.languagePack?.documentTypeNames?.['CreditNoteRelatedToFinancialAdjustments'] ?? null,
          debitNoteRelatedToFinancialAdjustments: profile.languagePack?.documentTypeNames?.['DebitNoteRelatedToFinancialAdjustments'] ?? null,
          taxNotification: profile.languagePack?.documentTypeNames?.['TaxNotification'] ?? null,
          invoicingDataSheet: profile.languagePack?.documentTypeNames?.['InvoicingDataSheet'] ?? null,
          directPaymentValuation: profile.languagePack?.documentTypeNames?.['DirectPaymentValuation'] ?? null,
          provisionalPaymentValuation: profile.languagePack?.documentTypeNames?.['ProvisionalPaymentValuation'] ?? null,
          paymentValuation: profile.languagePack?.documentTypeNames?.['PaymentValuation'] ?? null,
          interimApplicationForPayment: profile.languagePack?.documentTypeNames?.['InterimApplicationForPayment'] ?? null,
          finalPaymentRequestBasedOnCompletionOfWork: profile.languagePack?.documentTypeNames?.['FinalPaymentRequestBasedOnCompletionOfWork'] ?? null,
          paymentRequestForCompletedUnits: profile.languagePack?.documentTypeNames?.['PaymentRequestForCompletedUnits'] ?? null,
          selfBilledCreditNote: profile.languagePack?.documentTypeNames?.['SelfBilledCreditNote'] ?? null,
          consolidatedCreditNoteGoodsAndServices: profile.languagePack?.documentTypeNames?.['ConsolidatedCreditNoteGoodsAndServices'] ?? null,
          priceVariationInvoice: profile.languagePack?.documentTypeNames?.['PriceVariationInvoice'] ?? null,
          creditNoteForPriceVariation: profile.languagePack?.documentTypeNames?.['CreditNoteForPriceVariation'] ?? null,
          delcredereCreditNote: profile.languagePack?.documentTypeNames?.['DelcredereCreditNote'] ?? null,
          proformaInvoice: profile.languagePack?.documentTypeNames?.['ProformaInvoice'] ?? null,
          partialInvoice: profile.languagePack?.documentTypeNames?.['PartialInvoice'] ?? null,
          commercialInvoiceWhichIncludesPackingList: profile.languagePack?.documentTypeNames?.['CommercialInvoiceWhichIncludesPackingList'] ?? null,
          commercialInvoice: profile.languagePack?.documentTypeNames?.['CommercialInvoice'] ?? null,
          creditNote: profile.languagePack?.documentTypeNames?.['CreditNote'] ?? null,
          commissionNote: profile.languagePack?.documentTypeNames?.['CommissionNote'] ?? null,
          debitNote: profile.languagePack?.documentTypeNames?.['DebitNote'] ?? null,
          correctedInvoice: profile.languagePack?.documentTypeNames?.['CorrectedInvoice'] ?? null,
          consolidatedInvoice: profile.languagePack?.documentTypeNames?.['ConsolidatedInvoice'] ?? null,
          prepaymentInvoice: profile.languagePack?.documentTypeNames?.['PrepaymentInvoice'] ?? null,
          hireInvoice: profile.languagePack?.documentTypeNames?.['HireInvoice'] ?? null,
          taxInvoice: profile.languagePack?.documentTypeNames?.['TaxInvoice'] ?? null,
          selfBilledInvoice: profile.languagePack?.documentTypeNames?.['SelfBilledInvoice'] ?? null,
          delcredereInvoice: profile.languagePack?.documentTypeNames?.['DelcredereInvoice'] ?? null,
          factoredInvoice: profile.languagePack?.documentTypeNames?.['FactoredInvoice'] ?? null,
          leaseInvoice: profile.languagePack?.documentTypeNames?.['LeaseInvoice'] ?? null,
          consignmentInvoice: profile.languagePack?.documentTypeNames?.['ConsignmentInvoice'] ?? null,
          factoredCreditNote: profile.languagePack?.documentTypeNames?.['FactoredCreditNote'] ?? null,
          ocrPaymentCreditNote: profile.languagePack?.documentTypeNames?.['OcrPaymentCreditNote'] ?? null,
          debitAdvice: profile.languagePack?.documentTypeNames?.['DebitAdvice'] ?? null,
          reversalOfDebit: profile.languagePack?.documentTypeNames?.['ReversalOfDebit'] ?? null,
          reversalOfCredit: profile.languagePack?.documentTypeNames?.['ReversalOfCredit'] ?? null,
          selfBilledDebitNote: profile.languagePack?.documentTypeNames?.['SelfBilledDebitNote'] ?? null,
          forwardersCreditNote: profile.languagePack?.documentTypeNames?.['ForwardersCreditNote'] ?? null,
          forwardersInvoiceDiscrepancyReport: profile.languagePack?.documentTypeNames?.['ForwardersInvoiceDiscrepancyReport'] ?? null,
          insurersInvoice: profile.languagePack?.documentTypeNames?.['InsurersInvoice'] ?? null,
          forwardersInvoice: profile.languagePack?.documentTypeNames?.['ForwardersInvoice'] ?? null,
          portChargesDocuments: profile.languagePack?.documentTypeNames?.['PortChargesDocuments'] ?? null,
          invoiceInformationForAccountingPurposes: profile.languagePack?.documentTypeNames?.['InvoiceInformationForAccountingPurposes'] ?? null,
          freightInvoice: profile.languagePack?.documentTypeNames?.['FreightInvoice'] ?? null,
          claimNotification: profile.languagePack?.documentTypeNames?.['ClaimNotification'] ?? null,
          consularInvoice: profile.languagePack?.documentTypeNames?.['ConsularInvoice'] ?? null,
          partialConstructionInvoice: profile.languagePack?.documentTypeNames?.['PartialConstructionInvoice'] ?? null,
          partialFinalConstructionInvoice: profile.languagePack?.documentTypeNames?.['PartialFinalConstructionInvoice'] ?? null,
          finalConstructionInvoice: profile.languagePack?.documentTypeNames?.['FinalConstructionInvoice'] ?? null,
          customsInvoice: profile.languagePack?.documentTypeNames?.['CustomsInvoice'] ?? null,
        },
      },
    });
  }

  protected async chooseLogo(): Promise<void> {
    const logoFile = await this.importFileService.importFile('image/*');
    if (!logoFile) {
      return;
    }

    const data = await toBase64(logoFile);
    this.formGroup.patchValue({ logoBase64: data });
  }

  protected removeLogo() {
    this.formGroup.patchValue({ logoBase64: undefined });
  }
}

const toBase64 = (file: File): Promise<string> =>
  new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result as string);
    reader.onerror = reject;
  });
