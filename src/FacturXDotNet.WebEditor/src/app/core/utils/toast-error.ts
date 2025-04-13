import { catchError, of } from 'rxjs';
import { ToastService } from '../toasts/toast.service';

export function toastError(toastService: ToastService, messageFactory: (error: string) => string) {
  return catchError((err) => {
    const errorMessage = getErrorMessage(err);
    toastService.show({ type: 'error', message: messageFactory(errorMessage) });
    return of(void 0);
  });
}

function getErrorMessage(err: unknown): string {
  if (err instanceof Error) {
    return err.message;
  }

  if (err instanceof Object) {
    return err.toString();
  }

  return 'Unknown error';
}
