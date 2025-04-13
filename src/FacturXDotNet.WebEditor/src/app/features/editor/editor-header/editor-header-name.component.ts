import { Component, inject, input, signal } from '@angular/core';
import { EditorStateService } from '../editor-state.service';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-editor-header-name',
  imports: [ReactiveFormsModule],
  template: `
    @if (isEditing()) {
      <form class="d-flex gap-2" [formGroup]="form" (ngSubmit)="saveNewName()">
        <input type="text" class="form-control" formControlName="name" />
        <button class="btn btn-outline-primary">Save</button>
        <button class="btn btn-light" type="button" (click)="discardNewName()">Cancel</button>
      </form>
    } @else {
      <div class="ps-1 d-flex">
        <span class="fs-5 pe-1">{{ name() }}</span>
        <button class="btn btn-sm btn-link" (click)="startEditName()">Edit</button>
      </div>
    }
  `,
  styles: `
    input {
      max-width: 420px;
    }
  `,
})
export class EditorHeaderNameComponent {
  name = input.required<string>();

  private editorStateService = inject(EditorStateService);
  protected isEditing = signal(false);
  protected form = new FormGroup({ name: new FormControl('', { nonNullable: true, validators: [Validators.required] }) });

  startEditName(): void {
    this.form.patchValue({ name: this.name() });
    this.isEditing.set(true);
  }

  async saveNewName() {
    this.form.markAllAsTouched();
    if (this.form.invalid) {
      return;
    }

    await this.editorStateService.updateName(this.form.controls.name.value);
    this.isEditing.set(false);
  }

  discardNewName(): void {
    this.isEditing.set(false);
  }
}
