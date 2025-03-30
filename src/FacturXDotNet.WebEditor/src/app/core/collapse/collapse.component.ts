import { Component, computed, effect, ElementRef, model, Signal, untracked, viewChild } from '@angular/core';
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
  collapsed = model<boolean>(false);

  protected collapseElt = viewChild.required<ElementRef>('collapse');
  private collapse: Signal<Collapse>;

  constructor() {
    this.collapse = computed(() => {
      const collapse = new Collapse(this.collapseElt().nativeElement, { toggle: false });

      if (untracked(() => this.collapsed())) {
        collapse.hide();
      } else {
        collapse.show();
      }

      return collapse;
    });

    effect(() => {
      if (this.collapsed()) {
        this.collapse().hide();
      } else {
        this.collapse().show();
      }
    });
  }

  toggle() {
    this.collapsed.set(!this.collapsed());
  }
}
