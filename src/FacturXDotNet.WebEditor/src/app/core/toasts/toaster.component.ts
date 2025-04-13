import { Component, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ToastComponent } from './toast.component';
import { ToastInstance, ToastService } from './toast.service';

@Component({
  selector: 'app-toaster',
  imports: [ToastComponent],
  template: `
    <div aria-atomic="true" aria-live="assertive" class="toast-container position-fixed top-0 start-50 translate-middle-x p-3">
      @for (toast of currentToasts(); track toast.id) {
        <app-toast (closed)="close(toast)" [toast]="toast"></app-toast>
      }
    </div>
  `,
})
export class ToasterComponent {
  protected currentToasts = signal<ToastInstance[]>([]);

  constructor(private toastService: ToastService) {
    this.toastService.toasts.pipe(takeUntilDestroyed()).subscribe((t) => this.currentToasts.update((value) => [...value, t]));
  }

  close(toast: ToastInstance) {
    this.currentToasts.update((value) => value.filter((v) => v.id != toast.id));
  }
}
