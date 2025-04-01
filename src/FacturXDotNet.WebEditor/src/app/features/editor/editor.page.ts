import { Component, computed, HostListener, signal, Signal } from '@angular/core';
import { NgOptimizedImage, NgStyle } from '@angular/common';
import { environment } from '../../../environments/environment';
import { PdfViewerComponent } from './pdf-viewer.component';
import { CiiFormComponent } from './cii-form/cii-form.component';
import { CrossIndustryInvoice } from '../../core/facturx-models/cii/cross-industry-invoice';
import { EditorSettings, EditorSettingsService } from './editor-settings.service';
import { CiiSummaryComponent } from './cii-summary/cii-summary.component';
import { EditorDetailsDropdownComponent } from './editor-details-dropdown.component';

@Component({
  selector: 'app-editor',
  imports: [NgOptimizedImage, PdfViewerComponent, CiiFormComponent, CiiSummaryComponent, NgStyle, EditorDetailsDropdownComponent],
  template: `
    <div class="editor w-100 h-100 bg-body-tertiary d-flex flex-column">
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

      <main class="flex-grow-1 d-flex px-1 px-md-2 px-lg-3 pt-2 overflow-hidden">
        <div class="h-100 bg-body border rounded-3 d-flex flex-column overflow-hidden" [ngStyle]="{ 'width.px': leftColumnWidth() }">
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
              <div class="offcanvas-body justify-content-end small">
                <div class="overflow-x-hidden">
                  <h6 class="d-none d-xl-block">Summary</h6>
                  <app-cii-summary [value]="cii" />
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
                <app-cii-form [(value)]="cii" [settings]="settings()" />
              </div>
            </div>
          </div>
        </div>
        <a href="javascript:void(0)" style="width: {{ resizeHandleWidth }}px; cursor: col-resize;" (mousedown)="dragStart($event)" (touchstart)="dragStart($event)"> </a>
        <div class="h-100 border" [ngStyle]="{ 'width.px': rightColumnWidth() }">
          <app-pdf-viewer [disablePointerEvents]="disablePointerEvents()" />
        </div>
      </main>

      <div class="flex-shrink-0 d-flex justify-content-center gap-2 px-4 small" [class.d-none]="!environment.isUnsafeCloudEnvironment">
        <div class="text-truncate">
          <span class="text-danger fw-semibold"><i class="bi bi-exclamation-triangle-fill "></i> Do not share sensitive data </span>
          This application is hosted in an unsafe cloud environment. Although I do not store your data, or use your it for any purpose other than the application, the hosting
          environment is beyond my control.
        </div>
        <div class="text-nowrap">
          <a href="#">About self-hosting...</a>
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

  protected settings: Signal<EditorSettings>;
  protected totalWidth = signal<number>(0);
  protected leftColumnWidth: Signal<number>;
  protected rightColumnWidth: Signal<number>;
  protected disablePointerEvents = signal<boolean>(false);
  protected cii: CrossIndustryInvoice = {};

  private resizing = false;

  constructor(private settingsService: EditorSettingsService) {
    this.settings = this.settingsService.settings;
    this.leftColumnWidth = computed(() => this.totalWidth() - this.rightColumnWidth() - this.resizeHandleWidth);
    this.rightColumnWidth = computed(() => this.settings().rightPaneWidth ?? 0);
    this.updateWidth(window.innerWidth);
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
