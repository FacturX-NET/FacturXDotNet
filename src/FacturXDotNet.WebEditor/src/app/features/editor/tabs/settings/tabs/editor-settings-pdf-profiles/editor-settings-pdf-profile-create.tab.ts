import { Component, inject, Signal, signal, viewChild } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { EditorSettingsPdfProfileFormComponent } from './components/editor-settings-pdf-profile-form.component';
import { EditorPdfGenerationProfile, EditorPdfGenerationProfilesService } from '../../../../editor-pdf-generation-profiles.service';
import { ToastService } from '../../../../../../core/toasts/toast.service';
import { EditorPdfViewerService } from '../../../../components/editor-pdf-viewer/editor-pdf-viewer.service';

@Component({
  selector: 'app-editor-settings-pdf-profile-create',
  imports: [RouterLink, EditorSettingsPdfProfileFormComponent],
  template: `
    <div class="d-flex align-items-start justify-content-between">
      <h4>
        <a routerLink="/settings/profiles"><i class="bi bi-file-pdf"></i> PDF Profiles</a> / New profile
      </h4>
      <button class="btn btn-sm btn-outline-secondary" (click)="preview()">Preview</button>
    </div>

    <h4></h4>
    <div class="border-top mb-3"></div>
    <app-editor-settings-pdf-profile-form></app-editor-settings-pdf-profile-form>
    <button class="btn btn-outline-success mt-3" (click)="create()">Create</button>
  `,
  styles: ``,
})
export class EditorSettingsPdfProfileCreateTab {
  private editorPdfViewerService = inject(EditorPdfViewerService);
  private editorPdfGenerationProfilesService = inject(EditorPdfGenerationProfilesService);
  private toastService = inject(ToastService);
  private router = inject(Router);

  private form = viewChild(EditorSettingsPdfProfileFormComponent);

  protected async create() {
    const form = this.form();
    if (form === undefined || form === null) {
      return;
    }

    form.formGroup.markAllAsTouched();
    if (form.formGroup.invalid) {
      this.toastService.show({ type: 'error', message: 'Please fill in all required fields.' });
      return;
    }

    const value = form.formGroup.getRawValue() as EditorPdfGenerationProfile;
    this.editorPdfGenerationProfilesService.createProfile(value);

    await this.router.navigate(['/settings/profiles']);
  }

  protected async preview() {
    const form = this.form();
    if (form === undefined || form === null) {
      return;
    }

    form.formGroup.markAllAsTouched();
    if (form.formGroup.invalid) {
      this.toastService.show({ type: 'error', message: 'Please fill in all required fields.' });
      return;
    }

    const value = form.formGroup.getRawValue() as EditorPdfGenerationProfile;
    this.editorPdfViewerService.regenerateAndDisplayStandardPdf(value);
  }
}
