import { Component, inject, input, model } from '@angular/core';
import { EditorSavedState } from '../editor-state.service';
import { EditorSettings } from '../editor-settings.service';
import { CiiFormService } from '../tabs/cii/cii-form/cii-form.service';
import { EditorHeaderNameComponent } from './editor-header-name.component';

@Component({
  selector: 'app-editor-header',
  template: `
    <div>
      <div class="px-3 pt-3 pb-2">
        <app-editor-header-name [name]="state().name" />
      </div>

      <div>
        <ul class="nav nav-tabs ps-4">
          <li class="nav-item">
            <a class="nav-link" role="button" [class.active]="tab() === 'xmp'" (click)="tab.set('xmp')"> <i class="bi bi-code"></i> XMP Metadata </a>
          </li>
          <li class="nav-item">
            <a class="nav-link" role="button" [class.active]="tab() === 'cii'" (click)="tab.set('cii')">
              @if (ciiIsInvalid) {
                <i class="bi bi-x-circle-fill text-danger"></i>
              } @else {
                <i class="bi bi-code"></i>
              }

              Cross-Industry Invoice
            </a>
          </li>
          <li class="nav-item">
            <a class="nav-link" role="button" [class.active]="tab() === 'attachments'" (click)="tab.set('attachments')">
              <i class="bi bi-paperclip"></i> Attachments ({{ state().attachments.length }})
            </a>
          </li>
        </ul>
      </div>
    </div>
  `,
  imports: [EditorHeaderNameComponent],
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
