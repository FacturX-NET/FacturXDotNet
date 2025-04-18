import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class EditorPdfGenerationProfilesService {
  get profiles() {
    return this.profilesInternal.asReadonly();
  }
  private profilesInternal = signal<Record<string, EditorPdfGenerationProfile>>({
    default: { id: 'default', name: 'Default' },
    other1: { id: 'other1', name: 'Other' },
    other2: { id: 'other2', name: 'Other' },
  });

  get selectedProfile() {
    return this.selectedProfileInternal.asReadonly();
  }
  private selectedProfileInternal = signal<EditorPdfGenerationProfile | undefined>(undefined);

  getProfile(profileId: string) {
    const profiles = this.profilesInternal();
    return profiles[profileId];
  }

  createProfile(profile: EditorPdfGenerationProfileData) {
    const newId = idGenerator();
    const newProfile = { ...profile, id: newId };
    const profiles = this.profilesInternal();
    const newProfiles = { ...profiles, [newId]: newProfile };
    this.profilesInternal.set(newProfiles);
  }

  updateProfile(profileId: string, value: EditorPdfGenerationProfileData) {
    const profiles = this.profilesInternal();
    if (profiles[profileId] === undefined) {
      throw new Error(`Could not find profile ${profileId}`);
    }

    const newProfiles = { ...profiles };
    newProfiles[profileId] = { id: profileId, ...value };
    this.profilesInternal.set(newProfiles);
  }

  deleteProfile(profileId: string) {
    const profiles = this.profilesInternal();
    const newProfiles = { ...profiles };
    delete newProfiles[profileId];
    this.profilesInternal.set(newProfiles);
  }

  selectProfile(profileId: string) {
    const profiles = this.profilesInternal();
    const profile = profiles[profileId];
    if (profile === undefined) {
      throw new Error(`Could not find profile ${profileId}`);
    }

    this.selectedProfileInternal.set(profile);
  }
}

export interface EditorPdfGenerationProfileData {
  readonly name: string;
}

export type EditorPdfGenerationProfile = { readonly id: string } & EditorPdfGenerationProfileData;

function idGenerator() {
  return Math.random().toString(36).substring(2);
}
