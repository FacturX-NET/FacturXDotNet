import { Component, inject, input } from '@angular/core';
import { ControlContainer, FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ImportFileService } from '../../../../../../../core/import-file/import-file.service';

@Component({
  selector: 'app-editor-settings-general-form',
  viewProviders: [
    {
      provide: ControlContainer,
      useFactory: () => {
        return inject(ControlContainer, { skipSelf: true });
      },
    },
  ],
  imports: [ReactiveFormsModule],
  template: `
    <div class="editor__control mb-3">
      <label class="form-label fw-semibold" for="editor-settings-profile-name">Name</label>
      <input class="form-control" id="editor-settings-profile-name" formControlName="name" />
      <p class="form-text">The profile name is for your reference only and won’t appear in the generated PDF.</p>
    </div>

    <div class="editor__control mb-3">
      <label class="form-label fw-semibold" for="editor-settings-profile-logo">Logo</label>
      <div>
        @if (formGroup().controls['logoBase64'].value) {
          <div class="mb-2">
            <img class="img-thumbnail" [src]="formGroup().controls['logoBase64'].value" alt="Logo" />
          </div>
          <div class="d-flex flex-wrap gap-2">
            <button role="button" class="btn btn-light border" (click)="chooseLogo()">Change logo</button>
            <button role="button" class="btn btn-light border" (click)="removeLogo()">Remove logo</button>
          </div>
        } @else {
          <button role="button" class="btn btn-light border" (click)="chooseLogo()">Upload logo</button>
        }
      </div>
      <p class="form-text">Upload your logo to display it in the top-left corner of the generated invoice.</p>
    </div>

    <div class="mb-3">
      <label class="form-label fw-semibold" for="editor-settings-profile-footer">Footer</label>
      <textarea class="form-control" id="editor-settings-profile-footer" formControlName="footer"></textarea>
      <p class="form-text">
        Enter the text you want to appear at the bottom of the invoice. This is useful for adding legal notices, contact information, or any other custom message.
      </p>
    </div>
  `,
  styles: ``,
})
export class EditorSettingsGeneralFormComponent {
  public formGroup = input.required<FormGroup>();

  private importFileService = inject(ImportFileService);

  protected async chooseLogo(): Promise<void> {
    const logoFile = await this.importFileService.importFile('image/*');
    if (!logoFile) {
      return;
    }

    const data = await toBase64(logoFile);
    this.formGroup().patchValue({ logoBase64: data });
  }

  protected removeLogo() {
    this.formGroup().patchValue({ logoBase64: undefined });
  }
}

const toBase64 = (file: File): Promise<string> =>
  new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result as string);
    reader.onerror = reject;
  });
