import { Component, Signal } from '@angular/core';
import { NgOptimizedImage } from '@angular/common';
import { environment } from '../../../environments/environment';
import { PdfViewerComponent } from './pdf-viewer.component';
import { CiiFormComponent } from './cii-form/cii-form.component';
import { CrossIndustryInvoice } from '../../core/facturx-models/cii/cross-industry-invoice';
import { EditorSettings, EditorSettingsService } from './editor-settings.service';
import { CiiSummaryComponent } from './cii-summary/cii-summary.component';

@Component({
  selector: 'app-editor',
  imports: [NgOptimizedImage, PdfViewerComponent, CiiFormComponent, CiiSummaryComponent],
  template: `
    <div class="editor w-100 h-100 bg-body-tertiary d-flex flex-column gap-2">
      <header class="flex-shrink-0 text-bg-secondary d-flex align-items-center">
        <img ngSrc="logo.png" width="185" height="46" alt="Logo" class="logo" />
        <ul class="nav justify-content-center">
          <li class="nav-item dropdown">
            <a class="nav-link dropdown-toggle px-4 text-light" role="button" data-bs-toggle="dropdown" aria-expanded="false">File</a>
            <ul class="dropdown-menu">
              <li>
                <a class="dropdown-item" href="#">Create new document</a>
                <a class="dropdown-item" href="#">Import FacturX document</a>
                <hr />
                <a class="dropdown-item" href="#">Save FacturX as...</a>
                <a class="dropdown-item" href="#">Save Cross-Industry Invoice as...</a>
              </li>
            </ul>
          </li>
          <li class="nav-item dropdown">
            <a class="nav-link dropdown-toggle px-4 text-light" role="button" data-bs-toggle="dropdown" aria-expanded="false">About</a>
            <ul class="dropdown-menu">
              @if (environment.isUnsafeCloudEnvironment) {
                <li>
                  <a class="dropdown-item" href="#">Self-hosting</a>
                </li>
              }
              <li>
                <a class="dropdown-item" href="#">About FacturX.NET Web Editor</a>
              </li>
            </ul>
          </li>
        </ul>
        <div class="flex-grow-1"></div>
        <div class="px-4">
          {{ environment.version }}
        </div>
        <div class="px-4">
          <a href="https://github.com/FacturX-NET/FacturXDotNet" class="text-light">
            <i class="bi bi-github fs-4"></i>
          </a>
        </div>
      </header>

      <div class="flex-shrink-0 d-flex gap-2 px-4 small" [class.invisible]="!environment.isUnsafeCloudEnvironment">
        <div class="text-truncate">
          <span class="text-danger fw-semibold"><i class="bi bi-exclamation-triangle-fill "></i> Do not share sensitive data </span>
          This application is hosted in an unsafe cloud environment. Although I do not store your data, or use your it for any purpose other than the application, the hosting
          environment is beyond my control.
        </div>
        <div class="text-nowrap">
          <a href="#">About self-hosting...</a>
        </div>
      </div>

      <main class="editor__main-grid flex-grow-1 gap-1 gap-md-2 gap-lg-3 px-1 px-md-2 px-lg-3 overflow-hidden">
        <div class="h-100 bg-body border rounded-3 d-flex flex-column overflow-hidden">
          <header class="border-bottom d-flex">
            <div class="d-none d-xl-block col-3"><!--spacer--></div>
            <div class="navbar navbar-expand-xl">
              <div class="container justify-content-start gap-3">
                <div class="d-block d-xl-none">
                  <button class="navbar-toggler" data-bs-toggle="offcanvas" data-bs-target="#editor__cii-summary">
                    <span class="navbar-toggler-icon"></span>
                  </button>
                </div>
                <h5 class="navbar-brand m-0">Cross-Industry Invoice</h5>
                @if (settings(); as settings) {
                  <div class="dropdown">
                    <a href="javascript:void 0;" class="dropdown-toggle small" data-bs-toggle="dropdown" aria-expanded="false"> details </a>
                    <ul class="dropdown-menu">
                      <li>
                        <a class="dropdown-item" href="javascript:void 0;" (click)="toggleBusinessRules()" [class.text-body-tertiary]="settings.showBusinessRules !== true">
                          @if (settings.showBusinessRules === true) {
                            <i class="bi bi-eye"></i>
                          } @else {
                            <i class="bi bi-eye-slash"></i>
                          }
                          Business Rules
                        </a>
                      </li>
                      <li>
                        <a class="dropdown-item" href="javascript:void 0;" (click)="toggleRemarks()" [class.text-body-tertiary]="settings.showRemarks !== true">
                          @if (settings.showRemarks === true) {
                            <i class="bi bi-eye"></i>
                          } @else {
                            <i class="bi bi-eye-slash"></i>
                          }
                          Remarks
                        </a>
                      </li>
                      <li>
                        <a class="dropdown-item" href="javascript:void 0;" (click)="toggleChorusProRemarks()" [class.text-body-tertiary]="settings.showChorusProRemarks !== true">
                          @if (settings.showChorusProRemarks === true) {
                            <i class="bi bi-eye"></i>
                          } @else {
                            <i class="bi bi-eye-slash"></i>
                          }
                          Chorus Pro Remarks
                        </a>
                      </li>
                    </ul>
                  </div>
                }
              </div>
            </div>
          </header>
          <div class="flex-grow-1 overflow-hidden d-flex column-gap-4">
            <div id="editor__cii-summary" class="col-3 offcanvas-xl offcanvas-start overflow-auto ps-xl-3 pt-3" tabindex="-1" aria-labelledby="ciiSummaryTitle">
              <div class="offcanvas-header">
                <h5 class="offcanvas-title" id="ciiSummaryTitle">Summary</h5>
                <button type="button" class="btn-close" data-bs-dismiss="offcanvas" data-bs-target="#editor__cii-summary" aria-label="Close"></button>
              </div>
              <div class="d-none d-xl-block">
                <h6>Summary</h6>
              </div>
              <div class="offcanvas-body small">
                <app-cii-summary [value]="cii" />
              </div>
            </div>
            <div
              id="editor__cii-form"
              class="h-100 position-relative overflow-y-auto pt-3 pb-5"
              data-bs-spy="scroll"
              data-bs-target="#editor__cii-summary-content"
              data-bs-smooth-scroll="true"
              tabindex="0"
            >
              <div class="container">
                <app-cii-form [(value)]="cii" [settings]="settings()" />
              </div>
            </div>
          </div>
        </div>
        <div class="h-100 border">
          <app-pdf-viewer />
        </div>
      </main>

      <div class="flex-shrink-0 text-body-tertiary text-center px-4 text-truncate">
        <strong>© 2025 Ismail Bennani</strong>, made with <i class="bi-heart-fill"></i> and <i class="bi bi-cup-hot-fill"></i>. The tools are open source and under the MIT
        License, feel free to use, modify, and share.
      </div>
    </div>
  `,
})
export class EditorPage {
  protected readonly environment = environment;

  protected settings: Signal<EditorSettings>;
  protected cii: CrossIndustryInvoice = {};

  constructor(private settingsService: EditorSettingsService) {
    this.settings = this.settingsService.settings;
  }

  protected toggleBusinessRules() {
    const currentValue = this.settings()?.showBusinessRules == true;
    this.settingsService.saveShowBusinessRules(!currentValue);
  }

  protected toggleRemarks() {
    const currentValue = this.settings()?.showRemarks == true;
    this.settingsService.saveShowRemarks(!currentValue);
  }

  protected toggleChorusProRemarks() {
    const currentValue = this.settings()?.showChorusProRemarks == true;
    this.settingsService.saveShowChorusProRemarks(!currentValue);
  }
}
