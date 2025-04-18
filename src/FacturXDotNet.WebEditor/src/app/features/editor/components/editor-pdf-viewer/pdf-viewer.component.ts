import { Component, computed, inject, input, linkedSignal } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';

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
  pdf = input.required<{ id?: string; content: Blob }>();
  disablePointerEvents = input<boolean>(false);

  private sanitizer = inject(DomSanitizer);

  protected objectUrl = linkedSignal<{ id?: string; content: Blob }, { id?: string; blobUrl: string; trustedBlobUrl: SafeUrl } | undefined>({
    source: () => this.pdf(),
    computation: (pdf, previousBlobUrl) => {
      if (pdf.id === previousBlobUrl?.source.id && previousBlobUrl?.value !== undefined) {
        return previousBlobUrl.value;
      }

      if (previousBlobUrl?.value?.blobUrl !== undefined) {
        URL.revokeObjectURL(previousBlobUrl.value?.blobUrl as string);
      }

      if (pdf.content === undefined) {
        return undefined;
      }

      const blobUrl = URL.createObjectURL(pdf.content);
      const trustedBlobUrl = this.sanitizer.bypassSecurityTrustResourceUrl(blobUrl);

      return { id: pdf.id, blobUrl, trustedBlobUrl };
    },
    equal: (a, b) => a?.id === b?.id,
  });

  protected pointerEvents = computed(() => (this.disablePointerEvents() ? 'none' : 'auto'));
}
