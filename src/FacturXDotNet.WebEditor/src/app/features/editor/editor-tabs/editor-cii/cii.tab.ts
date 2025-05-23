import { Component, computed, inject, Resource, Signal } from '@angular/core';
import { EditorSettings, EditorSettingsService } from '../../services/editor-settings.service';
import { CiiFormComponent } from './cii-form/cii-form.component';
import { CiiSummaryComponent } from './cii-summary/cii-summary.component';
import { CiiMenuComponent } from './cii-menu/cii-menu.component';
import { CiiFormService } from './cii-form/cii-form.service';
import { EditorSavedState, EditorStateService } from '../../services/editor-state.service';
import { EditorResponsivenessService } from '../../services/editor-responsiveness.service';

@Component({
  selector: 'app-cii',
  imports: [CiiFormComponent, CiiSummaryComponent, CiiMenuComponent],
  template: `
    <div class="h-100 d-flex overflow-hidden position-relative">
      @if (settings().foldSummary || folded()) {
        <div id="editor__cii-summary--offcanvas" class="flex-shrink-0 offcanvas offcanvas-start overflow-y-auto ps-xl-3 pt-3" tabindex="-1" aria-labelledby="ciiSummaryTitle">
          <div class="offcanvas-header align-items-center gap-2">
            <h5 class="offcanvas-title" id="ciiSummaryTitle">Cross-Industry Invoice</h5>
            <button type="button" class="btn-close" data-bs-dismiss="offcanvas" data-bs-target="#editor__cii-summary" aria-label="Close"></button>
          </div>

          <div class="offcanvas-body small">
            <app-cii-summary [value]="cii()" [settings]="settings()" />
          </div>
        </div>
      } @else {
        <div id="editor__cii-summary" class="flex-shrink-0 overflow-y-auto ps-xl-3 pt-3" [class.small]="small()" tabindex="-1" aria-labelledby="ciiSummaryTitle">
          <div class="overflow-x-hidden small">
            <div class="justify-content-between">
              <h6>Cross-Industry Invoice</h6>
            </div>
            <app-cii-summary [value]="cii()" [settings]="settings()" />
          </div>
        </div>
      }

      <app-cii-menu [settings]="settings()" [forceFold]="folded()"></app-cii-menu>

      <div
        id="editor__cii-form"
        class="flex-grow-1 h-100 position-relative overflow-y-auto mt-3 pb-5"
        [class.small]="small()"
        data-bs-spy="scroll"
        data-bs-target="#editor__cii-summary-content"
        data-bs-smooth-scroll="true"
        tabindex="0"
      >
        <div class="container ms-0">
          <app-cii-form [value]="cii()" [settings]="settings()" />
        </div>
      </div>

      <div class="position-absolute top-0 end-0 pe-4" style="z-index:5555;">
        @switch (formState()) {
          @case ('pristine') {}
          @case ('dirty') {
            <div><i class="bi bi-asterisk text-body-tertiary small glow"></i></div>
          }
          @case ('saving') {
            <div><i class="bi bi-floppy2-fill text-body-tertiary small glow"></i></div>
          }
        }
      </div>
    </div>
  `,
  styles: `
    #editor__cii-summary {
      width: var(--editor-summary-width);
    }

    #editor__cii-summary.small {
      width: var(--editor-summary-width-small);
    }

    #editor__cii-summary--offcanvas {
      width: var(--editor-summary-offcanvas-width);
    }
  `,
})
export class CiiTab {
  private editorStateService = inject(EditorStateService);
  private editorResponsivenessService = inject(EditorResponsivenessService);
  private ciiFormService = inject(CiiFormService);
  private settingsService = inject(EditorSettingsService);

  protected state: Resource<EditorSavedState | null> = this.editorStateService.savedState;
  protected cii = computed(() => this.state.value()?.cii ?? {});
  protected settings: Signal<EditorSettings> = this.settingsService.settings;
  protected formState = this.ciiFormService.state;

  protected small = this.editorResponsivenessService.smallLeftColumn;
  protected folded = this.editorResponsivenessService.foldLeftColumn;
}
