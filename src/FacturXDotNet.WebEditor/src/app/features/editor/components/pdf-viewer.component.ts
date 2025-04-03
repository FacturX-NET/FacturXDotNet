import { Component, computed, inject, input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

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

  private sanitizer = inject(DomSanitizer);

  protected objectUrl = computed(() => {
    const blobUrl = URL.createObjectURL(this.pdf());
    return this.sanitizer.bypassSecurityTrustResourceUrl(blobUrl);
  });
  protected pointerEvents = computed(() => (this.disablePointerEvents() ? 'none' : 'auto'));
}
