import { Component, DestroyRef, inject } from '@angular/core';
import { EditorMenuService } from './editor-menu.service';
import { ToastService } from '../../../core/toasts/toast.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { toastError } from '../../../core/toasts/toast-error';
import { NgbDropdown, NgbDropdownItem, NgbDropdownMenu, NgbDropdownToggle } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-editor-file-menu',
  template: `
    <li class="nav-item" ngbDropdown>
      <button id="editor-file-menu" class="nav-link px-4 text-light" ngbDropdownToggle>File</button>
      <div ngbDropdownMenu aria-labelledby="editor-file-menu">
        <button (click)="createNewDocument()" ngbDropdownItem>New FacturX document</button>
        <button (click)="createNewDocumentFromFacturX()" ngbDropdownItem>Open FacturX document</button>
        <button (click)="createNewDocumentFromCrossIndustryInvoice()" ngbDropdownItem>Open Cross-Industry Invoice</button>
        <button (click)="createNewDocumentFromPdf()" ngbDropdownItem>Open PDF</button>
      </div>
    </li>
  `,
  imports: [NgbDropdown, NgbDropdownToggle, NgbDropdownMenu, NgbDropdownItem],
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
