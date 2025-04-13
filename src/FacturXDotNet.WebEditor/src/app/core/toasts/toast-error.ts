import { catchError, of } from 'rxjs';
import { ToastService } from './toast.service';

export function toastError(toastService: ToastService, messageFactory: (error: string) => string) {
  return catchError((err) => {
    toastService.showError(err, messageFactory);
    return of(void 0);
  });
}
