import { Component, input, model } from '@angular/core';
import { EditorPdfGenerationProfile, EditorPdfGenerationProfilesService } from '../../../../../editor-pdf-generation-profiles.service';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { tap } from 'rxjs';

@Component({
  selector: 'app-editor-settings-pdf-profile-form',
  imports: [ReactiveFormsModule],
  template: `
    <form [formGroup]="formGroup">
      <div class="editor__control mb-3">
        <label class="form-label" for="editor-settings-profile-name">Name *</label>
        <input class="form-control" id="editor-settings-profile-name" formControlName="name" />
      </div>
    </form>
  `,
  styles: ``,
})
export class EditorSettingsPdfProfileFormComponent {
  formGroup = new FormGroup({
    name: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
  });
}
