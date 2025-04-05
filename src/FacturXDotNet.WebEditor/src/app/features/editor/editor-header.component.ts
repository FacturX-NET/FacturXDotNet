import { Component, computed, DestroyRef, inject, input, model, output } from '@angular/core';
import { finalize } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { EditorMenuService } from './components/editor-menu/editor-menu.service';
import { EditorSavedState } from './editor-state.service';
import { ToastService } from '../../core/toasts/toast.service';
import { toastError } from '../../core/utils/toast-error';
import { EditorSettings } from './editor-settings.service';
import { CiiFormService } from './tabs/cii/cii-form/cii-form.service';

@Component({
  selector: 'app-editor-header',
  template: `
    <div>
      <div class="navbar justify-content-start gap-3 px-3 pb-0">
        <div class="navbar-brand h5">
          {{ state().name }}
        </div>
      </div>

      <div>
        <ul class="nav nav-tabs">
          <li class="nav-item">
            <a class="nav-link" [class.active]="tab() === 'xmp'" role="button"> <i class="bi bi-code"></i> XMP Metadata </a>
          </li>
          <li class="nav-item">
            <a class="nav-link" [class.active]="tab() === 'cii'" role="button">
              @if (ciiIsInvalid) {
                <i class="bi bi-x-circle-fill text-danger"></i>
              } @else {
                <i class="bi bi-code"></i>
              }

              Cross-Industry Invoice
            </a>
          </li>
          <li class="nav-item">
            <a class="nav-link" [class.active]="tab() === 'attachments'" role="button"> <i class="bi bi-paperclip"></i> Attachments ({{ state().attachments.length }}) </a>
          </li>
        </ul>
      </div>
    </div>
  `,
})
export class EditorHeaderComponent {
  state = input.required<EditorSavedState>();
  tab = model.required<EditorTab>();
  settings = input.required<EditorSettings>();

  private ciiFormService = inject(CiiFormService);

  protected get ciiIsInvalid(): boolean {
    return this.ciiFormService.form.touched && this.ciiFormService.form.invalid;
  }
}

export type EditorTab = 'xmp' | 'cii' | 'attachments';
