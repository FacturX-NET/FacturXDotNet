import { inject, Injectable } from '@angular/core';
import { InfoApi } from '../info.api';
import { rxResource } from '@angular/core/rxjs-interop';
import { forkJoin } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ApiConstantsService {
  private infoApi = inject(InfoApi);

  info = rxResource({
    loader: () => forkJoin({ build: this.infoApi.getBuildInformation(), hosting: this.infoApi.getHostingInformation(), dependencies: this.infoApi.getDependencies() }),
  });
}
