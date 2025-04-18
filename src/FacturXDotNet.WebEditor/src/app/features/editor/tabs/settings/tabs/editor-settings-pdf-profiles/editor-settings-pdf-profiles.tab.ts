import { Component, inject } from '@angular/core';
import { EditorPdfViewerService } from '../../../../editor-pdf-viewer/editor-pdf-viewer.service';
import { EditorPdfGenerationProfile, EditorPdfGenerationProfilesService } from '../../../../editor-pdf-generation-profiles.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-editor-settings-pdf-profiles',
  imports: [RouterLink],
  template: `
    <div class="d-flex align-items-start justify-content-between">
      <h4><i class="bi bi-file-pdf"></i> PDF Profiles</h4>
      @if (profiles().length > 0) {
        <button class="btn btn-sm btn-outline-success">New profile</button>
      }
    </div>
    <hr class="mt-0" />
    @if (profiles().length > 0) {
      <p class="small text-body-secondary">
        Manage how your PDFs are generated and styled. Create and customize profiles with your own logo, translations, fonts, and colors to match your brand or language
        preferences. Save multiple profiles to quickly switch between different layouts or presentation styles.
      </p>
      <div class="list-group">
        @for (profile of profiles(); track profile.id) {
          <div class="list-group-item">
            <div class="d-flex align-items-center py-2">
              <a [routerLink]="profile.id">
                {{ profile.name }}
              </a>
              <div class="flex-grow-1"><!-- spacer --></div>
              @if (selectedProfile() !== undefined && selectedProfile()?.id === profile.id) {
                <button class="btn btn-sm btn-outline-success ms-2" disabled>Selected</button>
              } @else {
                <button class="btn btn-sm btn-outline-secondary ms-2" (click)="selectProfile(profile)">Select</button>
              }
            </div>
          </div>
        }
      </div>
    } @else {
      <div class="card">
        <div class="card-body text-body-secondary text-center">
          <p class="fw-semibold">Create your first PDF profile to customize appearance and content.</p>
          <p>
            Manage how your PDFs are generated and styled. Create and customize profiles with your own logo, translations, fonts, and colors to match your brand or language
            preferences. Save multiple profiles to quickly switch between different layouts or presentation styles.
          </p>
          <button class="btn btn-sm btn-outline-success">Create a PDF profile</button>
        </div>
      </div>
    }
  `,
  styles: ``,
})
export class EditorSettingsPdfProfilesTab {
  private editorPdfGenerationProfilesService = inject(EditorPdfGenerationProfilesService);
  private editorPdfViewerService = inject(EditorPdfViewerService);

  protected profiles = this.editorPdfGenerationProfilesService.profiles;
  protected selectedProfile = this.editorPdfGenerationProfilesService.selectedProfile;

  protected selectProfile(profile: EditorPdfGenerationProfile) {
    this.editorPdfGenerationProfilesService.selectProfile(profile.id);
    this.regeneratePdf();
  }

  protected regeneratePdf() {
    this.editorPdfViewerService.regenerateAndDisplayStandardPdf();
  }
}
