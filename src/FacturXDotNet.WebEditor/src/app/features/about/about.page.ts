import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AboutLicensesComponent } from './about-licenses.component';
import { environment } from '../../../environments/environment';
import { InfoApi, Package } from '../../core/api/info.api';
import { rxResource } from '@angular/core/rxjs-interop';
import { DatePipe, NgOptimizedImage } from '@angular/common';
import { API_BASE_URL } from '../../app.config';
import licenses from '../../../licenses/licenses.json';
import { ApiServerStatusComponent } from '../../core/api/components/api-server-status.component';

@Component({
  selector: 'app-about',
  template: `
    <div class="container py-4">
      <div class="py-4">
        <a routerLink="/editor"><i class="bi bi-chevron-left"></i> Back to editor</a>
      </div>

      <div class="card mb-4">
        <div class="card-body">
          <h2 class="card-title">About</h2>
          <div class="row">
            <div class="col-6">
              <div class="w-100 h-100 d-flex align-items-center justify-content-center">
                <img ngSrc="logo.png" width="369" height="92" alt="FacturX.NET" />
              </div>
            </div>
            <div class="col">
              Available as
              <ul>
                <li>a <a href="https://github.com/FacturX-NET/FacturXDotNet/tree/master/FacturXDotNet.CLI">CLI tool</a></li>
                <li>a <a href="https://github.com/FacturX-NET/FacturXDotNet/tree/master/FacturXDotNet.API">web API</a> (used by this application)</li>
                <li>a <a href="https://github.com/FacturX-NET/FacturXDotNet/tree/master/FacturXDotNet.WebEditor">web editor</a> (this application)</li>
                <li>a <a href="https://github.com/FacturX-NET/FacturXDotNet/tree/master/FacturXDotNet">.NET library</a></li>
              </ul>

              <span class="fw-bold">Links</span>
              <ul>
                <li><span class="fw-semibold">Github</span>: <a href="https://github.com/FacturX-NET/FacturXDotNet">https://github.com/FacturX-NET/FacturXDotNet</a></li>
                <li><span class="fw-semibold">Nuget</span>: <a href="https://www.nuget.org/packages/FacturXDotNet/">https://www.nuget.org/packages/FacturXDotNet/</a></li>
              </ul>
            </div>
          </div>

          <div class="mx-auto pb-4 px-2 py-5">
            <div class="d-flex justify-content-evenly">
              <div class="position-relative col-3">
                <h5><i class="bi bi-envelope-at"></i> Contact</h5>
                <p>
                  Have questions about FacturX.NET?<br />
                  <a class="stretched-link" href="mailto:contact@facturxdotnet.org">Drop me a message</a>.
                </p>
              </div>
              <div class="position-relative col-3">
                <h5><i class="bi bi-chat-dots"></i> Issue</h5>
                <p>
                  Encountered a bug or unexpected behavior?<br />
                  Report it by <a class="stretched-link" href="https://github.com/FacturX-NET/FacturXDotNet">opening an issue</a>.
                </p>
              </div>
              <div class="position-relative col-3">
                <h5><i class="bi bi-star"></i> Support</h5>
                <p>
                  Enjoying FacturX.NET?<br />
                  Show your support by <a class="stretched-link" href="https://github.com/FacturX-NET/FacturXDotNet">starring the project</a>.
                </p>
              </div>
            </div>
          </div>

          <p class="text-center">
            <strong>© 2025 Ismail Bennani</strong>, with <i class="bi-heart-fill"></i> and <i class="bi bi-cup-hot-fill"></i><br />
            The tools are open source and under the MIT License, feel free to use, modify, and share.
          </p>
        </div>
      </div>

      <div class="row gy-4">
        <div class="col">
          <div class="card">
            <div class="card-body">
              <h2 class="card-title">API</h2>

              <p class="position-relative">
                @if (!status.loading()) {
                  <a class="pe-2 stretched-link" [href]="apiUrl">{{ apiUrl }}</a>
                }
                <app-api-server-status #status />
              </p>

              @if (apiDependencies.isLoading() || apiBuildInformation.isLoading()) {
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
              } @else {
                @if (apiBuildInformation.value(); as apiBuildInformation) {
                  <p>
                    The API server is currently in version <span class="fw-semibold">{{ apiBuildInformation.version }}</span> and was built on
                    {{ apiBuildInformation.buildDate | date }}.
                  </p>
                }

                @if (apiDependencies.value(); as dependencies) {
                  <p class="fw-bold">Dependencies</p>
                  <app-about-licenses [packages]="dependencies"></app-about-licenses>
                }
              }
            </div>
          </div>
        </div>

        <div class="col">
          <div class="card">
            <div class="card-body">
              <h2 class="card-title">Web Editor</h2>

              <p>
                The web editor (this application) is currently in version <span class="fw-semibold">{{ webEditorVersion }}</span> and was built on {{ webEditorBuildDate | date }}.
              </p>

              <p>
                The web editor is a demo application that allows you to create and edit FacturX documents. It is built using Angular and TypeScript. It is also open-source and
                under the <span class="fw-bold">MIT</span> license, and available
                <a href="https://github.com/FacturX-NET/FacturXDotNet/tree/main/src/Tests.FacturXDotNet.WebEditor">on GitHub</a>.
              </p>

              <p class="fw-bold">Dependencies</p>
              <app-about-licenses [packages]="webEditorPackages"></app-about-licenses>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  imports: [RouterLink, AboutLicensesComponent, DatePipe, ApiServerStatusComponent, NgOptimizedImage],
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
