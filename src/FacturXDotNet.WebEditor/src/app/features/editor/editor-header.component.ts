import { Component, DestroyRef, inject, input, output } from '@angular/core';
import { finalize } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { EditorMenuService } from './components/editor-menu/editor-menu.service';
import { EditorSavedState } from './editor-state.service';
import { ToastService } from '../../core/toasts/toast.service';
import { toastError } from '../../core/utils/toast-error';

@Component({
  selector: 'app-editor-header',
  template: `
    <div class="d-flex">
      <div class="d-none d-xl-block col-3"><!--spacer--></div>
      <div class="navbar navbar-expand-xl flex-grow-1">
        <div class="flex-grow-1 d-flex justify-content-start align-items-center gap-3 px-3">
          <div class="d-block d-xl-none">
            <button class="navbar-toggler" data-bs-toggle="offcanvas" data-bs-target="#editor__cii-summary">
              <span class="navbar-toggler-icon"></span>
            </button>
          </div>

          <div class="navbar-brand d-flex align-items-center gap-2">
            <i class="bi bi-code pe-1 fs-4"></i>
            <h5 class="m-0">
              {{ state().name }}
            </h5>
          </div>

          <div class="flex-grow-1"><!--spacer--></div>

          @if (saving()) {
            <div><i class="bi bi-floppy2-fill text-body-tertiary small glow"></i></div>
          } @else {
            @if (saved()) {
              <div class="text-success small"><i class="bi bi-check"></i> Saved</div>
            }

            <div>
              <div class="input-group">
                <button class="btn btn-outline-secondary" (click)="exportFacturX()">Export</button>
                <button class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false"></button>
                <div class="dropdown-menu dropdown-menu-end">
                  <li><a class="dropdown-item" (click)="exportCrossIndustryInvoice()">Export Cross-Industry Invoice</a></li>
                  <li><a class="dropdown-item" (click)="exportPdfImage()">Export PDF</a></li>
                </div>
              </div>
            </div>
          }
        </div>
      </div>
    </div>
  `,
})
export class EditorHeaderComponent {
  state = input.required<EditorSavedState>();
  saving = input<boolean>();
  saved = input<boolean>();
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
