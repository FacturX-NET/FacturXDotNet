import { Component, input, TemplateRef } from '@angular/core';
import { NgTemplateOutlet } from '@angular/common';

@Component({
  selector: 'app-cii-form-remark',
  imports: [NgTemplateOutlet],
  template: `
    <div class="alert alert-light small" [class.border-primary]="highlight()">
      @if (title(); as title) {
        <div class="fw-semibold"><i class="bi bi-info-circle pe-1"></i>{{ title }}</div>
      } @else {
        <i class="bi bi-info-circle pe-1"></i>
      }

      <ng-content></ng-content>
    </div>
  `,
  styles: ``,
})
export class CiiFormRemarkComponent {
  title = input<string>();
  highlight = input<boolean>();
}
