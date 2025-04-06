import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AboutLicensesComponent } from './about-licenses.component';
import { environment } from '../../../environments/environment';
import { InfoApi, Package } from '../../core/api/info.api';
import { rxResource } from '@angular/core/rxjs-interop';
import { DatePipe } from '@angular/common';
import { API_BASE_URL } from '../../app.config';
import licenses from '../../../licenses/licenses.json';

@Component({
  selector: 'app-about',
  template: `
    <div class="container py-4">
      <div class="py-4">
        <a routerLink="/editor"><i class="bi bi-chevron-left"></i> Back to editor</a>
      </div>
      <div class="h-100 row">
        <div class="col-3 d-none d-md-block"></div>
        <div class="h-100 col">
          <div class="my-4">
            <h2>About FacturX.NET</h2>

            Available as
            <ul>
              <li>a <a href="https://github.com/FacturX-NET/FacturXDotNet/tree/master/FacturXDotNet.CLI">CLI tool</a></li>
              <li>a <a href="https://github.com/FacturX-NET/FacturXDotNet/tree/master/FacturXDotNet.API">web API</a> (used by this application)</li>
              <li>a <a href="https://github.com/FacturX-NET/FacturXDotNet/tree/master/FacturXDotNet.WebEditor">web editor</a> (this application)</li>
              <li>a <a href="https://github.com/FacturX-NET/FacturXDotNet/tree/master/FacturXDotNet">.NET library</a></li>
            </ul>

            <div>
              <span class="fw-bold">Links</span>
              <ul>
                <li><span class="fw-semibold">Github</span>: <a href="https://github.com/FacturX-NET/FacturXDotNet">https://github.com/FacturX-NET/FacturXDotNet</a></li>
                <li><span class="fw-semibold">Nuget</span>: <a href="https://www.nuget.org/packages/FacturXDotNet/">https://www.nuget.org/packages/FacturXDotNet/</a></li>
              </ul>
            </div>

            <p><span class="fw-bold">License</span>: The tools are open source and under the <span class="fw-bold">MIT</span> License, feel free to use, modify, and share.</p>

            <p>Copyright © 2025 Ismail Bennani</p>
          </div>

          <div class="my-4">
            <h2>API</h2>

            @if (apiDependencies.isLoading()) {
              <div class="placeholder-glow">
                <p>
                  <span class="placeholder col-6"></span>
                  <span class="placeholder col-8"></span>
                </p>

                <p>
                  <span class="placeholder col-2"></span>
                </p>

                <div class="list-group">
                  <div class="list-group-item">
                    <div class="d-flex flex-column gap-2">
                      <span class="placeholder col-3"></span>
                      <span class="placeholder col-6"></span>
                    </div>
                  </div>
                  <div class="list-group-item">
                    <div class="d-flex flex-column gap-2">
                      <span class="placeholder col-2"></span>
                      <span class="placeholder col-7"></span>
                    </div>
                  </div>
                </div>
              </div>
            } @else if (apiDependencies.hasValue()) {
              <p>
                The API used by this application is located at <a [href]="apiUrl">{{ apiUrl }}</a
                >. <br />
                It is currently in version <span class="fw-semibold">{{ apiBuildInformation.value()?.version ?? '???' }}</span> and was built on
                {{ (apiBuildInformation.value()?.buildDate | date) ?? '???' }}.
              </p>

              <p class="fw-bold">Dependencies</p>
              <app-about-licenses [packages]="apiDependencies.value() ?? []"></app-about-licenses>
            } @else {
              <div class="alert alert-danger">
                <i class="bi bi-x-circle-fill text-danger"></i> The API server at <a [href]="apiUrl">{{ apiUrl }}</a> is unreachable.
              </div>
            }
          </div>

          <div class="my-4">
            <h2>Web Editor</h2>

            <p>
              The web editor (this application) is currently in version <span class="fw-semibold">{{ webEditorVersion }}</span> and was built on {{ webEditorBuildDate | date }}.
            </p>

            <p>
              The web editor is a demo application that allows you to create and edit FacturX documents. It is built using Angular and TypeScript. It is also open-source and under
              the <span class="fw-bold">MIT</span> license, and available
              <a href="https://github.com/FacturX-NET/FacturXDotNet/tree/main/src/Tests.FacturXDotNet.WebEditor">on GitHub</a>.
            </p>

            <p class="fw-bold">Dependencies</p>
            <app-about-licenses [packages]="webEditorPackages"></app-about-licenses>
          </div>
        </div>
      </div>
    </div>
  `,
  imports: [RouterLink, AboutLicensesComponent, DatePipe],
})
export class AboutPage {
  private infoApi = inject(InfoApi);
  protected apiUrl = inject(API_BASE_URL);

  protected apiBuildInformation = rxResource({ loader: () => this.infoApi.getBuildInformation() });
  protected apiDependencies = rxResource({ loader: () => this.infoApi.getDependencies() });
  protected webEditorVersion = environment.version;
  protected webEditorBuildDate = environment.buildDate;
  protected webEditorPackages: Package[] = licenses.map((l) => ({
    name: l.name,
    author: l.author,
    version: l.installedVersion,
    license: l.licenseType,
    link: l.link,
  }));
}
