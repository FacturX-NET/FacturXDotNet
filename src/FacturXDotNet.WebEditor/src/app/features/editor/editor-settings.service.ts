import { Injectable, signal } from '@angular/core';
import { EditorTab } from './editor-header.component';

@Injectable({
  providedIn: 'root',
})
export class EditorSettingsService {
  private localStorageKey: string = 'editor-settings';

  settings = signal<EditorSettings>({});

  constructor() {
    this.settings.set(this.loadSettings());
  }

  saveFoldSummary(value: boolean) {
    const settings = this.settings();
    const newSettings: EditorSettings = { ...settings, foldSummary: value };
    this.saveSettings(newSettings);
    this.settings.set(newSettings);
  }

  saveTab(value: EditorTab) {
    const settings = this.settings();
    const newSettings: EditorSettings = { ...settings, tab: value };
    this.saveSettings(newSettings);
    this.settings.set(newSettings);
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

  saveRightPaneWidth(value: number) {
    const settings = this.settings();
    const newSettings: EditorSettings = { ...settings, rightPaneWidth: value };
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
  readonly foldSummary?: boolean;
  readonly tab?: EditorTab;
  readonly showBusinessRules?: boolean;
  readonly showRemarks?: boolean;
  readonly showChorusProRemarks?: boolean;
  readonly rightPaneWidth?: number;
}
