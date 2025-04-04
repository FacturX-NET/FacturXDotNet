import { Component, DestroyRef, inject } from '@angular/core';
import { EditorStateService } from '../../services/editor-state.service';
import { GenerateApi } from '../../../../core/api/generate.api';
import { catchError, map, of } from 'rxjs';
import { ToastService } from '../../../../core/toasts/toast.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { downloadBlob, downloadFile } from '../../../../core/utils/download-blob';

@Component({
  selector: 'app-editor-export-menu',
  template: `
    <li class="nav-item dropdown">
      <a class="nav-link dropdown-toggle px-4 text-light" role="button" data-bs-toggle="dropdown" aria-expanded="false">Export</a>
      <ul class="dropdown-menu">
        <li>
          <a class="dropdown-item" href="javascript:void 0;">Download FacturX document</a>
          <a class="dropdown-item" href="javascript:void 0;" (click)="exportCrossIndustryInvoice()">Download Cross-Industry Invoice XML file</a>
          <a class="dropdown-item" href="javascript:void 0;" (click)="exportPdfImage()">Download PDF file</a>
        </li>
      </ul>
    </li>
  `,
})
export class EditorExportMenuComponent {
  private editorStateService = inject(EditorStateService);
  private generateApi = inject(GenerateApi);
  private toastService = inject(ToastService);
  private destroyRef = inject(DestroyRef);

  exportCrossIndustryInvoice() {
    if (!this.editorStateService.savedState.hasValue()) {
      return;
    }

    const value = this.editorStateService.savedState.value();
    if (value?.cii?.content === undefined) {
      return;
    }

    this.generateApi
      .generateCrossIndustryInvoice(value.cii.content)
      .pipe(
        map((blob) => {
          downloadBlob(blob, value.cii.name ?? 'factur-x.xml');
        }),
        catchError((err) => {
          this.toastService.show({ type: 'error', message: 'Could not generate CII file.' });
          console.error(err);
          return of(void 0);
        }),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe();
  }

  exportPdfImage() {
    if (!this.editorStateService.savedState.hasValue()) {
      return;
    }

    const value = this.editorStateService.savedState.value();
    if (value?.pdf?.content === undefined) {
      return;
    }

    downloadBlob(value.pdf.content, value.pdf.name ?? 'invoice.pdf');
  }
}
