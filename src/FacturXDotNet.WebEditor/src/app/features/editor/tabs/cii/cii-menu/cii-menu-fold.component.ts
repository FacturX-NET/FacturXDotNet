import { Component, inject, input } from '@angular/core';
import { EditorSettings, EditorSettingsService } from '../../../editor-settings.service';

@Component({
  selector: 'app-cii-menu-fold',
  imports: [],
  template: `
    @if (settings().foldSummary) {
      <button class="btn btn-link" (click)="unfoldSummary()">
        <i class="bi bi-chevron-right"></i>
      </button>
    } @else {
      <button class="btn btn-link" (click)="foldSummary()">
        <i class="bi bi-chevron-left"></i>
      </button>
    }
  `,
  styles: ``,
})
export class CiiMenuFoldComponent {
  settings = input.required<EditorSettings>();

  private editorSettingsService = inject(EditorSettingsService);

  foldSummary() {
    this.editorSettingsService.saveFoldSummary(true);
  }

  unfoldSummary() {
    this.editorSettingsService.saveFoldSummary(false);
  }
}
