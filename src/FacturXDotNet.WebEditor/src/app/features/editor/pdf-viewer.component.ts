import { Component, computed, input } from '@angular/core';

@Component({
  selector: 'app-pdf-viewer',
  imports: [],
  template: ` <embed src="https://pdfobject.com/pdf/sample.pdf" type="application/pdf" style="pointer-events: {{ pointerEvents() }}" /> `,
  styles: `
    embed {
      width: 100%;
      height: 100%;
    }
  `,
})
export class PdfViewerComponent {
  disablePointerEvents = input<boolean>(false);
  protected pointerEvents = computed(() => (this.disablePointerEvents() ? 'none' : 'auto'));
}
