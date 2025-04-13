import { Component, input } from '@angular/core';

@Component({
  selector: 'app-cii-form-business-rule-status-icon',
  imports: [],
  template: `
    @switch (status()) {
      @case ('valid') {
        <i class="bi bi-check-circle-fill"></i>
      }
      @case ('invalid') {
        <i class="bi bi-x-circle-fill"></i>
      }
      @default {
        @if (highlighted()) {
          <i class="bi bi-arrow-right"></i>
        } @else {
          <i class="bi bi-dash"></i>
        }
      }
    }
  `,
  styles: ``,
})
export class CiiFormBusinessRuleStatusIconComponent {
  highlighted = input.required<boolean>();
  status = input.required<'valid' | 'invalid' | undefined>();
}
