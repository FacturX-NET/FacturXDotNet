import { Component, computed, inject, input, Resource, Signal } from '@angular/core';
import { IXmpMetadata } from '../../../../core/api/api.models';
import { EditorSettings, EditorSettingsService } from '../../editor-settings.service';
import { XmpFormComponent } from './xmp-form/xmp-form.component';
import { XmpSummaryComponent } from './xmp-summary.component';
import { EditorSavedState, EditorStateService } from '../../editor-state.service';

@Component({
  selector: 'app-xmp',
  imports: [XmpFormComponent, XmpSummaryComponent],
  template: `
    <div class="h-100 overflow-auto">
      <div class="container py-4">
        <div class="d-flex">
          <div id="editor__xmp-summary" class="flex-shrink-0 ps-xl-3" tabindex="-1" aria-labelledby="ciiSummaryTitle">
            <div class="small position-sticky">
              <div class="d-flex flex-column">
                <h6>XMP Metadata</h6>
                <app-xmp-summary></app-xmp-summary>
              </div>
            </div>
          </div>

          <app-xmp-form [value]="xmp()" [settings]="settings()"></app-xmp-form>
        </div>
      </div>
    </div>
  `,
  styles: `
    #editor__xmp-summary {
      width: 220px;
    }

    #editor__xmp-summary > div {
      top: 10px;
    }
  `,
})
export class XmpTab {
  private editorStateService = inject(EditorStateService);
  private settingsService = inject(EditorSettingsService);

  protected state: Resource<EditorSavedState | null> = this.editorStateService.savedState;
  protected xmp = computed(() => this.state.value()?.xmp ?? {});
  protected settings: Signal<EditorSettings> = this.settingsService.settings;
}
