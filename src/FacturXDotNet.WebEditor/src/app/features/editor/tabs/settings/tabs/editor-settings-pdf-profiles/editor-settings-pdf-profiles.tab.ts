import { Component, inject } from '@angular/core';
import { EditorPdfViewerService } from '../../../../editor-pdf-viewer/editor-pdf-viewer.service';

@Component({
  selector: 'app-editor-settings-pdf-profiles',
  imports: [],
  template: ` <button class="btn btn-primary" (click)="regeneratePDf()">Show</button> `,
  styles: ``,
})
export class EditorSettingsPdfProfilesTab {
  private editorPdfViewerService = inject(EditorPdfViewerService);

  protected regeneratePDf() {
    this.editorPdfViewerService.regenerateAndDisplayStandardPdf();
  }
}
