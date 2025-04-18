import { computed, DestroyRef, inject, Injectable, resource, Resource } from '@angular/core';
import { EditorSavedState, EditorStateService } from '../editor-state.service';
import { firstValueFrom, map } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { EditorSettingsService } from '../editor-settings.service';
import { GenerateApi, GenerateStandardPdfOptions } from '../../../core/api/generate.api';
import { EditorPdfGenerationProfileData, EditorPdfGenerationProfilesService } from '../editor-pdf-generation-profiles.service';

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
  pdf = resource({
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

        const options: GenerateStandardPdfOptions | undefined = profile === undefined ? undefined : { logo };

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

  regenerateAndDisplayStandardPdf(profile?: EditorPdfGenerationProfileData) {
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
