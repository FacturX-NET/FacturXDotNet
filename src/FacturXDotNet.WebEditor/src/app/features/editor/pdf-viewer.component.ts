import { Component } from '@angular/core';

@Component({
  selector: 'app-pdf-viewer',
  imports: [],
  template: ` <embed src="https://pdfobject.com/pdf/sample.pdf" type="application/pdf" /> `,
  styles: `
    embed {
      width: 100%;
      height: 100%;
    }
  `,
})
export class PdfViewerComponent {}
