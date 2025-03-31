import { Component, computed, effect, input, model, untracked } from '@angular/core';
import { CiiSummaryNode } from './cii-summary.component';
import { Collapse } from 'bootstrap';
import { ScrollToDirective } from '../../../core/directives/scroll-to.directive';

@Component({
  selector: 'app-cii-summary-node',
  imports: [ScrollToDirective],
  template: `
    <div class="nav-item d-flex gap-2 text-truncate">
      <a [scrollTo]="'#' + node().code">
        <span class="fw-semibold"> {{ node().code }} </span> - {{ node().name }}
      </a>
    </div>
    @if (collapsed() !== true) {
      @if (node().children; as children) {
        <div class="ps-2">
          @for (child of node().children; track child.code) {
            <app-cii-summary-node [node]="child" />
          }
        </div>
      }
    }
  `,
  styles: `
    a.active {
      font-weight: bold;
    }
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
