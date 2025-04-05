import { Component, inject, input } from '@angular/core';
import { CiiMenuDetailsDropdownComponent } from './cii-menu-details-dropdown.component';
import { EditorSettings, EditorSettingsService } from '../../../editor-settings.service';
import { CiiMenuFoldComponent } from './cii-menu-fold.component';

@Component({
  selector: 'app-cii-menu',
  imports: [CiiMenuDetailsDropdownComponent, CiiMenuFoldComponent],
  template: `
    <div class="d-flex flex-column align-items-center py-2">
      <app-cii-menu-fold [settings]="settings()" />

      @if (settings().foldSummary) {
        <button class="btn btn-link" data-bs-toggle="offcanvas" data-bs-target="#editor__cii-summary">
          <span class="navbar-toggler-icon"></span>
        </button>
      }

      <app-cii-menu-details-dropdown [settings]="settings()" />
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
}
