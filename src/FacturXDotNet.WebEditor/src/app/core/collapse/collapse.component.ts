import { Component, computed, effect, ElementRef, input, model, Signal, untracked, viewChild } from '@angular/core';
import { Collapse } from 'bootstrap';

@Component({
  selector: 'app-collapse',
  imports: [],
  template: `
    <a role="button" (click)="toggle()" class="d-flex align-items-center gap-2">
      <ng-content select="trigger" />
    </a>
    <ng-content />
    <div class="collapse" #collapse>
      <ng-content select="collapsible" />
    </div>
  `,
})
export class CollapseComponent {
  /**
   * The ID of the collapse element. The ID is used to store the state of the collapse in local storage.
   * The ID is optional, if not provided, the collapse will not be stored in local storage, which means that the state of the element will be reset on page reload.
   */
  id = input<string>();

  /**
   * The current state of the collapse.
   */
  collapsed = model<boolean>();

  /**
   * The element that will be collapsed.
   * @protected
   */
  protected collapseElt = viewChild.required<ElementRef>('collapse');

  /**
   * The Collapse instance from Bootstrap.
   * @private
   */
  private collapse: Signal<Collapse>;

  constructor() {
    this.collapse = computed(() => new Collapse(this.collapseElt().nativeElement, { toggle: false }));

    // Initialize the collapse state whenever the component is created
    effect(() => {
      const collapse = this.collapse();
      const collapsed = untracked(() => this.collapsed());

      switch (collapsed) {
        case undefined:
          break;
        case true:
          collapse.hide();
          break;
        case false:
          collapse.show();
          break;
      }
    });

    // Update the collapse state whenever the collapsed signal changes
    effect(() => {
      const collapsed = this.collapsed();
      const collapse = untracked(() => this.collapse());

      switch (collapsed) {
        case true:
          collapse.hide();
          break;
        case false:
          collapse.show();
          break;
      }
    });

    effect(() => {
      const id = this.id();
      const collapsed = untracked(() => this.collapsed());

      if (collapsed === undefined) {
        const collapsedState = this.loadState();
        if (collapsedState) {
          this.collapsed.set(true);
        } else {
          this.collapsed.set(false);
        }
      }
    });
  }

  toggle() {
    const newState = !this.collapsed();
    this.saveState(newState);
    this.collapsed.set(newState);
  }

  private loadState(): boolean {
    const key = 'collapsed-' + this.id();
    const stateString = localStorage.getItem(key);
    return stateString == 'true';
  }

  private saveState(collapsed: boolean): void {
    const key = 'collapsed-' + this.id();
    const stateString = collapsed ? 'true' : 'false';
    localStorage.setItem(key, stateString);
  }
}
