import { Component, input } from '@angular/core';
import { MarkdownComponent } from 'ngx-markdown';

@Component({
  selector: 'app-cii-form-remark',
  imports: [MarkdownComponent],
  template: `
    <div class="alert alert-light small" [class.border-primary]="highlight()" [class.text-primary]="highlight()">
      @if (title(); as title) {
        <div class="fw-semibold"><i class="bi bi-info-circle pe-1"></i>{{ title }}</div>
        <markdown ngPreserveWhitespaces>
          {{ remark() }}
        </markdown>
      } @else {
        <markdown ngPreserveWhitespaces>
          <i class="bi bi-info-circle pe-1"></i>
          {{ remark() }}
        </markdown>
      }
    </div>
  `,
})
export class CiiFormRemarkComponent {
  remark = input.required<string>();
  title = input<string>();
  highlight = input<boolean>();
}
