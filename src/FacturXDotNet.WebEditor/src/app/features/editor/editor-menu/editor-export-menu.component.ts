import { Component, DestroyRef, inject, output } from '@angular/core';
import { finalize } from 'rxjs';
import { ToastService } from '../../../core/toasts/toast.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { EditorMenuService } from './editor-menu.service';
import { toastError } from '../../../core/utils/toast-error';

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

  private editorMenuService = inject(EditorMenuService);
  private toastService = inject(ToastService);
  private destroyRef = inject(DestroyRef);

  exportFacturX() {
    this.exporting.emit(true);

    this.editorMenuService
      .exportFacturX()
      .pipe(
        toastError(this.toastService, 'Could not export FacturX document.'),
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
        toastError(this.toastService, 'Could not export Cross-Industry Invoice data.'),
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
        toastError(this.toastService, 'Could not export PDF image.'),
        finalize(() => {
          this.exporting.emit(false);
        }),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe();
  }
}
