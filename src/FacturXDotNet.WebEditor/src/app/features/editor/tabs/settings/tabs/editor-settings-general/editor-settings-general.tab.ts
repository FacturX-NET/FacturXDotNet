import { Component, computed, inject } from '@angular/core';
import { EditorSettingsService } from '../../../../editor-settings.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-editor-settings-general',
  imports: [FormsModule],
  template: `
    <h4><i class="bi bi-code"></i> Cross-Industry Invoice</h4>
    <div class="border-top mb-3"></div>
    <h6>Verbosity</h6>
    <p class="small text-body-secondary">
      Customize how much detail you want to see when editing fields in the Cross-Industry Invoice form. You can choose to display additional information such as business rules, and
      helpful remarks for each field. Adjust these settings to match your preferred level of guidance and complexity.
    </p>
    <div class="form-check form-switch">
      <input class="form-check-input" type="checkbox" role="switch" id="showBusinessRules" [ngModel]="showBusinessRules()" (ngModelChange)="setBusinessRules($event)" />
      <label class="form-check-label" for="showBusinessRules">Show business rules</label>
    </div>
    <div class="form-check form-switch">
      <input class="form-check-input" type="checkbox" role="switch" id="showRemarks" [ngModel]="showRemarks()" (ngModelChange)="setRemarks($event)" />
      <label class="form-check-label" for="showRemarks">Show remarks</label>
    </div>
    <div class="form-check form-switch">
      <input class="form-check-input" type="checkbox" role="switch" id="showChorusProRemarks" [ngModel]="showChorusProRemarks()" (ngModelChange)="setChorusProRemarks($event)" />
      <label class="form-check-label" for="showChorusProRemarks">Show Chorus PRO remarks</label>
    </div>
  `,
  styles: ``,
})
export class EditorSettingsGeneralTab {
  private editorSettingsService = inject(EditorSettingsService);

  protected settings = this.editorSettingsService.settings;
  protected showBusinessRules = computed(() => this.settings()?.showBusinessRules);
  protected showRemarks = computed(() => this.settings()?.showRemarks);
  protected showChorusProRemarks = computed(() => this.settings()?.showChorusProRemarks);

  protected setBusinessRules(value: boolean) {
    this.editorSettingsService.saveShowBusinessRules(value);
  }

  protected setRemarks(value: boolean) {
    this.editorSettingsService.saveShowRemarks(value);
  }

  protected setChorusProRemarks(value: boolean) {
    this.editorSettingsService.saveShowChorusProRemarks(value);
  }
}
