import { Component, computed, effect, HostListener, inject, Resource, signal, Signal } from '@angular/core';
import { NgOptimizedImage, NgStyle } from '@angular/common';
import { environment } from '../../../environments/environment';
import { PdfViewerComponent } from './components/pdf-viewer.component';
import { CiiFormComponent } from './components/cii-form/cii-form.component';
import { EditorSettings, EditorSettingsService } from './editor-settings.service';
import { CiiSummaryComponent } from './components/cii-summary/cii-summary.component';
import { EditorDetailsDropdownComponent } from './components/editor-header/editor-details-dropdown.component';
import { EditorMenuComponent } from './components/editor-menu/editor-menu.component';
import { EditorSavedState, EditorStateService } from './services/editor-state.service';
import { debounceTime, delay, from, Subject, switchMap, tap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CrossIndustryInvoice, ICrossIndustryInvoice } from '../../core/api/api.models';
import { FormsModule } from '@angular/forms';
import { EditorHeaderComponent } from './components/editor-header/editor-header.component';
import { TwoColumnsComponent } from '../../core/two-columns/two-columns.component';

@Component({
  selector: 'app-editor',
  imports: [
    NgOptimizedImage,
    PdfViewerComponent,
    CiiFormComponent,
    CiiSummaryComponent,
    NgStyle,
    EditorDetailsDropdownComponent,
    EditorMenuComponent,
    FormsModule,
    EditorHeaderComponent,
    TwoColumnsComponent,
  ],
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
            <app-editor-header [state]="value" [saving]="saving()" [saved]="saved()" (exporting)="exporting.set($event)"></app-editor-header>
          </header>

          <div class="flex-grow-1 overflow-hidden">
            <app-two-columns (dragging)="disablePointerEvents.set($event)">
              <div class="h-100 d-flex column-gap-4 overflow-hidden" left>
                <div id="editor__cii-summary" class="col-3 offcanvas-xl offcanvas-start overflow-y-auto ps-xl-3 pt-3" tabindex="-1" aria-labelledby="ciiSummaryTitle">
                  <div class="offcanvas-header">
                    <h5 class="offcanvas-title" id="ciiSummaryTitle">Cross-Industry Invoice</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="offcanvas" data-bs-target="#editor__cii-summary" aria-label="Close"></button>
                  </div>
                  <div class="offcanvas-body small">
                    <div class="overflow-x-hidden">
                      <h6 class="d-none d-xl-block">Cross-Industry Invoice</h6>
                      <app-cii-summary [value]="value.cii" [settings]="settings()" />
                    </div>
                  </div>
                </div>
                <div
                  id="editor__cii-form"
                  class="flex-grow-1 h-100 position-relative overflow-y-auto pt-3 pb-5"
                  data-bs-spy="scroll"
                  data-bs-target="#editor__cii-summary-content"
                  data-bs-smooth-scroll="true"
                  tabindex="0"
                >
                  <div class="container ms-0">
                    <app-cii-form [value]="value.cii" (valueChange)="saveCii($event)" [settings]="settings()" [disabled]="exporting()" />
                  </div>
                </div>
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
  protected resizeHandleWidth = 16;

  private settingsService = inject(EditorSettingsService);
  private editorStateService = inject(EditorStateService);

  protected settings: Signal<EditorSettings> = this.settingsService.settings;
  protected totalWidth = signal<number>(0);
  protected leftColumnWidth: Signal<number> = computed(() => this.totalWidth() - this.rightColumnWidth() - this.resizeHandleWidth);
  protected rightColumnWidth: Signal<number> = computed(() => this.settings().rightPaneWidth ?? 0);
  protected disablePointerEvents = signal<boolean>(false);
  protected state: Resource<EditorSavedState | null> = this.editorStateService.savedState;

  protected saving = signal<boolean>(false);
  protected saved = signal<boolean>(false);
  private saveSubject = new Subject<EditorSavedState>();

  protected exporting = signal<boolean>(false);

  constructor() {
    this.saveSubject
      .pipe(
        debounceTime(1000),
        switchMap((state) => from(this.editorStateService.update(state))),
        tap(() => {
          this.saving.set(false);
          this.saved.set(true);
        }),
        delay(2000),
        tap(() => this.saved.set(false)),
        takeUntilDestroyed(),
      )
      .subscribe();
  }

  /**
   * Registers the current CII state for saving. The state will be saved after a debounce period.
   * @param cii The Cross-Industry Invoice to save.
   */
  saveCii(cii: ICrossIndustryInvoice) {
    const value = this.state.value();
    if (value === null) {
      return;
    }

    this.saving.set(true);
    this.saveSubject.next({ ...value, cii });
  }
}
