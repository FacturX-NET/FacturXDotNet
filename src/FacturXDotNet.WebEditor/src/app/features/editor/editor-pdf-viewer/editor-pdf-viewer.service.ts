import { computed, DestroyRef, inject, Injectable, resource, Resource } from '@angular/core';
import { EditorSavedState, EditorStateService } from '../editor-state.service';
import { firstValueFrom, map } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { EditorSettingsService } from '../editor-settings.service';
import { GenerateApi } from '../../../core/api/generate.api';

@Injectable({
  providedIn: 'root',
})
export class EditorPdfViewerService {
  private editorStateService = inject(EditorStateService);
  private editorSettingsService = inject(EditorSettingsService);
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
        return await firstValueFrom(
          this.generateApi.generateStandardPdf(state.request.value.cii).pipe(
            map((file) => ({ id: idGenerator(), content: file })),
            takeUntilDestroyed(this.destroyRef),
          ),
        );
      }

      return undefined;
    },
  });

  regenerateAndDisplayStandardPdf() {
    if (this.pdfTab() === 'generated') {
      this.pdf.reload();
    } else {
      this.editorSettingsService.savePdfTab('generated');
    }
  }
}

function idGenerator() {
  return Math.random().toString(36).substring(2);
}
