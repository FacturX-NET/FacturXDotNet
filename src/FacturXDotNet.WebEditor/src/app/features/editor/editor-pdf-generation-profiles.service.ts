import { computed, Injectable, signal, WritableSignal } from '@angular/core';
import { IStandardPdfGeneratorLanguagePackDto } from '../../core/api/api.models';

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

  get selectedProfileId() {
    return this.selectedProfileInternal.asReadonly();
  }

  private readonly selectedProfileInternal: WritableSignal<string | undefined>;

  selectedProfile = computed(() => {
    const selectedProfileId = this.selectedProfileId();
    if (selectedProfileId === undefined) {
      return undefined;
    }

    const profiles = this.profiles();
    return profiles[selectedProfileId];
  });

  constructor() {
    this.profilesInternal = signal(this.loadProfiles() ?? {});
    this.selectedProfileInternal = signal(this.loadSelectedProfile());
  }

  getProfile(profileId: string) {
    const profiles = this.profilesInternal();
    return profiles[profileId];
  }

  createProfile(profile: EditorPdfGenerationProfileData) {
    const newId = idGenerator();
    const newProfile = { id: newId, ...profile };
    const profiles = this.profilesInternal();
    const newProfiles = { [newId]: newProfile, ...profiles };

    this.saveProfiles(newProfiles);
    this.profilesInternal.set(newProfiles);

    this.saveSelectedProfile(newId);
    this.selectedProfileInternal.set(newId);
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

  selectProfile(profileId: string | undefined) {
    if (profileId === undefined) {
      this.saveSelectedProfile(undefined);
      this.selectedProfileInternal.set(undefined);
      return;
    }

    const profiles = this.profilesInternal();
    const profile = profiles[profileId];
    if (profile === undefined) {
      throw new Error(`Could not find profile ${profileId}`);
    }

    this.saveSelectedProfile(profile.id);
    this.selectedProfileInternal.set(profile.id);
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

  private saveSelectedProfile(selectedProfile: string | undefined): void {
    if (selectedProfile === undefined) {
      localStorage.removeItem(EditorPdfGenerationProfilesService.selectedProfileLocalStorageKey);
    } else {
      localStorage.setItem(EditorPdfGenerationProfilesService.selectedProfileLocalStorageKey, selectedProfile);
    }
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
  readonly logoBase64?: string;
  readonly footer?: string;
  readonly languagePack?: Omit<IStandardPdfGeneratorLanguagePackDto, 'documentTypeNames'> & { baseLanguagePack?: string; documentTypeNames?: Record<string, string | undefined> };
}

export type EditorPdfGenerationProfile = { readonly id: string } & EditorPdfGenerationProfileData;

function idGenerator() {
  return Math.random().toString(36).substring(2);
}
