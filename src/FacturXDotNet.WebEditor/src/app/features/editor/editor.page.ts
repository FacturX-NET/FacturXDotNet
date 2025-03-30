import { Component } from '@angular/core';
import { NgOptimizedImage } from '@angular/common';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-editor',
  imports: [NgOptimizedImage],
  template: `
    <div class="w-100 h-100 d-flex flex-column overflow-hidden">
      <header class="text-bg-secondary d-flex align-items-center">
        <img ngSrc="logo.png" width="185" height="46" alt="Logo" class="logo" />
        <ul class="nav justify-content-center">
          <li>
            <div class="dropdown">
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
            </div>
          </li>
          <li>
            <div class="dropdown">
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
            </div>
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

      <div class="bg-body-tertiary d-flex gap-2 px-4 small" [class.invisible]="!environment.isUnsafeCloudEnvironment">
        <div class="text-truncate">
          <i class="bi bi-exclamation-triangle-fill text-danger"></i> <strong class="text-danger"> Do not share sensitive data </strong>
          This application is hosted in an unsafe cloud environment. Although I do not store your data, or use your it for any purpose other than the application, the hosting
          environment is beyond my control.
        </div>
        <div class="text-nowrap">
          <a href="#">About self-hosting...</a>
        </div>
      </div>

      <main class="bg-body-tertiary flex-grow-1 container-fluid">
        <div class="h-100 row gap-3 px-3">
          <div class="bg-body border rounded-3 col"></div>
          <div class="bg-body border rounded-3 col-3" style="min-width:750px"></div>
        </div>
      </main>

      <div class="bg-body-tertiary text-body-tertiary text-center">
        <strong>© 2025 Ismail Bennani</strong>, made with <i class="bi-heart-fill"></i> and <i class="bi bi-cup-hot-fill"></i>. The tools are open source and under the MIT
        License, feel free to use, modify, and share.
      </div>
    </div>
  `,
  styles: ``,
})
export class EditorPage {
  protected readonly environment = environment;
}
