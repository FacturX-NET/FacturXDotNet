import { Component, computed, inject, Resource, Signal } from '@angular/core';
import { EditorSettings, EditorSettingsService } from '../../services/editor-settings.service';
import { XmpFormComponent } from './xmp-form/xmp-form.component';
import { XmpSummaryComponent } from './xmp-summary.component';
import { EditorSavedState, EditorStateService } from '../../services/editor-state.service';
import { EditorResponsivenessService } from '../../services/editor-responsiveness.service';

@Component({
  selector: 'app-xmp',
  imports: [XmpFormComponent, XmpSummaryComponent],
  template: `
    <div class="h-100 overflow-auto">
      <div class="container py-4">
        <div class="d-flex">
          <div id="editor__xmp-summary" class="flex-shrink-0 ps-xl-3" [class.small]="small()" tabindex="-1" aria-labelledby="ciiSummaryTitle">
            <div class="small position-sticky">
              <div class="d-flex flex-column">
                <h6>XMP Metadata</h6>
                <app-xmp-summary></app-xmp-summary>
              </div>
            </div>
          </div>

          <app-xmp-form [value]="xmp()" [settings]="settings()" [class.small]="small()"></app-xmp-form>
        </div>
      </div>
    </div>
  `,
  styles: `
    #editor__xmp-summary {
      width: 220px;
    }

    #editor__xmp-summary.small {
      width: 180px;
    }

    #editor__xmp-summary > div {
      top: 10px;
    }
  `,
})
export class XmpTab {
  private editorStateService = inject(EditorStateService);
  private editorResponsivenessService = inject(EditorResponsivenessService);
  private settingsService = inject(EditorSettingsService);

  protected state: Resource<EditorSavedState | null> = this.editorStateService.savedState;
  protected xmp = computed(() => this.state.value()?.xmp ?? {});
  protected settings: Signal<EditorSettings> = this.settingsService.settings;

  protected small = this.editorResponsivenessService.smallLeftColumn;
}
