import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { API_BASE_URL } from '../../app.config';
import { CrossIndustryInvoice } from './api.models';

@Injectable({
  providedIn: 'root',
})
export class ExtractApi {
  private httpClient = inject(HttpClient);
  private baseUrl = inject(API_BASE_URL);

  extractCrossIndustryInvoice(file: File): Observable<CrossIndustryInvoice> {
    const url = `${this.baseUrl}/extract/cii`;
    const headers = new HttpHeaders().append('Content-Disposition', 'multipart/form-data');
    const formData = new FormData();
    formData.append('file', file, file.name);

    return this.httpClient.post(url, formData, { headers }).pipe(map((result) => CrossIndustryInvoice.fromJS(result)));
  }
}
