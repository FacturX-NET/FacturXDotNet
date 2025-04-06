import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { API_BASE_URL } from '../../app.config';
import { CrossIndustryInvoice, ICrossIndustryInvoice, IXmpMetadata, IXmpMetadataAndCrossIndustryInvoice, XmpMetadata, XmpMetadataAndCrossIndustryInvoice } from './api.models';

@Injectable({
  providedIn: 'root',
})
export class ExtractApi {
  private httpClient = inject(HttpClient);
  private baseUrl = inject(API_BASE_URL);

  extractXmpMetadata(file: File): Observable<IXmpMetadata> {
    const url = `${this.baseUrl}/extract/xmp`;
    const headers = new HttpHeaders().append('Content-Disposition', 'multipart/form-data');
    const formData = new FormData();
    formData.append('file', file, file.name);

    return this.httpClient.post(url, formData, { headers }).pipe(map((result) => XmpMetadata.fromJS(result)));
  }

  extractCrossIndustryInvoice(file: File): Observable<ICrossIndustryInvoice> {
    const url = `${this.baseUrl}/extract/cii`;
    const headers = new HttpHeaders().append('Content-Disposition', 'multipart/form-data');
    const formData = new FormData();
    formData.append('file', file, file.name);

    return this.httpClient.post(url, formData, { headers }).pipe(map((result) => CrossIndustryInvoice.fromJS(result)));
  }

  extractXmpAndCrossIndustryInvoice(file: File): Observable<IXmpMetadataAndCrossIndustryInvoice> {
    const url = `${this.baseUrl}/extract/xmp-cii`;
    const headers = new HttpHeaders().append('Content-Disposition', 'multipart/form-data');
    const formData = new FormData();
    formData.append('file', file, file.name);

    return this.httpClient.post(url, formData, { headers }).pipe(map((result) => XmpMetadataAndCrossIndustryInvoice.fromJS(result)));
  }
}
