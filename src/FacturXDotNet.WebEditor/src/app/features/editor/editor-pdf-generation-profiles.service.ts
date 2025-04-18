import { Injectable, Signal, signal, WritableSignal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class EditorPdfGenerationProfilesService {
  private static readonly profilesLocalStorageKey = 'pdf-profiles';
  private static readonly selectedProfileLocalStorageKey = 'selected-profiles';

  get profiles() {
    return this.profilesInternal.asReadonly();
  }

  private readonly profilesInternal: WritableSignal<Record<string, EditorPdfGenerationProfile>>;

  get selectedProfile() {
    return this.selectedProfileInternal.asReadonly();
  }

  private readonly selectedProfileInternal: WritableSignal<EditorPdfGenerationProfile | undefined>;

  constructor() {
    this.profilesInternal = signal(this.loadProfiles() ?? {});

    const selectedProfileId = this.loadSelectedProfile();
    const selectedProfile = selectedProfileId === undefined ? undefined : this.getProfile(selectedProfileId);
    this.selectedProfileInternal = signal(selectedProfile);
  }

  getProfile(profileId: string) {
    const profiles = this.profilesInternal();
    return profiles[profileId];
  }

  createProfile(profile: EditorPdfGenerationProfileData) {
    const newId = idGenerator();
    const newProfile = { ...profile, id: newId };
    const profiles = this.profilesInternal();
    const newProfiles = { [newId]: newProfile, ...profiles };

    this.saveProfiles(newProfiles);
    this.profilesInternal.set(newProfiles);

    this.saveSelectedProfile(newId);
    this.selectedProfileInternal.set(newProfile);
  }

  updateProfile(profileId: string, value: EditorPdfGenerationProfileData) {
    const profiles = this.profilesInternal();
    if (profiles[profileId] === undefined) {
      throw new Error(`Could not find profile ${profileId}`);
    }

    const newProfiles = { ...profiles };
    newProfiles[profileId] = { id: profileId, ...value };

    this.saveProfiles(newProfiles);
    this.profilesInternal.set(newProfiles);
  }

  deleteProfile(profileId: string) {
    const profiles = this.profilesInternal();

    const newProfiles = { ...profiles };
    delete newProfiles[profileId];

    this.saveProfiles(newProfiles);
    this.profilesInternal.set(newProfiles);
  }

  selectProfile(profileId: string) {
    const profiles = this.profilesInternal();
    const profile = profiles[profileId];
    if (profile === undefined) {
      throw new Error(`Could not find profile ${profileId}`);
    }

    this.saveSelectedProfile(profile.id);
    this.selectedProfileInternal.set(profile);
  }

  private saveProfiles(profiles: Record<string, EditorPdfGenerationProfile>): void {
    localStorage.setItem(EditorPdfGenerationProfilesService.profilesLocalStorageKey, JSON.stringify(profiles));
  }

  private loadProfiles(): Record<string, EditorPdfGenerationProfile> | undefined {
    const serialized = localStorage.getItem(EditorPdfGenerationProfilesService.profilesLocalStorageKey);
    if (serialized === null) {
      return undefined;
    }

    return JSON.parse(serialized) as Record<string, EditorPdfGenerationProfile>;
  }

  private saveSelectedProfile(selectedProfile: string): void {
    localStorage.setItem(EditorPdfGenerationProfilesService.selectedProfileLocalStorageKey, selectedProfile);
  }

  private loadSelectedProfile(): string | undefined {
    const serialized = localStorage.getItem(EditorPdfGenerationProfilesService.selectedProfileLocalStorageKey);
    if (serialized === null) {
      return undefined;
    }

    return serialized;
  }
}

export interface EditorPdfGenerationProfileData {
  readonly name: string;
}

export type EditorPdfGenerationProfile = { readonly id: string } & EditorPdfGenerationProfileData;

interface LocalStorageData {
  readonly profiles: Record<string, EditorPdfGenerationProfile>;
  readonly selectedProfileId?: string;
}

function idGenerator() {
  return Math.random().toString(36).substring(2);
}
