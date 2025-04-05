import { Component, DestroyRef, inject } from '@angular/core';
import { EditorMenuService } from './components/editor-menu/editor-menu.service';
import { ToastService } from '../../core/toasts/toast.service';
import { toastError } from '../../core/utils/toast-error';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-editor-welcome',
  template: `
    <div class="w-100 h-100 d-flex flex-column gap-5 align-items-center justify-content-center">
      <h1 class="lead fs-1">Get started</h1>
      <div class="d-flex gap-5 align-items-center justify-content-center">
        <button class="btn btn-shadow d-flex flex-column gap-2 align-items-center justify-content-center border rounded-3 p-5" style="width: 400px">
          <i class="bi bi-file-excel text-primary fs-1"></i>
          <div class="lead text-primary">Import FacturX document</div>
        </button>
        <button class="btn btn-shadow d-flex flex-column gap-2 align-items-center justify-content-center border rounded-3 p-5" style="width: 400px" (click)="importPdfImage()">
          <i class="bi bi-file-pdf text-primary fs-1"></i>
          <div class="lead text-primary">Import PDF image</div>
        </button>
        <button
          class="btn btn-shadow d-flex flex-column gap-2 align-items-center justify-content-center border rounded-3 p-5"
          style="width: 400px"
          (click)="importCrossIndustryInvoice()"
        >
          <i class="bi bi-file-code text-primary fs-1"></i>
          <div class="lead text-primary">Import Cross-Industry Invoice file</div>
        </button>
      </div>
    </div>
  `,
})
export class EditorWelcomeComponent {
  private editorMenuService = inject(EditorMenuService);
  private toastService = inject(ToastService);
  private destroyRef = inject(DestroyRef);

  importCrossIndustryInvoice() {
    this.editorMenuService
      .importCrossIndustryInvoice()
      .pipe(toastError(this.toastService, 'Could not import Cross-Industry Invoice file.'), takeUntilDestroyed(this.destroyRef))
      .subscribe();
  }

  importPdfImage() {
    this.editorMenuService.importPdfImage().pipe(toastError(this.toastService, 'Could not create PDF image.'), takeUntilDestroyed(this.destroyRef)).subscribe();
  }
}
