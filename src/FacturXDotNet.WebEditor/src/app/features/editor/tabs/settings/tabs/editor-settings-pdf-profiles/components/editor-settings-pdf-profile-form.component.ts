import { Component, inject, input, model } from '@angular/core';
import { EditorPdfGenerationProfile, EditorPdfGenerationProfilesService } from '../../../../../editor-pdf-generation-profiles.service';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { tap } from 'rxjs';
import { ImportFileService } from '../../../../../../../core/import-file/import-file.service';

@Component({
  selector: 'app-editor-settings-pdf-profile-form',
  imports: [ReactiveFormsModule],
  template: `
    <form [formGroup]="formGroup">
      <div class="editor__control mb-3">
        <label class="form-label" for="editor-settings-profile-name">Name *</label>
        <input class="form-control" id="editor-settings-profile-name" formControlName="name" />
        <p class="form-text">The profile name is for your reference only and won’t appear in the generated PDF.</p>
      </div>
      <div class="editor__control mb-3">
        <label class="form-label" for="editor-settings-profile-logo">Logo</label>
        <div>
          @if (formGroup.controls.logoBase64.value) {
            <div class="mb-2">
              <img class="img-thumbnail" [src]="formGroup.controls.logoBase64.value" alt="Logo" />
            </div>
            <div class="d-flex flex-wrap gap-2">
              <button class="btn btn-outline-secondary" (click)="chooseLogo()">Change logo</button>
              <button class="btn btn-outline-secondary" (click)="removeLogo()">Remove logo</button>
            </div>
          } @else {
            <button class="btn btn-outline-secondary" (click)="chooseLogo()">Upload logo</button>
          }
        </div>
        <p class="form-text">Upload your logo to display it in the top-left corner of the generated invoice.</p>
      </div>
    </form>
  `,
  styles: ``,
})
export class EditorSettingsPdfProfileFormComponent {
  private importFileService = inject(ImportFileService);

  formGroup = new FormGroup({
    name: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    logoBase64: new FormControl<string | undefined>(undefined, { nonNullable: true }),
  });

  protected async chooseLogo(): Promise<void> {
    const logoFile = await this.importFileService.importFile('image/*');
    if (!logoFile) {
      return;
    }

    const data = await toBase64(logoFile);
    this.formGroup.patchValue({ logoBase64: data });
  }

  protected removeLogo() {
    this.formGroup.patchValue({ logoBase64: undefined });
  }
}

const toBase64 = (file: File): Promise<string> =>
  new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result as string);
    reader.onerror = reject;
  });
