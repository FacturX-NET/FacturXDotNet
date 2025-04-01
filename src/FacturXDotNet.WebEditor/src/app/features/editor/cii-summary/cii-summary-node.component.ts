import { Component, effect, input, model, untracked } from '@angular/core';
import { CiiSummaryNode } from './cii-summary.component';
import { ScrollToDirective } from '../../../core/directives/scroll-to.directive';
import { EditorSettings } from '../editor-settings.service';

@Component({
  selector: 'app-cii-summary-node',
  imports: [ScrollToDirective],
  template: `
    <div class="text-truncate d-flex gap-1">
      @if (node().disabled) {
        <span class="fw-semibold"> {{ node().code }} </span> - {{ node().name }}
      } @else {
        <a [scrollTo]="node().code" data-bs-dismiss="offcanvas" data-bs-target="#editor__cii-summary">
          <span class="fw-semibold"> {{ node().code }} </span> - {{ node().name }}
        </a>
      }

      @if (settings(); as settings) {
        <div class="text-body-tertiary d-flex align-items-center gap-1" style="font-size: smaller">
          @if (settings.showBusinessRules) {
            @if (node().businessRules; as businessRules) {
              @for (rule of businessRules; track rule) {
                <a [scrollTo]="rule" class="hoverlink">{{ rule }}</a>
              }
            }
          }

          @if (node().hasRemarks && settings.showRemarks) {
            <a [scrollTo]="node().code + '-remarks'" class="hoverlink"><i class="bi bi-info-circle"></i></a>
          }

          @if (node().hasChorusProRemarks && settings.showChorusProRemarks) {
            <a [scrollTo]="node().code + '-cpro-remarks'" class="hoverlink">CPRO</a>
          }
        </div>
      }
    </div>
    <div class="border-start ps-2">
      @if (node().children; as children) {
        @for (child of node().children; track child.code) {
          <app-cii-summary-node [node]="child" [settings]="settings()" />
        }
      }
    </div>
  `,
})
export class CiiSummaryNodeComponent {
  node = input.required<CiiSummaryNode>();
  settings = input.required<EditorSettings>();
}
