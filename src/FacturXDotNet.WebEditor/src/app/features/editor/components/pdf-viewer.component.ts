import { Component, computed, inject, input, SecurityContext } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { createLinkedSignal } from '@angular/core/primitives/signals';

@Component({
  selector: 'app-pdf-viewer',
  template: `
    @if (objectUrl(); as objectUrl) {
      <embed [src]="objectUrl.trustedBlobUrl" type="application/pdf" style="pointer-events: {{ pointerEvents() }}" />
    }
  `,
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

  protected objectUrl = createLinkedSignal<Blob, { blobUrl: string; trustedBlobUrl: SafeUrl } | undefined>(
    () => this.pdf(),
    (pdf, previousBlobUrl) => {
      if (previousBlobUrl?.value?.blobUrl !== undefined) {
        URL.revokeObjectURL(previousBlobUrl.value?.blobUrl as string);
      }

      if (pdf === undefined) {
        return undefined;
      }

      const blobUrl = URL.createObjectURL(pdf);
      const trustedBlobUrl = this.sanitizer.bypassSecurityTrustResourceUrl(blobUrl);

      return { blobUrl, trustedBlobUrl };
    },
  );

  protected pointerEvents = computed(() => (this.disablePointerEvents() ? 'none' : 'auto'));
}
