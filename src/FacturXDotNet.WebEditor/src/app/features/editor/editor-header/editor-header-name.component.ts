import { Component, inject, input, signal } from '@angular/core';
import { EditorStateService } from '../editor-state.service';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgStyle } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { map } from 'rxjs';

@Component({
  selector: 'app-editor-header-name',
  imports: [ReactiveFormsModule, NgStyle],
  template: `
    @if (isEditing()) {
      <form class="d-flex gap-2" [formGroup]="form" (ngSubmit)="saveNewName()" (keydown.escape)="discardNewName()">
        <input type="text" class="form-control" formControlName="name" [ngStyle]="{ 'width.ch': inputWidth() }" />
        <button class="btn btn-outline-primary">Save</button>
        <button class="btn btn-light" type="button" (click)="discardNewName()">Cancel</button>
      </form>
    } @else {
      <div class="ps-1 d-flex">
        <span class="fs-5 pe-1 text-truncate">{{ name() }}</span>
        <button class="btn btn-sm btn-link" (click)="startEditName()">Edit</button>
      </div>
    }
  `,
  styles: `
    form {
      max-width: 100%;
    }
  `,
})
export class EditorHeaderNameComponent {
  name = input.required<string>();

  private editorStateService = inject(EditorStateService);
  protected isEditing = signal(false);
  protected form = new FormGroup({ name: new FormControl('', { nonNullable: true, validators: [Validators.required] }) });

  protected inputWidth = signal(420);

  constructor() {
    this.form.valueChanges
      .pipe(
        map(() => this.updateInputWidth()),
        takeUntilDestroyed(),
      )
      .subscribe();
  }

  startEditName(): void {
    this.form.patchValue({ name: this.name() });
    this.updateInputWidth();
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

  private updateInputWidth() {
    const currentName = this.form.controls.name.value;
    const length = currentName.length > 10 ? currentName.length : 10;
    this.inputWidth.set(length + 2);
  }
}
