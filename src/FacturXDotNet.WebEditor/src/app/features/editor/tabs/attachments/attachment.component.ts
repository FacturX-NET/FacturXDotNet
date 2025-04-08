import { booleanAttribute, Component, effect, input, model, signal } from '@angular/core';
import { NgxFilesizeModule } from 'ngx-filesize';
import { EditorStateAttachment } from '../../editor-state.service';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-attachment',
  imports: [NgxFilesizeModule, ReactiveFormsModule],
  template: `
    @if (editable() && edit()) {
      <form [formGroup]="attachmentFormGroup" (ngSubmit)="save()">
        <input class="form-control form-control-sm mt-2" formControlName="name" placeholder="Name" />
        <div class="d-flex align-items-center gap-2 mt-2">
          <div class="flex-shrink-0 text-body-secondary small">{{ attachment().content.byteLength | filesize }} -</div>
          <input class="form-control form-control-sm" formControlName="description" placeholder="Description" />
        </div>
        <div class="d-flex gap-2">
          <button class="btn btn-sm btn-outline-secondary mt-3"><i class="bi bi-floppy2-fill"></i> Save</button>
          <button type="button" class="btn btn-sm btn-outline-danger mt-3" (click)="cancel()"><i class="bi bi-x-lg"></i> Cancel</button>
        </div>
      </form>
    } @else {
      <div class="d-flex gap-2">
        <h5 class="m-0">{{ attachment().name }}</h5>

        @if (editable()) {
          <a role="button" (click)="startEdition()">
            <i class="bi bi-pencil-fill text-body-secondary small"></i>
          </a>
        }
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
  editable = input(true, { transform: booleanAttribute });

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
    if (!this.editable()) {
      return;
    }

    this.edit.set(true);
  }

  protected save() {
    const attachment = this.attachment();
    this.attachment.set({ ...attachment, name: this.attachmentFormGroup.controls.name.value, description: this.attachmentFormGroup.controls.description.value });
    this.edit.set(false);
  }

  protected cancel() {
    const attachment = this.attachment();
    this.attachmentFormGroup.reset(attachment, { emitEvent: false });
    this.edit.set(false);
  }
}
