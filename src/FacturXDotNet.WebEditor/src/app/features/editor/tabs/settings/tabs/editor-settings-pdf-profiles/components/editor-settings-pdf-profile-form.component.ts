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
              <p class="form-text">The culture associated with the language pack, which defines language and regional settings for the generator.</p>
            </div>

            <div class="editor__control mb-3">
              <label class="form-label fw-semibold" for="editor-settings-profile-vat-number-label">Vat number label</label>
              <input class="form-control" id="editor-settings-profile-vat-number-label" formControlName="vatNumberLabel" [placeholder]="baseLanguagePack?.vatNumberLabel ?? ''" />
              <p class="form-text">The label used to denote the VAT (Value Added Tax) number in the generated PDF.</p>
            </div>

            <div class="d-flex flex-wrap gap-4 mb-3">
              <div class="editor__control">
                <label class="form-label fw-semibold" for="editor-settings-profile-supplier-references-label">Supplier references label</label>
                <input
                  class="form-control"
                  id="editor-settings-profile-supplier-references-label"
                  formControlName="supplierReferencesLabel"
                  [placeholder]="baseLanguagePack?.supplierReferencesLabel ?? ''"
                />
                <p class="form-text">
                  The label representing "Our References" in the localized language pack, typically used to identify the document references associated with the sender.
                </p>
              </div>

              <div class="editor__control">
                <label class="form-label fw-semibold" for="editor-settings-profile-customer-references-label">Customer references label</label>
                <input
                  class="form-control"
                  id="editor-settings-profile-customer-references-label"
                  formControlName="customerReferencesLabel"
                  [placeholder]="baseLanguagePack?.customerReferencesLabel ?? ''"
                />
                <p class="form-text">The label used for the "Your references" field in the localized resources of the generator.</p>
              </div>
            </div>

            <div class="editor__control mb-3">
              <label class="form-label fw-semibold" for="editor-settings-profile-orderLabel">Order label</label>
              <input class="form-control" id="editor-settings-profile-orderLabel" formControlName="orderLabel" [placeholder]="baseLanguagePack?.orderLabel ?? ''" />
              <p class="form-text">The label used for the order reference in the generator.</p>
            </div>

            <div class="editor__control mb-3">
              <label class="form-label fw-semibold" for="editor-settings-profile-invoiceReferencesLabel">InvoiceReferencesLabel</label>
              <input
                class="form-control"
                id="editor-settings-profile-invoiceReferencesLabel"
                formControlName="invoiceReferencesLabel"
                [placeholder]="baseLanguagePack?.invoiceReferencesLabel ?? ''"
              />
              <p class="form-text">The label representing invoice references, used to identify and localize these references in the generated PDF.</p>
            </div>

            <div class="editor__control mb-3">
              <label class="form-label fw-semibold" for="editor-settings-profile-businessProcessLabel">BusinessProcessLabel</label>
              <input
                class="form-control"
                id="editor-settings-profile-businessProcessLabel"
                formControlName="businessProcessLabel"
                [placeholder]="baseLanguagePack?.businessProcessLabel ?? ''"
              />
              <p class="form-text">The label used to represent a business process in the generated PDF.</p>
            </div>

            <div class="editor__control mb-3">
              <label class="form-label fw-semibold" for="editor-settings-profile-default-document-type-name">DefaultDocumentTypeName</label>
              <input
                class="form-control"
                id="editor-settings-profile-default-document-type-name"
                formControlName="defaultDocumentTypeName"
                [placeholder]="baseLanguagePack?.defaultDocumentTypeName ?? ''"
              />
              <p class="form-text">The name of the document type used in the generator.</p>
            </div>

            <div class="editor__control mb-3">
              <label class="form-label fw-semibold" for="editor-settings-profile-dateLabel">DateLabel</label>
              <input class="form-control" id="editor-settings-profile-dateLabel" formControlName="dateLabel" [placeholder]="baseLanguagePack?.dateLabel ?? ''" />
              <p class="form-text">The label representing the date field in the language pack used by the generator.</p>
            </div>

            <div class="editor__control mb-3">
              <label class="form-label fw-semibold" for="editor-settings-profile-customer-address-label">ClientAddressLabel</label>
              <input
                class="form-control"
                id="editor-settings-profile-customer-address-label"
                formControlName="customerAddressLabel"
                [placeholder]="baseLanguagePack?.customerAddressLabel ?? ''"
              />
              <p class="form-text">The label used to identify the client's address in the generated PDF.</p>
            </div>

            <div class="editor__control mb-3">
              <label class="form-label fw-semibold" for="editor-settings-profile-customer-identifiers-label">YourIdentifiersLabel</label>
              <input
                class="form-control"
                id="editor-settings-profile-customer-identifiers-label"
                formControlName="customerIdentifiersLabel"
                [placeholder]="baseLanguagePack?.customerIdentifiersLabel ?? ''"
              />
              <p class="form-text">The label associated with the recipient's identifiers in the context of generating a PDF.</p>
            </div>

            <div class="editor__control mb-3">
              <label class="form-label fw-semibold" for="editor-settings-profile-deliveryInformationLabel">DeliveryInformationLabel</label>
              <input
                class="form-control"
                id="editor-settings-profile-deliveryInformationLabel"
                formControlName="deliveryInformationLabel"
                [placeholder]="baseLanguagePack?.deliveryInformationLabel ?? ''"
              />
              <p class="form-text">The label for delivery information, used to display or identify delivery-related details in the PDF document.</p>
            </div>

            <div class="editor__control mb-3">
              <label class="form-label fw-semibold" for="editor-settings-profile-currencyLabel">CurrencyLabel</label>
              <input class="form-control" id="editor-settings-profile-currencyLabel" formControlName="currencyLabel" [placeholder]="baseLanguagePack?.currencyLabel ?? ''" />
              <p class="form-text">The label used to represent or display the currency in the generator's output.</p>
            </div>

            <div class="editor__control mb-3">
              <label class="form-label fw-semibold" for="editor-settings-profile-totalWithoutVatLabel">TotalWithoutVatLabel</label>
              <input
                class="form-control"
                id="editor-settings-profile-totalWithoutVatLabel"
                formControlName="totalWithoutVatLabel"
                [placeholder]="baseLanguagePack?.totalWithoutVatLabel ?? ''"
              />
              <p class="form-text">The label representing the total amount excluding VAT, used in the generator.</p>
            </div>

            <div class="editor__control mb-3">
              <label class="form-label fw-semibold" for="editor-settings-profile-totalVatLabel">TotalVatLabel</label>
              <input class="form-control" id="editor-settings-profile-totalVatLabel" formControlName="totalVatLabel" [placeholder]="baseLanguagePack?.totalVatLabel ?? ''" />
              <p class="form-text">The label used to represent the total VAT amount in the generator.</p>
            </div>

            <div class="editor__control mb-3">
              <label class="form-label fw-semibold" for="editor-settings-profile-totalWithVatLabel">TotalWithVatLabel</label>
              <input
                class="form-control"
                id="editor-settings-profile-totalWithVatLabel"
                formControlName="totalWithVatLabel"
                [placeholder]="baseLanguagePack?.totalWithVatLabel ?? ''"
              />
              <p class="form-text">The label representing the total amount including VAT in the generator.</p>
            </div>

            <div class="editor__control mb-3">
              <label class="form-label fw-semibold" for="editor-settings-profile-prepaidAmountLabel">PrepaidAmountLabel</label>
              <input
                class="form-control"
                id="editor-settings-profile-prepaidAmountLabel"
                formControlName="prepaidAmountLabel"
                [placeholder]="baseLanguagePack?.prepaidAmountLabel ?? ''"
              />
              <p class="form-text">The label representing the prepaid amount in the generated PDF, used for localization purposes in the generator.</p>
            </div>

            <div class="editor__control mb-3">
              <label class="form-label fw-semibold" for="editor-settings-profile-dueDateLabel">DueDateLabel</label>
              <input class="form-control" id="editor-settings-profile-dueDateLabel" formControlName="dueDateLabel" [placeholder]="baseLanguagePack?.dueDateLabel ?? ''" />
              <p class="form-text">The label representing the due date text in the generated PDF document.</p>
            </div>

            <div class="editor__control mb-3">
              <label class="form-label fw-semibold" for="editor-settings-profile-dueAmountLabel">DueAmountLabel</label>
              <input class="form-control" id="editor-settings-profile-dueAmountLabel" formControlName="dueAmountLabel" [placeholder]="baseLanguagePack?.dueAmountLabel ?? ''" />
              <p class="form-text">The label representing the due amount in the PDF document.</p>
            </div>

            <div class="editor__control mb-3">
              <label class="form-label fw-semibold" for="editor-settings-profile-default-legal-id-type">DefaultLegalIdType</label>
              <input
                class="form-control"
                id="editor-settings-profile-default-legal-id-type"
                formControlName="defaultLegalIdType"
                [placeholder]="baseLanguagePack?.defaultLegalIdType ?? ''"
              />
              <p class="form-text">The default label or identifier used to represent the legal ID type in the language pack for the generator.</p>
            </div>

            <div class="editor__control mb-3">
              <label class="form-label fw-semibold" for="editor-settings-profile-pageLabel">PageLabel</label>
              <input class="form-control" id="editor-settings-profile-pageLabel" formControlName="pageLabel" [placeholder]="baseLanguagePack?.pageLabel ?? ''" />
              <p class="form-text">The label used to denote or identify a page within the generator.</p>
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
      defaultDocumentTypeName: new FormControl<string | null>(null),
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
        defaultDocumentTypeName: this.formGroup.controls.languagePack.controls.defaultDocumentTypeName.value ?? undefined,
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
        defaultDocumentTypeName: profile.languagePack?.defaultDocumentTypeName ?? null,
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
