import { Component, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ToastComponent } from './toast.component';
import { ToastInstance, ToastService } from './toast.service';

@Component({
  selector: 'app-toaster',
  imports: [ToastComponent],
  templateUrl: './toaster.component.html',
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
