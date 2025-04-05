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

  generateFacturX(pdf: Blob, cii: ICrossIndustryInvoice): Observable<File> {
    const url = `${this.baseUrl}/generate/facturx`;
    const ciiObj = new CrossIndustryInvoice(cii);
    const ciiObjJson = ciiObj.toJSON();
    const ciiObjString = JSON.stringify(ciiObjJson);
    const ciiBlob = new Blob([ciiObjString], { type: 'application/json' });

    const headers = new HttpHeaders().append('Content-Disposition', 'multipart/form-data');
    const formData = new FormData();
    formData.append('pdf', pdf, 'base.pdf');
    formData.append('cii', ciiBlob, 'factur-x.xml');

    return this.httpClient.post(url, formData, { headers, observe: 'response', responseType: 'blob' }).pipe(
      map((response): File => {
        if (response.body === null) {
          throw new Error('No response body');
        }

        const contentDisposition = response.headers.get('Content-Disposition');
        const filename = contentDisposition?.split(';')[1].split('filename')[1].split('=')[1].trim();
        return new File([response.body], filename ?? 'invoice.pdf');
      }),
    );
  }

  generateCrossIndustryInvoice(cii: ICrossIndustryInvoice): Observable<File> {
    const url = `${this.baseUrl}/generate/cii`;
    const ciiObj = new CrossIndustryInvoice(cii);

    return this.httpClient.post(url, ciiObj.toJSON(), { observe: 'response', responseType: 'blob' }).pipe(
      map((response): File => {
        if (response.body === null) {
          throw new Error('No response body');
        }

        const contentDisposition = response.headers.get('content-disposition');
        const filename = contentDisposition?.split(';')[1].split('filename')[1].split('=')[1].trim();
        return new File([response.body], filename ?? 'factur-x.xml');
      }),
    );
  }
}
