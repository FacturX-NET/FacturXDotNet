import { Component } from '@angular/core';

@Component({
  selector: 'app-pdf-viewer',
  imports: [],
  template: ` <div class="w-100 h-100 position-relative"><embed src="https://pdfobject.com/pdf/sample.pdf" type="application/pdf" /></div> `,
  styles: `
    embed {
      position: absolute;
      left: 0;
      top: 0;
      width: 100%;
      height: 100%;
    }
  `,
})
export class PdfViewerComponent {}
