import { Component, inject } from '@angular/core';
import { EditorStateService } from '../../services/editor-state.service';

@Component({
  selector: 'app-editor-export-menu',
  template: `
    <li class="nav-item dropdown">
      <a class="nav-link dropdown-toggle px-4 text-light" role="button" data-bs-toggle="dropdown" aria-expanded="false">Export</a>
      <ul class="dropdown-menu">
        <li>
          <a class="dropdown-item" href="javascript:void 0;">Download FacturX document</a>
          <a class="dropdown-item" href="javascript:void 0;">Download Cross-Industry Invoice XML file</a>
          <a class="dropdown-item" href="javascript:void 0;" (click)="exportPdfImage()">Download PDF file</a>
        </li>
      </ul>
    </li>
  `,
})
export class EditorExportMenuComponent {
  private editorStateService = inject(EditorStateService);

  exportPdfImage() {
    if (!this.editorStateService.savedState.hasValue()) {
      return;
    }

    const value = this.editorStateService.savedState.value();
    if (value?.pdf?.content === undefined) {
      return;
    }

    const blobUrl = window.URL.createObjectURL(value.pdf.content);
    window.open(blobUrl);
  }
}
