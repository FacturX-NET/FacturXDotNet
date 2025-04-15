import { Component, computed, DestroyRef, inject, input, linkedSignal, resource, Resource } from '@angular/core';
import { PdfViewerComponent } from './pdf-viewer.component';
import { EditorSavedState, EditorStateService } from '../editor-state.service';
import { GenerateApi } from '../../../core/api/generate.api';
import { firstValueFrom, map } from 'rxjs';
import { toastError } from '../../../core/toasts/toast-error';
import { ToastService } from '../../../core/toasts/toast.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { EditorSettingsService, PdfModel } from '../editor-settings.service';
import { EditorMenuService } from '../editor-menu/editor-menu.service';

@Component({
  selector: 'app-editor-pdf-viewer',
  imports: [PdfViewerComponent],
  template: `
    <div class="h-100 position-relative">
      @if (pdfAlwaysSet(); as pdf) {
        <div class="h-100">
          <app-pdf-viewer [pdf]="pdf" [disablePointerEvents]="disablePointerEvents()" />
        </div>
      } @else {
        @switch (pdfTab()) {
          @case ('imported') {
            <div class="h-100 w-100 d-flex align-items-center justify-content-center">
              <button class="btn btn-shadow d-flex flex-column gap-2 align-items-center justify-content-center border rounded-3 p-5" (click)="importPdfImage()">
                <i class="bi bi-file-pdf text-primary fs-1"></i>
                <div class="lead text-primary">Import PDF</div>
              </button>
            </div>
          }
          @default {
            <div class="h-100 d-flex align-items-center justify-content-center">
              <div class="d-flex gap-1 align-items-end">
                <div class="spinner-border spinner-border-sm text-body-tertiary mb-2" role="status">
                  <span class="visually-hidden">Loading...</span>
                </div>
                <i class="bi bi-filetype-pdf text-body-tertiary fs-1"></i>
              </div>
            </div>
          }
        }
      }
    </div>
  `,
  styles: ``,
})
export class EditorPdfViewerComponent {
  disablePointerEvents = input<boolean>(false);

  private editorStateService = inject(EditorStateService);
  private editorMenuService = inject(EditorMenuService);
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

      if (state.request.pdfTab === 'generated' && state.request.value.cii !== undefined) {
        return await firstValueFrom(
          this.generateApi.generateStandardPdf(state.request.value.cii).pipe(
            map((file) => ({ id: idGenerator(), content: file })),
            toastError(this.toastService, (message) => `Error while generating PDF: ${message}`),
            takeUntilDestroyed(this.destroyRef),
          ),
        );
      }

      return undefined;
    },
  });

  protected pdfAlwaysSet = linkedSignal<{ pdf: { id?: string; content: Blob } | undefined; tab: PdfModel }, { id?: string; content: Blob } | undefined>({
    source: () => ({ pdf: this.pdf.value(), tab: this.pdfTab() }),
    computation: (source, previous) => {
      if (previous?.source.tab === 'imported' && source.pdf === undefined) {
        return previous?.value;
      }

      return source.pdf;
    },
  });

  protected async importPdfImage() {
    try {
      await this.editorMenuService.importPdfImageData();
    } catch (error) {
      this.toastService.showError(error, (message) => `Could not create PDF image: ${message}`);
    }
  }
}

function idGenerator() {
  return Math.random().toString(36).substring(2);
}
