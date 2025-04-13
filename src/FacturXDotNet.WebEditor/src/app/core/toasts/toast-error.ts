import { catchError, EMPTY, of, OperatorFunction } from 'rxjs';
import { ToastService } from './toast.service';

export function toastError<T>(toastService: ToastService, messageFactory: (error: string) => string): OperatorFunction<T, T> {
  return catchError((err) => {
    console.error(err);
    toastService.showError(err, messageFactory);
    return EMPTY;
  });
}
