import { Component, computed, DestroyRef, inject, input, linkedSignal, Signal, TemplateRef, WritableSignal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { rxResource, takeUntilDestroyed, toSignal } from '@angular/core/rxjs-interop';
import { GenerateApi } from '../../../../../../../core/api/generate.api';
import { delay, distinctUntilChanged, map, startWith } from 'rxjs';
import { IStandardPdfGeneratorLanguagePackDto } from '../../../../../../../core/api/api.models';
import { EditorPdfGenerationProfileData } from '../../../../../editor-pdf-generation-profiles.service';
import { EditorSettingsLanguagePackDocumentTypesFormComponent } from './editor-settings-language-pack-document-types-form.component';
import { EditorSettingsLanguagePackFormComponent } from './editor-settings-language-pack-form.component';
import { EditorSettingsGeneralFormComponent } from './editor-settings-general-form.component';
import { NgTemplateOutlet } from '@angular/common';

@Component({
  selector: 'app-editor-settings-pdf-profile-form',
  imports: [
    ReactiveFormsModule,
    EditorSettingsLanguagePackDocumentTypesFormComponent,
    EditorSettingsLanguagePackFormComponent,
    EditorSettingsGeneralFormComponent,
    NgTemplateOutlet,
  ],
  template: `
    <form [formGroup]="formGroup">
      <app-editor-settings-general-form [formGroup]="formGroup" />

      @if (confirmationTpl(); as confirmationTpl) {
        <ng-container [ngTemplateOutlet]="confirmationTpl"></ng-container>
      }

      <div class="pt-3">
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
            <app-editor-settings-language-pack-form formGroupName="languagePack" [languagePacks]="languagePacks.value() ?? []" [baseLanguagePack]="baseLanguagePack" />

            @if (confirmationTpl(); as confirmationTpl) {
              <ng-container [ngTemplateOutlet]="confirmationTpl"></ng-container>
            }

            <div class="pt-3">
              <h6>Document Types</h6>
              <div class="border-top mt-1 mb-3"></div>
              <p class="form-text">
                The fields above let you set the labels for common document types like invoices and credit notes. However, there are many more specific variations of these
                documents.
              </p>

              <div>
                @if (!overrideLanguagePack()) {
                  <button class="btn btn-sm btn-light border" (click)="overrideDocumentTypeNames(true)">Override document type names</button>
                } @else {
                  <button class="btn btn-sm btn-light border mb-3" (click)="overrideDocumentTypeNames(false)">
                    <i class="bi bi-x-lg"></i> Use default invoice and credit note names
                  </button>
                  <app-editor-settings-language-pack-document-types-form formGroupName="documentTypeNames" [baseLanguagePack]="baseLanguagePack" />
                }
              </div>

              @if (confirmationTpl(); as confirmationTpl) {
                <ng-container [ngTemplateOutlet]="confirmationTpl"></ng-container>
              }
            </div>
          }
        }
      </div>
    </form>
  `,
  styles: ``,
})
export class EditorSettingsPdfProfileFormComponent {
  confirmationTpl = input<TemplateRef<unknown> | undefined>(undefined);

  private generateApi = inject(GenerateApi);
  private destroyRef = inject(DestroyRef);

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

  hasChanges = toSignal(this.formGroup.events.pipe(map(() => this.formGroup.dirty)), { initialValue: false });

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
    this.formGroup.markAsPristine();
  }

  protected async overrideDocumentTypeNames(value: boolean) {
    this.formGroup.controls.languagePack.controls.overrideDocumentTypeNames.setValue(value);
  }
}
