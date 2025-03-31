import { Directive, ElementRef, HostListener, input, Input } from '@angular/core';

@Directive({
  selector: '[scrollTo]',
})
export class ScrollToDirective {
  scrollTo = input.required<string>();

  constructor(elt: ElementRef) {
    elt.nativeElement.href = 'javascript:void;';
  }

  @HostListener('click')
  onClick() {
    const targetElement = document.querySelector(this.scrollTo()) as HTMLElement;
    if (targetElement) {
      targetElement.scrollIntoView({ block: 'center', behavior: 'smooth', inline: 'nearest' });
      targetElement.focus();
    }
  }
}
