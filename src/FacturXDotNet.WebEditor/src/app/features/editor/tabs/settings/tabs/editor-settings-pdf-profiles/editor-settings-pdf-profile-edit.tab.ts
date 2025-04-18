import { Component, computed, effect, inject, input, viewChild } from '@angular/core';
import { EditorSettingsPdfProfileFormComponent } from './components/editor-settings-pdf-profile-form.component';
import { Router, RouterLink } from '@angular/router';
import { EditorPdfGenerationProfile, EditorPdfGenerationProfilesService } from '../../../../editor-pdf-generation-profiles.service';
import { ToastService } from '../../../../../../core/toasts/toast.service';

@Component({
  selector: 'app-editor-settings-pdf-profile-edit',
  imports: [EditorSettingsPdfProfileFormComponent, RouterLink],
  template: `
    <div class="d-flex align-items-start justify-content-between">
      <h4>
        <a routerLink="/settings/profiles"><i class="bi bi-file-pdf"></i> PDF Profiles</a> / {{ profile()?.name }}
      </h4>
      <button class="btn btn-sm btn-outline-danger" (click)="delete()"><i class="bi bi-trash"></i> Delete profile</button>
    </div>
    <div class="border-top mb-3"></div>
    <app-editor-settings-pdf-profile-form></app-editor-settings-pdf-profile-form>
    <div class="d-flex gap-2">
      <button class="btn btn-outline-success mt-3" (click)="save()">Save changes</button>
      <button class="btn btn-outline-secondary mt-3" (click)="cancel()">Revert changes</button>
    </div>
  `,
  styles: ``,
})
export class EditorSettingsPdfProfileEditTab {
  profileId = input.required<string>();

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

      form.formGroup.patchValue(profile);
    });
  }

  protected async save() {
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
    this.editorPdfGenerationProfilesService.updateProfile(this.profileId(), value);

    await this.router.navigate(['/settings/profiles']);
  }

  protected async cancel() {
    await this.router.navigate(['/settings/profiles']);
  }

  protected async delete() {
    this.editorPdfGenerationProfilesService.deleteProfile(this.profileId());
    await this.router.navigate(['/settings/profiles']);
  }
}
