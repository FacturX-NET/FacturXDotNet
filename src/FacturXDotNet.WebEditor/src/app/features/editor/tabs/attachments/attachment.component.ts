import { Component, effect, input, model, signal } from '@angular/core';
import { NgxFilesizeModule } from 'ngx-filesize';
import { EditorStateAttachment } from '../../editor-state.service';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { map } from 'rxjs';

@Component({
  selector: 'app-attachment',
  imports: [NgxFilesizeModule, ReactiveFormsModule],
  template: `
    @if (edit()) {
      <form [formGroup]="attachmentFormGroup" (ngSubmit)="save()">
        <input class="form-control form-control-sm mt-2" formControlName="name" placeholder="Name" />
        <div class="d-flex align-items-center gap-2 mt-2">
          <div class="flex-shrink-0 text-body-secondary small">{{ attachment().content.byteLength | filesize }} -</div>
          <input class="form-control form-control-sm" formControlName="description" placeholder="Description" />
        </div>
        <button class="btn btn-sm btn-outline-secondary mt-3"><i class="bi bi-floppy2-fill"></i> Save</button>
      </form>
    } @else {
      <div class="d-flex gap-2">
        <h5 class="m-0">{{ attachment().name }}</h5>
        <a role="button" (click)="startEdition()">
          <i class="bi bi-pencil-fill text-body-secondary small"></i>
        </a>
      </div>
      <div class="text-body-secondary small">
        {{ attachment().content.byteLength | filesize }}
        @if (attachment().description) {
          - {{ attachment().description }}
        }
      </div>
    }
  `,
  styles: ``,
})
export class AttachmentComponent {
  attachment = model.required<EditorStateAttachment>();

  protected edit = signal<boolean>(false);

  protected attachmentFormGroup = new FormGroup({
    name: new FormControl('', { nonNullable: true }),
    description: new FormControl<string | undefined>(undefined, { nonNullable: true }),
  });

  constructor() {
    effect(() => {
      const attachment = this.attachment();
      this.attachmentFormGroup.reset(attachment, { emitEvent: false });
    });
  }

  protected startEdition() {
    this.edit.set(true);
  }

  protected save() {
    const attachment = this.attachment();
    this.attachment.set({ ...attachment, name: this.attachmentFormGroup.controls.name.value, description: this.attachmentFormGroup.controls.description.value });
    this.edit.set(false);
  }
}
