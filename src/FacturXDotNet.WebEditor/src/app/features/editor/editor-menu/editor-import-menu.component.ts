import { Component, DestroyRef, inject } from '@angular/core';
import { ToastService } from '../../../core/toasts/toast.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { EditorMenuService } from './editor-menu.service';
import { toastError } from '../../../core/utils/toast-error';

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
  private editorMenuService = inject(EditorMenuService);
  private toastService = inject(ToastService);
  private destroyRef = inject(DestroyRef);

  importCrossIndustryInvoice() {
    this.editorMenuService
      .importCrossIndustryInvoiceData()
      .pipe(
        toastError(this.toastService, (message) => `Could not import Cross-Industry Invoice file: ${message}`),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe();
  }

  importPdfImage() {
    this.editorMenuService
      .importPdfImageData()
      .pipe(
        toastError(this.toastService, (message) => `Could not create PDF image: ${message}`),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe();
  }
}
