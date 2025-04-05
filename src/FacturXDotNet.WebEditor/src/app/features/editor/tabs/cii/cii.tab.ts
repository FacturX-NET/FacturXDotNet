import { Component, inject, input, Resource, signal } from '@angular/core';
import { ICrossIndustryInvoice } from '../../../../core/api/api.models';
import { EditorSettings, EditorSettingsService } from '../../editor-settings.service';
import { debounceTime, delay, from, Subject, switchMap, tap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CiiFormComponent } from './cii-form/cii-form.component';
import { CiiSummaryComponent } from './cii-summary/cii-summary.component';
import { EditorSavedState, EditorStateService } from '../../editor-state.service';
import { CiiMenuComponent } from './cii-menu/cii-menu.component';

@Component({
  selector: 'app-cii',
  imports: [CiiFormComponent, CiiSummaryComponent, CiiMenuComponent],
  template: `
    @if (state.value(); as state) {
      <div class="h-100 d-flex overflow-hidden position-relative">
        @if (settings().foldSummary) {
          <div id="editor__cii-summary--offcanvas" class="flex-shrink-0 offcanvas offcanvas-start overflow-y-auto ps-xl-3 pt-3" tabindex="-1" aria-labelledby="ciiSummaryTitle">
            <div class="offcanvas-header align-items-center gap-2">
              <h5 class="offcanvas-title" id="ciiSummaryTitle">Cross-Industry Invoice</h5>
              <button type="button" class="btn-close" data-bs-dismiss="offcanvas" data-bs-target="#editor__cii-summary" aria-label="Close"></button>
            </div>

            <div class="offcanvas-body small">
              <app-cii-summary [value]="state.cii" [settings]="settings()" />
            </div>
          </div>
        } @else {
          <div id="editor__cii-summary" class="flex-shrink-0 overflow-y-auto ps-xl-3 pt-3" tabindex="-1" aria-labelledby="ciiSummaryTitle">
            <div class="overflow-x-hidden small">
              <div class="d-none d-xl-flex justify-content-between">
                <h6>Cross-Industry Invoice</h6>
              </div>
              <app-cii-summary [value]="state.cii" [settings]="settings()" />
            </div>
          </div>
        }

        <app-cii-menu [settings]="settings()"></app-cii-menu>

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
  styles: `
    #editor__cii-summary {
      width: var(--editor-summary-width);
    }

    #editor__cii-summary--offcanvas {
      width: var(--editor-summary-offcanvas-width);
    }
  `,
})
export class CiiTab {
  settings = input.required<EditorSettings>();
  disabled = input<boolean>();

  protected saving = signal<boolean>(false);
  protected saved = signal<boolean>(false);
  private saveSubject = new Subject<EditorSavedState>();

  private editorStateService = inject(EditorStateService);
  private editorSettingsService = inject(EditorSettingsService);
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
