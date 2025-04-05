import { Component, DestroyRef, inject, input } from '@angular/core';
import { EditorStateAttachment, EditorStateService } from '../../editor-state.service';
import { NgxFilesizeModule } from 'ngx-filesize';
import { ImportFileService } from '../../../../core/import-file/import-file.service';
import { filter, from, map, switchMap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { toastError } from '../../../../core/utils/toast-error';
import { ToastService } from '../../../../core/toasts/toast.service';
import { downloadBlob } from '../../../../core/utils/download-blob';
import { AttachmentComponent } from './attachment.component';

@Component({
  selector: 'app-attachments',
  template: `
    <div class="container mt-5">
      <div class="d-flex gap-2 mb-4">
        <button class="btn btn btn-outline-secondary" (click)="addAttachment()"><i class="bi bi-plus"></i> Add attachment</button>
        <div class="flex-grow-1"><!-- spacer --></div>
        <button class="btn btn btn-outline-secondary" (click)="downloadAttachments()"><i class="bi bi-download"></i> Download all</button>
        <button class="btn btn btn-outline-danger" (click)="deleteAllAttachments()"><i class="bi bi-trash"></i> Delete all</button>
      </div>
      <div class="list-group">
        @for (attachment of attachments(); track attachment.name) {
          <div class="list-group-item">
            <div class="d-flex justify-content-between align-items-start">
              <div>
                <app-attachment [attachment]="attachment" (attachmentChange)="updateAttachment($index, $event)"></app-attachment>
              </div>
              <div class="d-flex gap-2">
                <button class="btn btn btn-outline-secondary" (click)="downloadAttachment($index)"><i class="bi bi-download"></i> Download</button>
                <button class="btn btn btn-outline-danger" (click)="deleteAttachment($index)"><i class="bi bi-trash"></i> Delete</button>
              </div>
            </div>
          </div>
        }

        <div class="d-flex justify-content-center py-4"></div>
      </div>
    </div>
  `,
  imports: [NgxFilesizeModule, AttachmentComponent],
})
export class AttachmentsTab {
  attachments = input.required<EditorStateAttachment[]>();

  private editorStateService = inject(EditorStateService);
  private importFileService = inject(ImportFileService);
  private toastService = inject(ToastService);
  private destroyRef = inject(DestroyRef);

  addAttachment() {
    from(this.importFileService.importFile())
      .pipe(
        filter((file) => file !== undefined),
        switchMap((file) => {
          return from(file.arrayBuffer()).pipe(map((content) => ({ file, content })));
        }),
        switchMap((result) => {
          const attachments = this.attachments();
          const newAttachments = [...attachments, { name: result.file.name, content: new Uint8Array(result.content) }];
          return from(this.editorStateService.updateAttachments(newAttachments));
        }),
        toastError(this.toastService, 'Could not import file.'),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe();
  }

  async updateAttachment(index: number, attachment: EditorStateAttachment) {
    const attachments = this.attachments();
    attachments[index] = attachment;
    await this.editorStateService.updateAttachments(attachments);
  }

  async downloadAttachment(index: number) {
    const attachments = this.attachments();
    const attachment = attachments[index];
    if (attachment === undefined) {
      this.toastService.show({ type: 'error', message: 'Could not find attachment with name ' + name + '.' });
      return;
    }

    const blob = new Blob([attachment.content]);

    downloadBlob(blob, attachment.name);
  }

  async deleteAttachment(index: number) {
    const attachments = this.attachments();
    const newAttachments = attachments.splice(index, 1);
    await this.editorStateService.updateAttachments(newAttachments);
  }

  async downloadAttachments() {
    const attachments = this.attachments();

    for (const attachment of attachments) {
      const blob = new Blob([attachment.content]);
      downloadBlob(blob, attachment.name);
    }
  }

  async deleteAllAttachments() {
    await this.editorStateService.updateAttachments([]);
  }
}
