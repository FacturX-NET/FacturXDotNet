import { inject, Injectable } from '@angular/core';
import { InfoApi } from '../info.api';
import { rxResource } from '@angular/core/rxjs-interop';
import { forkJoin, map, switchMap } from 'rxjs';
import { Sbom } from '../../sbom';

@Injectable({
  providedIn: 'root',
})
export class ApiConstantsService {
  private infoApi = inject(InfoApi);

  info = rxResource({
    loader: () =>
      forkJoin({
        build: this.infoApi.getBuildInformation(),
        hosting: this.infoApi.getHostingInformation(),
        sbom: this.infoApi.getSbom().pipe(
          switchMap((sbomFile) => {
            return sbomFile.text();
          }),
          map((sbomContent) => JSON.parse(sbomContent) as Sbom),
        ),
      }),
  });
}
