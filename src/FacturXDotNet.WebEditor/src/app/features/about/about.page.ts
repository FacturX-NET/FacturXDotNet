import { Component, computed, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AboutSbomComponent } from './about-sbom.component';
import { environment } from '../../../environments/environment';
import { DatePipe, NgOptimizedImage } from '@angular/common';
import { API_BASE_URL } from '../../app.config';
import sbom from '../../../dependencies/sbom.json';
import { ApiServerStatusComponent } from '../../core/api/components/api-server-status.component';
import { ApiConstantsService } from '../../core/api/services/api-constants.service';

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
          <div class="row gy-4">
            <div class="col col-lg-6">
              <div class="w-100 h-100 d-flex align-items-center justify-content-center">
                <img class="d-none d-sm-block" ngSrc="logo.png" width="369" height="92" alt="FacturX.NET" />
                <img class="d-block d-sm-none" ngSrc="logo.png" width="185" height="46" alt="FacturX.NET" />
              </div>
            </div>
            <div class="col">
              Available as
              <ul>
                <li>
                  a CLI tool:
                  <a href="https://www.nuget.org/packages/FacturXDotNet.CLI">nuget</a>,
                  <a href="https://github.com/FacturX-NET/FacturXDotNet/tree/master/FacturXDotNet.CLI">source</a>
                </li>
                <li>
                  a web API:
                  <a href="https://api.facturxdotnet.org">app</a>,
                  <a href="https://github.com/FacturX-NET/FacturXDotNet/tree/master/FacturXDotNet.API">source</a>
                </li>
                <li>
                  a web app:
                  <a href="https://editor.facturxdotnet.org">app</a>,
                  <a href="https://github.com/FacturX-NET/FacturXDotNet/tree/master/FacturXDotNet.WebEditor">source</a>
                </li>
                <li>
                  a .NET library:
                  <a href="https://www.nuget.org/packages/FacturXDotNet">nuget</a>,
                  <a href="https://github.com/FacturX-NET/FacturXDotNet/tree/master/FacturXDotNet">source</a>
                </li>
              </ul>

              <span class="fw-bold">Links</span>
              <ul>
                <li><span class="fw-semibold">Github</span>: <a href="https://github.com/FacturX-NET/FacturXDotNet">https://github.com/FacturX-NET/FacturXDotNet</a></li>
                <li><span class="fw-semibold">Nuget</span>: <a href="https://www.nuget.org/packages/FacturXDotNet/">https://www.nuget.org/packages/FacturXDotNet/</a></li>
              </ul>
            </div>
          </div>

          <div class="mx-auto pb-4 px-2 py-5">
            <div class="row text-center">
              <div class="position-relative col">
                <h5><i class="bi bi-envelope-at"></i> Contact</h5>
                <p>
                  Have questions about FacturX.NET?<br />
                  <a class="stretched-link" href="mailto:contact@facturxdotnet.org">Drop me a message</a>.
                </p>
              </div>
              <div class="position-relative col">
                <h5><i class="bi bi-chat-dots"></i> Issue</h5>
                <p>
                  Encountered a bug or unexpected behavior?<br />
                  Report it by <a class="stretched-link" href="https://github.com/FacturX-NET/FacturXDotNet">opening an issue</a>.
                </p>
              </div>
              <div class="position-relative col">
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
            The tools are open source and released under the MIT License, feel free to use, modify, and share.
          </p>
        </div>
      </div>

      <div class="row gy-4">
        <div class="col-6">
          <div class="card">
            <div class="card-body">
              <h2 class="card-title">API</h2>

              <p class="d-flex gap-2 position-relative">
                @if (!status.loading()) {
                  <a class="stretched-link" [href]="apiUrl">{{ apiUrl }}</a>
                }
                <app-api-server-status #status />
              </p>

              @if (apiConstants.isLoading()) {
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
                @if (apiConstants.value(); as apiConstants) {
                  <p>
                    The API server is currently in version <span class="fw-semibold text-truncate">{{ apiVersion() }}</span> and was built on
                    <span class="text-truncate">{{ apiConstants.build.buildDate | date }}</span
                    >. <br />

                    @if (apiVersionBuildMetadata(); as apiVersionBuildMetadata) {
                      <span class="small text-body-tertiary"><span class="fw-semibold">Build metadata</span>: {{ apiVersionBuildMetadata }}</span>
                    }
                  </p>

                  <hr />

                  <app-about-licenses [sbom]="apiConstants.sbom" sbomName="FacturXDotNet-API.bom.json"></app-about-licenses>
                }
              }
            </div>
          </div>
        </div>

        <div class="col-6">
          <div class="card">
            <div class="card-body">
              <h2 class="card-title">Web Editor</h2>

              <p>
                The web editor (this application) is currently in version <span class="fw-semibold text-truncate">{{ webEditorVersion }}</span> and was built on
                <span class="text-truncate">{{ webEditorBuildDate | date }}</span
                >. <br />

                @if (webEditorBuildMetadata) {
                  <span class="small text-body-tertiary"><span class="fw-semibold">Build metadata</span>: {{ webEditorBuildMetadata }}</span>
                }
              </p>

              <p>
                The web editor is a demo application that allows you to create and edit FacturX documents. It is built using Angular and TypeScript. It is also open-source and
                released under the <span class="fw-bold">MIT</span> license, and available
                <a href="https://github.com/FacturX-NET/FacturXDotNet/tree/main/src/Tests.FacturXDotNet.WebEditor">on GitHub</a>.
              </p>

              <hr />

              <app-about-licenses [sbom]="webEditorSbom" sbomName="FacturXDotNet-WebEditor.bom.json"></app-about-licenses>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  imports: [RouterLink, AboutSbomComponent, DatePipe, ApiServerStatusComponent, NgOptimizedImage],
})
export class AboutPage {
  protected webEditorSbom = sbom;

  protected webEditorVersion = removeBuildInformation(environment.version);
  protected webEditorBuildMetadata = extractBuildInformation(environment.version);
  protected webEditorBuildDate = environment.buildDate;

  protected apiUrl = inject(API_BASE_URL);

  private apiConstantsService = inject(ApiConstantsService);
  protected apiVersion = computed(() => {
    const apiConstants = this.apiConstantsService.info.value();
    if (apiConstants === undefined) {
      return undefined;
    }

    return removeBuildInformation(apiConstants.build.version);
  });

  protected apiVersionBuildMetadata = computed(() => {
    const apiConstants = this.apiConstantsService.info.value();
    if (apiConstants === undefined) {
      return undefined;
    }

    return extractBuildInformation(apiConstants.build.version);
  });

  protected apiConstants = this.apiConstantsService.info;
}

function extractBuildInformation(version: string) {
  const indexOfPlus = version.indexOf('+');
  if (indexOfPlus === -1) {
    return undefined;
  }

  return version.substring(indexOfPlus + 1);
}

function removeBuildInformation(version: string) {
  const indexOfPlus = version.indexOf('+');
  if (indexOfPlus === -1) {
    return version;
  }

  return version.substring(0, indexOfPlus);
}
