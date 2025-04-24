import { Component, DestroyRef, inject, output } from '@angular/core';
import { ToastService } from '../../../../core/toasts/toast.service';
import { EditorMenuService } from './editor-menu.service';
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

  async exportFacturX() {
    this.exporting.emit(true);
    try {
      await this.editorMenuService.exportFacturX();
    } catch (error) {
      this.toastService.showError(error, (message) => `Could not export Factur-X document: ${message}.`);
    } finally {
      this.exporting.emit(false);
    }
  }

  async exportCrossIndustryInvoice() {
    this.exporting.emit(true);
    try {
      await this.editorMenuService.exportCrossIndustryInvoice();
    } catch (error) {
      this.toastService.showError(error, (message) => `Could not export Cross-Industry Invoice data: ${message}.`);
    } finally {
      this.exporting.emit(false);
    }
  }

  async exportPdfImage() {
    this.exporting.emit(true);
    try {
      await this.editorMenuService.exportPdfImage();
    } catch (error) {
      this.toastService.showError(error, (message) => `Could not export PDF image: ${message}.`);
    } finally {
      this.exporting.emit(false);
    }
  }
}
