import { Component, inject, input } from '@angular/core';
import { CiiSummaryControlComponent } from './cii-summary-control.component';
import { CiiFormService } from '../cii-form/cii-form.service';
import { ICrossIndustryInvoice } from '../../../../../core/api/api.models';
import { EditorSettings } from '../../../editor-settings.service';

@Component({
  selector: 'app-cii-summary',
  imports: [CiiSummaryControlComponent],
  template: `
    <div id="editor__cii-summary-content">
      @for (node of doc; track node.term) {
        <app-cii-summary-node [node]="node" [settings]="settings()" />
      }
    </div>
  `,
})
export class CiiSummaryComponent {
  value = input.required<ICrossIndustryInvoice>();
  settings = input.required<EditorSettings>();

  private ciiFormService = inject(CiiFormService);
  protected doc = this.ciiFormService.doc;
}
