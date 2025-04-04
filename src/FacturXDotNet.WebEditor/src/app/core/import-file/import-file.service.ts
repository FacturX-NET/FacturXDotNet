import { Injectable } from '@angular/core';
import { ImportFileComponent } from './import-file.component';

@Injectable({
  providedIn: 'root',
})
export class ImportFileService {
  private importFileComponentInstance: ImportFileComponent | undefined;

  importFile(...accept: string[]): Promise<File | undefined> {
    if (this.importFileComponentInstance === undefined) {
      return Promise.reject(new Error('Internal error: ImportFileComponent instance not found.'));
    }

    return this.importFileComponentInstance.importFile(...accept);
  }

  registerImportFileComponent(instance: ImportFileComponent): void {
    this.importFileComponentInstance = instance;
  }
}
