import { Component, computed, DestroyRef, inject, Resource, signal, Signal } from '@angular/core';
import { NgOptimizedImage } from '@angular/common';
import { environment } from '../../../environments/environment';
import { EditorSettings, EditorSettingsService } from './editor-settings.service';
import { EditorMenuComponent } from './editor-menu/editor-menu.component';
import { FormsModule } from '@angular/forms';
import { TwoColumnsComponent } from '../../core/two-columns/two-columns.component';
import { EditorSavedState, EditorStateService } from './editor-state.service';
import { PdfViewerComponent } from './pdf-viewer.component';
import { EditorHeaderComponent } from './editor-header/editor-header.component';
import { EditorWelcomeComponent } from './editor-welcome.component';
import { API_BASE_URL } from '../../app.config';
import { ApiServerStatusComponent } from '../../core/api/components/api-server-status.component';
import { ApiConstantsService } from '../../core/api/services/api-constants.service';
import { toastError } from '../../core/toasts/toast-error';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { EditorMenuService } from './editor-menu/editor-menu.service';
import { ToastService } from '../../core/toasts/toast.service';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-editor',
  imports: [
    NgOptimizedImage,
    PdfViewerComponent,
    EditorMenuComponent,
    FormsModule,
    EditorHeaderComponent,
    TwoColumnsComponent,
    EditorWelcomeComponent,
    ApiServerStatusComponent,
    RouterOutlet,
  ],
  template: `
    <div class="editor w-100 h-100 bg-body-tertiary d-flex flex-column">
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

      <main class="flex-grow-1 d-flex d-flex flex-column bg-body border rounded-3 mx-2 mx-lg-3 mt-2 mt-lg-3 mb-1 overflow-auto position-relative">
        @if (state.value(); as value) {
          <header>
            <app-editor-header [state]="value" [settings]="settings()"></app-editor-header>
          </header>

          <div class="flex-grow-1 overflow-hidden">
            <app-two-columns key="editor" (dragging)="disablePointerEvents.set($event)">
              <div class="h-100 overflow-hidden" left>
                <router-outlet></router-outlet>
              </div>
              <div class="h-100" right>
                @if (value.pdf; as pdf) {
                  <app-pdf-viewer [pdf]="pdf" [disablePointerEvents]="disablePointerEvents()" />
                } @else {
                  <div class="h-100 d-flex flex-column gap-5 align-items-center justify-content-center">
                    <button class="btn btn-shadow d-flex flex-column gap-2 align-items-center justify-content-center border rounded-3 p-5" (click)="importPdfImage()">
                      <i class="bi bi-filetype-pdf text-primary fs-1"></i>
                      <div class="lead text-primary">Import PDF image</div>
                    </button>
                    <button class="btn btn-shadow d-flex flex-column gap-2 align-items-center justify-content-center border rounded-3 p-5">
                      <i class="bi bi-filetype-pdf text-primary fs-1"></i>
                      <div class="lead text-primary">Auto-generate PDF image</div>
                    </button>
                  </div>
                }
              </div>
            </app-two-columns>
          </div>
        } @else if (state.isLoading()) {
          <div class="w-100 h-100 d-flex justify-content-center align-items-center">
            <div class="spinner-border" role="status">
              <span class="visually-hidden">Loading...</span>
            </div>
          </div>
        } @else {
          <div class="w-100 h-100">
            <app-editor-welcome></app-editor-welcome>
          </div>
        }
      </main>

      <div class="py-lg-1">
        <div class="flex-shrink-0 d-flex justify-content-center gap-2 px-4 small" [class.d-none]="!unsafeEnvironment()">
          <div class="text-truncate">
            <span class="text-danger fw-semibold"><i class="bi bi-exclamation-triangle-fill "></i> Do not share sensitive data </span>
            This application is hosted in an unsafe cloud environment. Although I do not store your data, or use your it for any purpose other than the application, the hosting
            environment is beyond my control.
          </div>
          <div class="text-nowrap">
            <a href="javascript:void 0;">About self-hosting...</a>
          </div>
        </div>

        <div class="flex-shrink-0 text-body-tertiary text-center px-4 text-truncate small">
          <strong>© 2025 Ismail Bennani</strong>, made with <i class="bi-heart-fill"></i> and <i class="bi bi-cup-hot-fill"></i>. The tools are open source and under the MIT
          License, feel free to use, modify, and share.
        </div>
      </div>
    </div>
  `,
})
export class EditorPage {
  protected readonly environment = environment;

  private editorMenuService = inject(EditorMenuService);
  private toastService = inject(ToastService);
  private destroyRef = inject(DestroyRef);
  private apiConstantsService = inject(ApiConstantsService);
  protected apiUrl = inject(API_BASE_URL);
  private editorStateService = inject(EditorStateService);
  private settingsService = inject(EditorSettingsService);

  protected state: Resource<EditorSavedState | null> = this.editorStateService.savedState;
  protected settings: Signal<EditorSettings> = this.settingsService.settings;

  protected unsafeEnvironment = computed(() => this.apiConstantsService.info.value()?.hosting.unsafeEnvironment ?? false);
  protected disablePointerEvents = signal<boolean>(false);

  importPdfImage() {
    this.editorMenuService
      .importPdfImageData()
      .pipe(
        toastError(this.toastService, (message) => `Could not create PDF image: ${message}`),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe();
  }
}
