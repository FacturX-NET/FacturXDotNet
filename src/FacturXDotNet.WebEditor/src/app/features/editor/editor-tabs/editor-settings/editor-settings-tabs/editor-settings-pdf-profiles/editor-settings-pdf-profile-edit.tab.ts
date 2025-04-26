import { Component, computed, effect, inject, input, viewChild } from '@angular/core';
import { EditorSettingsPdfProfileFormComponent } from './components/editor-settings-pdf-profile-form.component';
import { Router, RouterLink } from '@angular/router';
import { EditorPdfGenerationProfilesService } from '../../../../services/editor-pdf-generation-profiles.service';
import { ToastService } from '../../../../../../core/toasts/toast.service';
import { EditorPdfViewerService } from '../../../editor-pdf-viewer/editor-pdf-viewer.service';
import { NgbDropdown, NgbDropdownItem, NgbDropdownMenu, NgbDropdownToggle } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-editor-settings-pdf-profile-edit',
  imports: [EditorSettingsPdfProfileFormComponent, RouterLink, NgbDropdown, NgbDropdownMenu, NgbDropdownToggle, NgbDropdownItem],
  template: `
    <div class="sticky-top pt-2 bg-body">
      <div class="d-flex align-items-start justify-content-between">
        <h4>
          <a routerLink="/settings/profiles"><i class="bi bi-file-pdf"></i> PDF Profiles</a> / {{ profile().name }}
        </h4>
        <div class="d-flex align-items-start gap-2">
          <button class="btn btn-sm btn-light border" (click)="preview()">Preview</button>

          <div ngbDropdown>
            <button id="editor-settings-profile-edit-menu" class="btn btn-sm btn-light border hide-toggle" ngbDropdownToggle><i class="bi bi-three-dots"></i></button>
            <div ngbDropdownMenu aria-labelledby="editor-settings-profile-edit-menu">
              <button class="text-danger" (click)="delete()" ngbDropdownItem><i class="bi bi-trash"></i> Delete profile</button>
            </div>
          </div>
        </div>
      </div>
      <div class="border-top mb-3"></div>
    </div>

    <app-editor-settings-pdf-profile-form [confirmationTpl]="confirmation" #profileForm>
      <ng-template #confirmation>
        <div class="d-flex gap-2">
          <button class="btn btn-outline-success mt-3" (click)="save()" [disabled]="!profileForm.hasChanges()">Save changes</button>
          <button class="btn btn-light border mt-3" (click)="revertChanges()">Revert changes</button>
        </div>
      </ng-template>
    </app-editor-settings-pdf-profile-form>
  `,
  styles: `
    .hide-toggle::after {
      content: none;
    }
  `,
})
export class EditorSettingsPdfProfileEditTab {
  profileId = input.required<string>();

  private editorPdfViewerService = inject(EditorPdfViewerService);
  private editorPdfGenerationProfilesService = inject(EditorPdfGenerationProfilesService);
  private toastService = inject(ToastService);
  private router = inject(Router);

  private form = viewChild(EditorSettingsPdfProfileFormComponent);

  protected profile = computed(() => this.editorPdfGenerationProfilesService.getProfile(this.profileId()));

  constructor() {
    effect(() => {
      const form = this.form();
      if (form === undefined || form === null) {
        return;
      }

      const profile = this.profile();
      if (profile === undefined || profile === null) {
        return;
      }

      form.setValue(profile);
    });
  }

  protected async save() {
    const form = this.form();
    if (form === undefined || form === null) {
      return;
    }

    try {
      const value = form.getValue();
      this.editorPdfGenerationProfilesService.updateProfile(this.profileId(), value);
      this.editorPdfViewerService.regenerateAndDisplayGeneratedPdf(value);
    } catch (error) {
      this.toastService.showError(error);
      throw error;
    }

    this.toastService.show({ type: 'success', message: 'Profile saved successfully.' });
  }

  protected async revertChanges() {
    const form = this.form();
    if (form === undefined) {
      return;
    }

    const profile = this.profile();
    form.setValue(profile);

    try {
      const value = form.getValue();
      this.editorPdfViewerService.regenerateAndDisplayGeneratedPdf(value);
    } catch (error) {
      this.toastService.showError(error);
      throw error;
    }

    this.toastService.show({ type: 'info', message: 'The values have been reverted to the original values. No changes have been saved.' });
  }

  protected async delete() {
    this.editorPdfGenerationProfilesService.deleteProfile(this.profileId());
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
}
