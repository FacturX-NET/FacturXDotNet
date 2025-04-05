import { Component, DestroyRef, inject, output } from '@angular/core';
import { EditorStateService } from '../../services/editor-state.service';
import { GenerateApi } from '../../../../core/api/generate.api';
import { catchError, finalize, map, of } from 'rxjs';
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
          <a class="dropdown-item" href="javascript:void 0;" (click)="exportFacturX()">Download FacturX document</a>
          <a class="dropdown-item" href="javascript:void 0;" (click)="exportCrossIndustryInvoice()">Download Cross-Industry Invoice XML file</a>
          <a class="dropdown-item" href="javascript:void 0;" (click)="exportPdfImage()">Download PDF file</a>
        </li>
      </ul>
    </li>
  `,
})
export class EditorExportMenuComponent {
  exporting = output<boolean>();

  private editorStateService = inject(EditorStateService);
  private generateApi = inject(GenerateApi);
  private toastService = inject(ToastService);
  private destroyRef = inject(DestroyRef);

  exportFacturX() {
    if (!this.editorStateService.savedState.hasValue()) {
      return;
    }

    const value = this.editorStateService.savedState.value();
    if (value?.pdf?.content === undefined || value?.cii?.content === undefined) {
      this.toastService.show({ type: 'error', message: 'PDF or CII not set.' });
      return;
    }

    this.exporting.emit(true);

    this.generateApi
      .generateFacturX(value.pdf.content, value.cii.content)
      .pipe(
        map((file) => {
          downloadFile(file, value.pdf?.name);
        }),
        catchError((err) => {
          this.toastService.show({ type: 'error', message: 'Could not generate CII file.' });
          console.error(err);
          return of(void 0);
        }),
        finalize(() => {
          this.exporting.emit(false);
        }),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe();
  }

  exportCrossIndustryInvoice() {
    if (!this.editorStateService.savedState.hasValue()) {
      return;
    }

    const value = this.editorStateService.savedState.value();
    if (value?.cii?.content === undefined) {
      this.toastService.show({ type: 'error', message: 'CII not set.' });
      return;
    }

    this.exporting.emit(true);

    this.generateApi
      .generateCrossIndustryInvoice(value.cii.content)
      .pipe(
        map((file) => {
          downloadFile(file);
        }),
        catchError((err) => {
          this.toastService.show({ type: 'error', message: 'Could not generate CII file.' });
          console.error(err);
          return of(void 0);
        }),
        finalize(() => {
          this.exporting.emit(false);
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
      this.toastService.show({ type: 'error', message: 'PDF not set.' });
      return;
    }

    this.exporting.emit(true);

    downloadBlob(value.pdf.content, value.pdf.name ?? 'invoice.pdf');

    this.exporting.emit(false);
  }
}
