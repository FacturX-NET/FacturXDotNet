import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
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

  getDependencies(): Observable<IPackageDto[]> {
    const url = `${this.baseUrl}/info/dependencies`;
    return this.httpClient.get<IPackageDto[]>(url);
  }
}
