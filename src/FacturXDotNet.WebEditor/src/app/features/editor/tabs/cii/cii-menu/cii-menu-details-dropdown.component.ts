import { Component, inject, input } from '@angular/core';
import { EditorSettings, EditorSettingsService } from '../../../editor-settings.service';
import { NgbDropdown, NgbDropdownItem, NgbDropdownMenu, NgbDropdownToggle, NgbTooltip } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-cii-menu-details-dropdown',
  imports: [NgbDropdown, NgbDropdownMenu, NgbDropdownToggle, NgbDropdownItem, NgbTooltip],
  template: `
    @if (settings(); as settings) {
      <div ngbDropdown ngbTooltip="Display settings">
        <button id="cii-menu-details" class="btn btn-link hide-toggle" ngbDropdownToggle><i class="bi bi-eye"></i></button>
        <div ngbDropdownMenu aria-labelledby="cii-menu-details">
          <button (click)="toggleBusinessRules()" [class.text-body-tertiary]="settings.showBusinessRules !== true" ngbDropdownItem>
            @if (settings.showBusinessRules === true) {
              <i class="bi bi-eye"></i>
            } @else {
              <i class="bi bi-eye-slash"></i>
            }
            Business Rules
          </button>
          <button (click)="toggleRemarks()" [class.text-body-tertiary]="settings.showRemarks !== true" ngbDropdownItem>
            @if (settings.showRemarks === true) {
              <i class="bi bi-eye"></i>
            } @else {
              <i class="bi bi-eye-slash"></i>
            }
            Remarks
          </button>
          <button (click)="toggleChorusProRemarks()" [class.text-body-tertiary]="settings.showChorusProRemarks !== true" ngbDropdownItem>
            @if (settings.showChorusProRemarks === true) {
              <i class="bi bi-eye"></i>
            } @else {
              <i class="bi bi-eye-slash"></i>
            }
            Chorus Pro Remarks
          </button>
        </div>
      </div>
    }
  `,
  styles: `
    .hide-toggle::after {
      content: none;
    }
  `,
})
export class CiiMenuDetailsDropdownComponent {
  public settings = input.required<EditorSettings>();

  private editorSettingsService = inject(EditorSettingsService);

  protected toggleBusinessRules() {
    const currentValue = this.settings()?.showBusinessRules == true;
    this.editorSettingsService.saveShowBusinessRules(!currentValue);
  }

  protected toggleRemarks() {
    const currentValue = this.settings()?.showRemarks == true;
    this.editorSettingsService.saveShowRemarks(!currentValue);
  }

  protected toggleChorusProRemarks() {
    const currentValue = this.settings()?.showChorusProRemarks == true;
    this.editorSettingsService.saveShowChorusProRemarks(!currentValue);
  }
}
