import { Component, inject } from '@angular/core';
import { ToastService } from '../../../../core/toasts/toast.service';
import { EditorMenuService } from './editor-menu.service';
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

  async importCrossIndustryInvoice() {
    try {
      await this.editorMenuService.importCrossIndustryInvoiceData();
    } catch (error) {
      this.toastService.showError(error, (message) => `Could not import Cross-Industry Invoice file: ${message}`);
    }
  }

  async importPdfImage() {
    try {
      await this.editorMenuService.importPdfImageData();
    } catch (error) {
      this.toastService.showError(error, (message) => `Could not create PDF image: ${message}`);
    }
  }
}
