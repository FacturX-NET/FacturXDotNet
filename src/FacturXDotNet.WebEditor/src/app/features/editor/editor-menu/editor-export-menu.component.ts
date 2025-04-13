import { Component, DestroyRef, inject, output } from '@angular/core';
import { finalize } from 'rxjs';
import { ToastService } from '../../../core/toasts/toast.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { EditorMenuService } from './editor-menu.service';
import { toastError } from '../../../core/toasts/toast-error';
import { NgbDropdown, NgbDropdownItem, NgbDropdownMenu, NgbDropdownToggle } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-editor-export-menu',
  template: `
    <li class="nav-item" ngbDropdown>
      <button id="editor-export-menu" class="nav-link px-4 text-light" ngbDropdownToggle>Export</button>
      <div ngbDropdownMenu aria-labelledby="editor-export-menu">
        <button (click)="exportFacturX()" ngbDropdownItem>Download FacturX document</button>
        <button (click)="exportCrossIndustryInvoice()" ngbDropdownItem>Download Cross-Industry Invoice XML file</button>
        <button (click)="exportPdfImage()" ngbDropdownItem>Download PDF file</button>
      </div>
    </li>
  `,
  imports: [NgbDropdown, NgbDropdownToggle, NgbDropdownMenu, NgbDropdownItem],
})
export class EditorExportMenuComponent {
  exporting = output<boolean>();

  private editorMenuService = inject(EditorMenuService);
  private toastService = inject(ToastService);
  private destroyRef = inject(DestroyRef);

  exportFacturX() {
    this.exporting.emit(true);

    this.editorMenuService
      .exportFacturX()
      .pipe(
        toastError(this.toastService, (message) => `Could not export Factur-X document: ${message}.`),
        finalize(() => {
          this.exporting.emit(false);
        }),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe();
  }

  exportCrossIndustryInvoice() {
    this.exporting.emit(true);

    this.editorMenuService
      .exportCrossIndustryInvoice()
      .pipe(
        toastError(this.toastService, (message) => `Could not export Cross-Industry Invoice data: ${message}.`),
        finalize(() => {
          this.exporting.emit(false);
        }),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe();
  }

  exportPdfImage() {
    this.exporting.emit(true);

    this.editorMenuService
      .exportPdfImage()
      .pipe(
        toastError(this.toastService, (message) => `Could not export PDF image: ${message}.`),
        finalize(() => {
          this.exporting.emit(false);
        }),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe();
  }
}
