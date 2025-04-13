import { Component, computed, DestroyRef, inject, Resource } from '@angular/core';
import { EditorSavedState, EditorStateAttachment, EditorStateService } from '../../editor-state.service';
import { NgxFilesizeModule } from 'ngx-filesize';
import { ImportFileService } from '../../../../core/import-file/import-file.service';
import { filter, from, map, switchMap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { toastError } from '../../../../core/toasts/toast-error';
import { ToastService } from '../../../../core/toasts/toast.service';
import { downloadBlob } from '../../../../core/utils/download-blob';
import { AttachmentComponent } from './attachment.component';
import { DeletedAttachment, DeletedAttachmentsService } from './deleted-attachments.service';

@Component({
  selector: 'app-attachments',
  template: `
    <div class="container">
      <div class="my-5">
        <h4 class="text-truncate">Attachments ({{ attachments().length }})</h4>
        <hr class="mt-0" />
        <div class="d-flex gap-2 mb-4">
          <button class="btn btn-sm btn-outline-secondary text-truncate" (click)="addAttachment()"><i class="bi bi-plus"></i> Add attachment</button>
          @if (attachments().length !== 0) {
            <div class="flex-grow-1"><!-- spacer --></div>
            <button class="btn btn-sm btn-outline-secondary text-truncate" (click)="downloadAllAttachments()"><i class="bi bi-download"></i> Download all</button>
            <button class="btn btn-sm btn-outline-danger text-truncate" (click)="deleteAllAttachments()"><i class="bi bi-trash"></i> Delete all</button>
          }
        </div>
        <div class="list-group">
          @for (attachment of attachments(); track attachment.name) {
            <div class="list-group-item">
              <div class="d-flex justify-content-between align-items-center">
                <div>
                  <app-attachment [attachment]="attachment" (attachmentChange)="updateAttachment($index, $event)"></app-attachment>
                </div>
                <div class="d-flex gap-2">
                  <button class="btn btn-sm btn-outline-secondary text-truncate" (click)="downloadAttachment($index)"><i class="bi bi-download"></i></button>
                  <button class="btn btn-sm btn-outline-danger text-truncate" (click)="deleteAttachment($index)"><i class="bi bi-trash"></i></button>
                </div>
              </div>
            </div>
          }
        </div>
      </div>

      @if (deletedAttachments().length > 0) {
        <div class="my-5 text-body-secondary">
          <h4 class="text-truncate">Deleted attachments ({{ deletedAttachments().length }})</h4>
          <hr class="mt-0" />
          <p>
            These attachments have been deleted, they will not be part of the exported results. They can be restored by clicking on the restore button. <br />
            <span class="fw-semibold">WARNING</span>: the deleted attachments are recorded in the session storage of the browser, they WILL BE LOST when the tab is closed, and they
            WILL NOT be shared between tabs.
          </p>
          <div class="d-flex justify-content-end gap-2 mb-4">
            <button class="btn btn-sm btn-outline-secondary text-truncate" (click)="downloadAllDeletedAttachments()"><i class="bi bi-download"></i> Download all</button>
            <button class="btn btn-sm btn-outline-primary text-truncate" (click)="restoreAllAttachments()"><i class="bi bi-arrow-bar-up"></i> Restore all</button>
          </div>
          <div class="list-group">
            @for (deletedAttachment of deletedAttachments(); track deletedAttachment.id) {
              <div class="list-group-item text-body-secondary">
                <div class="d-flex justify-content-between align-items-center">
                  <div>
                    <app-attachment [attachment]="deletedAttachment" editable="false"></app-attachment>
                  </div>
                  <div class="d-flex gap-2">
                    <button class="btn btn-sm btn-outline-secondary text-truncate" (click)="downloadDeletedAttachment(deletedAttachment)"><i class="bi bi-download"></i></button>
                    <button class="btn btn-sm btn-outline-primary text-truncate" (click)="restoreDeletedAttachment(deletedAttachment)"><i class="bi bi-arrow-bar-up"></i></button>
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
  private editorStateService = inject(EditorStateService);
  private importFileService = inject(ImportFileService);
  private deletedAttachmentsService = inject(DeletedAttachmentsService);
  private toastService = inject(ToastService);
  private destroyRef = inject(DestroyRef);

  protected state: Resource<EditorSavedState | null> = this.editorStateService.savedState;
  protected deletedAttachments = this.deletedAttachmentsService.deletedAttachments;
  protected attachments = computed(() => this.state.value()?.attachments ?? []);

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
        switchMap((result) => from(this.addAttachmentsInternal({ name: result.file.name, content: new Uint8Array(result.content) }))),
        toastError(this.toastService, (message) => `Could not import file: ${message}.`),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe();
  }

  async updateAttachment(index: number, attachment: EditorStateAttachment): Promise<void> {
    const attachments = this.attachments();
    attachments[index] = attachment;
    await this.editorStateService.updateAttachments(attachments);
  }

  async downloadAttachment(index: number): Promise<void> {
    const attachments = this.attachments();
    const attachment = attachments[index];
    if (attachment === undefined) {
      this.toastService.show({ type: 'error', message: 'Could not find attachment with name ' + name + '.' });
      return;
    }

    const blob = new Blob([attachment.content]);

    downloadBlob(blob, attachment.name);
  }

  async deleteAttachment(index: number): Promise<void> {
    const attachments = this.attachments();
    const toDelete = attachments[index];
    this.deletedAttachmentsService.recordDeletedAttachment(toDelete);

    const newAttachments = attachments.filter((_, i) => i !== index);
    await this.editorStateService.updateAttachments(newAttachments);
  }

  async downloadAllAttachments(): Promise<void> {
    const attachments = this.attachments();

    for (const attachment of attachments) {
      const blob = new Blob([attachment.content]);
      downloadBlob(blob, attachment.name);
    }
  }

  async deleteAllAttachments(): Promise<void> {
    for (const attachment of this.attachments()) {
      this.deletedAttachmentsService.recordDeletedAttachment(attachment);
    }

    await this.editorStateService.updateAttachments([]);
  }

  async downloadDeletedAttachment(attachment: DeletedAttachment): Promise<void> {
    const blob = new Blob([attachment.content]);
    downloadBlob(blob, attachment.name);
  }

  async restoreDeletedAttachment(attachment: DeletedAttachment): Promise<void> {
    this.deletedAttachmentsService.removeDeletedAttachments(attachment);
    await this.addAttachmentsInternal(attachment);
  }

  async downloadAllDeletedAttachments(): Promise<void> {
    const attachments = this.deletedAttachments();
    for (const attachment of attachments) {
      const blob = new Blob([attachment.content]);
      downloadBlob(blob, attachment.name);
    }
  }

  async restoreAllAttachments(): Promise<void> {
    const deletedAttachments = this.deletedAttachments();
    this.deletedAttachmentsService.removeDeletedAttachments(...deletedAttachments);
    await this.addAttachmentsInternal(...deletedAttachments);
  }

  async addAttachmentsInternal(...attachmentsToAdd: EditorStateAttachment[]): Promise<void> {
    const attachments = this.attachments();
    const newAttachments = [...attachments, ...attachmentsToAdd];
    await this.editorStateService.updateAttachments(newAttachments);
  }
}
