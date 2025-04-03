import { Component, input } from '@angular/core';

@Component({
  selector: 'app-cii-form-remark',
  template: `
    <div class="alert alert-light small" [class.border-primary]="highlight()" [class.text-primary]="highlight()">
      @if (title(); as title) {
        <div class="fw-semibold"><i class="bi bi-info-circle pe-1"></i>{{ title }}</div>
      } @else {
        <i class="bi bi-info-circle pe-1"></i>
      }

      <ng-content></ng-content>
    </div>
  `,
})
export class CiiFormRemarkComponent {
  title = input<string>();
  highlight = input<boolean>();
}
