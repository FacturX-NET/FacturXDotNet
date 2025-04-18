import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class EditorPdfGenerationProfilesService {
  get profiles() {
    return this.profilesInternal.asReadonly();
  }
  private profilesInternal = signal<EditorPdfGenerationProfile[]>([
    { id: 'default', name: 'Default' },
    { id: 'other1', name: 'Other' },
    { id: 'other2', name: 'Other' },
  ]);

  get selectedProfile() {
    return this.selectedProfileInternal.asReadonly();
  }
  private selectedProfileInternal = signal<EditorPdfGenerationProfile | undefined>(undefined);

  createProfile(profile: EditorPdfGenerationProfile) {
    const newProfile = { ...profile, id: idGenerator() };
    const profiles = this.profilesInternal();
    this.profilesInternal.set([...profiles, newProfile]);
  }

  selectProfile(profileId: string) {
    const profiles = this.profilesInternal();
    const profile = profiles.find((p) => p.id === profileId);
    if (profile === undefined) {
      throw new Error(`Could not find profile ${profileId}`);
    }

    this.selectedProfileInternal.set(profile);
  }
}

export interface EditorPdfGenerationProfile {
  id: string;
  name: string;
}

function idGenerator() {
  return Math.random().toString(36).substring(2);
}
