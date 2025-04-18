import { Component, inject, input } from '@angular/core';
import { ControlContainer, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { IStandardPdfGeneratorLanguagePackDto } from '../../../../../../../core/api/api.models';

@Component({
  selector: 'app-editor-settings-language-pack-form',
  viewProviders: [
    {
      provide: ControlContainer,
      useFactory: () => {
        return inject(ControlContainer, { skipSelf: true });
      },
    },
  ],
  imports: [ReactiveFormsModule],
  template: `
    <div [formGroupName]="formGroupName()">
      <div class="editor__control mb-3">
        <label class="form-label fw-semibold" for="editor-settings-profile-base-pack">Base pack</label>
        <select class="form-select" id="editor-settings-profile-base-pack" formControlName="baseLanguagePack">
          <option [ngValue]="undefined"></option>
          @for (languagePack of languagePacks(); track languagePack.culture) {
            <option [ngValue]="languagePack.culture">{{ languagePack.culture }}</option>
          }
        </select>
        <p class="form-text">
          Select a predefined language pack to use as a starting point for the labels on your invoice. You can override any of the default terms to better fit your needs.
        </p>
      </div>

      <div class="editor__control mb-3">
        <label class="form-label fw-semibold" for="editor-settings-profile-culture">Culture</label>
        <input class="form-control" id="editor-settings-profile-culture" formControlName="culture" [placeholder]="baseLanguagePack()?.culture ?? ''" />
        <p class="form-text">
          Specifies the language and regional settings used for the invoice. This affect region-specific formatting rules such as date formats, and number separators.
        </p>
      </div>

      <div class="editor__control mb-3">
        <label class="form-label fw-semibold" for="editor-settings-profile-date-label">Date</label>
        <input class="form-control" id="editor-settings-profile-date-label" formControlName="dateLabel" [placeholder]="baseLanguagePack()?.dateLabel ?? ''" />
        <p class="form-text">The label used for the invoice issue date.</p>
      </div>

      <div class="editor__control mb-3">
        <label class="form-label fw-semibold" for="editor-settings-profile-default-legal-id-type">Default Legal ID name</label>
        <input
          class="form-control"
          id="editor-settings-profile-default-legal-id-type"
          formControlName="defaultLegalIdType"
          [placeholder]="baseLanguagePack()?.defaultLegalIdType ?? ''"
        />
        <p class="form-text">
          The label used when the legal identification scheme in the invoice is not recognized. While known schemes like SIREN or SIRET have specific labels, this default value is
          used as a fallback for unrecognized or unmapped schemes.
        </p>
      </div>

      <div class="d-flex flex-wrap gap-3 mb-3">
        <div class="editor__control flex-grow-1">
          <label class="form-label fw-semibold" for="editor-settings-profile-vat-number-label">VAT Number</label>
          <input class="form-control" id="editor-settings-profile-vat-number-label" formControlName="vatNumberLabel" [placeholder]="baseLanguagePack()?.vatNumberLabel ?? ''" />
          <p class="form-text">The label used to display your VAT (Value Added Tax) number on the invoice.</p>
        </div>
        <div class="editor__control flex-grow-1">
          <label class="form-label fw-semibold" for="editor-settings-profile-customer-address-label">Customer Address</label>
          <input
            class="form-control"
            id="editor-settings-profile-customer-address-label"
            formControlName="customerAddressLabel"
            [placeholder]="baseLanguagePack()?.customerAddressLabel ?? ''"
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
            [placeholder]="baseLanguagePack()?.supplierReferencesLabel ?? ''"
          />
          <p class="form-text">The label for your internal reference or tracking number related to the invoice.</p>
        </div>

        <div class="editor__control flex-grow-1">
          <label class="form-label fw-semibold" for="editor-settings-profile-customer-identifiers-label">Customer Identifiers</label>
          <input
            class="form-control"
            id="editor-settings-profile-customer-identifiers-label"
            formControlName="customerIdentifiersLabel"
            [placeholder]="baseLanguagePack()?.customerIdentifiersLabel ?? ''"
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
            [placeholder]="baseLanguagePack()?.customerReferencesLabel ?? ''"
          />
          <p class="form-text">The label for the client’s reference number or identifier for this transaction.</p>
        </div>

        <div class="editor__control flex-grow-1">
          <label class="form-label fw-semibold" for="editor-settings-profile-delivery-information-label">Delivery Information</label>
          <input
            class="form-control"
            id="editor-settings-profile-delivery-information-label"
            formControlName="deliveryInformationLabel"
            [placeholder]="baseLanguagePack()?.deliveryInformationLabel ?? ''"
          />
          <p class="form-text">The label for the section containing shipping or delivery details.</p>
        </div>
      </div>

      <div class="editor__control mb-3">
        <label class="form-label fw-semibold" for="editor-settings-profile-order-label">Order</label>
        <input class="form-control" id="editor-settings-profile-order-label" formControlName="orderLabel" [placeholder]="baseLanguagePack()?.orderLabel ?? ''" />
        <p class="form-text">The label used for the purchase order or sales order associated with the invoice.</p>
      </div>

      <div class="d-flex flex-wrap gap-3 mb-3">
        <div class="editor__control flex-grow-1">
          <label class="form-label fw-semibold" for="editor-settings-profile-invoice-references-label">Invoice References</label>
          <input
            class="form-control"
            id="editor-settings-profile-invoice-references-label"
            formControlName="invoiceReferencesLabel"
            [placeholder]="baseLanguagePack()?.invoiceReferencesLabel ?? ''"
          />
          <p class="form-text">The label for any references specific to the invoice document itself.</p>
        </div>

        <div class="editor__control flex-grow-1">
          <label class="form-label fw-semibold" for="editor-settings-profile-currency-label">Currency</label>
          <input class="form-control" id="editor-settings-profile-currency-label" formControlName="currencyLabel" [placeholder]="baseLanguagePack()?.currencyLabel ?? ''" />
          <p class="form-text">The label used to indicate the currency used on the invoice.</p>
        </div>
      </div>

      <div class="editor__control mb-3">
        <label class="form-label fw-semibold" for="editor-settings-profile-business-process-label">Business Process</label>
        <input
          class="form-control"
          id="editor-settings-profile-business-process-label"
          formControlName="businessProcessLabel"
          [placeholder]="baseLanguagePack()?.businessProcessLabel ?? ''"
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
            [placeholder]="baseLanguagePack()?.totalWithoutVatLabel ?? ''"
          />
          <p class="form-text">The label for the total amount before VAT is applied.</p>
        </div>

        <div class="editor__control flex-grow-1">
          <label class="form-label fw-semibold" for="editor-settings-profile-total-vat-label">Total VAT</label>
          <input class="form-control" id="editor-settings-profile-total-vat-label" formControlName="totalVatLabel" [placeholder]="baseLanguagePack()?.totalVatLabel ?? ''" />
          <p class="form-text">The label for the total VAT amount calculated on the invoice.</p>
        </div>

        <div class="editor__control flex-grow-1">
          <label class="form-label fw-semibold" for="editor-settings-profile-total-gross-label">Total (Gross)</label>
          <input
            class="form-control"
            id="editor-settings-profile-total-gross-label"
            formControlName="totalWithVatLabel"
            [placeholder]="baseLanguagePack()?.totalWithVatLabel ?? ''"
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
          [placeholder]="baseLanguagePack()?.prepaidAmountLabel ?? ''"
        />
        <p class="form-text">The label for any amount already paid in advance.</p>
      </div>

      <div class="d-flex flex-wrap gap-3 mb-3">
        <div class="editor__control flex-grow-1">
          <label class="form-label fw-semibold" for="editor-settings-profile-due-date-label">Due Date</label>
          <input class="form-control" id="editor-settings-profile-due-date-label" formControlName="dueDateLabel" [placeholder]="baseLanguagePack()?.dueDateLabel ?? ''" />
          <p class="form-text">The label for the payment due date.</p>
        </div>

        <div class="editor__control flex-grow-1">
          <label class="form-label fw-semibold" for="editor-settings-profile-due-amount-label">Due Amount</label>
          <input class="form-control" id="editor-settings-profile-due-amount-label" formControlName="dueAmountLabel" [placeholder]="baseLanguagePack()?.dueAmountLabel ?? ''" />
          <p class="form-text">The label for the remaining amount that needs to be paid.</p>
        </div>
      </div>

      <div class="editor__control mb-3">
        <label class="form-label fw-semibold" for="editor-settings-profile-page-label">Page</label>
        <input class="form-control" id="editor-settings-profile-page-label" formControlName="pageLabel" [placeholder]="baseLanguagePack()?.pageLabel ?? ''" />
        <p class="form-text">The label used to indicate page numbers in multi-page invoices.</p>
      </div>

      <div class="d-flex flex-wrap gap-3 mb-3">
        <div class="editor__control">
          <label class="form-label fw-semibold" for="editor-settings-profile-default-invoice-documents-type-name">Default Invoice Documents Name</label>
          <input
            class="form-control"
            id="editor-settings-profile-default-invoice-documents-type-name"
            formControlName="defaultInvoiceDocumentsTypeName"
            [placeholder]="baseLanguagePack()?.defaultInvoiceDocumentsTypeName ?? ''"
          />
          <p class="form-text">
            The label used when the document is an invoice and the actual invoice type cannot be matched to a more specific name. Typically set to "Invoice", it serves as a
            fallback when no precise document type label is available.
          </p>
        </div>

        <div class="editor__control">
          <label class="form-label fw-semibold" for="editor-settings-profile-default-credit-note-documents-type-name">Default Credit Note Documents Name</label>
          <input
            class="form-control"
            id="editor-settings-profile-default-credit-note-documents-type-name"
            formControlName="defaultCreditNoteDocumentsTypeName"
            [placeholder]="baseLanguagePack()?.defaultCreditNoteDocumentsTypeName ?? ''"
          />
          <p class="form-text">
            The label used when the document is a credit note and the actual credit note type cannot be matched to a more specific name. Typically set to "Credit Note", it serves
            as a fallback when no precise document type label is available.
          </p>
        </div>
      </div>
    </div>
  `,
  styles: ``,
})
export class EditorSettingsLanguagePackFormComponent {
  formGroupName = input.required<string>();
  languagePacks = input.required<IStandardPdfGeneratorLanguagePackDto[]>();
  baseLanguagePack = input<IStandardPdfGeneratorLanguagePackDto>();
}
