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

  createNewFacturXDocument(): Observable<void> {
    return from(this.editorStateService.clear());
  }

  importFacturX(): Observable<void> {
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
        const newState = { name: nameWithoutExtension, cii: result.cii, autoGeneratePdf: false, pdf: result.file };
        return from(this.editorStateService.update(newState));
      }),
      takeUntilDestroyed(this.destroyRef),
    );
  }

  importCrossIndustryInvoice(): Observable<void> {
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

        const nameWithoutExtension = result.file.name.replace(/\.[^/.]+$/, '');
        const newState = { name: nameWithoutExtension, cii: result.cii, autoGeneratePdf: true };
        return from(this.editorStateService.update(newState));
      }),
      takeUntilDestroyed(this.destroyRef),
    );
  }

  importPdfImage(): Observable<void> {
    return from(this.importFileService.importFile('.pdf')).pipe(
      switchMap((file) => {
        if (file === undefined) {
          return of(void 0);
        }

        const nameWithoutExtension = file.name.replace(/\.[^/.]+$/, '');
        const newState = { name: nameWithoutExtension, cii: {}, autoGeneratePdf: false, pdf: file };
        return from(this.editorStateService.update(newState));
      }),
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

    return this.generateApi.generateFacturX(value.pdf, cii).pipe(
      map((file) => {
        downloadFile(file, value.name);
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

    downloadBlob(value.pdf, value.name ?? 'invoice.pdf');
    return of(void 0);
  }

  private getValidCii(): ICrossIndustryInvoice | undefined {
    if (!this.ciiFormService.validate()) {
      return undefined;
    }

    return this.ciiFormService.form.getRawValue();
  }
}
