import { Injectable } from '@angular/core';
import { Observable, of, Subject } from 'rxjs';
import { getApiErrorMessage } from '../api/api-errors';

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  private counter = 0;
  private toastsSubject = new Subject<ToastInstance>();

  public get toasts(): Observable<ToastInstance> {
    return this.toastsSubject;
  }

  show(toast: Toast) {
    this.toastsSubject.next({ ...toast, id: this.counter++ });
  }

  showError(error: unknown, messageFactory?: (error: string) => string) {
    const errorMessage = getErrorMessage(error);
    this.show({ type: 'error', message: messageFactory === undefined ? errorMessage : messageFactory(errorMessage) });
    return of(void 0);
  }
}

function getErrorMessage(error: unknown) {
  if (error instanceof Error) {
    return error.message;
  }

  return getApiErrorMessage(error);
}

export type Toast = SuccessToast | InfoToast | ErrorToast;

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

export interface InfoToast extends BaseToast {
  readonly type: 'info';
}

export interface ErrorToast extends BaseToast {
  readonly type: 'error';
}
