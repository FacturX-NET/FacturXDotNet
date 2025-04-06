import { catchError, of } from 'rxjs';
import { ToastService } from '../toasts/toast.service';

export function toastError(toastService: ToastService, message: string) {
  return catchError((err) => {
    toastService.show({ type: 'error', message: message });
    console.error(err);
    return of(void 0);
  });
}
