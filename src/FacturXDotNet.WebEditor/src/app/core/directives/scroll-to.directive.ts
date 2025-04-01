import { AfterViewInit, Directive, ElementRef, HostListener, inject, input } from '@angular/core';
import { Router } from '@angular/router';

@Directive({
  selector: '[scrollTo]',
})
export class ScrollToDirective implements AfterViewInit {
  scrollTo = input.required<string>();

  private elt = inject(ElementRef);
  private router = inject(Router);

  ngAfterViewInit() {
    this.elt.nativeElement.href = '#' + this.scrollTo();
  }

  @HostListener('click', ['$event'])
  onClick(evt: MouseEvent) {
    evt.preventDefault();

    const scrollTo = '#' + this.scrollTo();

    const targetElement = document.querySelector(scrollTo) as HTMLElement;
    if (targetElement === undefined || targetElement === null) {
      return;
    }

    this.router.navigateByUrl(scrollTo, { replaceUrl: true }).then();

    const closestLabel = targetElement.closest('label') as HTMLElement;
    if (closestLabel !== undefined && closestLabel !== null) {
      closestLabel.focus({ preventScroll: true });
      closestLabel.scrollIntoView({ block: 'center', behavior: 'smooth', inline: 'nearest' });
    } else {
      targetElement.scrollIntoView({ block: 'center', behavior: 'smooth', inline: 'nearest' });
    }
  }
}
