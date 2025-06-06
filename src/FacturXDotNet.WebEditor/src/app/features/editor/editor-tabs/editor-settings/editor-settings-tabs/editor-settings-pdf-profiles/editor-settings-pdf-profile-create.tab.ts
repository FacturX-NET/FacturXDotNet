import { Component, inject, viewChild } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { EditorSettingsPdfProfileFormComponent } from './components/editor-settings-pdf-profile-form.component';
import { EditorPdfGenerationProfilesService } from '../../../../services/editor-pdf-generation-profiles.service';
import { ToastService } from '../../../../../../core/toasts/toast.service';
import { EditorPdfViewerService } from '../../../editor-pdf-viewer/editor-pdf-viewer.service';

@Component({
  selector: 'app-editor-settings-pdf-profile-create',
  imports: [RouterLink, EditorSettingsPdfProfileFormComponent],
  template: `
    <div class="sticky-top pt-2 bg-body">
      <div class="d-flex align-items-start justify-content-between">
        <h4>
          <a routerLink="/settings/profiles"><i class="bi bi-file-pdf"></i> PDF Profiles</a> / New profile
        </h4>
        <button class="btn btn-sm btn-light border" (click)="preview()">Preview</button>
      </div>
      <div class="border-top mb-3"></div>
    </div>

    <app-editor-settings-pdf-profile-form [confirmationTpl]="confirmation">
      <ng-template #confirmation>
        <button class="btn btn-outline-success mt-3" (click)="create()">Create</button>
      </ng-template>
    </app-editor-settings-pdf-profile-form>
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

    try {
      const value = form.getValue();
      this.editorPdfGenerationProfilesService.createProfile(value);
    } catch (error) {
      this.toastService.showError(error);
    }

    await this.router.navigate(['/settings/profiles']);
  }

  protected async preview() {
    const form = this.form();
    if (form === undefined || form === null) {
      return;
    }

    try {
      const value = form.getValue();
      this.editorPdfViewerService.regenerateAndDisplayGeneratedPdf(value);
    } catch (error) {
      this.toastService.showError(error);
    }
  }

  protected readonly confirm = confirm;
}
