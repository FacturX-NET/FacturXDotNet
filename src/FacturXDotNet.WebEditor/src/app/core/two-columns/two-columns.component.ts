import { Component, computed, HostListener, input, linkedSignal, output, signal } from '@angular/core';
import { NgStyle } from '@angular/common';

@Component({
  selector: 'app-two-columns',
  imports: [NgStyle],
  template: `
    <div class="h-100 d-flex overflow-hidden">
      <div class="h-100" [ngStyle]="{ 'width.px': leftColumnWidth() }">
        <ng-content select="[left]" />
      </div>
      <div
        class="d-flex align-items-center justify-content-center"
        style="width: {{ resizeHandleWidth() }}px; cursor: col-resize;"
        (mousedown)="dragStart($event)"
        (touchstart)="dragStart($event)"
      >
        <i class="bi bi-grip-vertical text-body-secondary"></i>
      </div>
      <div class="h-100" [ngStyle]="{ 'width.px': rightColumnWidth() }">
        <ng-content select="[right]" />
      </div>
    </div>
  `,
  styles: ``,
})
export class TwoColumnsComponent {
  /**
   * The key used to store the width of the right column in the local storage. If not provided, the width will not be saved.
   * @protected
   */
  key = input<string>();
  resizeHandleWidth = input(16);
  dragging = output<boolean>();

  protected totalWidth = signal(window.innerWidth);
  protected rightColumnWidth = linkedSignal<{ key: string | undefined; totalWidth: number }, number>({
    source: () => ({ key: this.key(), totalWidth: this.totalWidth() }),
    computation: (input, previous) => {
      if (previous !== undefined) {
        return previous?.value;
      }

      if (input.key === undefined) {
        return input.totalWidth / 2;
      }

      return this.loadRightColumnWidth(input.key) ?? input.totalWidth / 2;
    },
  });
  protected leftColumnWidth = computed(() => this.totalWidth() - this.rightColumnWidth() - this.resizeHandleWidth());

  private resizing = false;

  @HostListener('window:resize', ['$event'])
  resize(event: Event) {
    const target = event.target as Window;
    const width = target?.innerWidth ?? 0;
    this.totalWidth.set(width);
  }

  @HostListener('window:mousemove', ['$event'])
  mousemove(event: Event) {
    if (this.resizing) {
      event.preventDefault();
      this.drag(event);
    }
  }

  @HostListener('window:touchmove', ['$event'])
  touchmove(event: Event) {
    if (this.resizing) {
      event.preventDefault();
      this.drag(event);
    }
  }

  @HostListener('window:mouseup', ['$event'])
  mouseup(event: Event) {
    if (this.resizing) {
      event.preventDefault();
      this.dragEnd();
    }
  }

  @HostListener('window:touchend', ['$event'])
  touchend(event: Event) {
    if (this.resizing) {
      event.preventDefault();
      this.dragEnd();
    }
  }

  protected dragStart(event: Event) {
    event.preventDefault();
    this.resizing = true;
    this.dragging.emit(true);
  }

  protected drag(event: Event) {
    event.preventDefault();

    if (this.resizing) {
      const width = this.totalWidth();
      const x = event.type === 'mousemove' ? (event as MouseEvent).clientX : (event as TouchEvent).touches[0].clientX;
      const newWidth = width - x - this.resizeHandleWidth() / 2;
      this.rightColumnWidth.set(newWidth);

      const key = this.key();
      if (key !== undefined) {
        this.saveRightColumnWidth(key, newWidth);
      }
    }
  }

  protected dragEnd(event?: Event) {
    event?.preventDefault();
    this.resizing = false;
    this.dragging.emit(false);
  }

  private saveRightColumnWidth(key: string, width: number) {
    const localStorageKey = `two-columns-${key}`;
    localStorage.setItem(localStorageKey, width.toString());
  }

  private loadRightColumnWidth(key: string): number | undefined {
    const localStorageKey = `two-columns-${key}`;

    const widthString = localStorage.getItem(localStorageKey);
    if (widthString === null) {
      return undefined;
    }

    return parseInt(widthString, 10);
  }
}
