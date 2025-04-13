import { Component, DestroyRef, inject } from '@angular/core';
import { ToastService } from '../../../core/toasts/toast.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { EditorMenuService } from './editor-menu.service';
import { toastError } from '../../../core/toasts/toast-error';
import { NgbDropdown, NgbDropdownItem, NgbDropdownMenu, NgbDropdownToggle } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-editor-import-menu',
  template: `
    <li class="nav-item" ngbDropdown>
      <button id="editor-import-menu" class="nav-link px-4 text-light" ngbDropdownToggle>Import</button>
      <div ngbDropdownMenu aria-labelledby="editor-import-menu">
        <button (click)="importCrossIndustryInvoice()" ngbDropdownItem>Import Cross-Industry Invoice data</button>
        <button (click)="importPdfImage()" ngbDropdownItem>Import PDF image</button>
      </div>
    </li>
  `,
  imports: [NgbDropdown, NgbDropdownToggle, NgbDropdownMenu, NgbDropdownItem],
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
