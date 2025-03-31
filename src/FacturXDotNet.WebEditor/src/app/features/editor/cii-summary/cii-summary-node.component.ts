import { Component, effect, input, model, untracked } from '@angular/core';
import { CiiSummaryNode } from './cii-summary.component';
import { ScrollToDirective } from '../../../core/directives/scroll-to.directive';

@Component({
  selector: 'app-cii-summary-node',
  imports: [ScrollToDirective],
  template: `
    <div class="text-truncate">
      <a [scrollTo]="'#' + node().code" data-bs-dismiss="offcanvas" data-bs-target="#editor__cii-summary">
        <span class="fw-semibold"> {{ node().code }} </span> - {{ node().name }}
      </a>
      @if (node().businessRules; as businessRules) {
        <span class="text-body-tertiary" style="font-size: smaller">
          (
          @for (rule of businessRules; track rule) {
            @if (!$last) {
              <a [scrollTo]="'#' + rule" class="hoverlink">{{ rule }}</a
              >,
            } @else {
              <a [scrollTo]="'#' + rule" class="hoverlink">{{ rule }}</a>
            }
          }
          )
        </span>
      }
    </div>
    <div class="border-start ps-2">
      @if (node().children; as children) {
        @for (child of node().children; track child.code) {
          <app-cii-summary-node [node]="child" />
        }
      }
    </div>
  `,
})
export class CiiSummaryNodeComponent {
  node = input.required<CiiSummaryNode>();
}
