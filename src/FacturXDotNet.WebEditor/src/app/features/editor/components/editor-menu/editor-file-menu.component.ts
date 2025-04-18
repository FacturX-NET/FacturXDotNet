import { Component, inject } from '@angular/core';
import { EditorMenuService } from './editor-menu.service';
import { ToastService } from '../../../../core/toasts/toast.service';
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

  protected async createNewDocument() {
    try {
      await this.editorMenuService.backToWelcomePage();
    } catch (error) {
      this.toastService.showError(error, (message) => `Could not create new Factur-X document: ${message}`);
    }
  }

  protected async createNewDocumentFromFacturX() {
    try {
      await this.editorMenuService.createNewDocumentFromFacturX();
    } catch (error) {
      this.toastService.showError(error, (message) => `Could not open Factur-X document: ${message}`);
    }
  }

  protected async createNewDocumentFromCrossIndustryInvoice() {
    try {
      await this.editorMenuService.createNewDocumentFromCrossIndustryInvoice();
    } catch (error) {
      this.toastService.showError(error, (message) => `Could not open Factur-X document: ${message}`);
    }
  }

  protected async createNewDocumentFromPdf() {
    try {
      await this.editorMenuService.createNewDocumentFromPdf();
    } catch (error) {
      this.toastService.showError(error, (message) => `Could not open Factur-X document: ${message}`);
    }
  }
}
