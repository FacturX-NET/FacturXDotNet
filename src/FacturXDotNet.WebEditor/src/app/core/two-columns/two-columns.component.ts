import { booleanAttribute, Component, computed, HostListener, input, model, numberAttribute, output, signal } from '@angular/core';
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
        style="width: {{ resizeHandleWidth() }}px;"
        [class.draggable]="draggable()"
        (mousedown)="dragStart($event)"
        (touchstart)="dragStart($event)"
      >
        @if (draggable()) {
          <i class="bi bi-grip-vertical text-body-secondary"></i>
        }
      </div>
      <div class="h-100" [ngStyle]="{ 'width.px': rightColumnWidth() }">
        <ng-content select="[right]" />
      </div>
    </div>
  `,
  styles: `
    .draggable {
      cursor: col-resize;
    }
  `,
})
export class TwoColumnsComponent {
  rightColumnWidth = model.required<number>();
  resizeHandleWidth = input(16, { transform: numberAttribute });
  draggable = input(false, { transform: booleanAttribute });
  dragging = output<boolean>();

  protected totalWidth = signal(window.innerWidth);
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

    if (!this.draggable()) {
      return;
    }

    this.resizing = true;
    this.dragging.emit(true);
  }

  protected drag(event: Event) {
    event.preventDefault();

    if (!this.draggable()) {
      return;
    }

    if (this.resizing) {
      const width = this.totalWidth();
      const x = event.type === 'mousemove' ? (event as MouseEvent).clientX : (event as TouchEvent).touches[0].clientX;
      const newWidth = width - x - this.resizeHandleWidth() / 2;
      this.rightColumnWidth.set(newWidth);
    }
  }

  protected dragEnd(event?: Event) {
    event?.preventDefault();
    this.resizing = false;
    this.dragging.emit(false);
  }
}
