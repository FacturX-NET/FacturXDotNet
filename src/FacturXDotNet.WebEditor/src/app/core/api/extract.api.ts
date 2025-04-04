import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CrossIndustryInvoice } from '../facturx-models/cii/cross-industry-invoice';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../app.config';

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
    formData.append('xml', file, file.name);

    return this.httpClient.post<CrossIndustryInvoice>(url, formData, { headers });
  }
}
