import { Component, input } from '@angular/core';
import { ICrossIndustryInvoice, IXmpMetadata } from '../../../../core/api/api.models';
import { EditorSettings } from '../../editor-settings.service';
import { XmpFormComponent } from './xmp-form/xmp-form.component';
import { CiiSummaryComponent } from '../cii/cii-summary/cii-summary.component';
import { XmpSummaryComponent } from './xmp-summary.component';

@Component({
  selector: 'app-xmp',
  imports: [XmpFormComponent, CiiSummaryComponent, XmpSummaryComponent],
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

          <app-xmp-form [value]="value() ?? {}" [settings]="settings()"></app-xmp-form>
        </div>
      </div>
    </div>
  `,
  styles: `
    #editor__xmp-summary {
      width: var(--editor-summary-width);
    }

    #editor__xmp-summary > div {
      top: 10px;
    }
  `,
})
export class XmpTab {
  value = input.required<IXmpMetadata | undefined>();
  settings = input.required<EditorSettings>();
}
