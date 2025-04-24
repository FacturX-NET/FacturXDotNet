import { Component, computed, effect, HostListener, inject, linkedSignal, Resource, Signal, signal } from '@angular/core';
import { EditorLeftPaneHeaderComponent } from './components/editor-header/editor-left-pane-header.component';
import { EditorPdfViewerComponent } from './editor-tabs/editor-pdf-viewer/editor-pdf-viewer.component';
import { EditorRightPaneHeaderComponent } from './components/editor-header/editor-right-pane-header.component';
import { EditorWelcomePage } from './editor-welcome.page';
import { Router, RouterOutlet } from '@angular/router';
import { TwoColumnsComponent } from '../../core/two-columns/two-columns.component';
import { EditorSavedState, EditorStateService } from './services/editor-state.service';
import { EditorSettings, EditorSettingsService, PdfModel } from './services/editor-settings.service';
import { EditorResponsivenessService } from './services/editor-responsiveness.service';

@Component({
  selector: 'app-editor',
  imports: [EditorLeftPaneHeaderComponent, EditorPdfViewerComponent, EditorRightPaneHeaderComponent, RouterOutlet, TwoColumnsComponent],
  template: `
    @if (state.value(); as value) {
      <div class="h-100 overflow-hidden position-relative">
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
    }
  `,
  styles: ``,
})
export class EditorPage {
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

  private editorStateService = inject(EditorStateService);
  protected state: Resource<EditorSavedState | null> = this.editorStateService.savedState;

  private editorSettingsService = inject(EditorSettingsService);
  private editorResponsivenessService = inject(EditorResponsivenessService);
  private settingsService = inject(EditorSettingsService);
  protected settings: Signal<EditorSettings> = this.settingsService.settings;

  private router = inject(Router);

  constructor() {
    effect(() => {
      if (!this.state.isLoading() && this.state.value() === null) {
        this.router.navigate(['/', 'welcome']).then();
      }
    });

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
