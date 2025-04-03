import { Component, computed, HostListener, inject, Resource, signal, Signal } from '@angular/core';
import { NgOptimizedImage, NgStyle } from '@angular/common';
import { environment } from '../../../environments/environment';
import { PdfViewerComponent } from './components/pdf-viewer.component';
import { CiiFormComponent } from './components/cii-form/cii-form.component';
import { CrossIndustryInvoice } from '../../core/facturx-models/cii/cross-industry-invoice';
import { EditorSettings, EditorSettingsService } from './editor-settings.service';
import { CiiSummaryComponent } from './components/cii-summary/cii-summary.component';
import { EditorDetailsDropdownComponent } from './editor-details-dropdown.component';
import { EditorMenuComponent } from './components/editor-menu/editor-menu.component';
import { EditorSavedState, EditorStateService } from './services/editor-state.service';
import { debounceTime, delay, from, Subject, switchMap, tap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-editor',
  imports: [NgOptimizedImage, PdfViewerComponent, CiiFormComponent, CiiSummaryComponent, NgStyle, EditorDetailsDropdownComponent, EditorMenuComponent],
  template: `
    <div class="editor w-100 h-100 bg-body-tertiary d-flex flex-column">
      <header class="flex-shrink-0 text-bg-secondary d-flex align-items-center">
        <img ngSrc="logo.png" width="185" height="46" alt="Logo" class="logo" />
        <app-menu #appMenu [showSelfHostingMenu]="environment.isUnsafeCloudEnvironment ?? false"></app-menu>
        <div class="flex-grow-1"></div>
        <div class="px-4">
          <a href="https://github.com/FacturX-NET/FacturXDotNet" class="text-light">
            <i class="bi bi-github fs-4"></i>
          </a>
        </div>
      </header>

      <main class="flex-grow-1 d-flex px-1 px-md-2 px-lg-3 pt-3 pb-1 overflow-hidden">
        <div class="h-100 bg-body border rounded-3 d-flex flex-column overflow-hidden" [ngStyle]="{ 'width.px': leftColumnWidth() }">
          @if (state.value(); as value) {
            <header class="border-bottom d-flex">
              <div class="d-none d-xl-block col-3"><!--spacer--></div>
              <div class="navbar navbar-expand-xl flex-grow-1">
                <div class="flex-grow-1 d-flex justify-content-start align-items-center gap-3 px-3">
                  <div class="d-block d-xl-none">
                    <button class="navbar-toggler" data-bs-toggle="offcanvas" data-bs-target="#editor__cii-summary">
                      <span class="navbar-toggler-icon"></span>
                    </button>
                  </div>
                  <h5 class="navbar-brand m-0"><i class="bi bi-code pe-1"></i> {{ value.cii.name ?? 'Cross-Industry Invoice' }}</h5>

                  <div class="flex-grow-1"><!--spacer--></div>

                  @if (saving()) {
                    <div><i class="bi bi-floppy2-fill text-body-tertiary small glow"></i></div>
                  } @else {
                    @if (saved()) {
                      <div class="text-success small"><i class="bi bi-check"></i> Saved</div>
                    }

                    <div>
                      <div class="input-group">
                        <button class="btn btn-outline-secondary">Export</button>
                        <button class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false"></button>
                        <div class="dropdown-menu dropdown-menu-end">
                          <li><a class="dropdown-item" href="javascript:void 0;">Export Cross-Industry Invoice</a></li>
                          <li><a class="dropdown-item" href="javascript:void 0;" (click)="appMenu.exportMenu()?.exportPdfImage()">Export PDF</a></li>
                        </div>
                      </div>
                    </div>
                  }

                  <app-editor-details-dropdown />
                </div>
              </div>
            </header>

            <div class="flex-grow-1 overflow-hidden d-flex column-gap-4">
              <div id="editor__cii-summary" class="col-3 offcanvas-xl offcanvas-start overflow-y-auto ps-xl-3 pt-3" tabindex="-1" aria-labelledby="ciiSummaryTitle">
                <div class="offcanvas-header">
                  <h5 class="offcanvas-title" id="ciiSummaryTitle">Summary</h5>
                  <button type="button" class="btn-close" data-bs-dismiss="offcanvas" data-bs-target="#editor__cii-summary" aria-label="Close"></button>
                </div>
                <div class="offcanvas-body small">
                  <div class="overflow-x-hidden">
                    <h6 class="d-none d-xl-block">Summary</h6>
                    <app-cii-summary [value]="value.cii.content" [settings]="settings()" />
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
                  <app-cii-form [value]="value.cii.content" (valueChange)="saveCii($event)" [settings]="settings()" />
                </div>
              </div>
            </div>
          } @else {
            @if (state.isLoading()) {
              <div class="w-100 h-100 d-flex justify-content-center align-items-center">
                <div class="spinner-border" role="status">
                  <span class="visually-hidden">Loading...</span>
                </div>
              </div>
            } @else {
              Error!
            }
          }
        </div>
        <a href="javascript:void(0)" style="width: {{ resizeHandleWidth }}px; cursor: col-resize;" (mousedown)="dragStart($event)" (touchstart)="dragStart($event)"> </a>
        <div class="h-100 bg-body border rounded-3 d-flex flex-column overflow-hidden" [ngStyle]="{ 'width.px': rightColumnWidth() }">
          @if (state.value(); as value) {
            @if (value.pdf?.content; as pdf) {
              <div class="flex-grow-1">
                <app-pdf-viewer [pdf]="pdf" [disablePointerEvents]="disablePointerEvents()" />
              </div>
            } @else {
              <div class="flex-grow-1 d-flex flex-column gap-2 align-items-center justify-content-center position-relative">
                <i class="bi bi-filetype-pdf text-body-tertiary fs-1"></i>
                <button class="btn btn-outline-secondary stretched-link" (click)="appMenu.importMenu()?.importPdfImage()">Import...</button>
              </div>
            }
          } @else {
            @if (state.isLoading()) {
              <div class="w-100 flex-grow-1 d-flex justify-content-center align-items-center">
                <div class="spinner-border" role="status">
                  <span class="visually-hidden">Loading...</span>
                </div>
              </div>
            } @else {
              Error!
            }
          }
        </div>
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
  protected state: Resource<EditorSavedState | undefined> = this.editorStateService.savedState;

  protected saving = signal<boolean>(false);
  protected saved = signal<boolean>(false);
  private saveSubject = new Subject<EditorSavedState>();
  private resizing = false;

  constructor() {
    this.updateWidth(window.innerWidth);
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

  saveCii(cii: CrossIndustryInvoice) {
    const value = this.state.value();
    this.saving.set(true);
    this.saveSubject.next({ ...(value?.pdf ?? {}), cii: { ...(value?.cii ?? {}), content: cii } });
  }

  @HostListener('window:resize', ['$event'])
  resize(event: Event) {
    const target = event.target as Window;
    const width = target?.innerWidth ?? 0;
    this.updateWidth(width);
  }

  @HostListener('window:mousemove', ['$event'])
  mousemove(event: Event) {
    if (this.resizing) {
      event.preventDefault();
      this.drag(event);
    }
  }

  @HostListener('window:touchmove', ['$event'])
  touchmove(event: Event) {
    if (this.resizing) {
      event.preventDefault();
      this.drag(event);
    }
  }

  @HostListener('window:mouseup', ['$event'])
  mouseup(event: Event) {
    if (this.resizing) {
      event.preventDefault();
      this.dragEnd();
    }
  }

  @HostListener('window:touchend', ['$event'])
  touchend(event: Event) {
    if (this.resizing) {
      event.preventDefault();
      this.dragEnd();
    }
  }

  protected dragStart(event: Event) {
    event.preventDefault();
    this.resizing = true;
    this.disablePointerEvents.set(true);
  }

  protected drag(event: Event) {
    event.preventDefault();

    if (this.resizing) {
      const width = this.totalWidth();
      const x = event.type === 'mousemove' ? (event as MouseEvent).clientX : (event as TouchEvent).touches[0].clientX;
      this.settingsService.saveRightPaneWidth(width - x - this.resizeHandleWidth / 2);
    }
  }

  protected dragEnd(event?: Event) {
    event?.preventDefault();
    this.resizing = false;
    this.disablePointerEvents.set(false);
  }

  private updateWidth(width: number) {
    this.totalWidth.set(width);
    if (this.rightColumnWidth() === 0) {
      this.settingsService.saveRightPaneWidth(width / 2);
    }
  }
}
