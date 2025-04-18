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
        const logo = profile?.logoBase64 === undefined ? undefined : profile.logoBase64.replace(/^data:image\/?[A-Za-z]*;base64,/, '');

        let languagePack: IStandardPdfGeneratorLanguagePackDto | undefined;
        if (profile?.languagePack !== undefined && profile.languagePack.baseLanguagePack !== undefined) {
          const basePack = await firstValueFrom(this.generateApi.getStandardPdfLanguagePack(profile?.languagePack?.baseLanguagePack).pipe(takeUntilDestroyed(this.destroyRef)));
          languagePack = {
            culture: profile.languagePack.culture ?? basePack.culture,
            vatNumberLabel: profile.languagePack.vatNumberLabel ?? basePack.vatNumberLabel,
            supplierReferencesLabel: profile.languagePack.supplierReferencesLabel ?? basePack.supplierReferencesLabel,
            customerReferencesLabel: profile.languagePack.customerReferencesLabel ?? basePack.customerReferencesLabel,
            orderLabel: profile.languagePack.orderLabel ?? basePack.orderLabel,
            invoiceReferencesLabel: profile.languagePack.invoiceReferencesLabel ?? basePack.invoiceReferencesLabel,
            businessProcessLabel: profile.languagePack.businessProcessLabel ?? basePack.businessProcessLabel,
            defaultDocumentTypeName: profile.languagePack.defaultDocumentTypeName ?? basePack.defaultDocumentTypeName,
            dateLabel: profile.languagePack.dateLabel ?? basePack.dateLabel,
            customerAddressLabel: profile.languagePack.customerAddressLabel ?? basePack.customerAddressLabel,
            customerIdentifiersLabel: profile.languagePack.customerIdentifiersLabel ?? basePack.customerIdentifiersLabel,
            deliveryInformationLabel: profile.languagePack.deliveryInformationLabel ?? basePack.deliveryInformationLabel,
            currencyLabel: profile.languagePack.currencyLabel ?? basePack.currencyLabel,
            totalWithoutVatLabel: profile.languagePack.totalWithoutVatLabel ?? basePack.totalWithoutVatLabel,
            totalVatLabel: profile.languagePack.totalVatLabel ?? basePack.totalVatLabel,
            totalWithVatLabel: profile.languagePack.totalWithVatLabel ?? basePack.totalWithVatLabel,
            prepaidAmountLabel: profile.languagePack.prepaidAmountLabel ?? basePack.prepaidAmountLabel,
            dueDateLabel: profile.languagePack.dueDateLabel ?? basePack.dueDateLabel,
            dueAmountLabel: profile.languagePack.dueAmountLabel ?? basePack.dueAmountLabel,
            defaultLegalIdType: profile.languagePack.defaultLegalIdType ?? basePack.defaultLegalIdType,
            pageLabel: profile.languagePack.pageLabel ?? basePack.pageLabel,
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
