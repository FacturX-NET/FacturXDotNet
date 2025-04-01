import { AfterViewInit, Directive, ElementRef, HostListener, inject, input } from '@angular/core';
import { Router } from '@angular/router';
import { ScrollToService } from './scroll-to.service';

@Directive({
  selector: '[scrollTo]',
})
export class ScrollToDirective implements AfterViewInit {
  scrollTo = input.required<string>();

  private elt = inject(ElementRef);
  private scrollToService = inject(ScrollToService);

  ngAfterViewInit() {
    this.elt.nativeElement.href = '#' + this.scrollTo();
  }

  @HostListener('click', ['$event'])
  onClick(evt: MouseEvent) {
    evt.preventDefault();
    this.scrollToService.scrollTo(this.scrollTo());
  }
}
