import { inject, Injectable, resource, Resource, signal, Signal } from '@angular/core';
import { IDBPDatabase, IDBPTransaction, openDB, StoreNames } from 'idb';
import { ICrossIndustryInvoice, IXmpMetadata } from '../../core/api/api.models';
import { GenerateApi } from '../../core/api/generate.api';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class EditorStateService {
  savedState: Resource<EditorSavedState | null>;

  private indexedDbName = 'cii-editor';
  private storeName = 'saved-state';

  constructor() {
    this.savedState = resource({
      loader: () => this.loadSavedState('last'),
      defaultValue: null,
    });
  }

  async new({ name, xmp, cii, pdf, attachments }: NewEditorStateArgs): Promise<void> {
    const state: EditorSavedState = {
      name,
      xmp,
      cii: cii ?? {},
      pdf:
        pdf === undefined
          ? undefined
          : {
              id: this.idGenerator(),
              content: pdf,
            },
      attachments: attachments ?? [],
    };

    await this.saveLast(state);
  }

  async updateName(name: string): Promise<void> {
    const currentState = this.savedState.value();
    if (!currentState) {
      throw new Error('No saved state found to update.');
    }

    currentState.name = name;
    await this.saveLast(currentState);
  }

  async updateCii(cii: ICrossIndustryInvoice): Promise<void> {
    const currentState = this.savedState.value();
    if (!currentState) {
      throw new Error('No saved state found to update.');
    }

    const newState = { ...currentState, cii };
    await this.saveLast(newState);
  }

  async updateXmp(xmp: IXmpMetadata | undefined): Promise<void> {
    const currentState = this.savedState.value();
    if (!currentState) {
      throw new Error('No saved state found to update.');
    }

    const newState = { ...currentState, xmp };
    await this.saveLast(newState);
  }

  async updatePdf(pdf: Blob): Promise<void> {
    const currentState = this.savedState.value();
    if (!currentState) {
      throw new Error('No saved state found to update.');
    }

    const newState = {
      ...currentState,
      pdf: {
        id: this.idGenerator(),
        content: pdf,
      },
    };

    await this.saveLast(newState);
  }

  async updatePdfAndAttachments(pdf: Blob, attachments: EditorStateAttachment[]): Promise<void> {
    const currentState = this.savedState.value();
    if (!currentState) {
      throw new Error('No saved state found to update.');
    }

    const newState = {
      ...currentState,
      pdf: {
        id: this.idGenerator(),
        content: pdf,
      },
      attachments,
    };

    await this.saveLast(newState);
  }

  async updateAttachments(attachments: EditorStateAttachment[]): Promise<void> {
    const currentState = this.savedState.value();
    if (!currentState) {
      throw new Error('No saved state found to update.');
    }

    const newState = { ...currentState, attachments };
    await this.saveLast(newState);
  }

  async clear(): Promise<void> {
    await this.clearSavedState('last');
    this.savedState.reload();
  }

  private async loadSavedState(key: string): Promise<EditorSavedState | null> {
    const db = await this.openDb();
    const savedState = await db.get(this.storeName, key);

    if (savedState === undefined || savedState === null) {
      return null;
    }

    return savedState as EditorSavedState;
  }

  private async saveLast(state: EditorSavedState) {
    await this.writeSavedState('last', state);
    this.savedState.reload();
  }

  private async writeSavedState(key: string, savedState: EditorSavedState): Promise<void> {
    const db = await this.openDb();
    await db.put(this.storeName, savedState, key);
  }

  private async clearSavedState(key: string): Promise<void> {
    const db = await this.openDb();
    await db.delete(this.storeName, key);
  }

  private async openDb(): Promise<IDBPDatabase> {
    return await openDB(this.indexedDbName, 1, {
      upgrade: (database: IDBPDatabase) => database.createObjectStore(this.storeName),
    });
  }

  private idGenerator() {
    return Math.random().toString(36).substring(2);
  }
}

export interface NewEditorStateArgs {
  name: string;
  xmp?: IXmpMetadata;
  cii?: ICrossIndustryInvoice;
  pdf?: Blob | undefined;
  attachments?: EditorStateAttachment[];
}

export interface EditorSavedState {
  name: string;
  xmp?: IXmpMetadata;
  cii: ICrossIndustryInvoice;
  pdf?: {
    id?: string;
    content: Blob;
  };
  attachments: EditorStateAttachment[];
}

export interface EditorStateAttachment {
  name: string;
  description?: string;
  content: Uint8Array;
}
