import { Component, DestroyRef, inject } from '@angular/core';
import { EditorStateService } from '../../services/editor-state.service';
import { ImportFileService } from '../../../../core/import-file/import-file.service';
import { catchError, filter, from, map, Observable, of, switchMap } from 'rxjs';
import { CrossIndustryInvoice } from '../../../../core/api/api.models';
import { ExtractApi } from '../../../../core/api/extract.api';
import { ToastService } from '../../../../core/toasts/toast.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-editor-import-menu',
  template: `
    <li class="nav-item dropdown">
      <a class="nav-link dropdown-toggle px-4 text-light" role="button" data-bs-toggle="dropdown" aria-expanded="false">Import</a>
      <ul class="dropdown-menu">
        <li>
          <a class="dropdown-item" href="javascript:void 0;" (click)="importCrossIndustryInvoice()">Import Cross-Industry Invoice data</a>
          <a class="dropdown-item" href="javascript:void 0;" (click)="importPdfImage()">Import PDF image</a>
        </li>
      </ul>
    </li>
  `,
})
export class EditorImportMenuComponent {
  private editorStateService = inject(EditorStateService);
  private importFileService = inject(ImportFileService);
  private extractApi = inject(ExtractApi);
  private toastService = inject(ToastService);
  private destroyRef = inject(DestroyRef);

  importCrossIndustryInvoice() {
    from(this.importFileService.importFile('.xml'))
      .pipe(
        switchMap((file): Observable<{ file: File; cii?: CrossIndustryInvoice } | undefined> => {
          if (file === undefined) {
            this.toastService.show({ type: 'error', message: 'Could not import CII file.' });
            return of(undefined);
          }

          return this.extractApi.extractCrossIndustryInvoice(file).pipe(
            catchError((err) => {
              console.error(err);
              return of(undefined);
            }),
            map((cii) => ({ file, cii })),
          );
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
      )
      .subscribe();
  }

  importPdfImage() {
    this.importFileService.importFile('.pdf').then(async (file) => {
      if (file === undefined) {
        return;
      }

      const nameWithoutExtension = file.name.replace(/\.[^/.]+$/, '');
      const newState = { name: nameWithoutExtension, cii: {}, autoGeneratePdf: false, pdf: file };
      await this.editorStateService.update(newState);
    });
  }
}
