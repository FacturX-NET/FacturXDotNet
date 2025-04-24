import { Component, computed, effect, HostListener, inject, linkedSignal, Resource, signal, Signal } from '@angular/core';
import { NgOptimizedImage } from '@angular/common';
import { EditorSettings, EditorSettingsService, PdfModel } from './services/editor-settings.service';
import { EditorMenuComponent } from './components/editor-menu/editor-menu.component';
import { FormsModule } from '@angular/forms';
import { EditorSavedState, EditorStateService } from './services/editor-state.service';
import { API_BASE_URL } from '../../app.config';
import { ApiServerStatusComponent } from '../../core/api/components/api-server-status.component';
import { ApiConstantsService } from '../../core/api/services/api-constants.service';
import { EditorMenuService } from './components/editor-menu/editor-menu.service';
import { RouterOutlet } from '@angular/router';
import { GlobalOverlayService } from '../../core/global-overlay/global-overlay.service';

@Component({
  selector: 'app-editor',
  imports: [NgOptimizedImage, EditorMenuComponent, FormsModule, ApiServerStatusComponent, RouterOutlet],
  template: `
    <div class="editor w-100 h-100 bg-body-tertiary d-flex flex-column overflow-hidden">
      <header class="flex-shrink-0 text-bg-secondary d-flex flex-wrap align-items-center justify-content-center justify-content-lg-start">
        <div class="my-2 my-lg-0">
          <img ngSrc="logo.png" width="185" height="46" alt="Logo" class="logo" />
        </div>

        <div class="col-12 col-lg d-flex align-items-center">
          <div class="flex-grow-1 d-flex flex-wrap align-items-center justify-content-center justify-content-lg-between">
            <app-menu #appMenu [showSelfHostingMenu]="unsafeEnvironment()"></app-menu>

            <div class="d-flex gap-2 px-4 small position-relative">
              <a class="text-light text-decoration-underline stretched-link" [href]="apiUrl" target="_blank"> API server: </a>
              <app-api-server-status></app-api-server-status>
            </div>
          </div>

          <div class="px-4">
            <a href="https://github.com/FacturX-NET/FacturXDotNet" class="text-light">
              <i class="bi bi-github fs-4"></i>
            </a>
          </div>
        </div>
      </header>

      <div class="flex-grow-1 overflow-hidden">
        <router-outlet></router-outlet>
      </div>

      <div class="py-lg-1">
        <div class="flex-shrink-0 d-flex justify-content-center gap-2 px-4 small" [class.d-none]="!unsafeEnvironment()">
          <div class="text-truncate">
            <span class="text-danger fw-semibold"><i class="bi bi-exclamation-triangle-fill "></i> Do not share sensitive data </span>
            This application is hosted in an unsafe cloud environment. Although I do not store your data, or use it for any purpose other than the application, the hosting
            environment is beyond my control.
          </div>
          <div class="text-nowrap">
            <a href="javascript:void 0;">About self-hosting...</a>
          </div>
        </div>

        <div class="flex-shrink-0 text-body-tertiary text-center px-4 text-truncate small">
          <strong>© 2025 Ismail Bennani</strong>, made with <i class="bi-heart-fill"></i> and <i class="bi bi-cup-hot-fill"></i>. The tools are open source and released under the
          MIT License, feel free to use, modify, and share.
        </div>
      </div>
    </div>
  `,
  styles: ``,
})
export class EditorLayout {
  protected apiUrl = inject(API_BASE_URL);

  private apiConstantsService = inject(ApiConstantsService);
  protected unsafeEnvironment = computed(() => this.apiConstantsService.info.value()?.hosting.unsafeEnvironment ?? false);

  private globalOverlayService = inject(GlobalOverlayService);
  private editorMenuService = inject(EditorMenuService);
  protected isImporting = this.editorMenuService.isImporting;
  protected isExporting = this.editorMenuService.isExporting;

  constructor() {
    effect(() => {
      const importing = this.isImporting();
      const exporting = this.isExporting();

      if (importing) {
        this.globalOverlayService.enable('Importing...');
      } else if (exporting) {
        this.globalOverlayService.enable('Importing...');
      } else {
        this.globalOverlayService.disable();
      }
    });
  }
}
