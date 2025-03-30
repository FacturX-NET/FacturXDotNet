import { Component, computed, inject, Signal, signal } from '@angular/core';
import { NgOptimizedImage } from '@angular/common';
import { environment } from '../../../environments/environment';
import { PdfViewerComponent } from './pdf-viewer.component';
import { CiiFormComponent, CrossIndustryInvoiceFormVerbosity } from './cii-form/cii-form.component';
import { CrossIndustryInvoice } from '../../core/facturx-models/cii/cross-industry-invoice';
import { EditorSettingsService } from './editor-settings.service';

@Component({
  selector: 'app-editor',
  imports: [NgOptimizedImage, PdfViewerComponent, CiiFormComponent],
  template: `
    <div class="w-100 h-100 bg-body-tertiary d-flex flex-column gap-2 overflow-hidden">
      <header class="flex-shrink-0 text-bg-secondary d-flex align-items-center">
        <img ngSrc="logo.png" width="185" height="46" alt="Logo" class="logo" />
        <ul class="nav justify-content-center">
          <li class="nav-item dropdown">
            <a class="nav-link dropdown-toggle px-4 text-light" type="button" data-bs-toggle="dropdown" aria-expanded="false">File</a>
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
            <a class="nav-link dropdown-toggle px-4 text-light" type="button" data-bs-toggle="dropdown" aria-expanded="false">About</a>
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

      <main class="flex-grow-1 d-grid gap-1 gap-md-2 gap-lg-3 px-1 px-md-2 px-lg-3 overflow-hidden" style="grid-template-columns: 1fr 1fr">
        <div class="h-100 bg-body border rounded-3 d-flex flex-column overflow-hidden">
          <header class="d-flex align-items-center gap-2 pt-3 px-3">
            <h5 class="m-0">Cross-Industry Invoice</h5>
            <div class="dropdown">
              <a href="javascript:void;" class="dropdown-toggle small" data-bs-toggle="dropdown" aria-expanded="false">
                @if (ciiVerbosity() === 'minimal') {
                  no details
                } @else if (ciiVerbosity() === 'normal') {
                  details
                } @else if (ciiVerbosity() === 'detailed') {
                  detailed
                }
              </a>
              <ul class="dropdown-menu">
                @if (ciiVerbosity() === 'minimal') {
                  <li><a class="dropdown-item" href="javascript:void;" (click)="setVerbosity('normal')">Show details</a></li>
                  <li><a class="dropdown-item" href="javascript:void;" (click)="setVerbosity('detailed')">Show all details</a></li>
                } @else if (ciiVerbosity() === 'normal') {
                  <li><a class="dropdown-item" href="javascript:void;" (click)="setVerbosity('minimal')">Hide details</a></li>
                  <li><a class="dropdown-item" href="javascript:void;" (click)="setVerbosity('detailed')">Show more details</a></li>
                } @else if (ciiVerbosity() === 'detailed') {
                  <li><a class="dropdown-item" href="javascript:void;" (click)="setVerbosity('minimal')">Hide details</a></li>
                  <li><a class="dropdown-item" href="javascript:void;" (click)="setVerbosity('normal')">Show less details</a></li>
                }
              </ul>
            </div>
          </header>
          <hr />
          <div class="flex-grow-1 overflow-auto px-3 pb-5">
            <app-cii-form [(value)]="cii" [verbosity]="ciiVerbosity()"></app-cii-form>
          </div>
        </div>
        <div class="h-100 border">
          <app-pdf-viewer></app-pdf-viewer>
        </div>
      </main>

      <div class="flex-shrink-0 text-body-tertiary text-center px-4 text-truncate">
        <strong>© 2025 Ismail Bennani</strong>, made with <i class="bi-heart-fill"></i> and <i class="bi bi-cup-hot-fill"></i>. The tools are open source and under the MIT
        License, feel free to use, modify, and share.
      </div>
    </div>
  `,
  styles: ``,
})
export class EditorPage {
  protected readonly environment = environment;

  protected ciiVerbosity: Signal<CrossIndustryInvoiceFormVerbosity>;
  protected cii: CrossIndustryInvoice = {};

  constructor(private settingsService: EditorSettingsService) {
    this.ciiVerbosity = computed(() => this.settingsService.settings().ciiFormVerbosity ?? 'normal');
  }

  protected setVerbosity(verbosity: CrossIndustryInvoiceFormVerbosity) {
    this.settingsService.saveVerbosity(verbosity);
  }
}
