import { Component, inject, input } from '@angular/core';
import { CiiMenuDetailsDropdownComponent } from './cii-menu-details-dropdown.component';
import { EditorSettings, EditorSettingsService } from '../../../services/editor-settings.service';
import { CiiFormService } from '../cii-form/cii-form.service';
import { NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { ToastService } from '../../../../../core/toasts/toast.service';

@Component({
  selector: 'app-cii-menu',
  imports: [CiiMenuDetailsDropdownComponent, NgbTooltip],
  template: `
    <div class="d-flex flex-column align-items-center py-2">
      @if (settings().foldSummary || forceFold()) {
        @if (!forceFold()) {
          <button class="btn btn-link" (click)="unfoldSummary()" ngbTooltip="Unfold summary">
            <i class="bi bi-chevron-right"></i>
          </button>
        }

        <button class="btn btn-link" data-bs-toggle="offcanvas" data-bs-target="#editor__cii-summary--offcanvas">
          <i class="bi bi-body-text"></i>
        </button>
      } @else {
        @if (!forceFold()) {
          <button class="btn btn-link" (click)="foldSummary()" ngbTooltip="Fold summary">
            <i class="bi bi-chevron-left"></i>
          </button>
        }
      }

      <app-cii-menu-details-dropdown [settings]="settings()" />

      <button class="btn btn-link" (click)="validate()" ngbTooltip="Validate">
        @if (validating()) {
          <div class="spinner-border spinner-border-sm" role="status">
            <span class="visually-hidden">Loading...</span>
          </div>
        } @else {
          <i class="bi bi-check-all"></i>
        }
      </button>
    </div>
  `,
  styles: `
    :host {
      --bs-navbar-toggler-icon-bg: url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 30 30'%3e%3cpath stroke='rgba%2833, 37, 41, 0.75%29' stroke-linecap='round' stroke-miterlimit='10' stroke-width='2' d='M4 7h22M4 15h22M4 23h22'/%3e%3c/svg%3e");
    }
  `,
})
export class CiiMenuComponent {
  settings = input.required<EditorSettings>();
  forceFold = input<boolean>(false);

  private editorSettingsService = inject(EditorSettingsService);
  private ciiFormService = inject(CiiFormService);
  private toastService = inject(ToastService);

  protected validating = this.ciiFormService.validating;

  foldSummary() {
    this.editorSettingsService.saveFoldSummary(true);
  }

  unfoldSummary() {
    this.editorSettingsService.saveFoldSummary(false);
  }

  async validate() {
    try {
      const result = await this.ciiFormService.validate();
      if (result.valid) {
        this.toastService.show({ message: 'The Cross-Industry Invoice data is valid.', type: 'success' });
      } else {
        this.toastService.show({ message: 'The Cross-Industry Invoice data is invalid.', type: 'error' });
      }
    } catch (error: unknown) {
      this.toastService.showError(error, (message) => `Validation attempt failed: ${message}.`);
    }
  }
}
