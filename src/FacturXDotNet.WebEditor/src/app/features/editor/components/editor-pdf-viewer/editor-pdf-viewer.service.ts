import { computed, DestroyRef, inject, Injectable, resource, Resource } from '@angular/core';
import { EditorSavedState, EditorStateService } from '../../editor-state.service';
import { firstValueFrom, map } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { EditorSettingsService } from '../../editor-settings.service';
import { GenerateApi, GenerateStandardPdfOptions } from '../../../../core/api/generate.api';
import { EditorPdfGenerationProfileData, EditorPdfGenerationProfilesService } from '../../editor-pdf-generation-profiles.service';
import { IStandardPdfGeneratorLanguagePackDto, StandardPdfGeneratorLanguagePackDto } from '../../../../core/api/api.models';

@Injectable({
  providedIn: 'root',
})
export class EditorPdfViewerService {
  private editorStateService = inject(EditorStateService);
  private editorSettingsService = inject(EditorSettingsService);
  private editorPdfGenerationProfilesService = inject(EditorPdfGenerationProfilesService);
  private generateApi = inject(GenerateApi);
  private destroyRef = inject(DestroyRef);

  protected settings = this.editorSettingsService.settings;
  protected pdfTab = computed(() => this.settings().pdfTab);
  protected state: Resource<EditorSavedState | null> = this.editorStateService.savedState;

  get pdf() {
    return this.pdfInternal.asReadonly();
  }

  private pdfInternal = resource({
    request: () => ({ value: this.state.value(), pdfTab: this.pdfTab() }),
    loader: async (state): Promise<{ id?: string; content: Blob } | undefined> => {
      if (state.request.value === null || state.request.value === undefined) {
        return undefined;
      }

      if (state.request.pdfTab === 'imported' && state.request.value.pdf !== undefined) {
        return state.request.value.pdf;
      }

      if (state.request.pdfTab === 'generated' && state.request.value.cii !== undefined) {
        const profile = this.profileOverride ?? this.editorPdfGenerationProfilesService.selectedProfile();
        const logo = profile?.logoBase64 === undefined || profile?.logoBase64 === null ? undefined : profile.logoBase64.replace(/^data:image\/?[A-Za-z]*;base64,/, '');

        let languagePack: IStandardPdfGeneratorLanguagePackDto | undefined;
        if (profile?.languagePack !== undefined) {
          let basePack: IStandardPdfGeneratorLanguagePackDto | undefined;
          if (profile.languagePack.baseLanguagePack !== undefined) {
            basePack = await firstValueFrom(this.generateApi.getStandardPdfLanguagePack(profile?.languagePack?.baseLanguagePack).pipe(takeUntilDestroyed(this.destroyRef)));
          }

          languagePack = {
            culture: profile.languagePack['Culture'] ?? basePack?.culture,
            vatNumberLabel: profile.languagePack['VatNumberLabel'] ?? basePack?.vatNumberLabel,
            supplierReferencesLabel: profile.languagePack['SupplierReferencesLabel'] ?? basePack?.supplierReferencesLabel,
            customerReferencesLabel: profile.languagePack['CustomerReferencesLabel'] ?? basePack?.customerReferencesLabel,
            orderLabel: profile.languagePack['OrderLabel'] ?? basePack?.orderLabel,
            invoiceReferencesLabel: profile.languagePack['InvoiceReferencesLabel'] ?? basePack?.invoiceReferencesLabel,
            businessProcessLabel: profile.languagePack['BusinessProcessLabel'] ?? basePack?.businessProcessLabel,
            dateLabel: profile.languagePack['DateLabel'] ?? basePack?.dateLabel,
            customerAddressLabel: profile.languagePack['CustomerAddressLabel'] ?? basePack?.customerAddressLabel,
            customerIdentifiersLabel: profile.languagePack['CustomerIdentifiersLabel'] ?? basePack?.customerIdentifiersLabel,
            deliveryInformationLabel: profile.languagePack['DeliveryInformationLabel'] ?? basePack?.deliveryInformationLabel,
            currencyLabel: profile.languagePack['CurrencyLabel'] ?? basePack?.currencyLabel,
            totalWithoutVatLabel: profile.languagePack['TotalWithoutVatLabel'] ?? basePack?.totalWithoutVatLabel,
            totalVatLabel: profile.languagePack['TotalVatLabel'] ?? basePack?.totalVatLabel,
            totalWithVatLabel: profile.languagePack['TotalWithVatLabel'] ?? basePack?.totalWithVatLabel,
            prepaidAmountLabel: profile.languagePack['PrepaidAmountLabel'] ?? basePack?.prepaidAmountLabel,
            dueDateLabel: profile.languagePack['DueDateLabel'] ?? basePack?.dueDateLabel,
            dueAmountLabel: profile.languagePack['DueAmountLabel'] ?? basePack?.dueAmountLabel,
            defaultLegalIdType: profile.languagePack['DefaultLegalIdType'] ?? basePack?.defaultLegalIdType,
            pageLabel: profile.languagePack['PageLabel'] ?? basePack?.pageLabel,
            defaultInvoiceDocumentsTypeName: profile.languagePack['DefaultInvoiceDocumentsTypeName'] ?? basePack?.defaultInvoiceDocumentsTypeName,
            defaultCreditNoteDocumentsTypeName: profile.languagePack['DefaultCreditNoteDocumentsTypeName'] ?? basePack?.defaultCreditNoteDocumentsTypeName,
            documentTypeNames:
              profile.languagePack?.documentTypeNames === undefined
                ? undefined
                : {
                    RequestForPayment: profile.languagePack.documentTypeNames['RequestForPayment'] ?? basePack?.documentTypeNames?.['RequestForPayment'] ?? '',
                    DebitNoteRelatedToGoodsOrServices:
                      profile.languagePack.documentTypeNames['DebitNoteRelatedToGoodsOrServices'] ?? basePack?.documentTypeNames?.['DebitNoteRelatedToGoodsOrServices'] ?? '',
                    CreditNoteRelatedToGoodsOrServices:
                      profile.languagePack.documentTypeNames['CreditNoteRelatedToGoodsOrServices'] ?? basePack?.documentTypeNames?.['CreditNoteRelatedToGoodsOrServices'] ?? '',
                    MeteredServicesInvoice: profile.languagePack.documentTypeNames['MeteredServicesInvoice'] ?? basePack?.documentTypeNames?.['MeteredServicesInvoice'] ?? '',
                    CreditNoteRelatedToFinancialAdjustments:
                      profile.languagePack.documentTypeNames['CreditNoteRelatedToFinancialAdjustments'] ??
                      basePack?.documentTypeNames?.['CreditNoteRelatedToFinancialAdjustments'] ??
                      '',
                    DebitNoteRelatedToFinancialAdjustments:
                      profile.languagePack.documentTypeNames['DebitNoteRelatedToFinancialAdjustments'] ??
                      basePack?.documentTypeNames?.['DebitNoteRelatedToFinancialAdjustments'] ??
                      '',
                    TaxNotification: profile.languagePack.documentTypeNames['TaxNotification'] ?? basePack?.documentTypeNames?.['TaxNotification'] ?? '',
                    InvoicingDataSheet: profile.languagePack.documentTypeNames['InvoicingDataSheet'] ?? basePack?.documentTypeNames?.['InvoicingDataSheet'] ?? '',
                    DirectPaymentValuation: profile.languagePack.documentTypeNames['DirectPaymentValuation'] ?? basePack?.documentTypeNames?.['DirectPaymentValuation'] ?? '',
                    ProvisionalPaymentValuation:
                      profile.languagePack.documentTypeNames['ProvisionalPaymentValuation'] ?? basePack?.documentTypeNames?.['ProvisionalPaymentValuation'] ?? '',
                    PaymentValuation: profile.languagePack.documentTypeNames['PaymentValuation'] ?? basePack?.documentTypeNames?.['PaymentValuation'] ?? '',
                    InterimApplicationForPayment:
                      profile.languagePack.documentTypeNames['InterimApplicationForPayment'] ?? basePack?.documentTypeNames?.['InterimApplicationForPayment'] ?? '',
                    FinalPaymentRequestBasedOnCompletionOfWork:
                      profile.languagePack.documentTypeNames['FinalPaymentRequestBasedOnCompletionOfWork'] ??
                      basePack?.documentTypeNames?.['FinalPaymentRequestBasedOnCompletionOfWork'] ??
                      '',
                    PaymentRequestForCompletedUnits:
                      profile.languagePack.documentTypeNames['PaymentRequestForCompletedUnits'] ?? basePack?.documentTypeNames?.['PaymentRequestForCompletedUnits'] ?? '',
                    SelfBilledCreditNote: profile.languagePack.documentTypeNames['SelfBilledCreditNote'] ?? basePack?.documentTypeNames?.['SelfBilledCreditNote'] ?? '',
                    ConsolidatedCreditNoteGoodsAndServices:
                      profile.languagePack.documentTypeNames['ConsolidatedCreditNoteGoodsAndServices'] ??
                      basePack?.documentTypeNames?.['ConsolidatedCreditNoteGoodsAndServices'] ??
                      '',
                    PriceVariationInvoice: profile.languagePack.documentTypeNames['PriceVariationInvoice'] ?? basePack?.documentTypeNames?.['PriceVariationInvoice'] ?? '',
                    CreditNoteForPriceVariation:
                      profile.languagePack.documentTypeNames['CreditNoteForPriceVariation'] ?? basePack?.documentTypeNames?.['CreditNoteForPriceVariation'] ?? '',
                    DelcredereCreditNote: profile.languagePack.documentTypeNames['DelcredereCreditNote'] ?? basePack?.documentTypeNames?.['DelcredereCreditNote'] ?? '',
                    ProformaInvoice: profile.languagePack.documentTypeNames['ProformaInvoice'] ?? basePack?.documentTypeNames?.['ProformaInvoice'] ?? '',
                    PartialInvoice: profile.languagePack.documentTypeNames['PartialInvoice'] ?? basePack?.documentTypeNames?.['PartialInvoice'] ?? '',
                    CommercialInvoiceWhichIncludesPackingList:
                      profile.languagePack.documentTypeNames['CommercialInvoiceWhichIncludesPackingList'] ??
                      basePack?.documentTypeNames?.['CommercialInvoiceWhichIncludesPackingList'] ??
                      '',
                    CommercialInvoice: profile.languagePack.documentTypeNames['CommercialInvoice'] ?? basePack?.documentTypeNames?.['CommercialInvoice'] ?? '',
                    CreditNote: profile.languagePack.documentTypeNames['CreditNote'] ?? basePack?.documentTypeNames?.['CreditNote'] ?? '',
                    CommissionNote: profile.languagePack.documentTypeNames['CommissionNote'] ?? basePack?.documentTypeNames?.['CommissionNote'] ?? '',
                    DebitNote: profile.languagePack.documentTypeNames['DebitNote'] ?? basePack?.documentTypeNames?.['DebitNote'] ?? '',
                    CorrectedInvoice: profile.languagePack.documentTypeNames['CorrectedInvoice'] ?? basePack?.documentTypeNames?.['CorrectedInvoice'] ?? '',
                    ConsolidatedInvoice: profile.languagePack.documentTypeNames['ConsolidatedInvoice'] ?? basePack?.documentTypeNames?.['ConsolidatedInvoice'] ?? '',
                    PrepaymentInvoice: profile.languagePack.documentTypeNames['PrepaymentInvoice'] ?? basePack?.documentTypeNames?.['PrepaymentInvoice'] ?? '',
                    HireInvoice: profile.languagePack.documentTypeNames['HireInvoice'] ?? basePack?.documentTypeNames?.['HireInvoice'] ?? '',
                    TaxInvoice: profile.languagePack.documentTypeNames['TaxInvoice'] ?? basePack?.documentTypeNames?.['TaxInvoice'] ?? '',
                    SelfBilledInvoice: profile.languagePack.documentTypeNames['SelfBilledInvoice'] ?? basePack?.documentTypeNames?.['SelfBilledInvoice'] ?? '',
                    DelcredereInvoice: profile.languagePack.documentTypeNames['DelcredereInvoice'] ?? basePack?.documentTypeNames?.['DelcredereInvoice'] ?? '',
                    FactoredInvoice: profile.languagePack.documentTypeNames['FactoredInvoice'] ?? basePack?.documentTypeNames?.['FactoredInvoice'] ?? '',
                    LeaseInvoice: profile.languagePack.documentTypeNames['LeaseInvoice'] ?? basePack?.documentTypeNames?.['LeaseInvoice'] ?? '',
                    ConsignmentInvoice: profile.languagePack.documentTypeNames['ConsignmentInvoice'] ?? basePack?.documentTypeNames?.['ConsignmentInvoice'] ?? '',
                    FactoredCreditNote: profile.languagePack.documentTypeNames['FactoredCreditNote'] ?? basePack?.documentTypeNames?.['FactoredCreditNote'] ?? '',
                    OcrPaymentCreditNote: profile.languagePack.documentTypeNames['OcrPaymentCreditNote'] ?? basePack?.documentTypeNames?.['OcrPaymentCreditNote'] ?? '',
                    DebitAdvice: profile.languagePack.documentTypeNames['DebitAdvice'] ?? basePack?.documentTypeNames?.['DebitAdvice'] ?? '',
                    ReversalOfDebit: profile.languagePack.documentTypeNames['ReversalOfDebit'] ?? basePack?.documentTypeNames?.['ReversalOfDebit'] ?? '',
                    ReversalOfCredit: profile.languagePack.documentTypeNames['ReversalOfCredit'] ?? basePack?.documentTypeNames?.['ReversalOfCredit'] ?? '',
                    SelfBilledDebitNote: profile.languagePack.documentTypeNames['SelfBilledDebitNote'] ?? basePack?.documentTypeNames?.['SelfBilledDebitNote'] ?? '',
                    ForwardersCreditNote: profile.languagePack.documentTypeNames['ForwardersCreditNote'] ?? basePack?.documentTypeNames?.['ForwardersCreditNote'] ?? '',
                    ForwardersInvoiceDiscrepancyReport:
                      profile.languagePack.documentTypeNames['ForwardersInvoiceDiscrepancyReport'] ?? basePack?.documentTypeNames?.['ForwardersInvoiceDiscrepancyReport'] ?? '',
                    InsurersInvoice: profile.languagePack.documentTypeNames['InsurersInvoice'] ?? basePack?.documentTypeNames?.['InsurersInvoice'] ?? '',
                    ForwardersInvoice: profile.languagePack.documentTypeNames['ForwardersInvoice'] ?? basePack?.documentTypeNames?.['ForwardersInvoice'] ?? '',
                    PortChargesDocuments: profile.languagePack.documentTypeNames['PortChargesDocuments'] ?? basePack?.documentTypeNames?.['PortChargesDocuments'] ?? '',
                    InvoiceInformationForAccountingPurposes:
                      profile.languagePack.documentTypeNames['InvoiceInformationForAccountingPurposes'] ??
                      basePack?.documentTypeNames?.['InvoiceInformationForAccountingPurposes'] ??
                      '',
                    FreightInvoice: profile.languagePack.documentTypeNames['FreightInvoice'] ?? basePack?.documentTypeNames?.['FreightInvoice'] ?? '',
                    ClaimNotification: profile.languagePack.documentTypeNames['ClaimNotification'] ?? basePack?.documentTypeNames?.['ClaimNotification'] ?? '',
                    ConsularInvoice: profile.languagePack.documentTypeNames['ConsularInvoice'] ?? basePack?.documentTypeNames?.['ConsularInvoice'] ?? '',
                    PartialConstructionInvoice:
                      profile.languagePack.documentTypeNames['PartialConstructionInvoice'] ?? basePack?.documentTypeNames?.['PartialConstructionInvoice'] ?? '',
                    PartialFinalConstructionInvoice:
                      profile.languagePack.documentTypeNames['PartialFinalConstructionInvoice'] ?? basePack?.documentTypeNames?.['PartialFinalConstructionInvoice'] ?? '',
                    FinalConstructionInvoice: profile.languagePack.documentTypeNames['FinalConstructionInvoice'] ?? basePack?.documentTypeNames?.['FinalConstructionInvoice'] ?? '',
                    CustomsInvoice: profile.languagePack.documentTypeNames['CustomsInvoice'] ?? basePack?.documentTypeNames?.['CustomsInvoice'] ?? '',
                  },
          };
        }

        const options: GenerateStandardPdfOptions | undefined =
          profile === undefined
            ? undefined
            : { logo, footer: profile.footer, languagePack: languagePack === undefined ? undefined : new StandardPdfGeneratorLanguagePackDto(languagePack) };

        return await firstValueFrom(
          this.generateApi.generateStandardPdf(state.request.value.cii, options).pipe(
            map((file) => ({ id: idGenerator(), content: file })),
            takeUntilDestroyed(this.destroyRef),
          ),
        );
      }

      return undefined;
    },
  });

  private profileOverride: EditorPdfGenerationProfileData | undefined;

  regenerateAndDisplayGeneratedPdf(profile?: EditorPdfGenerationProfileData) {
    if (this.pdfTab() === 'generated') {
      this.profileOverride = profile;
      this.pdf.reload();
    } else {
      this.editorSettingsService.savePdfTab('generated');
    }
  }
}

function idGenerator() {
  return Math.random().toString(36).substring(2);
}
