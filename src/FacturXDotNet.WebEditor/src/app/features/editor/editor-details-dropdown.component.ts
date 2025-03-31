import { Component, Signal } from '@angular/core';
import { EditorSettings, EditorSettingsService } from './editor-settings.service';

@Component({
  selector: 'app-editor-details-dropdown',
  imports: [],
  template: `
    @if (settings(); as settings) {
      <div class="dropdown">
        <a href="javascript:void 0;" class="dropdown-toggle small" data-bs-toggle="dropdown" aria-expanded="false"> details </a>
        <ul class="dropdown-menu">
          <li>
            <a class="dropdown-item" href="javascript:void 0;" (click)="toggleBusinessRules()" [class.text-body-tertiary]="settings.showBusinessRules !== true">
              @if (settings.showBusinessRules === true) {
                <i class="bi bi-eye"></i>
              } @else {
                <i class="bi bi-eye-slash"></i>
              }
              Business Rules
            </a>
          </li>
          <li>
            <a class="dropdown-item" href="javascript:void 0;" (click)="toggleRemarks()" [class.text-body-tertiary]="settings.showRemarks !== true">
              @if (settings.showRemarks === true) {
                <i class="bi bi-eye"></i>
              } @else {
                <i class="bi bi-eye-slash"></i>
              }
              Remarks
            </a>
          </li>
          <li>
            <a class="dropdown-item" href="javascript:void 0;" (click)="toggleChorusProRemarks()" [class.text-body-tertiary]="settings.showChorusProRemarks !== true">
              @if (settings.showChorusProRemarks === true) {
                <i class="bi bi-eye"></i>
              } @else {
                <i class="bi bi-eye-slash"></i>
              }
              Chorus Pro Remarks
            </a>
          </li>
        </ul>
      </div>
    }
  `,
  styles: ``,
})
export class EditorDetailsDropdownComponent {
  public settings: Signal<EditorSettings>;

  constructor(private settingsService: EditorSettingsService) {
    this.settings = this.settingsService.settings;
  }

  protected toggleBusinessRules() {
    const currentValue = this.settings()?.showBusinessRules == true;
    this.settingsService.saveShowBusinessRules(!currentValue);
  }

  protected toggleRemarks() {
    const currentValue = this.settings()?.showRemarks == true;
    this.settingsService.saveShowRemarks(!currentValue);
  }

  protected toggleChorusProRemarks() {
    const currentValue = this.settings()?.showChorusProRemarks == true;
    this.settingsService.saveShowChorusProRemarks(!currentValue);
  }
}
