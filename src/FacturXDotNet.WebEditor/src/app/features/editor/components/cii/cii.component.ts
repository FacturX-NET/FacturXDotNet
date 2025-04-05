import { Component, inject, input, Resource, signal } from '@angular/core';
import { CiiFormComponent } from '../cii-form/cii-form.component';
import { CiiSummaryComponent } from '../cii-summary/cii-summary.component';
import { ICrossIndustryInvoice } from '../../../../core/api/api.models';
import { EditorSettings } from '../../editor-settings.service';
import { debounceTime, delay, from, Subject, switchMap, tap } from 'rxjs';
import { EditorSavedState, EditorStateService } from '../../services/editor-state.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CiiDropdownDetailsComponent } from './cii-dropdown-details.component';

@Component({
  selector: 'app-cii',
  imports: [CiiFormComponent, CiiSummaryComponent, CiiDropdownDetailsComponent],
  template: `
    @if (state.value(); as state) {
      <div class="h-100 d-flex column-gap-4 overflow-hidden position-relative">
        <div id="editor__cii-summary" class="col-3 offcanvas-xl offcanvas-start overflow-y-auto ps-xl-3 pt-3" tabindex="-1" aria-labelledby="ciiSummaryTitle">
          <div class="offcanvas-header">
            <h5 class="offcanvas-title" id="ciiSummaryTitle">Cross-Industry Invoice</h5>
            <button type="button" class="btn-close" data-bs-dismiss="offcanvas" data-bs-target="#editor__cii-summary" aria-label="Close"></button>
          </div>
          <div class="offcanvas-body small">
            <div class="overflow-x-hidden">
              <div class="d-flex justify-content-between">
                <h6 class="d-none d-xl-block">Cross-Industry Invoice</h6>
                <app-editor-details-dropdown />
              </div>
              <app-cii-summary [value]="state.cii" [settings]="settings()" />
            </div>
          </div>
        </div>
        <div
          id="editor__cii-form"
          class="flex-grow-1 h-100 position-relative overflow-y-auto pt-3 pb-5"
          data-bs-spy="scroll"
          data-bs-target="#editor__cii-summary-content"
          data-bs-smooth-scroll="true"
          tabindex="0"
        >
          <div class="container ms-0">
            <app-cii-form [value]="state.cii" (valueChange)="saveCii($event)" [settings]="settings()" [disabled]="disabled()" />
          </div>
        </div>

        <div class="position-absolute top-0 end-0 pe-4">
          @if (saving()) {
            <div><i class="bi bi-floppy2-fill text-body-tertiary small glow"></i></div>
          }
          @if (saved()) {
            <div class="text-success small"><i class="bi bi-check"></i> Saved</div>
          }
        </div>
      </div>
    }
  `,
  styles: ``,
})
export class CiiComponent {
  settings = input.required<EditorSettings>();
  disabled = input<boolean>();

  protected saving = signal<boolean>(false);
  protected saved = signal<boolean>(false);
  private saveSubject = new Subject<EditorSavedState>();

  private editorStateService = inject(EditorStateService);
  protected state: Resource<EditorSavedState | null> = this.editorStateService.savedState;

  constructor() {
    this.saveSubject
      .pipe(
        debounceTime(1000),
        switchMap((state) => from(this.editorStateService.update(state))),
        tap(() => {
          this.saving.set(false);
          this.saved.set(true);
        }),
        delay(2000),
        tap(() => this.saved.set(false)),
        takeUntilDestroyed(),
      )
      .subscribe();
  }

  /**
   * Registers the current CII state for saving. The state will be saved after a debounce period.
   * @param cii The Cross-Industry Invoice to save.
   */
  saveCii(cii: ICrossIndustryInvoice) {
    const value = this.editorStateService.savedState.value();
    if (value === null) {
      return;
    }

    this.saving.set(true);
    this.saveSubject.next({ ...value, cii });
  }
}
