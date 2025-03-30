import {Component, ElementRef, input, output, viewChild} from '@angular/core';
import {takeUntilDestroyed, toObservable} from '@angular/core/rxjs-interop';
import {Toast} from 'bootstrap';
import {fromEvent, of, switchMap, take, tap} from 'rxjs';
import {ToastInstance} from './toast.service';

@Component({
  selector: 'app-toast',
  imports: [],
  templateUrl: './toast.component.html',
})
export class ToastComponent {
  public readonly toast = input.required<ToastInstance>();
  public readonly closed = output();

  protected bootstrapToast: Toast | undefined;
  protected readonly toastElt = viewChild<ElementRef>('toastElt');

  constructor() {
    toObservable(this.toastElt)
      .pipe(
        switchMap(toastElt => {
          if (toastElt == undefined) {
            return of({});
          }

          this.bootstrapToast = new Toast(toastElt.nativeElement, {
            autohide: this.toast().closeAfter !== 0,
            delay: this.toast().closeAfter ?? 5000,
          });

          this.bootstrapToast.show();

          return fromEvent(toastElt.nativeElement, 'hidden.bs.toast').pipe(
            take(1),
          );
        }),
        tap(() => this.hide()),
        takeUntilDestroyed(),
      )
      .subscribe();
  }

  private hide() {
    this.bootstrapToast?.dispose();
    this.closed.emit();
  }
}
