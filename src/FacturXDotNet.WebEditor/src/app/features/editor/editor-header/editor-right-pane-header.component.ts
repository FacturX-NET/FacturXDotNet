import { Component, inject, model } from '@angular/core';
import { NgbNav, NgbNavItem, NgbNavLinkButton } from '@ng-bootstrap/ng-bootstrap';
import { PdfModel } from '../editor-settings.service';
import { EditorPdfViewerService } from '../editor-pdf-viewer/editor-pdf-viewer.service';

@Component({
  selector: 'app-editor-right-pane-header',
  imports: [NgbNav, NgbNavItem, NgbNavLinkButton],
  template: `
    <div class="d-flex align-items-center">
      <span class="fw-semibold"><i class="bi bi-file-pdf"></i> PDF</span>
      <ul ngbNav [(activeId)]="tab" class="nav-underline small ps-4">
        <li ngbNavItem="imported">
          <button ngbNavLink>Imported</button>
        </li>
        <li ngbNavItem="generated">
          <button ngbNavLink>Standard</button>
        </li>
      </ul>

      @if (tab() === 'generated') {
        <div class="ps-2">
          @if (pdf.isLoading()) {
            <div class="spinner-border spinner-border-sm" role="status">
              <span class="visually-hidden">Loading...</span>
            </div>
          } @else {
            <button class="btn btn-sm" (click)="regeneratePdf()"><i class="bi bi-arrow-repeat"></i> Regenerate</button>
          }
        </div>
      }
    </div>
  `,
  styles: ``,
})
export class EditorRightPaneHeaderComponent {
  tab = model.required<PdfModel>();

  private editorPdfViewerService = inject(EditorPdfViewerService);
  protected pdf = this.editorPdfViewerService.pdf;

  protected regeneratePdf() {
    this.editorPdfViewerService.regenerateAndDisplayStandardPdf();
  }
}
