import { inject, Injectable } from '@angular/core';
import { EventType, Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { filter } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ScrollToService {
  private router = inject(Router);

  constructor() {
    this.router.events
      .pipe(
        filter((evt) => evt.type === EventType.NavigationEnd),
        takeUntilDestroyed(),
      )
      .subscribe((p) => {
        const index = p.urlAfterRedirects.indexOf('#');
        if (index < 0) {
          return;
        }

        const scrollTo = p.urlAfterRedirects.substring(index + 1);
        this.doScrollTo(scrollTo);
      });
  }

  scrollTo(id: string) {
    this.router.navigateByUrl('#' + id, {}).then();
  }

  private doScrollTo(id: string) {
    const scrollTo = '#' + id;

    const targetElement = document.querySelector(scrollTo) as HTMLElement;
    if (targetElement === undefined || targetElement === null) {
      return;
    }

    targetElement.focus({ preventScroll: true });
    targetElement.scrollIntoView({ block: 'center', behavior: 'smooth', inline: 'nearest' });
  }
}
