import { Injectable, resource, Resource } from '@angular/core';
import { CrossIndustryInvoice, ICrossIndustryInvoice } from '../../../core/api/api.models';
import { IDBPDatabase, IDBPTransaction, openDB, StoreNames } from 'idb';

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

  async update(value: EditorSavedState): Promise<void> {
    await this.writeSavedState('last', value);
    this.savedState.reload();
  }

  async clear(): Promise<void> {
    await this.clearSavedState('last');
  }

  private async loadSavedState(key: string): Promise<EditorSavedState | null> {
    const db = await this.openDb();
    const savedState = await db.get(this.storeName, key);

    if (savedState === undefined || savedState === null) {
      return null;
    }

    return savedState as EditorSavedState;
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
      upgrade: (
        database: IDBPDatabase<unknown>,
        oldVersion: number,
        newVersion: number | null,
        transaction: IDBPTransaction<unknown, StoreNames<unknown>[], 'versionchange'>,
        event: IDBVersionChangeEvent,
      ) => database.createObjectStore(this.storeName),
    });
  }
}

export interface EditorSavedState {
  name: string;
  cii: ICrossIndustryInvoice;
  autoGeneratePdf: boolean;
  pdf?: Blob;
}
