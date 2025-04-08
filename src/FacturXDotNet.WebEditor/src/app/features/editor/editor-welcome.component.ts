import { Component, DestroyRef, inject } from '@angular/core';
import { EditorMenuService } from './editor-menu/editor-menu.service';
import { ToastService } from '../../core/toasts/toast.service';
import { toastError } from '../../core/utils/toast-error';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-editor-welcome',
  template: `
    <div class="w-100 h-100 d-flex flex-column gap-4 align-items-center justify-content-center">
      <h1 class="lead fs-1">Get started</h1>

      <div class="w-100 px-4 d-flex flex-wrap gap-4 justify-content-center">
        <button
          class="btn btn-shadow col-12 col-md-5 col-xl-3 d-flex flex-column gap-2 align-items-center justify-content-center border rounded-3 p-5"
          (click)="createNewDocument()"
        >
          <i class="bi bi-file-excel text-primary fs-1"></i>
          <div class="lead text-primary">Create blank document</div>
        </button>
      </div>

      <div class="w-100 px-4 d-flex flex-wrap gap-4 justify-content-center">
        <button
          class="btn btn-shadow col-12 col-md-5 col-xl-3 d-flex flex-column gap-2 align-items-center justify-content-center border rounded-3 p-5"
          (click)="createNewDocumentFromFacturX()"
        >
          <i class="bi bi-file-excel text-primary fs-1"></i>
          <div class="lead text-primary">Open FacturX</div>
        </button>
        <button
          class="btn btn-shadow col-12 col-md-5 col-xl-3 d-flex flex-column gap-2 align-items-center justify-content-center border rounded-3 p-5"
          (click)="createNewDocumentFromPdf()"
        >
          <i class="bi bi-file-pdf text-primary fs-1"></i>
          <div class="lead text-primary">Open PDF</div>
        </button>
        <button
          class="btn btn-shadow col-12 col-md-5 col-xl-3 d-flex flex-column gap-2 align-items-center justify-content-center border rounded-3 p-5"
          (click)="createNewDocumentFromCrossIndustryInvoice()"
        >
          <i class="bi bi-file-code text-primary fs-1"></i>
          <div class="lead text-primary">Open Cross-Industry Invoice</div>
        </button>
      </div>
    </div>
  `,
})
export class EditorWelcomeComponent {
  private editorMenuService = inject(EditorMenuService);
  private toastService = inject(ToastService);
  private destroyRef = inject(DestroyRef);

  createNewDocument() {
    this.editorMenuService.createNewDocument().pipe(toastError(this.toastService, 'Could not create blank document.'), takeUntilDestroyed(this.destroyRef)).subscribe();
  }

  createNewDocumentFromFacturX() {
    this.editorMenuService.createNewDocumentFromFacturX().pipe(toastError(this.toastService, 'Could not import FacturX file.'), takeUntilDestroyed(this.destroyRef)).subscribe();
  }

  createNewDocumentFromCrossIndustryInvoice() {
    this.editorMenuService
      .createNewDocumentFromCrossIndustryInvoice()
      .pipe(toastError(this.toastService, 'Could not import Cross-Industry Invoice file.'), takeUntilDestroyed(this.destroyRef))
      .subscribe();
  }

  createNewDocumentFromPdf() {
    this.editorMenuService.createNewDocumentFromPdf().pipe(toastError(this.toastService, 'Could not import PDF file.'), takeUntilDestroyed(this.destroyRef)).subscribe();
  }
}
