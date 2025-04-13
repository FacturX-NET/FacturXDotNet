import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map, Observable, of, throwError } from 'rxjs';
import { API_BASE_URL } from '../../app.config';
import { CrossIndustryInvoice, ICrossIndustryInvoice } from './api.models';

@Injectable({
  providedIn: 'root',
})
export class ValidateApi {
  private httpClient = inject(HttpClient);
  private baseUrl = inject(API_BASE_URL);

  validateCrossIndustryInvoice(cii: ICrossIndustryInvoice): Observable<ValidationResult> {
    const url = `${this.baseUrl}/validate/cii`;
    const ciiObj = new CrossIndustryInvoice(cii);

    return this.httpClient.post(url, ciiObj.toJSON()).pipe(
      map(() => ({ valid: true })),
      catchError((err) => {
        if (err.status === 400 && err.error !== undefined) {
          const errors = err.error.errors as Record<string, string[]>;
          return of({ valid: false, errors });
        }

        return throwError(() => err);
      }),
    );
  }
}

export interface ValidationResult {
  valid: boolean;
  errors?: Record<string, string[]>;
}
