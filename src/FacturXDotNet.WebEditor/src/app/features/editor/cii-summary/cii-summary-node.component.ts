import { Component, computed, effect, input, model, untracked } from '@angular/core';
import { CiiSummaryNode } from './cii-summary.component';
import { Collapse } from 'bootstrap';
import { ScrollToDirective } from '../../../core/directives/scroll-to.directive';

@Component({
  selector: 'app-cii-summary-node',
  imports: [ScrollToDirective],
  template: `
    <a class="text-truncate" [scrollTo]="'#' + node().code" data-bs-dismiss="offcanvas" data-bs-target="#editor__cii-summary">
      <span class="fw-semibold"> {{ node().code }} </span> - {{ node().name }}
    </a>
    <div class="border-start ps-2">
      @if (collapsed() !== true) {
        @if (node().children; as children) {
          @for (child of node().children; track child.code) {
            <app-cii-summary-node [node]="child" />
          }
        }
      }
    </div>
  `,
})
export class CiiSummaryNodeComponent {
  node = input.required<CiiSummaryNode>();
  collapsed = model<boolean | undefined>(undefined);

  constructor() {
    effect(() => {
      const node = this.node();
      const collapsed = untracked(() => this.collapsed());

      if (collapsed === undefined) {
        const collapsedState = this.loadState(node);
        if (collapsedState) {
          this.collapsed.set(true);
        } else {
          this.collapsed.set(false);
        }
      }
    });
  }

  toggle() {
    const node = this.node();

    const newState = !this.collapsed();
    this.saveState(node, newState);
    this.collapsed.set(newState);
  }

  private loadState(node: CiiSummaryNode): boolean {
    const key = 'collapsed-' + node.code;
    const stateString = localStorage.getItem(key);
    return stateString == 'true';
  }

  private saveState(node: CiiSummaryNode, collapsed: boolean): void {
    const key = 'collapsed-' + node.code;
    const stateString = collapsed ? 'true' : 'false';
    localStorage.setItem(key, stateString);
  }
}
