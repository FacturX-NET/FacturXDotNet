import { DestroyRef, inject, Injectable } from '@angular/core';
import { ImportFileService } from '../../../../core/import-file/import-file.service';
import { ExtractApi } from '../../../../core/api/extract.api';
import { ToastService } from '../../../../core/toasts/toast.service';
import { filter, from, map, Observable, of, switchMap, throwError } from 'rxjs';
import { CrossIndustryInvoice, ICrossIndustryInvoice } from '../../../../core/api/api.models';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { downloadBlob, downloadFile } from '../../../../core/utils/download-blob';
import { GenerateApi } from '../../../../core/api/generate.api';
import { CiiFormService } from '../../tabs/cii/cii-form/cii-form.service';
import { EditorStateService } from '../../editor-state.service';
import * as pdf from 'pdfjs-dist';

@Injectable({
  providedIn: 'root',
})
export class EditorMenuService {
  private editorStateService = inject(EditorStateService);
  private ciiFormService = inject(CiiFormService);
  private importFileService = inject(ImportFileService);
  private extractApi = inject(ExtractApi);
  private generateApi = inject(GenerateApi);
  private toastService = inject(ToastService);
  private destroyRef = inject(DestroyRef);

  constructor() {
    pdf.GlobalWorkerOptions.workerSrc = 'pdf.worker.min.mjs';
  }

  createNewDocument(): Observable<void> {
    return from(this.editorStateService.clear());
  }

  /**
   * Import the PDF data, the Cross-Industry Invoice data and the attachments into a new document.
   */
  createNewDocumentFromFacturX(): Observable<void> {
    return from(this.importFileService.importFile('.pdf')).pipe(
      switchMap((file): Observable<{ file: File; cii?: CrossIndustryInvoice } | undefined> => {
        if (file === undefined) {
          return of(undefined);
        }

        return this.extractApi.extractCrossIndustryInvoice(file).pipe(map((cii) => ({ file, cii })));
      }),
      filter((result) => result !== undefined),
      switchMap((result) => {
        return from(this.extractPdfAttachments(result.file)).pipe(map((attachments) => ({ file: result.file, cii: result.cii, attachments })));
      }),
      switchMap((result) => {
        if (result.cii === undefined) {
          this.toastService.show({ type: 'error', message: 'Could not extract CII data from file ' + result.file.name + '.' });
          return of(void 0);
        }

        const nameWithoutExtension = result.file.name.replace(/\.[^/.]+$/, '');
        return from(this.editorStateService.save(nameWithoutExtension, result.cii, result.file, false, result.attachments));
      }),
      takeUntilDestroyed(this.destroyRef),
    );
  }

  /**
   * Import the Cross-Industry Invoice data into a new document.
   */
  createNewDocumentFromCrossIndustryInvoice(): Observable<void> {
    return from(this.importFileService.importFile('.pdf')).pipe(
      switchMap((file): Observable<{ file: File; cii?: CrossIndustryInvoice } | undefined> => {
        if (file === undefined) {
          return of(undefined);
        }

        return this.extractApi.extractCrossIndustryInvoice(file).pipe(map((cii) => ({ file, cii })));
      }),
      filter((result) => result !== undefined),
      switchMap((result) => {
        if (result.cii === undefined) {
          this.toastService.show({ type: 'error', message: 'Could not extract CII data from file ' + result.file.name + '.' });
          return of(void 0);
        }

        const nameWithoutExtension = result.file.name.replace(/\.[^/.]+$/, '');
        return from(this.editorStateService.save(nameWithoutExtension, result.cii, undefined, true, []));
      }),
      takeUntilDestroyed(this.destroyRef),
    );
  }

  /**
   * Import the PDF data into a new document.
   */
  createNewDocumentFromPdf(): Observable<void> {
    return from(this.importFileService.importFile('.pdf')).pipe(
      filter((file) => file !== undefined),
      switchMap((file) => {
        return from(this.extractPdfAttachments(file)).pipe(map((attachments) => ({ file: file, attachments })));
      }),
      switchMap((result) => {
        const nameWithoutExtension = result.file.name.replace(/\.[^/.]+$/, '');
        return from(this.editorStateService.save(nameWithoutExtension, {}, result.file, false, result.attachments));
      }),
      takeUntilDestroyed(this.destroyRef),
    );
  }

  /**
   * Imports a Cross-Industry Invoice file and merge its data with the current state.
   */
  importCrossIndustryInvoiceData(): Observable<void> {
    return from(this.importFileService.importFile('.xml')).pipe(
      switchMap((file): Observable<{ file: File; cii?: CrossIndustryInvoice } | undefined> => {
        if (file === undefined) {
          return of(undefined);
        }

        return this.extractApi.extractCrossIndustryInvoice(file).pipe(map((cii) => ({ file, cii })));
      }),
      filter((result) => result !== undefined),
      switchMap((result) => {
        if (result.cii === undefined) {
          this.toastService.show({ type: 'error', message: 'Could not extract CII data from file ' + result.file.name + '.' });
          return of(void 0);
        }

        return from(this.editorStateService.updateCii(result.cii));
      }),
      takeUntilDestroyed(this.destroyRef),
    );
  }

  /**
   * Imports a PDF file and merge its data with the current state.
   * The attachments of the PDF are ignored.
   */
  importPdfImageData(): Observable<void> {
    return from(this.importFileService.importFile('.pdf')).pipe(
      filter((file) => file != undefined),
      switchMap((file) => {
        return from(this.extractPdfAttachments(file)).pipe(map((attachments) => ({ file, attachments })));
      }),
      switchMap((result) => from(this.editorStateService.updatePdf(result.file, false))),
    );
  }

  exportFacturX(): Observable<void> {
    if (!this.editorStateService.savedState.hasValue()) {
      return throwError(() => new Error('Internal Error: no saved state available.'));
    }

    const value = this.editorStateService.savedState.value();
    if (value === null) {
      return throwError(() => new Error('Internal Error: no saved state available.'));
    }

    if (value?.pdf === undefined) {
      return throwError(() => new Error('Internal Error: the PDF is not set.'));
    }

    const cii = this.getValidCii();
    if (cii === undefined) {
      return throwError(() => new Error('Internal Error: no valid CII data available.'));
    }

    return this.generateApi.generateFacturX(value.pdf.content, cii, ...value.attachments).pipe(
      map((file) => {
        downloadFile(file, value.name + '.pdf');
      }),
      takeUntilDestroyed(this.destroyRef),
    );
  }

  exportCrossIndustryInvoice(): Observable<void> {
    if (!this.editorStateService.savedState.hasValue()) {
      return throwError(() => new Error('Internal Error: no saved state available.'));
    }

    const value = this.editorStateService.savedState.value();
    if (value === null) {
      return throwError(() => new Error('Internal Error: no saved state available.'));
    }

    const cii = this.getValidCii();
    if (cii === undefined) {
      return throwError(() => new Error('Internal Error: no valid CII data available.'));
    }

    return this.generateApi.generateCrossIndustryInvoice(cii).pipe(
      map((file) => {
        downloadFile(file, value.name + '.xml');
      }),
      takeUntilDestroyed(this.destroyRef),
    );
  }

  exportPdfImage(): Observable<void> {
    if (!this.editorStateService.savedState.hasValue()) {
      return throwError(() => new Error('Internal Error: no saved state available.'));
    }

    const value = this.editorStateService.savedState.value();
    if (value?.pdf === undefined) {
      return throwError(() => new Error('Internal Error: the PDF is not set.'));
    }

    downloadBlob(value.pdf.content, value.name ?? 'invoice.pdf');
    return of(void 0);
  }

  private getValidCii(): ICrossIndustryInvoice | undefined {
    if (!this.ciiFormService.validate()) {
      return undefined;
    }

    return this.ciiFormService.form.getRawValue();
  }

  private async extractPdfAttachments(file: File): Promise<{ name: string; description?: string; content: Uint8Array }[]> {
    const buffer = await file.arrayBuffer();
    const pdfDocumentLoadingTask = pdf.getDocument(buffer);
    const pdfDocument = await pdfDocumentLoadingTask.promise;
    const attachmentsObj = await pdfDocument.getAttachments();
    const attachments: { filename: string; description: string; content: Uint8Array }[] = Object.values(attachmentsObj);
    return attachments.filter((a) => a.filename !== 'factur-x.xml').map((a) => ({ name: a.filename, description: a.description, content: a.content }));
  }
}
