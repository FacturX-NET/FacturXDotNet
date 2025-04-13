import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, Subject } from 'rxjs';

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

  showError(error: unknown, messageFactory: (error: string) => string) {
    const errorMessage = getErrorMessage(error);
    this.show({ type: 'error', message: messageFactory(errorMessage) });
    return of(void 0);
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

function getErrorMessage(err: unknown): string {
  if (err instanceof Error) {
    return err.message;
  }

  if (err instanceof HttpErrorResponse) {
    if (err.status === 0) {
      return "Can't connect to server. Please check your internet connection.";
    }

    return `${err.status} ${err.statusText} - ${err.message}`;
  }

  return 'Unknown error';
}
