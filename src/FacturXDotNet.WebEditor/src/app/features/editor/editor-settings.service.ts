import { Injectable, signal } from '@angular/core';
import { CrossIndustryInvoiceFormVerbosity } from './cii-form.component';

@Injectable({
  providedIn: 'root',
})
export class EditorSettingsService {
  private localStorageKey: string = 'editor-settings';

  settings = signal<EditorSettings>({});

  constructor() {
    this.settings.set(this.loadSettings());
  }

  saveVerbosity(verbosity: CrossIndustryInvoiceFormVerbosity) {
    const settings = this.settings();
    const newSettings: EditorSettings = { ...settings, ciiFormVerbosity: verbosity };
    this.saveSettings(newSettings);
    this.settings.set(newSettings);
  }

  private saveSettings(settings: EditorSettings): void {
    const jsonSettings = JSON.stringify(settings);
    localStorage.setItem(this.localStorageKey, jsonSettings);
  }

  private loadSettings(): EditorSettings {
    const jsonSettings = localStorage.getItem(this.localStorageKey);
    return jsonSettings == null ? {} : (JSON.parse(jsonSettings) as EditorSettings);
  }
}

export interface EditorSettings {
  readonly ciiFormVerbosity?: CrossIndustryInvoiceFormVerbosity;
}
