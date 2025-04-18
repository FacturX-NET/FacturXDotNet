import { Component, computed, effect, HostListener, inject, linkedSignal, Resource, signal, Signal } from '@angular/core';
import { NgOptimizedImage } from '@angular/common';
import { EditorSettings, EditorSettingsService, PdfModel } from './editor-settings.service';
import { EditorMenuComponent } from './editor-menu/editor-menu.component';
import { FormsModule } from '@angular/forms';
import { TwoColumnsComponent } from '../../core/two-columns/two-columns.component';
import { EditorSavedState, EditorStateService } from './editor-state.service';
import { EditorLeftPaneHeaderComponent } from './editor-header/editor-left-pane-header.component';
import { EditorWelcomeComponent } from './editor-welcome.component';
import { API_BASE_URL } from '../../app.config';
import { ApiServerStatusComponent } from '../../core/api/components/api-server-status.component';
import { ApiConstantsService } from '../../core/api/services/api-constants.service';
import { EditorMenuService } from './editor-menu/editor-menu.service';
import { RouterOutlet } from '@angular/router';
import { EditorPdfViewerComponent } from './editor-pdf-viewer/editor-pdf-viewer.component';
import { EditorHeaderNameComponent } from './editor-header/editor-header-name.component';
import { EditorRightPaneHeaderComponent } from './editor-header/editor-right-pane-header.component';
import { EditorResponsivenessService } from './editor-responsiveness.service';

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
    EditorHeaderNameComponent,
    EditorRightPaneHeaderComponent,
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
          <header>
            <div class="px-3 pt-3 pb-2">
              <app-editor-header-name [name]="value.name" />
            </div>
            <div>
              <app-two-columns [rightColumnWidth]="rightColumnWidth()" resizeHandleWidth="16">
                <div class="h-100" left>
                  <app-editor-left-pane-header [state]="value" [settings]="settings()"></app-editor-left-pane-header>
                </div>
                <div class="h-100" right>
                  <app-editor-right-pane-header [tab]="pdfTab()" (tabChange)="changePdfTab($event)"></app-editor-right-pane-header>
                </div>
              </app-two-columns>
            </div>
          </header>

          <div class="flex-grow-1 overflow-hidden">
            <app-two-columns [(rightColumnWidth)]="rightColumnWidth" resizeHandleWidth="16" (dragging)="disablePointerEvents.set($event)" draggable>
              <div class="h-100 overflow-hidden" left>
                <router-outlet></router-outlet>
              </div>
              <div class="h-100" right>
                <app-editor-pdf-viewer [disablePointerEvents]="disablePointerEvents()" />
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
  styles: `
    .backdrop {
      background-color: rgba(0, 0, 0, 0.5);
    }
  `,
})
export class EditorPage {
  private rightColumnWidthLocalStorageKey = 'editor';

  private apiConstantsService = inject(ApiConstantsService);
  private editorStateService = inject(EditorStateService);
  private editorMenuService = inject(EditorMenuService);
  private editorSettingsService = inject(EditorSettingsService);
  private editorResponsivenessService = inject(EditorResponsivenessService);
  private settingsService = inject(EditorSettingsService);

  protected apiUrl = inject(API_BASE_URL);
  protected state: Resource<EditorSavedState | null> = this.editorStateService.savedState;
  protected settings: Signal<EditorSettings> = this.settingsService.settings;
  protected pdfTab = computed(() => this.settings().pdfTab);

  protected isImporting = this.editorMenuService.isImporting;
  protected isExporting = this.editorMenuService.isExporting;

  protected unsafeEnvironment = computed(() => this.apiConstantsService.info.value()?.hosting.unsafeEnvironment ?? false);
  protected disablePointerEvents = signal<boolean>(false);

  protected totalWidth = signal(window.innerWidth);

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

  protected rightColumnWidth = linkedSignal<number, number>({
    source: () => this.totalWidth(),
    computation: (input, previous) => {
      if (previous !== undefined) {
        return previous?.value;
      }

      return this.loadRightColumnWidth(this.rightColumnWidthLocalStorageKey) ?? input / 2;
    },
  });

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
