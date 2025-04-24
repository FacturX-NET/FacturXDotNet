import { Component, computed, inject, model } from '@angular/core';
import { NgbNav, NgbNavItem, NgbNavLinkButton } from '@ng-bootstrap/ng-bootstrap';
import { PdfModel } from '../../services/editor-settings.service';
import { EditorPdfViewerService } from '../../editor-tabs/editor-pdf-viewer/editor-pdf-viewer.service';
import { EditorPdfGenerationProfile, EditorPdfGenerationProfilesService } from '../../services/editor-pdf-generation-profiles.service';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-editor-right-pane-header',
  imports: [NgbNav, NgbNavItem, NgbNavLinkButton, FormsModule, RouterLink],
  template: `
    <div class="d-flex flex-column gap-2">
      <div class="ps-1 d-flex align-items-center">
        <span class="fs-5 fw-semibold"><i class="bi bi-file-pdf"></i> PDF</span>
        <ul ngbNav [(activeId)]="tab" class="nav-underline small ps-4">
          <li ngbNavItem="imported">
            <button ngbNavLink>Imported</button>
          </li>
          <li ngbNavItem="generated">
            <button ngbNavLink>Generated</button>
          </li>
        </ul>
      </div>

      <div class="d-flex align-items-center">
        @if (tab() === 'generated') {
          @if (profiles().length > 0) {
            <div class="flex-grow-1 editor__control row">
              <div class="col-3">
                <label class="col-form-label col-form-label-sm">Profile:</label>
              </div>
              <div class="col">
                <select class="form-select form-select-sm" [ngModel]="selectedProfile()" (ngModelChange)="changeProfile($event)">
                  <option class="text-body-secondary" [ngValue]="undefined">(none)</option>
                  @for (profile of profiles(); track profile.id) {
                    <option [ngValue]="profile">{{ profile.name }}</option>
                  }
                </select>
              </div>
            </div>
          } @else {
            <button class="btn btn-sm btn-light border text-nowrap" routerLink="/settings/profiles">Customize PDF generation</button>
          }

          <div class="flex-grow-1"><!--spacer--></div>

          <div class="px-2">
            @if (pdf.isLoading()) {
              <div class="spinner-border spinner-border-sm" role="status">
                <span class="visually-hidden">Loading...</span>
              </div>
            } @else {
              <button class="btn btn-sm btn-light border text-nowrap" (click)="regeneratePdf()"><i class="bi bi-arrow-repeat"></i> Regenerate</button>
            }
          </div>
        }
      </div>
    </div>
  `,
  styles: ``,
})
export class EditorRightPaneHeaderComponent {
  tab = model.required<PdfModel>();

  private router: Router = inject(Router);
  private editorPdfViewerService = inject(EditorPdfViewerService);
  protected pdf = this.editorPdfViewerService.pdf;

  private editorPdfGenerationProfilesService = inject(EditorPdfGenerationProfilesService);
  protected profiles = computed(() => Object.values(this.editorPdfGenerationProfilesService.profiles()));
  protected selectedProfile = this.editorPdfGenerationProfilesService.selectedProfile;

  protected regeneratePdf() {
    this.editorPdfViewerService.regenerateAndDisplayGeneratedPdf();
  }

  protected async changeProfile(newProfile: EditorPdfGenerationProfile | undefined) {
    this.editorPdfGenerationProfilesService.selectProfile(newProfile?.id);
    this.regeneratePdf();
  }
}
