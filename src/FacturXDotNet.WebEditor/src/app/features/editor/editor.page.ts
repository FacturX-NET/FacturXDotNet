import { Component, computed, inject, Resource, signal, Signal } from '@angular/core';
import { NgOptimizedImage } from '@angular/common';
import { environment } from '../../../environments/environment';
import { PdfViewerComponent } from './components/pdf-viewer.component';
import { EditorSettings, EditorSettingsService } from './editor-settings.service';
import { EditorMenuComponent } from './components/editor-menu/editor-menu.component';
import { EditorSavedState, EditorStateService } from './services/editor-state.service';
import { FormsModule } from '@angular/forms';
import { EditorHeaderComponent } from './components/editor-header/editor-header.component';
import { TwoColumnsComponent } from '../../core/two-columns/two-columns.component';
import { CiiComponent } from './components/cii/cii.component';

@Component({
  selector: 'app-editor',
  imports: [NgOptimizedImage, PdfViewerComponent, EditorMenuComponent, FormsModule, EditorHeaderComponent, TwoColumnsComponent, CiiComponent],
  template: `
    <div class="editor w-100 h-100 bg-body-tertiary d-flex flex-column">
      <header class="flex-shrink-0 text-bg-secondary d-flex align-items-center">
        <img ngSrc="logo.png" width="185" height="46" alt="Logo" class="logo" />
        <app-menu #appMenu [showSelfHostingMenu]="environment.isUnsafeCloudEnvironment ?? false" (exporting)="exporting.set($event)"></app-menu>
        <div class="flex-grow-1"></div>
        <div class="px-4">
          <a href="https://github.com/FacturX-NET/FacturXDotNet" class="text-light">
            <i class="bi bi-github fs-4"></i>
          </a>
        </div>
      </header>

      <main class="flex-grow-1 d-flex d-flex flex-column bg-body border rounded-3 mx-1 mx-md-2 mx-lg-3 mt-3 mb-1 overflow-hidden">
        @if (state.value(); as value) {
          <header class="border-bottom">
            <app-editor-header [state]="value" (exporting)="exporting.set($event)"></app-editor-header>
          </header>

          <div class="flex-grow-1 overflow-hidden">
            <app-two-columns key="editor" (dragging)="disablePointerEvents.set($event)">
              <div class="h-100 overflow-hidden" left>
                <app-cii [settings]="settings()" [disabled]="exporting()" />
              </div>
              <div class="h-100 d-flex flex-column gap-4 align-items-center justify-content-center" right>
                @if (value.pdf; as pdf) {
                  <app-pdf-viewer [pdf]="pdf" [disablePointerEvents]="disablePointerEvents()" />
                } @else {
                  <button
                    class="btn btn-shadow d-flex flex-column gap-2 align-items-center justify-content-center border rounded-3 p-5"
                    (click)="appMenu.importMenu()?.importPdfImage()"
                  >
                    <i class="bi bi-filetype-pdf text-body-tertiary fs-1"></i>
                    <div class="lead">Import PDF image</div>
                  </button>
                  OR
                  <button class="btn btn-shadow d-flex flex-column gap-2 align-items-center justify-content-center border rounded-3 p-5">
                    <i class="bi bi-filetype-pdf text-body-tertiary fs-1"></i>
                    <div class="lead">Auto-generate PDF image</div>
                  </button>
                }
              </div>
            </app-two-columns>
          </div>
        } @else {
          @if (state.isLoading()) {
            <div class="w-100 h-100 d-flex justify-content-center align-items-center">
              <div class="spinner-border" role="status">
                <span class="visually-hidden">Loading...</span>
              </div>
            </div>
          } @else {
            <div class="w-100 h-100 d-flex flex-column gap-5 align-items-center justify-content-center">
              <h1>Get started</h1>
              <div class="d-flex gap-5 align-items-center justify-content-center">
                <button class="btn btn-shadow d-flex flex-column gap-2 align-items-center justify-content-center border rounded-3 p-5" style="width: 400px">
                  <i class="bi bi-filetype-pdf text-body-tertiary fs-1"></i>
                  <div class="lead">Import FacturX document</div>
                </button>
                OR
                <button
                  class="btn btn-shadow d-flex flex-column gap-2 align-items-center justify-content-center border rounded-3 p-5"
                  style="width: 400px"
                  (click)="appMenu.importMenu()?.importPdfImage()"
                >
                  <i class="bi bi-filetype-pdf text-body-tertiary fs-1"></i>
                  <div class="lead">Import PDF image</div>
                </button>
                OR
                <button
                  class="btn btn-shadow d-flex flex-column gap-2 align-items-center justify-content-center border rounded-3 p-5"
                  style="width: 400px"
                  (click)="appMenu.importMenu()?.importCrossIndustryInvoice()"
                >
                  <i class="bi bi-code text-body-tertiary fs-1"></i>
                  <div class="lead">Import Cross-Industry Invoice file</div>
                </button>
              </div>
            </div>
          }
        }
      </main>

      <div class="flex-shrink-0 d-flex justify-content-center gap-2 px-4 small" [class.d-none]="!environment.isUnsafeCloudEnvironment">
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
  `,
  styles: `
    .btn-shadow {
      box-shadow: var(--bs-box-shadow);
    }

    .btn-shadow:hover {
      box-shadow: var(--bs-box-shadow-lg);
    }
  `,
})
export class EditorPage {
  protected readonly environment = environment;

  private settingsService = inject(EditorSettingsService);
  private editorStateService = inject(EditorStateService);

  protected settings: Signal<EditorSettings> = this.settingsService.settings;
  protected disablePointerEvents = signal<boolean>(false);
  protected state: Resource<EditorSavedState | null> = this.editorStateService.savedState;

  protected exporting = signal<boolean>(false);
}
