import { Directive, ElementRef, HostListener, input } from '@angular/core';

@Directive({
  selector: '[scrollTo]',
})
export class ScrollToDirective {
  scrollTo = input.required<string>();

  constructor(elt: ElementRef) {
    elt.nativeElement.href = 'javascript:void 0;';
  }

  @HostListener('click')
  onClick() {
    const targetElement = document.querySelector(this.scrollTo()) as HTMLElement;
    if (targetElement === undefined || targetElement === null) {
      return;
    }

    const closestLabel = targetElement.closest('label') as HTMLElement;
    if (closestLabel !== undefined && closestLabel !== null) {
      closestLabel.focus({ preventScroll: true });
      closestLabel.scrollIntoView({ block: 'center', behavior: 'smooth', inline: 'nearest' });
    } else {
      targetElement.scrollIntoView({ block: 'center', behavior: 'smooth', inline: 'nearest' });
    }
  }
}
