import { Injectable, signal } from '@angular/core';
import { EditorStateAttachment } from '../../editor-state.service';

@Injectable({
  providedIn: 'root',
})
export class DeletedAttachmentsService {
  private sessionStorageKey = 'deleted-attachments';

  public get deletedAttachments() {
    return this.deletedAttachmentsInternal.asReadonly();
  }

  private deletedAttachmentsInternal = signal<DeletedAttachment[]>([]);

  recordDeletedAttachment(...attachmentsToRecord: EditorStateAttachment[]) {
    const attachments = this.loadAttachments();
    const newAttachments = [...attachmentsToRecord.map((attachment) => ({ ...attachment, id: this.generateId() })), ...attachments];
    this.saveAttachments(newAttachments);
    this.deletedAttachmentsInternal.set(newAttachments);
  }

  removeDeletedAttachments(...attachmentsToRemove: DeletedAttachment[]) {
    const attachments = this.loadAttachments();
    const newAttachments = attachments.filter((a) => attachments.every((attachmentToRemove) => a.id !== attachmentToRemove.id));
    this.saveAttachments(newAttachments);
    this.deletedAttachmentsInternal.set(newAttachments);
  }

  refresh() {
    this.deletedAttachmentsInternal.set(this.loadAttachments());
  }

  private loadAttachments(): DeletedAttachment[] {
    const attachmentsStr = sessionStorage.getItem(this.sessionStorageKey);
    if (attachmentsStr === null) {
      return [];
    }

    const serializedAttachments = JSON.parse(attachmentsStr) as SerializedAttachment[];
    return serializedAttachments.map((attachment) => this.deserializeAttachment(attachment));
  }

  private saveAttachments(attachments: DeletedAttachment[]): void {
    const serialized = attachments.map((attachment) => this.serializeAttachment(attachment));
    const attachmentsStr = JSON.stringify(serialized);
    sessionStorage.setItem(this.sessionStorageKey, attachmentsStr);
  }

  private serializeAttachment(attachment: DeletedAttachment): SerializedAttachment {
    const charArray = [];
    for (let i = 0; i < attachment.content.length; i++) {
      const number = attachment.content.at(i) ?? 0;
      charArray.push(String.fromCharCode(number));
    }

    return {
      id: attachment.id,
      name: attachment.name,
      description: attachment.description,
      content: charArray.join(''),
    };
  }

  private deserializeAttachment(attachment: SerializedAttachment): DeletedAttachment {
    const numberArray = [];
    for (let i = 0; i < attachment.content.length; i++) {
      numberArray.push(attachment.content.charCodeAt(i));
    }

    return {
      id: attachment.id,
      name: attachment.name,
      description: attachment.description,
      content: new Uint8Array(numberArray),
    };
  }

  private generateId() {
    return Math.random().toString(36).substring(2);
  }
}

export interface DeletedAttachment extends EditorStateAttachment {
  id: string;
}

interface SerializedAttachment {
  id: string;
  name: string;
  description?: string;
  content: string;
}
