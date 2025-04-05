import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../app.config';

@Injectable({
  providedIn: 'root',
})
export class InfoApi {
  private httpClient = inject(HttpClient);
  private baseUrl = inject(API_BASE_URL);

  getBuildInformation(): Observable<BuildInformation> {
    const url = `${this.baseUrl}/info/build`;
    return this.httpClient.get<BuildInformation>(url);
  }

  getDependencies(): Observable<Package[]> {
    const url = `${this.baseUrl}/info/dependencies`;
    return this.httpClient.get<Package[]>(url);
  }
}

export interface BuildInformation {
  version: string;
  buildDate: Date;
}

export interface Package {
  name: string;
  author: string;
  version: string;
  license: string;
  link: string;
}
