import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class EditorSettingsService {
  private localStorageKey: string = 'editor-settings';

  settings = signal<EditorSettings>({});

  constructor() {
    this.settings.set(this.loadSettings());
  }

  saveShowBusinessRules(value: boolean) {
    const settings = this.settings();
    const newSettings: EditorSettings = { ...settings, showBusinessRules: value };
    this.saveSettings(newSettings);
    this.settings.set(newSettings);
  }

  saveShowRemarks(value: boolean) {
    const settings = this.settings();
    const newSettings: EditorSettings = { ...settings, showRemarks: value };
    this.saveSettings(newSettings);
    this.settings.set(newSettings);
  }

  saveShowChorusProRemarks(value: boolean) {
    const settings = this.settings();
    const newSettings: EditorSettings = { ...settings, showChorusProRemarks: value };
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
  readonly showBusinessRules?: boolean;
  readonly showRemarks?: boolean;
  readonly showChorusProRemarks?: boolean;
}
