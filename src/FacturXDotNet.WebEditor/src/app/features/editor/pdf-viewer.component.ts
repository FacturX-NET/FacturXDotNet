import { Component, computed, input } from '@angular/core';

@Component({
  selector: 'app-pdf-viewer',
  template: ` <embed [src]="objectUrl()" type="application/pdf" style="pointer-events: {{ pointerEvents() }}" /> `,
  styles: `
    embed {
      width: 100%;
      height: 100%;
    }
  `,
})
export class PdfViewerComponent {
  pdf = input.required<Blob>();
  disablePointerEvents = input<boolean>(false);

  protected objectUrl = computed(() => URL.createObjectURL(this.pdf()));
  protected pointerEvents = computed(() => (this.disablePointerEvents() ? 'none' : 'auto'));
}
