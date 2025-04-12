import { Injectable, Signal, signal, WritableSignal } from '@angular/core';
import { EditorTab } from './editor-header.component';

@Injectable({
  providedIn: 'root',
})
export class EditorSettingsService {
  private localStorageKey: string = 'editor-settings';
  private defaultSettings: EditorSettings = {
    tab: 'cii',
    foldSummary: false,
    showBusinessRules: true,
    showRemarks: true,
    showChorusProRemarks: true,
  };

  get settings(): Signal<EditorSettings> {
    return this.settingsInternal.asReadonly();
  }
  private settingsInternal: WritableSignal<EditorSettings>;

  constructor() {
    this.settingsInternal = signal<EditorSettings>(this.loadSettings());
  }

  saveFoldSummary(value: boolean) {
    const settings = this.settings();
    const newSettings: EditorSettings = { ...settings, foldSummary: value };
    this.saveSettings(newSettings);
    this.settingsInternal.set(newSettings);
  }

  saveTab(value: EditorTab) {
    const settings = this.settings();
    const newSettings: EditorSettings = { ...settings, tab: value };
    this.saveSettings(newSettings);
    this.settingsInternal.set(newSettings);
  }

  saveShowBusinessRules(value: boolean) {
    const settings = this.settings();
    const newSettings: EditorSettings = { ...settings, showBusinessRules: value };
    this.saveSettings(newSettings);
    this.settingsInternal.set(newSettings);
  }

  saveShowRemarks(value: boolean) {
    const settings = this.settings();
    const newSettings: EditorSettings = { ...settings, showRemarks: value };
    this.saveSettings(newSettings);
    this.settingsInternal.set(newSettings);
  }

  saveShowChorusProRemarks(value: boolean) {
    const settings = this.settings();
    const newSettings: EditorSettings = { ...settings, showChorusProRemarks: value };
    this.saveSettings(newSettings);
    this.settingsInternal.set(newSettings);
  }

  private saveSettings(settings: EditorSettings): void {
    const jsonSettings = JSON.stringify(settings);
    localStorage.setItem(this.localStorageKey, jsonSettings);
  }

  private loadSettings(): EditorSettings {
    const jsonSettings = localStorage.getItem(this.localStorageKey);
    return jsonSettings == null ? this.defaultSettings : (JSON.parse(jsonSettings) as EditorSettings);
  }
}

export interface EditorSettings {
  readonly foldSummary?: boolean;
  readonly tab?: EditorTab;
  readonly showBusinessRules?: boolean;
  readonly showRemarks?: boolean;
  readonly showChorusProRemarks?: boolean;
}
