import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  private counter = 0;

  public get toasts(): Observable<ToastInstance> {
    return this.toastsSubject;
  }
  private toastsSubject = new Subject<ToastInstance>();

  show(toast: Toast) {
    this.toastsSubject.next({ ...toast, id: this.counter++ });
  }
}

export type Toast = SuccessToast | ErrorToast;

export type ToastInstance = Toast & {
  readonly id: number;
};

interface BaseToast {
  readonly message: string;
  readonly closeAfter?: number;
}

export interface SuccessToast extends BaseToast {
  readonly type: 'success';
}

export interface ErrorToast extends BaseToast {
  readonly type: 'error';
}
