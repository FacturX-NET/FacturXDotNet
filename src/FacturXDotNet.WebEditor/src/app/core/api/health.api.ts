import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '../../app.config';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class HealthApi {
  private httpClient = inject(HttpClient);
  private baseUrl = inject(API_BASE_URL);

  getHealth(): Observable<string> {
    const url = `${this.baseUrl}/health`;
    return this.httpClient.get(url, { responseType: 'text' });
  }
}
