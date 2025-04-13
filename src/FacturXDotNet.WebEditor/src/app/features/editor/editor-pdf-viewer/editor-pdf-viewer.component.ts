import { Component, DestroyRef, inject, input, linkedSignal, resource, Resource } from '@angular/core';
import { PdfViewerComponent } from './pdf-viewer.component';
import { EditorSavedState, EditorStateService } from '../editor-state.service';
import { GenerateApi } from '../../../core/api/generate.api';
import { firstValueFrom, map } from 'rxjs';
import { toastError } from '../../../core/toasts/toast-error';
import { ToastService } from '../../../core/toasts/toast.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

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
  private generateApi = inject(GenerateApi);
  private toastService = inject(ToastService);
  private destroyRef = inject(DestroyRef);

  protected state: Resource<EditorSavedState | null> = this.editorStateService.savedState;
  protected pdf = resource({
    request: this.state.value,
    loader: async (state): Promise<{ id?: string; content: Blob } | undefined> => {
      if (state.request === null || state.request === undefined) {
        return undefined;
      }

      if (state.request.pdf !== undefined) {
        return state.request.pdf;
      }

      return await firstValueFrom(
        this.generateApi.generateStandardPdf(state.request.cii).pipe(
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
