import { Component, DestroyRef, inject, input } from '@angular/core';
import { EditorStateAttachment, EditorStateService } from '../../editor-state.service';
import { NgxFilesizeModule } from 'ngx-filesize';
import { ImportFileService } from '../../../../core/import-file/import-file.service';
import { filter, from, map, Observable, switchMap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { toastError } from '../../../../core/utils/toast-error';
import { ToastService } from '../../../../core/toasts/toast.service';
import { downloadBlob } from '../../../../core/utils/download-blob';
import { AttachmentComponent } from './attachment.component';
import { DeletedAttachment, DeletedAttachmentsService } from './deleted-attachments.service';

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
      </div>

      @if (deletedAttachments().length > 0) {
        <div class="mt-5 text-body-secondary">
          <hr />
          <h4>Deleted attachments</h4>
          <p>
            These attachments have been deleted, they will not be part of the exported results. They can be restored by clicking on the restore button. <br />
            <span class="fw-semibold">WARNING</span>: the deleted attachments are recorded in the session storage of the browser, they WILL BE LOST when the tab is closed, and they
            WILL NOT be shared between tabs.
          </p>
          <div class="list-group">
            @for (deletedAttachment of deletedAttachments(); track deletedAttachment.id) {
              <div class="list-group-item text-body-secondary">
                <div class="d-flex justify-content-between align-items-start">
                  <div>
                    <app-attachment [attachment]="deletedAttachment" editable="false"></app-attachment>
                  </div>
                  <div class="d-flex gap-2">
                    <button class="btn btn btn-outline-secondary" (click)="downloadDeletedAttachment(deletedAttachment)"><i class="bi bi-download"></i> Download</button>
                    <button class="btn btn btn-outline-secondary" (click)="restoreDeletedAttachment(deletedAttachment)"><i class="bi bi-arrow-bar-up"></i> Restore</button>
                  </div>
                </div>
              </div>
            }
          </div>
        </div>
      }
    </div>
  `,
  imports: [NgxFilesizeModule, AttachmentComponent],
})
export class AttachmentsTab {
  attachments = input.required<EditorStateAttachment[]>();

  private editorStateService = inject(EditorStateService);
  private importFileService = inject(ImportFileService);
  private deletedAttachmentsService = inject(DeletedAttachmentsService);
  private toastService = inject(ToastService);
  private destroyRef = inject(DestroyRef);

  protected deletedAttachments = this.deletedAttachmentsService.deletedAttachments;

  constructor() {
    this.deletedAttachmentsService.refresh();
  }

  addAttachment() {
    from(this.importFileService.importFile())
      .pipe(
        filter((file) => file !== undefined),
        switchMap((file) => {
          return from(file.arrayBuffer()).pipe(map((content) => ({ file, content })));
        }),
        switchMap((result) => this.addAttachmentInternal({ name: result.file.name, content: new Uint8Array(result.content) })),
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
    const toDelete = attachments[index];
    this.deletedAttachmentsService.recordDeletedAttachment(toDelete);

    const newAttachments = attachments.filter((_, i) => i !== index);
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
    for (const attachment of this.attachments()) {
      this.deletedAttachmentsService.recordDeletedAttachment(attachment);
    }

    await this.editorStateService.updateAttachments([]);
  }

  async downloadDeletedAttachment(attachment: DeletedAttachment) {
    const blob = new Blob([attachment.content]);
    downloadBlob(blob, attachment.name);
  }

  async restoreDeletedAttachment(attachment: DeletedAttachment) {
    this.deletedAttachmentsService.removeDeletedAttachment(attachment);
    this.addAttachmentInternal(attachment).pipe(takeUntilDestroyed(this.destroyRef)).subscribe();
  }

  private addAttachmentInternal(attachment: EditorStateAttachment): Observable<void> {
    const attachments = this.attachments();
    const newAttachments = [...attachments, attachment];
    return from(this.editorStateService.updateAttachments(newAttachments));
  }
}
