import { AfterViewInit, Directive, ElementRef, HostListener, input, OnInit } from '@angular/core';

@Directive({
  selector: '[scrollTo]',
})
export class ScrollToDirective implements AfterViewInit {
  scrollTo = input.required<string>();

  constructor(private elt: ElementRef) {}

  ngAfterViewInit() {
    this.elt.nativeElement.href = this.scrollTo();
  }

  @HostListener('click', ['$event'])
  onClick(evt: MouseEvent) {
    evt.preventDefault();

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
