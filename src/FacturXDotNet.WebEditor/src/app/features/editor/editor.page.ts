import { Component, computed, effect, HostListener, inject, linkedSignal, Resource, signal, Signal } from '@angular/core';
import { NgOptimizedImage } from '@angular/common';
import { EditorSettings, EditorSettingsService, PdfModel } from './services/editor-settings.service';
import { EditorMenuComponent } from './components/editor-menu/editor-menu.component';
import { FormsModule } from '@angular/forms';
import { TwoColumnsComponent } from '../../core/two-columns/two-columns.component';
import { EditorSavedState, EditorStateService } from './services/editor-state.service';
import { EditorLeftPaneHeaderComponent } from './components/editor-header/editor-left-pane-header.component';
import { EditorWelcomeComponent } from './editor-welcome.component';
import { API_BASE_URL } from '../../app.config';
import { ApiServerStatusComponent } from '../../core/api/components/api-server-status.component';
import { ApiConstantsService } from '../../core/api/services/api-constants.service';
import { EditorMenuService } from './components/editor-menu/editor-menu.service';
import { RouterOutlet } from '@angular/router';
import { EditorPdfViewerComponent } from './components/editor-pdf-viewer/editor-pdf-viewer.component';
import { EditorHeaderNameComponent } from './components/editor-header/editor-header-name.component';
import { EditorRightPaneHeaderComponent } from './components/editor-header/editor-right-pane-header.component';
import { EditorResponsivenessService } from './services/editor-responsiveness.service';

@Component({
  selector: 'app-editor',
  imports: [
    NgOptimizedImage,
    EditorMenuComponent,
    FormsModule,
    EditorLeftPaneHeaderComponent,
    TwoColumnsComponent,
    EditorWelcomeComponent,
    ApiServerStatusComponent,
    RouterOutlet,
    EditorPdfViewerComponent,
    EditorRightPaneHeaderComponent,
  ],
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

      @if (isImporting() || isExporting()) {
        <div class="position-absolute top-0 bottom-0 start-0 end-0 d-flex flex-column justify-content-center align-items-center backdrop" style="z-index: 9999">
          <div class="card">
            <div class="card-body">
              @if (isImporting()) {
                <div class="spinner-border" role="status">
                  <span class="visually-hidden">Loading...</span>
                </div>
                Importing...
              } @else if (isExporting()) {
                <div class="spinner-border" role="status">
                  <span class="visually-hidden">Loading...</span>
                </div>
                Exporting...
              }
            </div>
          </div>
        </div>
      }

      @if (state.value(); as value) {
        <div class="flex-grow-1 overflow-hidden position-relative">
          <app-two-columns
            [(rightColumnWidth)]="rightColumnWidth"
            leftColumnMinWidth="500"
            rightColumnMinWidth="350"
            resizeHandleWidth="16"
            (dragging)="disablePointerEvents.set($event)"
            draggable
          >
            <div class="h-100 d-flex flex-column ps-2 ps-lg-3 pt-2 pt-lg-3 pb-1 position-relative" left>
              <div class="h-100 bg-body border rounded-3 d-flex flex-column">
                <header>
                  <div class="pt-3 pb-2">
                    <app-editor-left-pane-header [state]="value" [settings]="settings()"></app-editor-left-pane-header>
                  </div>
                </header>
                <div class="flex-grow-1 overflow-hidden">
                  <router-outlet></router-outlet>
                </div>
              </div>
            </div>
            <div class="h-100 pe-2 pe-lg-3 pt-2 pt-lg-3 pb-1 overflow-hidden position-relative" right>
              <div class="h-100 d-flex flex-column bg-body border rounded-3 overflow-hidden">
                <div class="px-3 pt-3 pb-2">
                  <app-editor-right-pane-header [tab]="pdfTab()" (tabChange)="changePdfTab($event)"></app-editor-right-pane-header>
                </div>
                <div class="flex-grow-1">
                  <app-editor-pdf-viewer [disablePointerEvents]="disablePointerEvents()" />
                </div>
              </div>
            </div>
          </app-two-columns>
        </div>
      } @else if (state.isLoading()) {
        <main class="flex-grow-1 d-flex d-flex flex-column bg-body border rounded-3 mx-2 mx-lg-3 mt-2 mt-lg-3 mb-1 overflow-auto position-relative">
          <div class="w-100 h-100 d-flex justify-content-center align-items-center">
            <div class="spinner-border" role="status">
              <span class="visually-hidden">Loading...</span>
            </div>
          </div>
        </main>
      } @else {
        <main class="flex-grow-1 d-flex d-flex flex-column bg-body border rounded-3 mx-2 mx-lg-3 mt-2 mt-lg-3 mb-1 overflow-auto position-relative">
          <div class="w-100 h-100">
            <app-editor-welcome></app-editor-welcome>
          </div>
        </main>
      }

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
  styles: `
    .backdrop {
      background-color: rgba(0, 0, 0, 0.5);
    }
  `,
})
export class EditorPage {
  protected apiUrl = inject(API_BASE_URL);
  protected pdfTab = computed(() => this.settings().pdfTab);
  protected disablePointerEvents = signal<boolean>(false);
  protected totalWidth = signal(window.innerWidth);
  private rightColumnWidthLocalStorageKey = 'editor';
  protected rightColumnWidth = linkedSignal<number, number>({
    source: () => this.totalWidth(),
    computation: (input, previous) => {
      if (previous !== undefined) {
        return previous?.value;
      }

      return this.loadRightColumnWidth(this.rightColumnWidthLocalStorageKey) ?? input / 2;
    },
  });
  private apiConstantsService = inject(ApiConstantsService);
  protected unsafeEnvironment = computed(() => this.apiConstantsService.info.value()?.hosting.unsafeEnvironment ?? false);
  private editorStateService = inject(EditorStateService);
  protected state: Resource<EditorSavedState | null> = this.editorStateService.savedState;
  private editorMenuService = inject(EditorMenuService);
  protected isImporting = this.editorMenuService.isImporting;
  protected isExporting = this.editorMenuService.isExporting;
  private editorSettingsService = inject(EditorSettingsService);
  private editorResponsivenessService = inject(EditorResponsivenessService);
  private settingsService = inject(EditorSettingsService);
  protected settings: Signal<EditorSettings> = this.settingsService.settings;

  constructor() {
    effect(() => {
      const rightColumnWidth = this.rightColumnWidth();
      this.saveRightColumnWidth(this.rightColumnWidthLocalStorageKey, rightColumnWidth);
    });

    effect(() => {
      const editorLeftColumnWidth = this.totalWidth() - this.rightColumnWidth() - 16;
      this.editorResponsivenessService.setLeftColumnWidth(editorLeftColumnWidth);
    });
  }

  @HostListener('window:resize', ['$event'])
  resize(event: Event) {
    const target = event.target as Window;
    const width = target?.innerWidth ?? 0;
    this.totalWidth.set(width);
  }

  protected changePdfTab(tab: PdfModel) {
    this.editorSettingsService.savePdfTab(tab);
  }

  private saveRightColumnWidth(key: string, width: number) {
    const localStorageKey = `two-columns-${key}`;
    localStorage.setItem(localStorageKey, width.toString());
  }

  private loadRightColumnWidth(key: string): number | undefined {
    const localStorageKey = `two-columns-${key}`;

    const widthString = localStorage.getItem(localStorageKey);
    if (widthString === null) {
      return undefined;
    }

    return parseInt(widthString, 10);
  }
}
