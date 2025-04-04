import { Component, ElementRef, inject, viewChild } from '@angular/core';
import { firstValueFrom, Subject } from 'rxjs';
import { ImportFileService } from './import-file.service';

@Component({
  selector: 'app-import-file',
  imports: [],
  template: ` <input #input class="d-none" type="file" name="import-file" (change)="handleChange($event)" (cancel)="handleCancel($event)" /> `,
  styles: ``,
})
export class ImportFileComponent {
  private inputElement = viewChild<ElementRef>('input');
  private fileSubject = new Subject<File | undefined>();

  constructor() {
    const importFileService = inject(ImportFileService);
    importFileService.registerImportFileComponent(this);
  }

  importFile(...accept: string[]): Promise<File | undefined> {
    const input = this.inputElement()?.nativeElement as HTMLInputElement;
    if (input == null) {
      return Promise.reject(new Error('Internal error: input field not found.'));
    }

    input.accept = accept.join(',');
    input.click();
    return firstValueFrom(this.fileSubject);
  }

  protected handleChange(_: Event) {
    const input = this.inputElement()?.nativeElement as HTMLInputElement;
    if (input == null) {
      return;
    }

    const file: File | undefined = input.files?.[0];
    this.fileSubject.next(file);
  }

  protected handleCancel(_: Event) {
    this.fileSubject.next(undefined);
  }
}
