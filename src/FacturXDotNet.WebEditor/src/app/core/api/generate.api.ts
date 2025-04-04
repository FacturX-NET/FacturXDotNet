import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { API_BASE_URL } from '../../app.config';
import { CrossIndustryInvoice, ICrossIndustryInvoice } from './api.models';

@Injectable({
  providedIn: 'root',
})
export class GenerateApi {
  private httpClient = inject(HttpClient);
  private baseUrl = inject(API_BASE_URL);

  generateCrossIndustryInvoice(cii: ICrossIndustryInvoice): Observable<Blob> {
    const url = `${this.baseUrl}/generate/cii`;
    const ciiObj = new CrossIndustryInvoice(cii);

    return this.httpClient.post(url, ciiObj.toJSON(), { responseType: 'blob' });
  }
}
