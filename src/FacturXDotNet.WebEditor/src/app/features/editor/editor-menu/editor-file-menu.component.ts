import { Component, DestroyRef, inject } from '@angular/core';
import { EditorMenuService } from './editor-menu.service';
import { ToastService } from '../../../core/toasts/toast.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { toastError } from '../../../core/utils/toast-error';

@Component({
  selector: 'app-editor-file-menu',
  template: `
    <li class="nav-item dropdown">
      <a class="nav-link dropdown-toggle px-4 text-light" role="button" data-bs-toggle="dropdown" aria-expanded="false">File</a>
      <ul class="dropdown-menu">
        <li>
          <a role="button" class="dropdown-item" (click)="createNewDocument()">New FacturX document</a>
          <a role="button" class="dropdown-item" (click)="createNewDocumentFromFacturX()">Open FacturX document</a>
          <a role="button" class="dropdown-item" (click)="createNewDocumentFromCrossIndustryInvoice()">Open Cross-Industry Invoice</a>
          <a role="button" class="dropdown-item" (click)="createNewDocumentFromPdf()">Open PDF</a>
        </li>
      </ul>
    </li>
  `,
})
export class EditorFileMenuComponent {
  private editorMenuService = inject(EditorMenuService);
  private toastService = inject(ToastService);
  private destroyRef = inject(DestroyRef);

  protected createNewDocument() {
    this.editorMenuService
      .backToWelcomePage()
      .pipe(
        toastError(this.toastService, (message) => `Could not create new Factur-X document: ${message}`),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe();
  }

  protected createNewDocumentFromFacturX() {
    this.editorMenuService
      .createNewDocumentFromFacturX()
      .pipe(
        toastError(this.toastService, (message) => `Could not open Factur-X document: ${message}`),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe();
  }

  protected createNewDocumentFromCrossIndustryInvoice() {
    this.editorMenuService
      .createNewDocumentFromCrossIndustryInvoice()
      .pipe(
        toastError(this.toastService, (message) => `Could not open Factur-X document: ${message}`),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe();
  }

  protected createNewDocumentFromPdf() {
    this.editorMenuService
      .createNewDocumentFromPdf()
      .pipe(
        toastError(this.toastService, (message) => `Could not open Factur-X document: ${message}`),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe();
  }
}
