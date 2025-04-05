import { Component, DestroyRef, inject, output } from '@angular/core';
import { EditorStateService } from '../../services/editor-state.service';
import { GenerateApi } from '../../../../core/api/generate.api';
import { catchError, finalize, map, of } from 'rxjs';
import { ToastService } from '../../../../core/toasts/toast.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { downloadBlob, downloadFile } from '../../../../core/utils/download-blob';
import { CiiFormService } from '../cii-form/cii-form.service';
import { ICrossIndustryInvoice } from '../../../../core/api/api.models';

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
  private ciiFormService = inject(CiiFormService);
  private generateApi = inject(GenerateApi);
  private toastService = inject(ToastService);
  private destroyRef = inject(DestroyRef);

  exportFacturX() {
    if (!this.editorStateService.savedState.hasValue()) {
      return;
    }

    const pdf = this.editorStateService.savedState.value()?.pdf;
    if (pdf?.content === undefined) {
      this.toastService.show({ type: 'error', message: 'The PDF is not set.' });
      return;
    }

    const cii = this.getValidCii();
    if (cii === undefined) {
      return;
    }

    this.exporting.emit(true);

    this.generateApi
      .generateFacturX(pdf.content, cii)
      .pipe(
        map((file) => {
          downloadFile(file, pdf.name);
        }),
        catchError((err) => {
          this.toastService.show({ type: 'error', message: 'Could not generate the Cross-Industry Invoice file.' });
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

    const cii = this.getValidCii();
    if (cii === undefined) {
      return;
    }

    this.exporting.emit(true);

    this.generateApi
      .generateCrossIndustryInvoice(cii)
      .pipe(
        map((file) => {
          downloadFile(file);
        }),
        catchError((err) => {
          this.toastService.show({ type: 'error', message: 'Could not generate the Cross-Industry Invoice file.' });
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
      this.toastService.show({ type: 'error', message: 'The PDF is not set.' });
      return;
    }

    this.exporting.emit(true);

    downloadBlob(value.pdf.content, value.pdf.name ?? 'invoice.pdf');

    this.exporting.emit(false);
  }

  private getValidCii(): ICrossIndustryInvoice | undefined {
    if (!this.ciiFormService.validate()) {
      this.toastService.show({ type: 'error', message: 'The Cross-Industry Invoice data is not valid.' });
      return undefined;
    }

    return this.ciiFormService.form.getRawValue();
  }
}
