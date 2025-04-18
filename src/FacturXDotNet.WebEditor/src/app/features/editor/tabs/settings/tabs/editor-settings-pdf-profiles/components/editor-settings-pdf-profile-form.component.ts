import { Component, computed, inject, Signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ImportFileService } from '../../../../../../../core/import-file/import-file.service';
import { rxResource, toSignal } from '@angular/core/rxjs-interop';
import { GenerateApi } from '../../../../../../../core/api/generate.api';
import { distinctUntilChanged, map } from 'rxjs';
import { IStandardPdfGeneratorLanguagePackDto } from '../../../../../../../core/api/api.models';
import { EditorPdfGenerationProfileData } from '../../../../../editor-pdf-generation-profiles.service';
import { EditorSettingsLanguagePackDocumentTypesFormComponent } from './editor-settings-language-pack-document-types-form.component';

@Component({
  selector: 'app-editor-settings-pdf-profile-form',
  imports: [ReactiveFormsModule, EditorSettingsLanguagePackDocumentTypesFormComponent],
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

            <div class="d-flex flex-wrap gap-3 mb-3">
              <div class="editor__control mb-3">
                <label class="form-label fw-semibold" for="editor-settings-profile-default-invoice-documents-type-name">Default Invoice Documents Name</label>
                <input
                  class="form-control"
                  id="editor-settings-profile-default-invoice-documents-type-name"
                  formControlName="defaultInvoiceDocumentsTypeName"
                  [placeholder]="baseLanguagePack?.defaultInvoiceDocumentsTypeName ?? ''"
                />
                <p class="form-text">
                  The label used when the document is an invoice and the actual invoice type cannot be matched to a more specific name. Typically set to "Invoice", it serves as a
                  fallback when no precise document type label is available.
                </p>
              </div>

              <div class="editor__control mb-3">
                <label class="form-label fw-semibold" for="editor-settings-profile-default-credit-note-documents-type-name">Default Credit Note Documents Name</label>
                <input
                  class="form-control"
                  id="editor-settings-profile-default-credit-note-documents-type-name"
                  formControlName="defaultCreditNoteDocumentsTypeName"
                  [placeholder]="baseLanguagePack?.defaultCreditNoteDocumentsTypeName ?? ''"
                />
                <p class="form-text">
                  The label used when the document is a credit note and the actual credit note type cannot be matched to a more specific name. Typically set to "Credit Note", it
                  serves as a fallback when no precise document type label is available.
                </p>
              </div>
            </div>

            <h6>Document Types</h6>
            <div class="border-top mt-1 mb-3"></div>
            <p class="form-text">
              The fields above let you set the labels for common document types like invoices and credit notes. However, there are many more specific variations of these documents.
            </p>
            @if (!overrideLanguagePack()) {
              <button class="btn btn-sm btn-light border" (click)="overrideDocumentTypeNames(true)">Override document type names</button>
            } @else {
              <button class="btn btn-sm btn-light border mb-3" (click)="overrideDocumentTypeNames(false)">
                <i class="bi bi-x-lg"></i> Use default invoice and credit note names
              </button>
              <app-editor-settings-language-pack-document-types-form formGroupName="documentTypeNames" [baseLanguagePack]="baseLanguagePack" />
            }
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
      defaultInvoiceDocumentsTypeName: new FormControl<string | null>(null),
      defaultCreditNoteDocumentsTypeName: new FormControl<string | null>(null),
      overrideDocumentTypeNames: new FormControl(false),
      documentTypeNames: new FormGroup({
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

  protected overrideLanguagePack = toSignal(
    this.formGroup.valueChanges.pipe(
      map((_) => this.formGroup.controls.languagePack.controls.overrideDocumentTypeNames.value),
      distinctUntilChanged(),
    ),
  );

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

    return languagePacks.find((x) => x.culture === selectedPack) ?? {};
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
        defaultInvoiceDocumentsTypeName: this.formGroup.controls.languagePack.controls.defaultInvoiceDocumentsTypeName.value ?? undefined,
        defaultCreditNoteDocumentsTypeName: this.formGroup.controls.languagePack.controls.defaultCreditNoteDocumentsTypeName.value ?? undefined,
        documentTypeNames: this.formGroup.controls.languagePack.controls.overrideDocumentTypeNames.value
          ? {
              RequestForPayment: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.requestForPayment.value ?? undefined,
              DebitNoteRelatedToGoodsOrServices: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.debitNoteRelatedToGoodsOrServices.value ?? undefined,
              CreditNoteRelatedToGoodsOrServices: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.creditNoteRelatedToGoodsOrServices.value ?? undefined,
              MeteredServicesInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.meteredServicesInvoice.value ?? undefined,
              CreditNoteRelatedToFinancialAdjustments:
                this.formGroup.controls.languagePack.controls.documentTypeNames.controls.creditNoteRelatedToFinancialAdjustments.value ?? undefined,
              DebitNoteRelatedToFinancialAdjustments:
                this.formGroup.controls.languagePack.controls.documentTypeNames.controls.debitNoteRelatedToFinancialAdjustments.value ?? undefined,
              TaxNotification: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.taxNotification.value ?? undefined,
              InvoicingDataSheet: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.invoicingDataSheet.value ?? undefined,
              DirectPaymentValuation: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.directPaymentValuation.value ?? undefined,
              ProvisionalPaymentValuation: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.provisionalPaymentValuation.value ?? undefined,
              PaymentValuation: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.paymentValuation.value ?? undefined,
              InterimApplicationForPayment: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.interimApplicationForPayment.value ?? undefined,
              FinalPaymentRequestBasedOnCompletionOfWork:
                this.formGroup.controls.languagePack.controls.documentTypeNames.controls.finalPaymentRequestBasedOnCompletionOfWork.value ?? undefined,
              PaymentRequestForCompletedUnits: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.paymentRequestForCompletedUnits.value ?? undefined,
              SelfBilledCreditNote: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.selfBilledCreditNote.value ?? undefined,
              ConsolidatedCreditNoteGoodsAndServices:
                this.formGroup.controls.languagePack.controls.documentTypeNames.controls.consolidatedCreditNoteGoodsAndServices.value ?? undefined,
              PriceVariationInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.priceVariationInvoice.value ?? undefined,
              CreditNoteForPriceVariation: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.creditNoteForPriceVariation.value ?? undefined,
              DelcredereCreditNote: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.delcredereCreditNote.value ?? undefined,
              ProformaInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.proformaInvoice.value ?? undefined,
              PartialInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.partialInvoice.value ?? undefined,
              CommercialInvoiceWhichIncludesPackingList:
                this.formGroup.controls.languagePack.controls.documentTypeNames.controls.commercialInvoiceWhichIncludesPackingList.value ?? undefined,
              CommercialInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.commercialInvoice.value ?? undefined,
              CreditNote: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.creditNote.value ?? undefined,
              CommissionNote: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.commissionNote.value ?? undefined,
              DebitNote: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.debitNote.value ?? undefined,
              CorrectedInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.correctedInvoice.value ?? undefined,
              ConsolidatedInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.consolidatedInvoice.value ?? undefined,
              PrepaymentInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.prepaymentInvoice.value ?? undefined,
              HireInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.hireInvoice.value ?? undefined,
              TaxInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.taxInvoice.value ?? undefined,
              SelfBilledInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.selfBilledInvoice.value ?? undefined,
              DelcredereInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.delcredereInvoice.value ?? undefined,
              FactoredInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.factoredInvoice.value ?? undefined,
              LeaseInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.leaseInvoice.value ?? undefined,
              ConsignmentInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.consignmentInvoice.value ?? undefined,
              FactoredCreditNote: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.factoredCreditNote.value ?? undefined,
              OcrPaymentCreditNote: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.ocrPaymentCreditNote.value ?? undefined,
              DebitAdvice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.debitAdvice.value ?? undefined,
              ReversalOfDebit: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.reversalOfDebit.value ?? undefined,
              ReversalOfCredit: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.reversalOfCredit.value ?? undefined,
              SelfBilledDebitNote: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.selfBilledDebitNote.value ?? undefined,
              ForwardersCreditNote: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.forwardersCreditNote.value ?? undefined,
              ForwardersInvoiceDiscrepancyReport: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.forwardersInvoiceDiscrepancyReport.value ?? undefined,
              InsurersInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.insurersInvoice.value ?? undefined,
              ForwardersInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.forwardersInvoice.value ?? undefined,
              PortChargesDocuments: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.portChargesDocuments.value ?? undefined,
              InvoiceInformationForAccountingPurposes:
                this.formGroup.controls.languagePack.controls.documentTypeNames.controls.invoiceInformationForAccountingPurposes.value ?? undefined,
              FreightInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.freightInvoice.value ?? undefined,
              ClaimNotification: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.claimNotification.value ?? undefined,
              ConsularInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.consularInvoice.value ?? undefined,
              PartialConstructionInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.partialConstructionInvoice.value ?? undefined,
              PartialFinalConstructionInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.partialFinalConstructionInvoice.value ?? undefined,
              FinalConstructionInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.finalConstructionInvoice.value ?? undefined,
              CustomsInvoice: this.formGroup.controls.languagePack.controls.documentTypeNames.controls.customsInvoice.value ?? undefined,
            }
          : undefined,
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
        culture: profile.languagePack?.['Culture'] ?? null,
        vatNumberLabel: profile.languagePack?.['VatNumberLabel'] ?? null,
        supplierReferencesLabel: profile.languagePack?.['SupplierReferencesLabel'] ?? null,
        customerReferencesLabel: profile.languagePack?.['CustomerReferencesLabel'] ?? null,
        orderLabel: profile.languagePack?.['OrderLabel'] ?? null,
        invoiceReferencesLabel: profile.languagePack?.['InvoiceReferencesLabel'] ?? null,
        businessProcessLabel: profile.languagePack?.['BusinessProcessLabel'] ?? null,
        dateLabel: profile.languagePack?.['DateLabel'] ?? null,
        customerAddressLabel: profile.languagePack?.['CustomerAddressLabel'] ?? null,
        customerIdentifiersLabel: profile.languagePack?.['CustomerIdentifiersLabel'] ?? null,
        deliveryInformationLabel: profile.languagePack?.['DeliveryInformationLabel'] ?? null,
        currencyLabel: profile.languagePack?.['CurrencyLabel'] ?? null,
        totalWithoutVatLabel: profile.languagePack?.['TotalWithoutVatLabel'] ?? null,
        totalVatLabel: profile.languagePack?.['TotalVatLabel'] ?? null,
        totalWithVatLabel: profile.languagePack?.['TotalWithVatLabel'] ?? null,
        prepaidAmountLabel: profile.languagePack?.['PrepaidAmountLabel'] ?? null,
        dueDateLabel: profile.languagePack?.['DueDateLabel'] ?? null,
        dueAmountLabel: profile.languagePack?.['DueAmountLabel'] ?? null,
        defaultLegalIdType: profile.languagePack?.['DefaultLegalIdType'] ?? null,
        pageLabel: profile.languagePack?.['PageLabel'] ?? null,
        defaultInvoiceDocumentsTypeName: profile.languagePack?.['DefaultInvoiceDocumentsTypeName'] ?? null,
        defaultCreditNoteDocumentsTypeName: profile.languagePack?.['DefaultCreditNoteDocumentsTypeName'] ?? null,
        overrideDocumentTypeNames: this.formGroup.controls.languagePack.controls.overrideDocumentTypeNames.value,
        documentTypeNames: {
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

  protected async overrideDocumentTypeNames(value: boolean) {
    this.formGroup.controls.languagePack.controls.overrideDocumentTypeNames.setValue(value);
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
