import { inject, Injectable } from '@angular/core';
import { InfoApi } from '../info.api';
import { rxResource } from '@angular/core/rxjs-interop';
import { map, switchMap } from 'rxjs';
import { Sbom } from '../../sbom';

@Injectable({
  providedIn: 'root',
})
export class ApiConstantsService {
  private infoApi = inject(InfoApi);

  buildInfo = rxResource({ loader: () => this.infoApi.getBuildInformation() });
  hostingInfo = rxResource({ loader: () => this.infoApi.getHostingInformation() });
  sbom = rxResource({
    loader: () =>
      this.infoApi.getSbom().pipe(
        switchMap((sbomFile) => sbomFile.text()),
        map((sbomContent) => JSON.parse(sbomContent) as Sbom),
      ),
  });
}
