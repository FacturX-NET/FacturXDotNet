import { Component, DestroyRef, inject, input, model, output } from '@angular/core';
import { finalize } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { EditorMenuService } from './components/editor-menu/editor-menu.service';
import { EditorSavedState } from './editor-state.service';
import { ToastService } from '../../core/toasts/toast.service';
import { toastError } from '../../core/utils/toast-error';
import { EditorSettings } from './editor-settings.service';

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
            <a class="nav-link" [class.active]="tab() === 'xmp'" role="button"> XMP Metadata </a>
          </li>
          <li class="nav-item">
            <a class="nav-link" [class.active]="tab() === 'cii'" role="button"> Cross-Industry Invoice </a>
          </li>
          <li class="nav-item">
            <a class="nav-link" [class.active]="tab() === 'attachments'" role="button"> Attachments </a>
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
}

export type EditorTab = 'xmp' | 'cii' | 'attachments';
