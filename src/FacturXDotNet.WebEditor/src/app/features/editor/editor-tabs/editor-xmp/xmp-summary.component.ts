import { Component } from '@angular/core';
import { ScrollToDirective } from '../../../../core/scroll-to/scroll-to.directive';

@Component({
  selector: 'app-xmp-summary',
  imports: [ScrollToDirective],
  template: `
    <div class="d-flex flex-column">
      <a scrollTo="pdf-a-identification"> PDF/A Identification </a>
      <a scrollTo="basic"> Basic XMP metadata </a>
      <a scrollTo="pdf"> PDF metadata </a>
      <a scrollTo="dublin-core"> Dublin Core </a>
      <a scrollTo="facturx"> Factur-X </a>
    </div>
  `,
  styles: ``,
})
export class XmpSummaryComponent {}
