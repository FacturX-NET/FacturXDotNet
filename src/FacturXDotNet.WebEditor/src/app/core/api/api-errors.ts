import { HttpErrorResponse } from '@angular/common/http';

export function getApiErrorMessage(err: unknown): string {
  if (err instanceof HttpErrorResponse) {
    if (err.status === 0) {
      return "Can't connect to server. Please check your internet connection.";
    }

    return err.message;
  }

  return 'Unknown error';
}
