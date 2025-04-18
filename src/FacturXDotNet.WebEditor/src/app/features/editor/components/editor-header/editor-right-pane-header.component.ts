import { Component, inject, model } from '@angular/core';
import { NgbNav, NgbNavItem, NgbNavLinkButton } from '@ng-bootstrap/ng-bootstrap';
import { PdfModel } from '../../editor-settings.service';
import { EditorPdfViewerService } from '../editor-pdf-viewer/editor-pdf-viewer.service';
import { EditorPdfGenerationProfilesService } from '../../editor-pdf-generation-profiles.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-editor-right-pane-header',
  imports: [NgbNav, NgbNavItem, NgbNavLinkButton, RouterLink],
  template: `
    <div class="d-flex align-items-center">
      <span class="fw-semibold"><i class="bi bi-file-pdf"></i> PDF</span>
      <ul ngbNav [(activeId)]="tab" class="nav-underline small ps-4">
        <li ngbNavItem="imported">
          <button ngbNavLink>Imported</button>
        </li>
        <li ngbNavItem="generated">
          <button ngbNavLink>Generated</button>
        </li>
      </ul>

      @if (tab() === 'generated') {
        @if (selectedProfile(); as selectedProfile) {
          <div class="small text-body-secondary px-4">
            Current profile: <span class="fw-semibold"> {{ selectedProfile.name }} </span>
            <a class="btn btn-sm btn-light border mx-2" routerLink="/settings/profiles">Change profile</a>
          </div>
        } @else {
          <a class="btn btn-sm btn-light border mx-4" routerLink="/settings/profiles">Customize generation</a>
        }

        <div class="flex-grow-1"><!--spacer--></div>

        <div class="px-2">
          @if (pdf.isLoading()) {
            <div class="spinner-border spinner-border-sm" role="status">
              <span class="visually-hidden">Loading...</span>
            </div>
          } @else {
            <button class="btn btn-sm btn-light border" (click)="regeneratePdf()"><i class="bi bi-arrow-repeat"></i> Regenerate</button>
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

  private editorPdfGenerationProfilesService = inject(EditorPdfGenerationProfilesService);
  protected selectedProfile = this.editorPdfGenerationProfilesService.selectedProfile;

  protected regeneratePdf() {
    this.editorPdfViewerService.regenerateAndDisplayGeneratedPdf();
  }
}
