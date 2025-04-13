import { Component, computed, DestroyRef, inject, input, linkedSignal, resource, Resource } from '@angular/core';
import { PdfViewerComponent } from './pdf-viewer.component';
import { EditorSavedState, EditorStateService } from '../editor-state.service';
import { GenerateApi } from '../../../core/api/generate.api';
import { firstValueFrom, map } from 'rxjs';
import { toastError } from '../../../core/toasts/toast-error';
import { ToastService } from '../../../core/toasts/toast.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { EditorSettingsService } from '../editor-settings.service';

@Component({
  selector: 'app-editor-pdf-viewer',
  imports: [PdfViewerComponent],
  template: `
    <div class="h-100 position-relative">
      <div class="h-100" [class.visually-hidden]="pdf.value() === undefined">
        <app-pdf-viewer [pdf]="pdfAlwaysSet()" [disablePointerEvents]="disablePointerEvents()" />
      </div>
      <div class="h-100 d-flex align-items-center justify-content-center" [class.d-none]="pdf.value() !== undefined">
        <div class="d-flex gap-1 align-items-end">
          <div class="spinner-border spinner-border-sm text-body-tertiary mb-2" role="status">
            <span class="visually-hidden">Loading...</span>
          </div>
          <i class="bi bi-filetype-pdf text-body-tertiary fs-1"></i>
        </div>
      </div>
    </div>
  `,
  styles: ``,
})
export class EditorPdfViewerComponent {
  disablePointerEvents = input<boolean>(false);

  private editorStateService = inject(EditorStateService);
  private editorSettingsService = inject(EditorSettingsService);
  private generateApi = inject(GenerateApi);
  private toastService = inject(ToastService);
  private destroyRef = inject(DestroyRef);

  protected settings = this.editorSettingsService.settings;
  protected pdfTab = computed(() => this.settings().pdfTab);
  protected state: Resource<EditorSavedState | null> = this.editorStateService.savedState;
  protected pdf = resource({
    request: () => ({ value: this.state.value(), pdfTab: this.pdfTab() }),
    loader: async (state): Promise<{ id?: string; content: Blob } | undefined> => {
      if (state.request.value === null || state.request.value === undefined) {
        return undefined;
      }

      if (state.request.pdfTab === 'imported' && state.request.value.pdf !== undefined) {
        return state.request.value.pdf;
      }

      return await firstValueFrom(
        this.generateApi.generateStandardPdf(state.request.value.cii).pipe(
          map((file) => ({ id: idGenerator(), content: file })),
          toastError(this.toastService, (message) => `Error while generating PDF: ${message}`),
          takeUntilDestroyed(this.destroyRef),
        ),
      );
    },
  });
  protected pdfAlwaysSet = linkedSignal<{ id?: string; content: Blob } | undefined, { id?: string; content: Blob }>({
    source: () => this.pdf.value(),
    computation: (source, previous) => {
      if (source === undefined) {
        return previous?.value ?? { content: new Blob([]) };
      }

      return source;
    },
  });
}

function idGenerator() {
  return Math.random().toString(36).substring(2);
}
