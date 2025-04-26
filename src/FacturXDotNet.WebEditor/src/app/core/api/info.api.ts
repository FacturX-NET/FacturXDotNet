import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { API_BASE_URL } from '../../app.config';
import { IBuildInformationDto, IHostingInformationDto, IPackageDto } from './api.models';

@Injectable({
  providedIn: 'root',
})
export class InfoApi {
  private httpClient = inject(HttpClient);
  private baseUrl = inject(API_BASE_URL);

  getBuildInformation(): Observable<IBuildInformationDto> {
    const url = `${this.baseUrl}/info/build`;
    return this.httpClient.get<IBuildInformationDto>(url);
  }

  getHostingInformation(): Observable<IHostingInformationDto> {
    const url = `${this.baseUrl}/info/hosting`;
    return this.httpClient.get<IHostingInformationDto>(url);
  }

  getSbom(): Observable<File> {
    const url = `${this.baseUrl}/info/sbom`;
    return this.httpClient.get(url, { observe: 'response', responseType: 'blob' }).pipe(
      map((response) => {
        if (response.body === null) {
          throw new Error('No response body');
        }

        const contentDisposition = response.headers.get('content-disposition');
        const filename = contentDisposition?.split(';')[1].split('filename')[1].split('=')[1].trim();
        return new File([response.body], filename ?? 'FacturXDotNet-API.sbom.json', { type: 'application/json' });
      }),
    );
  }
}
